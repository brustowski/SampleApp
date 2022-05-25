using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Cargowise.DataLayer.Repositories
{
    /// <summary>
    /// Represents the repository of the <see cref="DomesticPort" />
    /// </summary>
    public class DomesticPortsRepository : SearchRepositoryWithTypedId<DomesticPort, int>, IDomesticPortsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomesticPort"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public DomesticPortsRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }


        /// <summary>
        /// Searches for records in the repository specified by search request
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IList<DomesticPort> Search(string searchInfo, int limit)
        {
            IQueryable<DomesticPort> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.PortCode.StartsWith(searchInfo));
            }

            return query.OrderBy(x => x.PortCode).Take(limit).ToList();
        }

        /// <summary>
        /// Searches for Domestic Port in the repository specified by search request
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IEnumerable<string> SearchUNLOCO(string searchInfo, int limit)
        {
            IQueryable<string> query = Set.Select(x => x.UNLOCO).Distinct();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.StartsWith(searchInfo));
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return query.OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Checks whether specified port code exists in the table
        /// </summary>
        /// <param name="port">The Port code</param>
        public bool IsExist(string port)
        {
            return Set.Any(x => x.PortCode == port);
        }
    }
}
