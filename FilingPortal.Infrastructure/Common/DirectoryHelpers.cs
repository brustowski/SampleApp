namespace FilingPortal.Infrastructure.Common
{
    using System.IO;

    /// <summary>
    /// Provides method for working with directories
    /// </summary>
    public static class DirectoryHelpers
    {
        /// <summary>
        /// Ensures that working folder exists and provides full path to it.
        /// </summary>
        /// <param name="folderPath">Relative folder path</param>
        public static string EnsureWorkingFolder(string folderPath)
        {
            var basePath = Path.GetTempPath();

            var absoluteBaseFolderPath = Path.Combine(basePath, folderPath);

            Directory.CreateDirectory(absoluteBaseFolderPath);

            return absoluteBaseFolderPath;
        }
    }
}
