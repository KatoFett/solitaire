using System;
using System.Numerics;

namespace GameEngine
{
    /// <summary>
    /// An object which interacts with the game engine.
    /// </summary>
    public abstract class GameObject : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Creates a new instance of a <see cref="GameObject"/>.
        /// </summary>
        public GameObject()
        {
            Scene.ActiveScene.GameObjects.Add(this);
        }

        ~GameObject()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Scene.ActiveScene.DisposedGameObjects.Add(this);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Gets or sets the body's position.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the z-index for draw.  The default is 0.
        /// </summary>
        /// <remarks>
        /// Lower values go to background and higher values come to foreground.
        /// </remarks>
        public int ZIndex { get; set; } = 0;

        /// <summary>
        /// Gets or sets whether the element should be drawn on the screen.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Called once per frame.
        /// </summary>
        /// <remarks>
        /// base.Update() does nothing.
        /// </remarks>
        protected internal virtual void Update() { }

        /// <summary>
        /// Draws this <see cref="GameObject"/> onto the screen.
        /// </summary>
        /// <param name="videoService"></param>
        protected internal abstract void Draw(VideoService videoService);

        /// <summary>
        /// Moves the <see cref="GameObject"/>.
        /// </summary>
        /// <param name="translation">A <see cref="Vector2"/> containing the X- and Y-coordinates of the translation in pixels.</param>
        public void Translate(Vector2 translation)
        {
            Position += translation;
        }

        /// <summary>
        /// Moves the <see cref="GameObject"/>.
        /// </summary>
        /// <param name="x">The amount in pixels to move horizontally.</param>
        /// <param name="y">The amount in pixels to move vertically.</param>
        public void Translate(float x, float y)
        {
            Translate(new Vector2(x, y));
        }
    }
}
