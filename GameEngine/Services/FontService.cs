using Raylib_cs;
using System.Collections.Generic;
using System.IO;

namespace GameEngine
{
    public static class FontService
    {
        private static readonly Dictionary<string, Font> _Fonts = new();

        /// <summary>
        /// Loads all fonts in the given <paramref name="directory"/> into memory.
        /// </summary>
        /// <param name="directory">The directory in which the fonts are stored.</param>
        public static void LoadFonts(string directory)
        {
            string[] filters = new string[] { "*.otf", "*.ttf" };
            List<string> filepaths = FileService.GetAllFilePaths(directory, filters);
            foreach (string filepath in filepaths)
            {
                var font = Raylib.LoadFont(filepath);
                var filename = Path.GetFileNameWithoutExtension(filepath);
                _Fonts[filename] = font;
            }
        }

        /// <summary>
        /// Unloads all fonts from memory.
        /// </summary>
        public static void UnloadFonts()
        {
            foreach (string key in _Fonts.Keys)
            {
                var font = _Fonts[key];
                Raylib.UnloadFont(font);
            }
        }

        /// <summary>
        /// Gets a font by name.
        /// </summary>
        /// <param name="name">The name of the font.</param>
        /// <returns>The <see cref="Font"/> found.</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public static Font GetFont(string name)
        {
            if (!_Fonts.ContainsKey(name))
                throw new KeyNotFoundException($"Unable to find a font with the name '{name}'.");

            return _Fonts[name];
        }
    }
}
