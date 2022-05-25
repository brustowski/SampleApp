using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Inbond.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Inbond.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="InBondCarrier"/>
    /// </summary>
    public class InBondCarrierRepository : RepositoryWithTypedId<InBondCarrier, string>, IDataProviderRepository<InBondCarrier, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InBondCarrierRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public InBondCarrierRepository(IUnitOfWorkFactory<UnitOfWorkInbondContext> unitOfWork) : base(unitOfWork)
        { }

        /// <summary>
        /// Searches for records in the repository specified by search request
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IList<InBondCarrier> Search(string searchInfo, int limit)
        {
            IQueryable<InBondCarrier> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.Id.StartsWith(searchInfo) || x.Name.StartsWith(searchInfo));
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return query.OrderBy(x => x.Id).ToList();
        }
    }
}
