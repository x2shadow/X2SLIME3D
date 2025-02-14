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

        private readonly string[] levelSceneNames = { "Level1", "Level2"};
        private int currentLevelIndex = 0;

        GamePresenter()
        {
        }

        public void Start()
        {
            currentLevelIndex = GetLoadedLevelNumber() - 1;
            RunGameFlow().Forget();
        }

        private async UniTask RunGameFlow()
        {
            while (currentLevelIndex < levelSceneNames.Length)
            {
                await LoadAndPlayCurrentLevel();
                currentLevelIndex++;
            }
            Debug.Log("Все уровни пройдены!");
            currentLevelIndex = 0;
        }

        private async UniTask LoadAndPlayCurrentLevel()
        {
            string sceneName = levelSceneNames[currentLevelIndex];
            Scene scene = SceneManager.GetSceneByName(sceneName);

            // Если сцена ещё не загружена, загружаем её
            if (!scene.isLoaded)
            {
                Debug.Log($"Загрузка уровня: {sceneName}");
                AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                await loadOp.ToUniTask();
                Debug.Log($"Уровень {sceneName} загружен");
            }
            else
            {
                Debug.Log($"Сцена {sceneName} уже загружена, пропускаем загрузку.");
            }

            // Находим SpawnPoint и перемещаем игрока туда
            GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
            if (spawnPoint == null)
            {
                Debug.LogError("SpawnPoint не найден в сцене!");
                return;
            }
            MovePlayerToSpawnPoint(spawnPoint);

            // Находим компоненты, отвечающие за завершение уровня и за "падение в воду"
            LevelFinish finish   = GameObject.FindObjectOfType<LevelFinish>();
            LevelRestart restart = GameObject.FindObjectOfType<LevelRestart>();

            if (finish == null)
            {
                Debug.LogWarning("LevelFinish не найден в сцене!");
                return;
            }

            if (restart == null)
            {
                Debug.LogWarning("LevelRestart не найден в сцене!");
                return;
            }

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
                    // Событие перезапуска (игрок упал в воду)
                    Debug.Log("Игрок упал в воду! Перемещаем в SpawnPoint.");
                    MovePlayerToSpawnPoint(spawnPoint);
                    // После перемещения продолжаем ждать события завершения уровня
                }
                else
                {
                    // Событие завершения уровня
                    Debug.Log("Уровень завершён!");
                    levelCompleted = true;
                }
            }
            
            DOTween.KillAll();
            
            // После завершения уровня выгружаем сцену
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
            await unloadOp.ToUniTask();
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


        private async UniTask RestartCurrentLevel()
        {
            string sceneName = levelSceneNames[currentLevelIndex];
            Debug.Log($"Перезапуск уровня: {sceneName}");

            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
            await unloadOp.ToUniTask();

            AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            await loadOp.ToUniTask();

            //MovePlayerToSpawnPoint();
        }

        private void MovePlayerToSpawnPoint(GameObject spawnPoint)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Игрок не найден!");
                return;
            }

            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.transform.position = spawnPoint.transform.position;
            
            spawnPoint.SetActive(false);
            Debug.Log("Игрок перемещён в SpawnPoint");
        }


        public void Dispose() => disposable.Dispose();
    }
}
