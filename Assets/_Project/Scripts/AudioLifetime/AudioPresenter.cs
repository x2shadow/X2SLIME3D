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
            audioService.OnSoundPlayed
                .Subscribe( sound =>
                {
                    audioView.PlaySound(sound);
                })
                .AddTo(disposable);

            audioService.OnSoundJumpInPlayed
                .Subscribe( _ =>
                {
                    audioView.PlaySoundJumpIn();
                })
                .AddTo(disposable);
            
            audioService.OnSoundJumpInStoped
                .Subscribe( _ =>
                {
                    audioView.StopSoundJumpIn();
                })
                .AddTo(disposable);

            audioService.OnSoundCollisionPlayed
                .Subscribe( _ =>
                {
                    audioView.PlaySoundCollision();
                })
                .AddTo(disposable);
        }

        public void Dispose() => disposable.Dispose();
    }
}
