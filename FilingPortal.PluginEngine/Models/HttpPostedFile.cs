namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Provides access to individual file that have been uploaded by a client 
    /// </summary>
    public class HttpPostedFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostedFile"/> class
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="path">Path to the file</param>
        public HttpPostedFile(string fileName, string path)
        {
            Name = fileName;
            Path = path;
        }
        /// <summary>
        /// Gets or sets Name of the file
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets or sets file stream
        /// </summary>
        public string Path { get; private set; }
    }
}