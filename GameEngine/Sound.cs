using System;

namespace GameEngine
{
    /// <summary>
    /// A sound.
    /// </summary>
    public class Sound
    {
        /// <summary>
        /// Creates a new <see cref="Sound"/>.
        /// </summary>
        /// <param name="filename">The sound's file location.</param>
        /// <param name="volume">The sound's volume from 0 (mute) to 1 (max).</param>
        /// <param name="isRepeating">Whether the sound should repeat.</param>
        public Sound(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            Filename = filename;
        }

        /// <summary>
        /// Gets or sets the sound's filename.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the sound's volume (0-1).
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public float Volume
        {
            get => _Volume;
            set
            {
                if (value < 0f || value > 1f)
                    throw new ArgumentOutOfRangeException(nameof(value), "Volume must be between 0 and 1.");
                _Volume = value;
            }
        }
        private float _Volume = 1.0f;

        /// <summary>
        /// Gets or sets the sound's pan.
        /// </summary>
        /// <remarks>
        /// 0 is full left; 0.5 is center; 1 is full right.
        /// </remarks>
        public float Pan
        {
            get => _Pan;
            set
            {
                if (value < 0f || value > 1f)
                    throw new ArgumentOutOfRangeException(nameof(value), "Pan must be between 0 and 1.");
                _Pan = value;
            }
        }
        private float _Pan = 0.5f;

        /// <summary>
        /// Gets or sets the sound's pitch.
        /// </summary>
        /// <remarks>
        /// 0 is full left; 0.5 is center; 1.0 is full right.
        /// </remarks>
        public float Pitch
        {
            get => _Pitch;
            set
            {
                if (value < 0f)
                    throw new ArgumentOutOfRangeException(nameof(value), "Pitch cannot be negative.");
                _Pitch = value;
            }
        }
        private float _Pitch = 1.0f;
    }
}
