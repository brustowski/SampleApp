using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Represents the repository of the <see cref="HtsTariff" />
    /// </summary>
    public class HtsTariffRepository : SearchRepository<HtsTariff>, ITariffRepository<HtsTariff>, IHtsNumbersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtsTariff"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public HtsTariffRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Search if the record already exist
        /// </summary>
        /// <param name="tariffCode">Tariff Code</param>
        public bool IsDuplicated(HtsTariff tariffCode) 
        {
            return false;
        }

        /// <summary>
        /// Search for the lookup data 
        /// </summary>
        /// <param name="searchData">Data for check</param>
        public bool IsExist(string searchData)
        {
            return Set.Any(x => x.USC_Tariff == searchData);
        }

        /// <summary>
        /// Gets list of Tariff codes
        /// </summary>
        /// <param name="search">Data for search</param>
        /// <param name="count">The search limit</param>
        public IEnumerable<HtsTariff> GetTariffData(string search, int count)
        {
            IQueryable<HtsTariff> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.USC_Tariff.StartsWith(search));
            }

            if (count > 0)
            {
                query = query.Take(count);
            }

            return query.OrderBy(x => x.USC_Tariff).ToList();
        }

        /// <summary>
        /// Gets list of HTS numbers
        /// </summary>
        /// <param name="search">The search data</param>
        /// <param name="count">The search limit</param>
        /// <param name="htsDigits">Length of HTS number</param>
        public IList<string> GetHtsNumbers(string search, int count, int htsDigits = 6)
        {
            var query = Set
                .Select(x => x.USC_Tariff.Substring(0, htsDigits))
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Contains(search));
            }

            if (count > 0)
            {
                query = query.Take(count);
            }

            return query.Distinct().OrderBy(x => x).ToList();
        }
    }
}
