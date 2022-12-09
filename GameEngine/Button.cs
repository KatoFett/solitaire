using Raylib_cs;
using System;

namespace GameEngine
{
    /// <summary>
    /// A clickable UI button.
    /// </summary>
    public class Button : Sprite
    {
        /// <summary>
        /// Creates a new <see cref="Button"/>.
        /// </summary>
        /// <param name="texture">The button's texture.</param>
        /// <param name="click">The action performed on click.</param>
        public Button(string texture, Action click) : base(texture)
        {
            Click = click;
        }

        /// <summary>
        /// Gets or sets the text displayed on the button.
        /// </summary>
        public TextObject? Text { get; set; }

        /// <summary>
        /// Gets or sets the button's tint palette.
        /// </summary>
        public UIColorPalette ColorPalette { get; set; }

        /// <summary>
        /// Gets or sets the text's tint palette.
        /// </summary>
        public UIColorPalette? TextColorPalette { get; set; }

        /// <summary>
        /// Gets or sets whether the button is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get => _IsEnabled;
            set
            {
                _IsEnabled = value;
                if (!value)
                {
                    Tint = ColorPalette.Disabled;
                    TrySetTextTint(TextColorPalette?.Disabled);
                }
            }
        }
        private bool _IsEnabled = true;

        /// <summary>
        /// Gets or sets the action that runs when the button is clicked.
        /// </summary>
        public Action Click { get; set; }

        private bool _IsMouseOver;
        private bool _IsMousePressedOnButton;
        private bool _IsMouseReleased;

        protected internal override void Update()
        {
            if (Text != null)
            {
                Text.Position = GetCenter();
            }

            if (!IsEnabled) return;

            _IsMouseOver = Raylib.CheckCollisionPointRec(MouseService.GetMouseCoordinates(), ToRectangle());
            _IsMouseReleased = MouseService.IsButtonReleased(MouseButton.MOUSE_BUTTON_LEFT);

            Tint = ColorPalette.Regular;
            TrySetTextTint(TextColorPalette?.Regular);

            if (_IsMouseOver)
            {
                if (MouseService.IsButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                    _IsMousePressedOnButton = true;

                Tint = _IsMousePressedOnButton ? ColorPalette.Click : ColorPalette.Hover;
                TrySetTextTint(_IsMousePressedOnButton ? TextColorPalette?.Click : TextColorPalette?.Hover);

                if (_IsMousePressedOnButton && _IsMouseReleased)
                    Click();
            }

            if (_IsMouseReleased)
                _IsMousePressedOnButton = false;
        }

        private void TrySetTextTint(Color? tint)
        {
            if (Text != null && tint != null)
                Text.FontColor = tint.Value;
        }
    }
}
