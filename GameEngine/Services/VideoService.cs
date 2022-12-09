using GameEngine;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace GameEngine
{
#pragma warning disable CA1822 // Mark members as static
    public class VideoService
    {
        private Color _Background;
        private readonly Vector2 _Size;
        private readonly string _Title;
        private static readonly Dictionary<string, Texture2D> _Textures = new();
        private readonly int _FPS;

        public VideoService(string title, Vector2 size, Color backgroundColor, int fps)
        {
            _Title = title;
            _Size = size;
            _Background = backgroundColor;
            _FPS = fps;
        }

        #region Window Management

        /// <summary>
        /// Initializes the window.
        /// </summary>
        public void Initialize()
        {
            Raylib.InitWindow(_Size.X.ToInt(), _Size.Y.ToInt(), _Title);
            Raylib.SetTargetFPS(_FPS);
        }

        /// <summary>
        /// Gets whether the window is open.
        /// </summary>
        public bool IsWindowOpen()
        {
            return !Raylib.WindowShouldClose();
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void Close()
        {
            Raylib.CloseWindow();
        }

        #endregion

        #region Frame Management

        /// <summary>
        /// Clears the window and begins drawing a new frame.
        /// </summary>
        public void BeginFrame()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(_Background);
        }

        /// <summary>
        /// Ends the frame drawing process.
        /// </summary>
        public void EndFrame()
        {
            Raylib.EndDrawing();
        }

        #endregion

        #region Images

        /// <summary>
        /// Loads all images in the given <paramref name="directory"/> into memory.
        /// </summary>
        /// <param name="directory">The directory in which the images are stored.</param>
        public void LoadImages(string directory)
        {
            string[] filters = new string[] { "*.png", "*.gif", "*.jpg", "*.jpeg", "*.bmp" };
            List<string> filepaths = FileService.GetAllFilePaths(directory, filters);
            foreach (string filepath in filepaths)
            {
                var texture = Raylib.LoadTexture(filepath);
                var filename = Path.GetFileNameWithoutExtension(filepath);
                _Textures[filename] = texture;
            }
        }

        /// <summary>
        /// Unloads all images from memory.
        /// </summary>
        public void UnloadImages()
        {
            foreach (string key in _Textures.Keys)
            {
                var texture = _Textures[key];
                Raylib.UnloadTexture(texture);
            }
        }

        /// <summary>
        /// Gets a <see cref="Texture2D"/> with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the texture.</param>
        /// <returns>The <see cref="Texture2D"/> found.</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public static Texture2D GetTexture(string name)
        {
            if (!_Textures.ContainsKey(name))
                throw new KeyNotFoundException($"Could not locate a texture with the name '{name}'.");
            return _Textures[name];
        }

        /// <summary>
        /// Draws an image on the screen
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="position">The position of the image's top-left corner.</param>
        public void DrawImage(Sprite sprite)
        {
            if (!_Textures.ContainsKey(sprite.TextureName))
                throw new KeyNotFoundException($"Unable to find a texture with the name '{sprite.TextureName}'.");

            var texture = _Textures[sprite.TextureName];
            Raylib.DrawTextureEx(texture, sprite.Position, 0f, 1f, sprite.Tint);
        }

        /// <summary>
        /// Draws a rectangle on the screen.
        /// </summary>
        /// <param name="size">The size of the rectangle.</param>
        /// <param name="position">The position of the rectangle's top-left corner.</param>
        /// <param name="color">The color of the rectangle.</param>
        /// <param name="filled">Whether the rectangle is filled or hollow.</param>
        public void DrawRectangle(Vector2 size, Vector2 position, Color color, bool filled = false)
        {
            if (filled)
                Raylib.DrawRectangle(position.X.ToInt(), position.Y.ToInt(), size.X.ToInt(), size.Y.ToInt(), color);
            else
                Raylib.DrawRectangleLines(position.X.ToInt(), position.Y.ToInt(), size.X.ToInt(), size.Y.ToInt(), color);
        }

        /// <summary>
        /// Draws a circle on the screen.
        /// </summary>
        /// <param name="position">The position of the circle's center.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color of the circlle.</param>
        /// <param name="filled">Whether the circle is filled or hollow.</param>
        public void DrawCircle(Vector2 position, float radius, Color color, bool filled = false)
        {
            if (filled)
                Raylib.DrawCircle(position.X.ToInt(), position.Y.ToInt(), radius, color);
            else
                Raylib.DrawCircleLines(position.X.ToInt(), position.Y.ToInt(), radius, color);
        }

        #endregion

        #region Text

        /// <summary>
        /// Draws text on the screen.
        /// </summary>
        /// <param name="text">The text object to be drawn.</param>
        /// <param name="position">The screen position to draw the text at.</param>
        public void DrawText(TextObject text)
        {
            var offset = text.GetOffset();
            Vector2 vector = new(text.Position.X - offset.X, text.Position.Y - offset.Y);
            Raylib.DrawTextEx(text.Font, text.Text, vector, text.FontSize, text.FontSpacing, text.FontColor);
        }

        #endregion
    }
#pragma warning restore CA1822 // Mark members as static
}