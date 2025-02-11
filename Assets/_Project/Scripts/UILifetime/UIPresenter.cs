
using System;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace X2SLIME3D
{
    public class UIPresenter : IStartable, IDisposable
    {
        readonly CompositeDisposable disposable = new CompositeDisposable();

        readonly UIService uiService;
        readonly UIView uiView;

        UIPresenter(UIView uiView, UIService uiService)
        {
            this.uiView = uiView;
            this.uiService = uiService;
        }

        public void Start()
        {
            uiView.buttonHello.OnClickAsObservable().Subscribe(_ => uiService.Hello()).AddTo(disposable);
        }

        public void Dispose() => disposable.Dispose();
    }
}
