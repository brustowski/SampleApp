using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.AppSystem
{
    /// <summary>
    /// Defines the file entity
    /// </summary>
    public class AppDocument : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the file type
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// Gets or sets the binary content of the file
        /// </summary>
        public byte[] FileContent { get; set; }
    }
}
