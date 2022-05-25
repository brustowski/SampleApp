using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Cargowise.DataLayer.Repositories
{
    /// <summary>
    /// Represents the repository of the <see cref="Country" />
    /// </summary>
    public class CountryRepository : SearchRepository<Country>, ICountryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public CountryRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }


        /// <summary>
        /// Searches for loading port in repository
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IList<Country> Search(string searchInfo, int limit)
        {
            IQueryable<Country> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.Name.Contains(searchInfo) || x.Code.Contains(searchInfo));
            }

            return query.OrderBy(x => x.Name).Take(limit).ToList();
        }

        /// <summary>
        /// Gets country by country code
        /// </summary>
        /// <param name="code">The country code</param>
        public Country GetByCode(string code) => Set.FirstOrDefault(x => x.Code.Equals(code));
    }
}
