using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Cargowise.DataLayer.Repositories
{
    /// <summary>
    /// Represents the repository of the <see cref="ForeignPort" />
    /// </summary>
    public class ForeignPortsRepository : SearchRepositoryWithTypedId<ForeignPort, int>, IForeignPortsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomesticPort"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public ForeignPortsRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }


        /// <summary>
        /// Searches for records in the repository specified by search request
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IList<ForeignPort> Search(string searchInfo, int limit)
        {
            IQueryable<ForeignPort> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.PortCode.StartsWith(searchInfo));
            }

            return query.OrderBy(x => x.PortCode).Take(limit).ToList();
        }

        /// <summary>
        /// Gets Foreign port by port code
        /// </summary>
        /// <param name="portCode">The port code</param>
        public ForeignPort GetByCode(string portCode) => Set.FirstOrDefault(x => x.PortCode.Equals(portCode));

        /// <summary>
        /// Gets the list of ports specified by country code
        /// </summary>
        /// <param name="countryCode">Country code</param>
        public IEnumerable<ForeignPort> GetCountryPorts(string countryCode)
        {
            return Set.Where(x => x.Country == countryCode).ToArray();
        }
    }
}
