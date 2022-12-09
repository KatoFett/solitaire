using Raylib_cs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace GameEngine
{
    /// <summary>
    /// A 2D game utilizing the engine.
    /// </summary>
    public abstract partial class Scene : IDisposable
    {
        public Scene(string title, Vector2 size, Color background, int fps)
        {
            if (_ActiveScene != null)
                throw new InvalidOperationException("Cannot instantiate multiple scenes.");

            _ActiveScene = this;
            Size = size;
            VideoService = new VideoService(title, size, background, fps);
            VideoService.Initialize();
            AudioService.Initialize();
            AudioService.LoadSounds(Path.Combine(AssetsDirectory, "sounds"));
            FontService.LoadFonts(Path.Combine(AssetsDirectory, "fonts"));
            VideoService.LoadImages(Path.Combine(AssetsDirectory, "images"));
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Scene()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    AudioService.Release();
                    AudioService.UnloadSounds();
                    FontService.UnloadFonts();
                    VideoService.UnloadImages();
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

        public static Scene ActiveScene => _ActiveScene ?? throw new Exception("No active scene exists.");
        private static Scene? _ActiveScene;

        public static string AssetsDirectory { get; } = Path.Combine(Environment.CurrentDirectory, "assets");

        /// <summary>
        /// Gets a <see cref="Vector2"/> representing the size in pixels of the scene.
        /// </summary>
        public Vector2 Size { get; }

        /// <summary>
        /// Gets the time in seconds from the last frame draw.
        /// </summary>
        public static float DeltaTime => Raylib.GetFrameTime();

        protected VideoService VideoService { get; }

        /// <summary>
        /// Gets all GameObjects currently in the scene.
        /// </summary>
        internal List<GameObject> GameObjects { get; } = new();

        /// <summary>
        /// Gets all GameObjects that are marked for disposal.
        /// </summary>
        internal List<GameObject> DisposedGameObjects { get; } = new();

        /// <summary>
        /// Runs the game.
        /// </summary>
        public void Run()
        {
            while (VideoService.IsWindowOpen())
            {
                VideoService.BeginFrame();
                Update();
                VideoService.EndFrame();
            }
        }

        /// <summary>
        /// Processes and renders a frame to the screen.
        /// </summary>
        public virtual void Update()
        {
            var mouseCords = MouseService.GetMouseCoordinates();
            var mouseHit = GetObjectAtPosition(mouseCords);

            // Mouse interaction.
            if (mouseHit != null)
            {
                mouseHit.MouseOver();
                if (MouseService.IsButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    mouseHit.MouseDown();
                }
                if (MouseService.IsButtonReleased(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    mouseHit.MouseUp();
                }
            }

            // Draw objects.
            foreach (var gameObj in GameObjects.Where(g => g.IsVisible).OrderBy(g => g.ZIndex))
            {
                gameObj.Update();
                gameObj.Draw(VideoService);
            }

            // Dispose objects.
            foreach (var gameObj in DisposedGameObjects)
            {
                GameObjects.Remove(gameObj);
            }
        }

        public PhysicsBody? GetObjectAtPosition(Vector2 position, int maxZIndex = int.MaxValue)
        {
            PhysicsBody? physicsBody = null;
            foreach (PhysicsBody gameObj in GameObjects.Where(g => g is PhysicsBody && g.IsVisible && g.ZIndex <= maxZIndex).OrderBy(g => g.ZIndex))
            {
                if (Raylib.CheckCollisionPointRec(position, gameObj.ToRectangle()))
                {
                    physicsBody = gameObj;
                }
            }
            return physicsBody;
        }
    }
}
