using Raylib_cs;
using System.Numerics;

namespace GameEngine
{
    /// <summary>
    /// Service for interacting with the mouse.
    /// </summary>
    public static class MouseService
    {
        /// <summary>
        /// Gets the coordinates of the mouse cursor.
        /// </summary>
        public static Vector2 GetMouseCoordinates()
        {
            int x = Raylib.GetMouseX();
            int y = Raylib.GetMouseY();
            return new Vector2(x, y);
        }

        /// <summary>
        /// Gets whether a mouse button is currently held down.
        /// </summary>
        public static bool IsButtonDown(MouseButton button)
        {
            return Raylib.IsMouseButtonDown(button);
        }

        /// <summary>
        /// Gets whether a mouse button has been pressed.
        /// </summary>
        public static bool IsButtonPressed(MouseButton button)
        {
            return Raylib.IsMouseButtonPressed(button);
        }

        /// <summary>
        /// Gets whether a mouse button is currently being released.
        /// </summary>
        public static bool IsButtonReleased(MouseButton button)
        {
            return Raylib.IsMouseButtonReleased(button);
        }

        /// <summary>
        /// Gets whether a mouse button is currently up.
        /// </summary>
        public static bool IsButtonUp(MouseButton button)
        {
            return Raylib.IsMouseButtonUp(button);
        }
    }
}