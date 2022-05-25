using System.IO;

namespace FilingPortal.Domain.Common.Import
{
    /// <summary>
    /// File import service
    /// </summary>
    /// <typeparam name="TProcessResult">Return result</typeparam>
    public abstract class FileImportService<TProcessResult> : IFileImportService<TProcessResult>
    {
        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="filePath">Fully qualified file path</param>
        /// <param name="userName">Current user name</param>
        public TProcessResult Process(string fileName, string filePath, string userName = null)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", Path.GetFileName(filePath));
            }

            using (FileStream fileStream = File.OpenRead(filePath))
            {
                return Process(fileName, fileStream, userName);
            }
        }

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="fileStream">File stream</param>
        /// <param name="userName">Current user name</param>
        public abstract TProcessResult Process(string fileName, Stream fileStream, string userName = null);

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="fileContent">File contents</param>
        /// <param name="userName">Current user name</param>
        public TProcessResult Process(string fileName, byte[] fileContent, string userName = null)
        {
            using (MemoryStream memoryStream = new MemoryStream(fileContent))
            {
                return Process(fileName, memoryStream, userName);
            }
        }
    }
}