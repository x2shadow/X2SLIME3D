
using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace X2SLIME3D
{
    public class UIPresenter : IStartable, IDisposable
    {
        readonly CompositeDisposable disposable = new CompositeDisposable();

        readonly UIView uiView;
        readonly UIService uiService;

        UIPresenter(UIView uiView, UIService uiService)
        {
            this.uiView = uiView;
            this.uiService = uiService;
        }

        public void Start()
        {
            uiView.buttonHello.OnClickAsObservable()
                .Subscribe( _ => 
                {
                    uiService.Hello();
                })
                .AddTo(disposable);
            
            uiView.buttonRestart.OnClickAsObservable()
                .Subscribe( _ => 
                {
                    uiService.Restart();
                })
                .AddTo(disposable);
            
            uiService.OnLevelUpdated
                .Subscribe( number =>
                { 
                    uiView.UpdateLevelNumber(number);
                })
                .AddTo(disposable);
        }

        public void Dispose() => disposable.Dispose();
    }
}
