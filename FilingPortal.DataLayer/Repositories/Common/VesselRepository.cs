using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Implements methods working with vessels handbook
    /// </summary>
    internal class VesselRepository : SearchRepository<VesselHandbookRecord>, IVesselRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="VesselRepository"/>
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        public VesselRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Searches for vessel in repository
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        public IList<VesselHandbookRecord> SearchVessel(string searchInfoSearch, int searchInfoLimit)
        {
            IQueryable<VesselHandbookRecord> query = Set.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchInfoSearch))
            {
                query = query.Where(x => x.Name.Contains(searchInfoSearch));
            }

            if (searchInfoLimit > 0)
            {
                query = query.Take(searchInfoLimit);
            }

            return query.OrderBy(x => x.Name).ToList();
        }
    }
}
