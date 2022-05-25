namespace FilingPortal.Parts.Zones.Ftz214.Domain.Repositories
{
    /// <summary>
    /// Interface to create inbound records based on XML file
    /// </summary>
    public interface IImportXmlRepository
    {
        /// <summary>
        /// Creates records in inbound and resulting tables
        /// </summary>
        /// <param name="entry">XML Ftz214</param>
        void CreateInboundFromXml(FTZ_214FTZ_ADMISSION entry);
    }
}
