using UnityEngine;
using UnityEngine.Audio;
using R3;

namespace X2SLIME3D
{
    public class AudioService
    {
        readonly AudioMixer audioMixer;
        readonly AudioSource soundSource;

        const string MusicVolumeKey = "MusicVolume";
        const string SoundVolumeKey = "SoundVolume";

        // Reactive-свойства для громкости
        public ReactiveProperty<float> MusicVolume { get; private set; }
        public ReactiveProperty<float> SoundVolume { get; private set; }

      public Subject<Unit> OnSoundJumpPlayed = new Subject<Unit>();

        public AudioService(AudioView audioView)
        {
            audioMixer = audioView.audioMixer;
            soundSource = audioView.soundSource;

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

            PlayerPrefs.SetFloat(SoundVolumeKey, volume); 
            PlayerPrefs.Save(); 
        }

        public void PlayJumpSound()
        {
            OnSoundJumpPlayed.OnNext(Unit.Default);
        }

        public void PlaySound(AudioClip sound)
        {
            soundSource.PlayOneShot(sound);
        }
    }
}
