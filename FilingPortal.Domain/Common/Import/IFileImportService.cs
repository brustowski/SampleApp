using System.IO;

namespace FilingPortal.Domain.Common.Import
{
    /// <summary>
    /// Describes File Import Service
    /// </summary>
    public interface IFileImportService<out TProcessResult>
    {
        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="filePath">Fully qualified file path</param>
        /// <param name="userName">Current user name</param>
        TProcessResult Process(string fileName, string filePath, string userName = null);

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="fileStream">File stream</param>
        /// <param name="userName">Current user name</param>
        TProcessResult Process(string fileName, Stream fileStream, string userName = null);

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="fileContent">File contents</param>
        /// <param name="userName">Current user name</param>
        TProcessResult Process(string fileName, byte[] fileContent, string userName = null);
    }
}
