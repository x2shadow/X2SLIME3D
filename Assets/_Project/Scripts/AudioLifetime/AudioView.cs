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
        [HideInInspector] public AudioSource collisionSoundSource;


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

            collisionSoundSource = gameObject.AddComponent<AudioSource>();
            collisionSoundSource.playOnAwake = false;
            collisionSoundSource.volume = 1f;
            collisionSoundSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("CollisionSound")[0];
        }

        public void PlaySound(AudioClip sound) => soundSource.PlayOneShot(sound);
        
        public void PlaySoundCollision()
        {
            collisionSoundSource.pitch = Random.Range(0.85f, 1.15f); 
            AudioClip randomClip = Random.Range(0, 2) == 0 ? soundCollision1 : soundCollision2;
            collisionSoundSource.PlayOneShot(randomClip);
        }

        public void PlaySoundJumpIn()
        {
            soundSource.clip = soundJumpIn;
            soundSource.Play();
        }

        public void StopSoundJumpIn()
        {
            if (soundSource.clip == soundJumpIn && soundSource.isPlaying) soundSource.Stop();
        }
    }
}