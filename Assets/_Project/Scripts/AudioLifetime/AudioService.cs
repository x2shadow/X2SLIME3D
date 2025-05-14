using UnityEngine;
using UnityEngine.Audio;
using R3;
using System.Collections.Generic;
using GamePush;

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
        const string CollisionSoundVolumeKey = "CollisionVolume";

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
        }

        bool adMutedMusic = false;
        bool adMutedSound = false;

        public async void Initialize()
        {
            await GP_Init.Ready;

            bool musicMuted = GP_Player.GetBool("music_muted");
            bool soundMuted = GP_Player.GetBool("sound_muted");

            if (musicMuted) SetMusicVolume(0);
            if (soundMuted) SetSoundVolume(0);
        }

        public void SetMusicVolume(float volume)
        {
            float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
            audioMixer.SetFloat(MusicVolumeKey, dB);

            PlayerPrefs.SetFloat(MusicVolumeKey, volume); 
            PlayerPrefs.Save(); 
        }

        public void SetSoundVolume(float volume)
        {
            float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
            audioMixer.SetFloat(SoundVolumeKey, dB);
            audioMixer.SetFloat(CollisionSoundVolumeKey, dB);


            PlayerPrefs.SetFloat(SoundVolumeKey, volume); 
            PlayerPrefs.SetFloat(CollisionSoundVolumeKey, volume); 
            PlayerPrefs.Save(); 
        }

        public void ToggleMusic()
        {
            float dB;
            audioMixer.GetFloat(MusicVolumeKey, out dB);
            float currentVolume = Mathf.Pow(10, dB / 20);

            if (dB > -80f)
            {
                PlayerPrefs.SetFloat("LastMusicVolume", currentVolume); 
                PlayerPrefs.Save();

                GP_Player.Set("music_muted", true);
                GP_Player.Sync();

                SetMusicVolume(0);
            }
            else
            {
                float lastVolume = PlayerPrefs.GetFloat("LastMusicVolume", 0.5f);

                GP_Player.Set("music_muted", false);
                GP_Player.Sync();

                SetMusicVolume(lastVolume);
            }
        }

        public void ToggleSound()
        {
            float dB;
            audioMixer.GetFloat(SoundVolumeKey, out dB);
            float currentVolume = Mathf.Pow(10, dB / 20);

            if (dB > -80f)
            {
                PlayerPrefs.SetFloat("LastSoundVolume", currentVolume); 
                PlayerPrefs.Save();

                GP_Player.Set("sound_muted", true);
                GP_Player.Sync();
                
                SetSoundVolume(0);
            }
            else
            {
                float lastVolume = PlayerPrefs.GetFloat("LastSoundVolume", 0.5f);

                GP_Player.Set("sound_muted", false);
                GP_Player.Sync();

                SetSoundVolume(lastVolume);
            }
        }

        public void MuteForAd()
        {
            float musicDB;
            audioMixer.GetFloat(MusicVolumeKey, out musicDB);
            
            if (musicDB > -80f)
            {
                float currentmusicVolume = Mathf.Pow(10, musicDB / 20);
                PlayerPrefs.SetFloat("LastMusicVolume", currentmusicVolume); 
                PlayerPrefs.Save();
                adMutedMusic = true;
                SetMusicVolume(0);
            }

            float soundDB;
            audioMixer.GetFloat(SoundVolumeKey, out soundDB);
            
            if (soundDB > -80f)
            {
                float currentSoundVolume = Mathf.Pow(10, soundDB / 20);
                PlayerPrefs.SetFloat("LastSoundVolume", currentSoundVolume); 
                PlayerPrefs.Save();
                adMutedSound = true;
                SetSoundVolume(0);
            }
        }

        public void UnmuteAfterAd()
        {
            if (adMutedMusic)
            {
                float lastMusicVolume = PlayerPrefs.GetFloat("LastMusicVolume", 0.5f);
                SetMusicVolume(lastMusicVolume);
                adMutedMusic = false;
            }

            if (adMutedSound)
            {
                float lastSoundVolume = PlayerPrefs.GetFloat("LastSoundVolume", 0.5f);
                SetSoundVolume(lastSoundVolume);
                adMutedSound = false;
            }
        }

        public void PlaySound(SoundType type) => OnSoundPlayed.OnNext(soundClips[type]);
        
        public void PlaySoundJumpIn() => OnSoundJumpInPlayed.OnNext(Unit.Default);
        public void StopSoundJumpIn() => OnSoundJumpInStoped.OnNext(Unit.Default);

        public void PlaySoundCollision() => OnSoundCollisionPlayed.OnNext(Unit.Default);
    }
}
