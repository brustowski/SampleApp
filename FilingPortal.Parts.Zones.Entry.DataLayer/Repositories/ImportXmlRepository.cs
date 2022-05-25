using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Repositories
{
    /// <summary>
    /// Class to create inbound records based on XML model
    /// </summary>
    internal class ImportXmlRepository : Repository<CUSTOMS_ENTRY_FILEENTRY>, IImportXmlRepository
    {
        public ImportXmlRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Creates records in inbound and resulting tables
        /// </summary>
        /// <param name="entry">XML Entry</param>
        public void CreateInboundFromXml(CUSTOMS_ENTRY_FILEENTRY entry)
        {
            InboundRecord inboundRecord = entry.Map<CUSTOMS_ENTRY_FILEENTRY, InboundRecord>();
            GetSet<InboundRecord>().Add(inboundRecord);



            Save();
        }
    }
}
