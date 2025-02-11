
using System;
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
        readonly InputReader inputReader;

        UIPresenter(UIView uiView, UIService uiService, InputReader inputReader)
        {
            this.uiView = uiView;
            this.uiService = uiService;
            this.inputReader = inputReader;
        }

        public void Start()
        {
            uiView.buttonHello.OnClickAsObservable().Subscribe(_ => uiService.Hello()).AddTo(disposable);
        }

        public void Dispose() => disposable.Dispose();
    }
}
