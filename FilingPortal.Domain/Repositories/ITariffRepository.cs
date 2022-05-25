using FilingPortal.Domain.Entities.Handbooks;
using Framework.Domain.Repositories;
using System.Collections.Generic;
using Framework.Domain;

namespace FilingPortal.Domain.Repositories
{
    /// <summary>
    /// Defines the Generic Tariff Repository
    /// </summary>

    public interface ITariffRepository<TRuleEntity> : ISearchRepository<TRuleEntity>
        where TRuleEntity: Entity
    {
        /// <summary>
        /// Checks whether Tariff Exist 
        /// </summary>
        /// <param name="searchData">The data identifier</param>
        bool IsExist(string searchData);

        /// <summary>
        /// Gets list of Tariff codes 
        /// </summary>
        /// <param name="search">The search data</param>
        /// <param name="count">The search limit</param>
        IEnumerable<TRuleEntity> GetTariffData(string search, int count);
    }
}