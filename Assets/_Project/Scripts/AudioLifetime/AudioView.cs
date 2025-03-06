using UnityEngine;
using UnityEngine.Audio;

namespace X2SLIME3D
{
    public class AudioView : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public AudioClip music;
        public AudioClip soundCollision1;
        public AudioClip soundCollision2;
        public AudioClip soundJump;
        public AudioClip soundJumpIn;
        public AudioClip soundSplash;
        public AudioClip soundWin1;
        public AudioClip soundWin2;
        public AudioClip soundWin3;
        public AudioClip soundWin4;
        public AudioClip soundWin5;

        [HideInInspector] public AudioSource musicSource;
        [HideInInspector] public AudioSource soundSource;

        void Awake()
        {
            // Создаём и настраиваем аудиоисточники
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.playOnAwake = true;
            musicSource.volume = 1f;
            musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
            //musicSource.Play();

            soundSource = gameObject.AddComponent<AudioSource>();
            soundSource.playOnAwake = false;
            soundSource.volume = 1f;
            soundSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sound")[0];
        }

        public void PlaySound(AudioClip sound) => soundSource.PlayOneShot(sound);

        public void PlaySoundJumpIn()
        {
            soundSource.clip = soundJumpIn;
            if (soundSource.clip == soundJumpIn && !soundSource.isPlaying)
            soundSource.Play();
        }

        public void StopSoundJumpIn()
        {
            if (soundSource.clip == soundJumpIn && soundSource.isPlaying) soundSource.Stop();
        }
    }
}