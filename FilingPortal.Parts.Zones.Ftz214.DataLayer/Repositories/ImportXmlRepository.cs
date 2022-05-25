using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.Parts.Zones.Ftz214.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Repositories
{
    /// <summary>
    /// Class to create inbound records based on XML model
    /// </summary>
    internal class ImportXmlRepository : Repository<FTZ_214FTZ_ADMISSION>, IImportXmlRepository
    {
        public ImportXmlRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Creates records in inbound and resulting tables
        /// </summary>
        /// <param name="entry">XML Ftz214</param>
        public void CreateInboundFromXml(FTZ_214FTZ_ADMISSION entry)
        {
            InboundRecord inboundRecord = entry.Map<FTZ_214FTZ_ADMISSION, InboundRecord>();
            GetSet<InboundRecord>().Add(inboundRecord);



            Save();
        }
    }
}
