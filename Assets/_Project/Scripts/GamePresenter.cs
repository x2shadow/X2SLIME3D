using System;
using System.ComponentModel.Design;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        }

        private async UniTask LoadAndPlayCurrentLevel()
        {
            string sceneName = levelSceneNames[currentLevelIndex];
            Debug.Log($"Загрузка уровня: {sceneName}");

            // Загружаем сцену аддитивно
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            await loadOp.ToUniTask();
            Debug.Log($"Уровень {sceneName} загружен");

            // Находим компонент LevelFinish в загруженной сцене
            LevelFinish finish = GameObject.FindObjectOfType<LevelFinish>();
            if (finish == null)
            {
                Debug.LogWarning("LevelFinish не найден в сцене!");
                return;
            }

            // Создаем UniTaskCompletionSource для ожидания первого события завершения уровня
            var tcs = new UniTaskCompletionSource();

            // Подписываемся на реактивное событие OnLevelFinished
            // Когда срабатывает событие, вызывается tcs.TrySetResult(), что разблокирует await
            var subscription = finish.OnLevelFinished.Subscribe(_ => tcs.TrySetResult());

            // Ожидаем, пока не придет первое уведомление о завершении уровня
            await tcs.Task;

            // Отписываемся от события, чтобы не было утечек памяти
            subscription.Dispose();

            Debug.Log("Уровень завершён, выгружаем сцену...");

            // Выгружаем текущую сцену
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
            await unloadOp.ToUniTask();
        }

        public void Dispose() => disposable.Dispose();
    }
}
