using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Parts.Common.Domain.Mapping;
using Framework.DataLayer;
using Framework.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    /// <summary>
    /// Class for repository of <see cref="RailBdParsed"/>
    /// </summary>
    public class BdParsedRepository : SearchRepository<RailBdParsed>, IBdParsedRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BdParsedRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public BdParsedRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets the total rows asynchronously
        /// </summary>
        public Task<TableInfo> GetTotalRowsAsync()
        {
            return GetTotalRowsAsync("imp_rail_inbound");
        }

        /// <summary>
        /// Gets the total Rail BD rows asynchronously by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public TableInfo GetTotalRailBdRows(int filingHeaderId)
        {
            return new TableInfo {RowCount = Set.Count(x => x.FilingHeaders.Any(y => y.Id == filingHeaderId))};
        }

        /// <summary>
        /// Gets the list of rail bd parsed records with details by specified list of identifiers
        /// </summary>
        /// <param name="ids">The list of identifiers</param>
        public IEnumerable<RailBdParsed> GetListWithDetails(IEnumerable<int> ids)
        {
            return Set.Include(x => x.RailEdiMessage).Where(x => ids.Contains(x.Id));
        }

        /// <summary>
        /// Gets record manifest
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Manifest GetManifest(int id)
        {
            return Get(id).Map<RailBdParsed, Manifest>();
        }

        /// <summary>
        /// Gets inbound records by filing header id
        /// </summary>
        /// <param name="filingHeaderId">Filing header id</param>
        public IEnumerable<RailBdParsed> GetByFilingId(int filingHeaderId)
        {
            return Set.Where(x => x.FilingHeaders.Select(f => f.Id).Contains(filingHeaderId));
        }
    }
}
