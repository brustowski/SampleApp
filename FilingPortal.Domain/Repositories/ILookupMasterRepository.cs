namespace FilingPortal.Domain.Repositories
{
    using Framework.Domain;
    using Framework.Domain.Repositories;
    using FilingPortal.Domain.Entities;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the Generic Lookup Repository
    /// </summary>
    /// <typeparam name="TRuleEntity">Truck Rule type</typeparam>
    public interface ILookupMasterRepository<TRuleEntity> : ISearchRepository<LookupMaster>
    {
        /// <summary>
        /// Checks whether client searchData exist in client table 
        /// </summary>
        /// <param name="searchData">The data identifier</param>
        bool IsExist(string searchData);

        /// <summary>
        /// Gets list of Port
        /// </summary>
        /// <param name="search">The search data</param>
        /// <param name="count">The search limit</param>
        IEnumerable<LookupMaster> GetPortCodes(string search,int count);

        /// <summary>
        /// Gets list of Firms codes
        /// </summary>
        /// <param name="search">The search data</param>
        /// <param name="count">The search limit</param>
        IEnumerable<LookupMaster> GetFIRMsCodes(string search, int count);

        /// <summary>
        /// Gets list of Origin codes 
        /// </summary>
        /// <param name="search">The search data</param>
        /// <param name="count">The search limit</param>
        IEnumerable<LookupMaster> GetOriginCodes(string search, int count);

        /// <summary>
        /// Gets list destination codes 
        /// </summary>
        IEnumerable<LookupMaster> GetDestinationCodes(string search, int count);

        /// <summary>
        /// Gets list of Country Codes
        /// </summary>
        /// <param name="search">The search data</param>
        /// <param name="count">The search limit</param>
        IEnumerable<LookupMaster> GetCountryCodes(string search, int count);

        /// <summary>
        /// Gets list of Foreign Discharge Ports
        /// </summary>
        /// <param name="search">The search data</param>
        /// <param name="count">The search limit</param>
        IEnumerable<LookupMaster> GetExportDischargePort(string search, int count);
    }
}

