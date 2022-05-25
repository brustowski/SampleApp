using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Rail
{
    /// <summary>
    /// Interface for repository of <see cref="RailFilingHeader"/>
    /// </summary>
    public interface IRailFilingHeadersRepository : IFilingHeaderRepository<RailFilingHeader>, IFilingSectionRepository
    {
        /// <summary>
        /// Gets the rail filing header with details by identifier
        /// </summary>
        /// <param name="id">The rail filing header identifier</param>
        RailFilingHeader GetRailFilingHeaderWithDetails(int id);
    }
}