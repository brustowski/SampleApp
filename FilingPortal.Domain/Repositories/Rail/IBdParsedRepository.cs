using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Rail
{
    /// <summary>
    /// Interface for repository of <see cref="RailBdParsed"/>
    /// </summary>
    public interface IBdParsedRepository : IInboundRecordsRepository<RailBdParsed>
    {
        /// <summary>
        /// Gets the total rows asynchronously
        /// </summary>
        Task<TableInfo> GetTotalRowsAsync();

        /// <summary>
        /// Gets the total Rail BD rows asynchronous
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        TableInfo GetTotalRailBdRows(int filingHeaderId);

        /// <summary>
        /// Gets the list of rail bd parsed records with details by specified list of identifiers
        /// </summary>
        /// <param name="ids">The list of identifiers</param>
        IEnumerable<RailBdParsed> GetListWithDetails(IEnumerable<int> ids);

        /// <summary>
        /// Gets record manifest
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Manifest GetManifest(int id);
    }
}
