using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Represents the repository of the <see cref="SchbTariff" />
    /// </summary>
    public class SchbTariffRepository : SearchRepository<SchbTariff>, ITariffRepository<SchbTariff>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchbTariff"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public SchbTariffRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Search if the record already exist
        /// </summary>
        /// <param name="tariffCode">Tariff Code</param>
        public bool IsDuplicated(SchbTariff tariffCode) 
        {
            return false;
        }

        /// <summary>
        /// Search for the lookup data 
        /// </summary>
        /// <param name="searchData">Data for check</param>
        public bool IsExist(string searchData)
        {
            return Set.Any(x => x.UB_Tariff == searchData);
        }

        /// <summary>
        /// Gets list of Tariff codes
        /// </summary>
        /// <param name="search">Data for search</param>
        /// <param name="count">The search limit</param>
        public IEnumerable<SchbTariff> GetTariffData(string search, int count)
        {
            IQueryable<SchbTariff> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.UB_Tariff.StartsWith(search));
            }

            if (count > 0)
            {
                query = query.Take(count);
            }

            return query.OrderBy(x => x.UB_Tariff).ToList();
        }
    }
}
