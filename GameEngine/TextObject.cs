using System;
using Raylib_cs;
using System.Numerics;

namespace GameEngine
{
    /// <summary>
    /// An object that displays text on the screen.
    /// </summary>
    public class TextObject : GameObject
    {
        /// <summary>
        /// Creates a new instance of an empty <see cref="TextObject"/>.
        /// </summary>
        public TextObject()
        {
            if (DefaultFont == string.Empty)
                throw new Exception($"The {nameof(DefaultFont)} property must be set to a valid font name.");
        }

        /// <summary>
        /// Creates a new instance of a <see cref="TextObject"/> with the specified <paramref name="text"/>.
        /// </summary>
        /// <param name="text">The text to initially display.</param>
        public TextObject(string? text) : this()
        {
            Text = text;
        }

        #region GameObject

        protected internal override void Draw(VideoService videoService)
        {
            videoService.DrawText(this);
        }

        #endregion

        public enum Alignment
        {
            Left,
            Center,
            Right,
        }

        public enum Justification
        {
            Top,
            Center,
            Bottom,
        }

        /// <summary>
        /// Gets or sets the default font for all TextObjects.
        /// </summary>
        public static string DefaultFont { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text of this <see cref="TextObject"/>.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets the name of the font of this <see cref="TextObject"/>.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="DefaultFont"/>.
        /// </remarks>
        public string FontName { get => _FontName ?? DefaultFont; set => _FontName = value; }
        private string? _FontName;

        /// <summary>
        /// Gets or sets the color of this <see cref="TextObject"/>.
        /// </summary>
        public Color FontColor { get; set; } = Color.BLACK;

        /// <summary>
        /// Gets or sets the font size of this <see cref="TextObject"/>.
        /// </summary>
        public int FontSize { get; set; } = 12;

        /// <summary>
        /// Gets or sets the font spacing of this <see cref="TextObject"/>.
        /// </summary>
        public float FontSpacing { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the horizontal alignment of this <see cref="TextObject"/>.
        /// </summary>
        public Alignment HorizontalAlignment { get; set; } = Alignment.Left;

        /// <summary>
        /// Gets or sets the vertical alignment of this <see cref="TextObject"/>.
        /// </summary>
        public Justification VerticalAlignment { get; set; } = Justification.Bottom;

        /// <summary>
        /// Gets the <see cref="Font"/> associated with this <see cref="TextObject"/>.
        /// </summary>
        public Font Font => FontService.GetFont(FontName);

        /// <summary>
        /// Gets the offset or anchor position of this <see cref="TextObject"/> based on the <see cref="TextAlignment"/>.
        /// </summary>
        public Vector2 GetOffset()
        {
            var measure = Raylib.MeasureTextEx(Font, Text, FontSize, FontSpacing);

            var result = new Vector2
            (
                HorizontalAlignment switch
                {
                    Alignment.Center => measure.X / 2,
                    Alignment.Right => measure.X,
                    _ => 0
                },
                VerticalAlignment switch
                {
                    Justification.Center => measure.Y / 2,
                    Justification.Bottom => measure.Y,
                    _ => 0
                }
            );
            return result;
        }
    }
}
