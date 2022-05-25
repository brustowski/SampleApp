using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Cargowise.DataLayer.Repositories
{
    /// <summary>
    /// Implements methods working with UNLOCO dictionary
    /// </summary>
    internal class UnlocoDictionaryRepository : SearchRepository<UnlocoDictionaryEntry>, IUnlocoDictionaryRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="UnlocoDictionaryRepository"/>
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        public UnlocoDictionaryRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Searches for UNLOCO in repository
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        public IList<UnlocoDictionaryEntry> Search(string searchInfoSearch, int searchInfoLimit)
        {
            IQueryable<UnlocoDictionaryEntry> query = Set;
            return Search(query, searchInfoSearch, searchInfoLimit);
        }

        private static IList<UnlocoDictionaryEntry> Search(IQueryable<UnlocoDictionaryEntry> query, string searchInfoSearch, int searchInfoLimit)
        {
            if (!string.IsNullOrWhiteSpace(searchInfoSearch))
            {
                query = query.Where(x => x.Unloco.Contains(searchInfoSearch) || x.PortName.Contains(searchInfoSearch));
            }

            if (searchInfoLimit != default(int))
            {
                query = query.Take(searchInfoLimit);
            }

            return query.OrderBy(x => x.Unloco).ToList();
        }

        /// <summary>
        /// Searches for UNLOCO specified by country code
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        /// <param name="countryCode">Country code</param>
        public IList<UnlocoDictionaryEntry> Search(string searchInfoSearch, int searchInfoLimit, string countryCode)
        {
            IQueryable<UnlocoDictionaryEntry> query = Set.Where(x => x.CountryCode == countryCode);

            return Search(query, searchInfoSearch, searchInfoLimit);
        }

        /// <summary>
        /// Gets dictionary entry by UNLOCO code
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        public UnlocoDictionaryEntry GetByCode(string searchInfoSearch)
        {
            return Set.FirstOrDefault(x => x.Unloco == searchInfoSearch);
        }
    }
}
