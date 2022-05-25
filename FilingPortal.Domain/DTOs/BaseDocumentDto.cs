using FilingPortal.Domain.Enums;

namespace FilingPortal.Domain.DTOs
{
    /// <summary>
    /// Defines the <see cref="BaseDocumentDto" /> class - parent of all Documents DTO
    /// </summary>
    public abstract class BaseDocumentDto
    {
        /// <summary>
        /// Gets or sets the string Document Type
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the File Contents
        /// </summary>
        public byte[] FileContent { get; internal set; }

        /// <summary>
        /// Gets or sets the user description of file
        /// </summary>
        public string FileDesc { get; set; }

        /// <summary>
        /// Gets or sets the extension of file
        /// </summary>
        public string FileExtension { get; internal set; }

        /// <summary>
        /// Gets or sets the File Name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the FilingHeadersFk - field for linking model with corresponding database object
        /// </summary>
        public int FilingHeadersFk { get; set; }

        /// <summary>
        /// Gets or sets the model Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Status of file
        /// </summary>
        public InboundRecordDocumentStatus Status { get; set; }
    }
}
