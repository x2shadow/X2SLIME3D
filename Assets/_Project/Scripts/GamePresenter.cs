using System;
using System.ComponentModel.Design;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using R3;

namespace X2SLIME3D
{
    public class GamePresenter : IAsyncStartable, IDisposable
    {
        readonly CompositeDisposable disposable = new CompositeDisposable();

        readonly HelloWorldService helloWorldService;
        readonly HelloScreen helloScreen;

        public GamePresenter(HelloWorldService helloWorldService, HelloScreen helloScreen)
        {
            this.helloWorldService = helloWorldService;
            this.helloScreen = helloScreen;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.Yield();  // Просто заглушка, чтобы убрать warning об отсутсвии async методов (TODO: добавить async методы или убрать асинхронность)
            //await UniTask.WaitForSeconds(0.1f, cancellationToken: cancellation); // Просто задержка в 0.1 секунду
            
            helloScreen.buttonHello.OnClickAsObservable().Subscribe(_ => helloWorldService.Hello()).AddTo(disposable);
        }

        public void Dispose() => disposable.Dispose();
    }
}
