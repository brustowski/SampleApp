namespace FilingPortal.Domain.DTOs.Rail
{
    /// <summary>
    /// Defines the <see cref="RailDocumentDto" /> - data transfer object for Rail document
    /// </summary>
    public class RailDocumentDto : BaseDocumentDto
    {
        /// <summary>
        /// Gets or sets whether current model is Manifest
        /// </summary>
        public byte IsManifest { get; set; }
    }
}
