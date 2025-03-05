using System;
using R3;
using VContainer.Unity;

namespace X2SLIME3D
{
    public class AudioPresenter : IStartable, IDisposable
    {
        readonly CompositeDisposable disposable = new CompositeDisposable();
        
        readonly AudioView audioView;
        readonly AudioService audioService;

        public AudioPresenter(AudioView audioView, AudioService audioService)
        {
            this.audioView = audioView;
            this.audioService = audioService;
        }

        public void Start()
        {
            audioService.OnSoundJumpPlayed
                .Subscribe( _ =>
                {
                    audioView.PlayJumpSound();
                })
                .AddTo(disposable);
        }

        public void Dispose() => disposable.Dispose();
    }
}
