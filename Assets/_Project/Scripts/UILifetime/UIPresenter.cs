
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
        readonly AudioService audioService;

        UIPresenter(UIView uiView, UIService uiService, AudioService audioService)
        {
            this.uiView = uiView;
            this.uiService = uiService;
            this.audioService = audioService;
        }

        public void Start()
        {
            uiView.buttonMusic.OnClickAsObservable()
                .Subscribe( _ => 
                {
                    audioService.ToggleMusic();
                })
                .AddTo(disposable);

            uiView.buttonSound.OnClickAsObservable()
                .Subscribe( _ => 
                {
                    audioService.ToggleSound();
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
