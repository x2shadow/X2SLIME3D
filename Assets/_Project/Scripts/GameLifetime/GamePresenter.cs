using System;
using System.ComponentModel.Design;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using GamePush;

namespace X2SLIME3D
{
    public class GamePresenter : IStartable, IDisposable
    {
        readonly CompositeDisposable disposable = new CompositeDisposable();

        readonly UIService uiService;
        readonly PlayerView player;
        readonly AudioService audioService;

        private int currentLevelIndex = 0;
        private ColorPalette palette;


        private int winSoundIndex = 0;
        private readonly SoundType[] winSounds = 
        {
            SoundType.Win1, SoundType.Win2, SoundType.Win3, SoundType.Win4, SoundType.Win5
        };

        GamePresenter(UIService uiService, PlayerView player, AudioService audioService)
        {
            this.uiService = uiService;
            this.player = player;
            this.audioService = audioService;
        }

        public async void Start()
        {
            await GP_Init.Ready;
            if(GP_Ads.IsPreloaderPlaying()) audioService.MuteForAd();
            GP_Ads.OnPreloaderClose += (bool isClosed) => audioService.UnmuteAfterAd();

            palette = Resources.Load<ColorPalette>("ColorPalette");

            currentLevelIndex = GetLoadedLevelNumber() - 1;
            RunGameFlow().Forget();
        }

        private async UniTask RunGameFlow()
        {
            while (currentLevelIndex < (SceneManager.sceneCountInBuildSettings - 1))
            {
                await LoadAndPlayCurrentLevel();
                currentLevelIndex++;
            }
            Debug.Log("Все уровни пройдены!");
            uiService.ShowYouWin();
            currentLevelIndex = 0;
        }

        private async UniTask LoadAndPlayCurrentLevel()
        {
            string sceneName = "Level" + (currentLevelIndex + 1);
            uiService.UpdateLevel(currentLevelIndex + 1); 
            Scene scene = SceneManager.GetSceneByName(sceneName);

            // Если сцена ещё не загружена, загружаем её
            if (!scene.isLoaded)
            {
                Debug.Log($"Загрузка уровня: {sceneName}");
                AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                await loadOp.ToUniTask();
                // Обновляем ссылку на сцену после загрузки
                scene = SceneManager.GetSceneByName(sceneName);
                Debug.Log($"Уровень {sceneName} загружен");
            }
            else
            {
                Debug.Log($"Сцена {sceneName} уже загружена, пропускаем загрузку.");
            }

            // Находим SpawnPoint и перемещаем игрока туда
            GameObject spawnPoint = FindObjectByNameInScene(scene, "SpawnPoint");
            if (spawnPoint == null)
            {
                Debug.LogError("SpawnPoint не найден в сцене!");
                return;
            }
            await UniTask.Delay(100);
            MovePlayerToSpawnPoint(spawnPoint);
            await UniTask.Delay(100);
            player.playerRenderer.material.color = palette.characterColor;
            player.gameObject.SetActive(true);


            // Находим компоненты, отвечающие за завершение уровня и за "падение в воду"
            LevelFinish finish   = GameObject.FindObjectOfType<LevelFinish>();
            LevelRestart restart = GameObject.FindObjectOfType<LevelRestart>();

            if (finish  == null) { Debug.LogWarning("LevelFinish не найден в сцене!");  return; }
            if (restart == null) { Debug.LogWarning("LevelRestart не найден в сцене!"); return; }

            // Пока уровень не завершён, ждём событий
            bool levelCompleted = false;
            while (!levelCompleted)
            {
                var waterRestart = restart.OnLevelRestarted.Select(_ => RestartSource.Water);
                var uiRestart    = uiService.OnLevelRestarted.Select(_ => RestartSource.UI);

                var restartTcs = new UniTaskCompletionSource<RestartSource>();
                var finishTcs  = new UniTaskCompletionSource();

                var subscriptionFinish  = finish.OnLevelFinished.Subscribe(_ => finishTcs.TrySetResult());
                var subscriptionRestart = Observable.Merge(waterRestart, uiRestart).Subscribe(source => restartTcs.TrySetResult(source));

                // Ожидаем либо завершения уровня, либо "падения в воду"
                var completedTask = await UniTask.WhenAny(finishTcs.Task, restartTcs.Task);

                // Отписываемся от событий, чтобы избежать утечек памяти
                subscriptionFinish.Dispose();
                subscriptionRestart.Dispose();

                if (completedTask == 1)
                {
                    var source = await restartTcs.Task;
                    
                    if (source == RestartSource.Water)
                    {
                        Debug.Log("Игрок упал в воду! Перемещаем в SpawnPoint.");
                        audioService.PlaySound(SoundType.Splash);
                        ShowAd();
                    }

                    player.gameObject.SetActive(false);
                    MovePlayerToSpawnPoint(spawnPoint);
                    await UniTask.Delay(100);
                    player.gameObject.SetActive(true);  
                }
                else
                {
                    Debug.Log("Уровень завершён!");
                    levelCompleted = true;
                    PlayNextWinSound();
                }
            }
            
            DOTween.KillAll();
            
            // После завершения уровня выгружаем сцену
            player.gameObject.SetActive(false);
            await UniTask.Delay(250);

            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
            await unloadOp.ToUniTask();
        }


        //vvv
        private GameObject FindObjectByNameInScene(Scene scene, string objectName)
        {
            foreach (GameObject rootObj in scene.GetRootGameObjects())
            {
                if (rootObj.name.Equals(objectName))
                    return rootObj;

                GameObject found = FindInChildren(rootObj.transform, objectName);
                if (found != null)
                    return found;
            }
            return null;
        }

        private GameObject FindInChildren(Transform parent, string objectName)
        {
            foreach (Transform child in parent)
            {
                if (child.gameObject.name.Equals(objectName))
                    return child.gameObject;

                GameObject result = FindInChildren(child, objectName);
                if (result != null)
                    return result;
            }
            return null;
        }
        //^^^^

        public void PlayNextWinSound()
        {
            audioService.PlaySound(winSounds[winSoundIndex]); 
            winSoundIndex = (winSoundIndex + 1) % winSounds.Length; // Инкрементируем и сбрасываем при 5
        }

        int GetLoadedLevelNumber()
        {
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name.StartsWith("Level"))
                {
                    // Извлекаем часть имени, которая следует за "Level"
                    string numberPart = scene.name.Substring("Level".Length);
                    if (int.TryParse(numberPart, out int levelNumber))
                    {
                        return levelNumber;
                    }
                }
            }
            return 1; // Если ни одна сцена не найдена или число не удалось распарсить
        }

        void MovePlayerToSpawnPoint(GameObject spawnPoint)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            player.transform.position = spawnPoint.transform.position;
            rb.isKinematic = false;
            Debug.Log("Игрок перемещён в SpawnPoint");
        }

        void ShowAd()
        {
            if (GP_Ads.IsFullscreenAvailable())
            {
                GP_Ads.ShowFullscreen(OnFullscreenStart, OnFullscreenClose);
            }
        }

        private void OnFullscreenStart()
        {
            Debug.Log("ON FULLSCREEN START");
            audioService.MuteForAd();
        }

        private void OnFullscreenClose(bool success)
        {
            Debug.Log("ON FULLSCREEN CLOSE: " + success);
            audioService.UnmuteAfterAd();
        }

        public void Dispose() => disposable.Dispose();
    }

    public enum RestartSource
    {
        Water, // игрок упал в воду
        UI     // рестарт через кнопку в UI
    }
}
