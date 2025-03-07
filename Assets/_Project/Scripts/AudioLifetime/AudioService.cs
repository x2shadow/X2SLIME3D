using UnityEngine;
using UnityEngine.Audio;
using R3;
using System.Collections.Generic;

namespace X2SLIME3D
{
    public enum SoundType
    {
        Collision1,
        Collision2,
        Jump,
        JumpIn,
        Splash,
        Win1,
        Win2,
        Win3,
        Win4,
        Win5
    }

    public class AudioService
    {
        readonly AudioMixer audioMixer;
        readonly Dictionary<SoundType, AudioClip> soundClips;

        const string MusicVolumeKey = "MusicVolume";
        const string SoundVolumeKey = "SoundVolume";
        const string CollisionSoundVolumeKey = "CollisionSoundVolume";


        // Reactive-свойства для громкости
        public ReactiveProperty<float> MusicVolume { get; private set; }
        public ReactiveProperty<float> SoundVolume { get; private set; }

        public Subject<AudioClip> OnSoundPlayed  = new Subject<AudioClip>();
        public Subject<Unit> OnSoundCollisionPlayed = new Subject<Unit>();
        public Subject<Unit> OnSoundJumpInPlayed = new Subject<Unit>();
        public Subject<Unit> OnSoundJumpInStoped = new Subject<Unit>();

        public AudioService(AudioView audioView)
        {
            audioMixer = audioView.audioMixer;

            soundClips = new Dictionary<SoundType, AudioClip>()
            {
                { SoundType.Collision1, audioView.soundCollision1 },
                { SoundType.Collision2, audioView.soundCollision2 },
                { SoundType.Jump,       audioView.soundJump },
                { SoundType.JumpIn,     audioView.soundJumpIn },
                { SoundType.Splash,     audioView.soundSplash },
                { SoundType.Win1,       audioView.soundWin1 },
                { SoundType.Win2,       audioView.soundWin2 },
                { SoundType.Win3,       audioView.soundWin3 },
                { SoundType.Win4,       audioView.soundWin4 },
                { SoundType.Win5,       audioView.soundWin5 }
            };

            // Инициализация реактивных свойств из PlayerPrefs
            MusicVolume = new ReactiveProperty<float>(PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f));
            SoundVolume = new ReactiveProperty<float>(PlayerPrefs.GetFloat(SoundVolumeKey, 0.5f));

            // Подписка на изменения громкости музыки
            MusicVolume.Subscribe(volume =>
            {
                float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
                audioMixer.SetFloat("MusicVolume", dB);
                PlayerPrefs.SetFloat(MusicVolumeKey, volume);
                PlayerPrefs.Save();
            });

            // Подписка на изменения громкости звуков
            SoundVolume.Subscribe(volume =>
            {
                float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
                audioMixer.SetFloat("SoundVolume", dB);
                PlayerPrefs.SetFloat(SoundVolumeKey, volume);
                PlayerPrefs.Save();
            });
        }

        public void SetMusicVolume(float volume)
        {
            float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
            audioMixer.SetFloat("MusicVolume", dB);

            PlayerPrefs.SetFloat(MusicVolumeKey, volume); 
            PlayerPrefs.Save(); 
        }

        public void SetSoundVolume(float volume)
        {
            float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
            audioMixer.SetFloat("SoundVolume", dB);
            audioMixer.SetFloat("CollisionSoundVolume", dB);


            PlayerPrefs.SetFloat(SoundVolumeKey, volume); 
            PlayerPrefs.SetFloat(CollisionSoundVolumeKey, volume); 
            PlayerPrefs.Save(); 
        }

        public void PlaySound(SoundType type) => OnSoundPlayed.OnNext(soundClips[type]);
        
        public void PlaySoundJumpIn() => OnSoundJumpInPlayed.OnNext(Unit.Default);
        public void StopSoundJumpIn() => OnSoundJumpInStoped.OnNext(Unit.Default);

        public void PlaySoundCollision() => OnSoundCollisionPlayed.OnNext(Unit.Default);
    }
}
