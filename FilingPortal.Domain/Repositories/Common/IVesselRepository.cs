using System.Collections.Generic;
using FilingPortal.Domain.Entities.Handbooks;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Common
{
    /// <summary>
    /// Describes methods working with vessels handbook
    /// </summary>
    public interface IVesselRepository : ISearchRepository<VesselHandbookRecord>
    {
        /// <summary>
        /// Searches for vessel in repository
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        IList<VesselHandbookRecord> SearchVessel(string searchInfoSearch, int searchInfoLimit);
    }
}
