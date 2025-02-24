using System;
using System.ComponentModel.Design;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace X2SLIME3D
{
    public class GamePresenter : IStartable, IDisposable
    {
        readonly CompositeDisposable disposable = new CompositeDisposable();

        readonly UIService uiService;
        readonly PlayerView player;

        private int currentLevelIndex = 0;

        GamePresenter(UIService uiService, PlayerView player)
        {
            this.uiService = uiService;
            this.player = player;
        }

        public void Start()
        {
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
                var finishTcs  = new UniTaskCompletionSource();
                var restartTcs = new UniTaskCompletionSource();

                var subscriptionFinish  = finish.OnLevelFinished.Subscribe(_ => finishTcs.TrySetResult());
                var subscriptionRestart = restart.OnLevelRestarted.Subscribe(_ => restartTcs.TrySetResult());

                // Ожидаем либо завершения уровня, либо "падения в воду"
                var completedTask = await UniTask.WhenAny(finishTcs.Task, restartTcs.Task);

                // Отписываемся от событий, чтобы избежать утечек памяти
                subscriptionFinish.Dispose();
                subscriptionRestart.Dispose();

                if (completedTask == 1)
                {
                    Debug.Log("Игрок упал в воду! Перемещаем в SpawnPoint.");
                    MovePlayerToSpawnPoint(spawnPoint);
                }
                else
                {
                    Debug.Log("Уровень завершён!");
                    levelCompleted = true;
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

        public void Dispose() => disposable.Dispose();
    }
}
