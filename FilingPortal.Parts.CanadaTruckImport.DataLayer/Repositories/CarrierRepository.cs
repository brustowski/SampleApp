using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="Carrier"/>
    /// </summary>
    public class CarrierRepository : RepositoryWithTypedId<Carrier, string>, IDataProviderRepository<Carrier, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CarrierRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public CarrierRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        { }

        /// <summary>
        /// Searches for records in the repository specified by search request
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IList<Carrier> Search(string searchInfo, int limit)
        {
            IQueryable<Carrier> query = Set.AsQueryable();

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
