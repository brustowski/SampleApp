using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Models;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Recon.Domain.Repositories
{
    /// <summary>
    /// Describes the repository of the inbound read models
    /// </summary>
    public interface IInboundRepository : ISearchRepository<InboundRecord>
    {
        /// <summary>
        /// Prepare report from CargoWise using user-defined filters
        /// </summary>
        /// <param name="filter">Filters set for report</param>
        int LoadFromCargoWise(ReconFilter filter);

        /// <summary>
        /// Gets entity by filer, entry number and line number
        /// </summary>
        /// <param name="filer">The filer</param>
        /// <param name="entryNo">The entry number</param>
        /// <param name="lineNo">The line number</param>
        InboundRecord Get(string filer, string entryNo, string lineNo);
    }
}