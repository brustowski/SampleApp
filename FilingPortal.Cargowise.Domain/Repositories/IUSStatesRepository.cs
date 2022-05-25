using System.Collections.Generic;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using Framework.Domain.Repositories;

namespace FilingPortal.Cargowise.Domain.Repositories
{
    /// <summary>
    /// Defines the Generic US States Repository
    /// </summary>

    public interface IUSStatesRepository<TRuleEntity> : ISearchRepository<USStates>
    {
        /// <summary>
        /// Checks whether State Code Exist 
        /// </summary>
        /// <param name="searchData">The data identifier</param>
        bool IsExist(string searchData);

        /// <summary>
        /// Gets list of State codes 
        /// </summary>
        /// <param name="search">The search data</param>
        /// <param name="count">The search limit</param>
        IEnumerable<USStates> GetStateData(string search, int count);
    }
}

