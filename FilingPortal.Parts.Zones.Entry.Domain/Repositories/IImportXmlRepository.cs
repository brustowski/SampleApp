namespace FilingPortal.Parts.Zones.Entry.Domain.Repositories
{
    /// <summary>
    /// Interface to create inbound records based on XML file
    /// </summary>
    public interface IImportXmlRepository
    {
        /// <summary>
        /// Creates records in inbound and resulting tables
        /// </summary>
        /// <param name="entry">XML Entry</param>
        void CreateInboundFromXml(CUSTOMS_ENTRY_FILEENTRY entry);
    }
}
