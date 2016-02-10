using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Assets;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Engine.Utilities
{
    // Single Tone Class
    public class SoundManager
    {
        private float _musicVolume = 1.0f;
        private float _soundsVolume = 1.0f;

        private bool _enableMusic = true;
        private bool _enableSounds = true;

        private static SoundManager _instance;

        



        public void SetMusicVolume(int percent)
        {
            float newVolume = (percent/100f)*1f;
            _musicVolume = newVolume;
            MediaPlayer.Volume = newVolume;
        }

        public void SetSoundsVolume(int percent)
        {
            float newVolume = (percent / 100f) * 1f;
            _soundsVolume = newVolume;
        }

        public void EnableMusic()
        {
            if (!_enableMusic)
            {
                _enableMusic = true;
                PlaySong("gameplay");
            }
        }

        public void DisableMusic()
        {
            if (_enableMusic)
            {
                _enableMusic = false;
                MediaPlayer.Stop();
            }
        }

        public void EnableSounds()
        {
            _enableSounds = true;
        }

        public void DisableSounds()
        {
            _enableSounds = false;
        }

        protected SoundManager()
        {
        }

        public static SoundManager Instance()
        {
            if (_instance == null)
                _instance = new SoundManager();
            return _instance;

        }

       
        public void PlaySong(string identifier)
        {
            if (!_enableMusic)
            {
                return;
            }
            Song song = GameAudio.GetSong(identifier);
            if (MediaPlayer.GameHasControl)
            {
                MediaPlayer.Volume = _musicVolume;
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = true;
            }
        }

        public void PlaySoundEffect(string identifier)
        {
            if (!_enableSounds)
            {
                return;
            }
            SoundEffect soundEffect = GameAudio.GetSoundEffect(identifier);
            if (MediaPlayer.GameHasControl)
            {
                SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();
                soundEffectInstance.Volume = _soundsVolume;
                soundEffectInstance.Play();
                MediaPlayer.IsRepeating = true;
            }
        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
        }

    }
}
