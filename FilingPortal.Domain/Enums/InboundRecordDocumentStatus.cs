namespace FilingPortal.Domain.Enums
{
    /// <summary>
    /// Enum describing possible document status
    /// </summary>
    public enum InboundRecordDocumentStatus
    {
        /// <summary>
        /// Not defined status
        /// </summary>
        None = 0,
        /// <summary>
        /// New document
        /// </summary>
        New,
        /// <summary>
        /// Updated document
        /// </summary>
        Updated,
        /// <summary>
        /// Deleted document
        /// </summary>
        Deleted
    }
}
