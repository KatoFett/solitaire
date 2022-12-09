using Raylib_cs;


namespace GameEngine
{
    /// <summary>
    /// Service for interacting with the keyboard.
    /// </summary>
    public static class KeyboardService
    {
        /// <summary>
        /// Gets whether a keyboard key is currently held down.
        /// </summary>
        public static bool IsKeyDown(KeyboardKey key)
        {
            return Raylib.IsKeyDown(key);
        }

        /// <summary>
        /// Gets whether a keyboard key was pressed.
        /// </summary>
        public static bool IsKeyPressed(KeyboardKey key)
        {
            return Raylib.IsKeyPressed(key);
        }

        /// <summary>
        /// Gets whether a keyboard key is currently being released.
        /// </summary>
        public static bool IsKeyReleased(KeyboardKey key)
        {
            return Raylib.IsKeyReleased(key);
        }

        /// <summary>
        /// Gets whether a keyboard key is currently up.
        /// </summary>
        public static bool IsKeyUp(KeyboardKey key)
        {
            return Raylib.IsKeyUp(key);
        }
    }
}