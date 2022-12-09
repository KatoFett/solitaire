using System.Collections.Generic;
using System.IO;
using Raylib_cs;

namespace GameEngine
{
    /// <summary>
    /// Service for playing audio.
    /// </summary>
    public static class AudioService
    {
        private static readonly Dictionary<string, Raylib_cs.Sound> _Sounds = new();

        /// <summary>
        /// Initializes the audio service.
        /// </summary>
        public static void Initialize()
        {
            Raylib.InitAudioDevice();
        }

        /// <summary>
        /// Loads all sounds in a directory into memory.
        /// </summary>
        /// <param name="directory">The directory to search in.</param>
        public static void LoadSounds(string directory)
        {
            string[] filters = new string[] { "*.wav", "*.mp3", "*.acc", "*.wma" };
            List<string> filepaths = FileService.GetAllFilePaths(directory, filters);
            foreach (string filepath in filepaths)
            {
                var sound = Raylib.LoadSound(filepath);
                var filename = Path.GetFileNameWithoutExtension(filepath);
                _Sounds[filename] = sound;
            }
        }

        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="sound">The sound to play.</param>
        public static void PlaySound(Sound sound)
        {
            if (_Sounds.ContainsKey(sound.Filename))
            {
                var raylibSound = _Sounds[sound.Filename];
                Raylib.SetSoundVolume(raylibSound, sound.Volume);
                Raylib.SetSoundPan(raylibSound, sound.Pan);
                Raylib.SetSoundPitch(raylibSound, sound.Pitch);
                Raylib.PlaySound(raylibSound);
            }
        }

        /// <summary>
        /// Closes the audio device.
        /// </summary>
        public static void Release()
        {
            Raylib.CloseAudioDevice();
        }

        /// <summary>
        /// Unloads all sounds from memory
        /// </summary>
        public static void UnloadSounds()
        {
            foreach (string filepath in _Sounds.Keys)
            {
                var raylibSound = _Sounds[filepath];
                Raylib.UnloadSound(raylibSound);
            }
        }
    }
}