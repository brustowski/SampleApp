namespace FilingPortal.Domain.Common
{
    /// <summary>
    /// Class describes binary file 
    /// </summary>
    public class BinaryFileModel
    {
        /// <summary>
        /// Gets or sets the name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the content
        /// </summary>
        public byte[] Content { get; set; }
    }
}