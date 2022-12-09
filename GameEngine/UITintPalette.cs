using Raylib_cs;

namespace GameEngine
{
    /// <summary>
    /// A batch of colors for different UI states.
    /// </summary>
    public struct UIColorPalette
    {
        /// <summary>
        /// Creates a new <see cref="UIColorPalette"/>.
        /// </summary>
        public UIColorPalette()
        {
            Regular = Color.WHITE;
            Hover = Color.WHITE;
            Click = Color.WHITE;
            Disabled = Color.WHITE;
        }

        /// <summary>
        /// Gets or sets the tint in the default state.
        /// </summary>
        public Color Regular { get; set; }

        /// <summary>
        /// Gets or sets the tint when the mouse is over it.
        /// </summary>
        public Color Hover { get; set; }

        /// <summary>
        /// Gets or sets the tint when it is clicked.
        /// </summary>
        public Color Click { get; set; }

        /// <summary>
        /// Gets or sets the tint when it is disabled.
        /// </summary>
        public Color Disabled { get; set; }
    }
}
