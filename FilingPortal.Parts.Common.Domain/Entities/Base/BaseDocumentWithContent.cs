namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Defines the file stored in database
    /// </summary>
    public abstract class BaseDocumentWithContent : BaseDocument
    {
        /// <summary>
        /// Gets or sets the binary content of the file
        /// </summary>
        public byte[] Content { get; set; }
    }
}