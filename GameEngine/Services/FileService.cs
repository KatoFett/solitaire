using System.Collections.Generic;
using System.IO;

namespace GameEngine
{
    public static class FileService
    {

        /// <summary>
        /// Gets the file paths for all files matching the search filters.
        /// </summary>
        /// <param name="directory">The directory to search in.</param>
        /// <param name="searchPatterns">The search patterns to use.</param>
        /// <returns>A list containing the filepaths of all files that match the search patterns.</returns>
        public static List<string> GetAllFilePaths(string directory, IEnumerable<string> searchPatterns)
        {
            List<string> results = new();
            if (Directory.Exists(directory))
            {
                foreach (string filter in searchPatterns)
                {
                    string[] filepaths = Directory.GetFiles(directory, filter);
                    results.AddRange(filepaths);
                }
            }
            return results;
        }
    }
}
