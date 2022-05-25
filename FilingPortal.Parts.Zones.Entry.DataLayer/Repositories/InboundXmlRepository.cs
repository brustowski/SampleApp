using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Repositories
{
    /// <summary>
    /// Search repository for <see cref="InboundXml"/> entities
    /// </summary>
    public class InboundXmlRepository : SearchRepository<InboundXml>, IInboundXmlRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="InboundXmlRepository"/>
        /// </summary>
        /// <param name="unitOfWork">The Unit of work</param>
        public InboundXmlRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Returns files to process
        /// </summary>
        public async Task<IList<InboundXml>> GetUnprocessedFiles()
        {
            return await Set.Where(x => x.Status == 0).ToListAsync();
        }
    }
}
