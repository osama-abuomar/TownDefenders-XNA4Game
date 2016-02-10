using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Utilities
{
    // Single Tone Class
    public class SoundManager
    {
        private static SoundManager _instance;

        //TODO: add new sounds here..
        public enum GameSongs { BGMusic1 }
        public enum GameSoundEffects { }

        IDictionary<GameSongs, Song> gameSongs;
        IDictionary<GameSoundEffects, SoundEffect> gameSoundEffects;

        private static ContentManager Content;


        protected SoundManager()
        {
            gameSongs = new Dictionary<GameSongs, Song>();
            gameSoundEffects = new Dictionary<GameSoundEffects, SoundEffect>();
        }

        public static SoundManager Instance()
        {
            if (_instance == null)
                _instance = new SoundManager();
            return _instance;

        }


        public void LoadContent(ContentManager content)
        {
            Content = content;
            gameSongs[GameSongs.BGMusic1] = Content.Load<Song>(@"sounds/Music/bg_music_1");

        }

        public void PlaySong(GameSongs aSong)
        {
            if (MediaPlayer.GameHasControl)
                MediaPlayer.Play(gameSongs[aSong]);
            MediaPlayer.IsRepeating = true;
        }

        public void PlaySoundEffect(GameSoundEffects aSoundEffect)
        {
            if (MediaPlayer.GameHasControl)
                gameSoundEffects[aSoundEffect].Play();
            MediaPlayer.IsRepeating = true;
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}
