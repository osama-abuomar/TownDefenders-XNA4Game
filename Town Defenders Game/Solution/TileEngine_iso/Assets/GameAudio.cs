using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Engine.Assets
{
    public class GameAudio
    {
        public static bool AudioLoaded = false;
        static private Dictionary<string, Song> _songs = new Dictionary<string, Song>();
        static private Dictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();

        public static Song GetSong(string identifier)
        {
            if (_songs.ContainsKey(identifier))
                return _songs[identifier];
            throw new Exception("Wrong Song Identifier");
        }

        public static SoundEffect GetSoundEffect(string identifier)
        {
            if (_soundEffects.ContainsKey(identifier))
                return _soundEffects[identifier];
            throw new Exception("Wrong SoundEffect Identifier");
        }

        public static void Load(ContentManager content)
        {
            if (AudioLoaded) return;

            _songs["intro"] = content.Load<Song>(@"Sounds\Music\Intro");
            _songs["gameplay"] = content.Load<Song>(@"Sounds\Music\GamePlay");

            _soundEffects["focus"] = content.Load<SoundEffect>(@"Sounds\SoundEffects\focus");
            _soundEffects["select"] = content.Load<SoundEffect>(@"Sounds\SoundEffects\select");
            _soundEffects["horse1"] = content.Load<SoundEffect>(@"Sounds\SoundEffects\horse1");
            _soundEffects["horse2"] = content.Load<SoundEffect>(@"Sounds\SoundEffects\horse2");
            _soundEffects["horse_running"] = content.Load<SoundEffect>(@"Sounds\SoundEffects\horse_running");








            AudioLoaded = true;
        }
    }
}
