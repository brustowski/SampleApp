using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Represents the repository of the <see cref="LookupMaster" />
    /// </summary>
    public class LookupMasterRepository : SearchRepository<LookupMaster>, ILookupMasterRepository<LookupMaster>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LookupMaster"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public LookupMasterRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Search if the record already exist
        /// </summary>
        /// <param name="lookupData">Lookup data</param>
        public bool IsDublicated(LookupMaster lookupData)
        {
            return false;
        }

        /// <summary>
        /// Search for the lookupData 
        /// </summary>
        /// <param name="searchData">Data for check</param>
        public bool IsExist(string searchData)
        {
            return Set.Any(x => x.Value == searchData);
        }

        /// <summary>
        /// Gets list of Port Codes
        /// </summary>
        /// <param name="search">Data for search</param>
        /// <param name="count">The search limit</param>
        public IEnumerable<LookupMaster> GetPortCodes(string search, int count)
        {
            IQueryable<LookupMaster> query = Set.Where(x => x.Type == "Port" && x.DisplayValue.StartsWith(search)).OrderBy(x => x.DisplayValue);
            if (count > 0)
            {
                query = query.Take(count);
            }
            return query.ToList();
        }

        /// <summary>
        /// Gets list of Firms codes
        /// </summary>
        /// <param name="search">Data for search</param>
        /// <param name="count">The search limit</param>
        public IEnumerable<LookupMaster> GetFIRMsCodes(string search, int count)
        {
            IQueryable<LookupMaster> query = Set.Where(x => x.Type == "FIRMsCode" && x.DisplayValue.StartsWith(search)).OrderBy(x => x.DisplayValue);
            if (count > 0)
            {
                query = query.Take(count);
            }
            return query.ToList();
        }

        /// <summary>
        /// Gets list of Origin Codes
        /// </summary>
        /// <param name="search">Data for search</param>
        /// <param name="count">The search limit</param>
        public IEnumerable<LookupMaster> GetOriginCodes(string search, int count)
        {
            IQueryable<LookupMaster> query = Set.Where(x => x.Type == "Origin" && x.DisplayValue.StartsWith(search)).OrderBy(x => x.DisplayValue);

            if (count > 0)
            {
                query = query.Take(count);
            }
            return query.ToList();
        }

        /// <summary>
        /// Get list of Destination Codes
        /// </summary>
        /// <param name="search">Data for search</param>
        /// <param name="count">The search limit</param>
        public IEnumerable<LookupMaster> GetDestinationCodes(string search, int count)
        {
            IQueryable<LookupMaster> query = Set.Where(x => x.Type == "Destination" && x.DisplayValue.StartsWith(search)).OrderBy(x => x.DisplayValue);
            if (count > 0)
            {
                query = query.Take(count);
            }
            return query.ToList();
        }

        /// <summary>
        /// Get list of Country Codes 
        /// </summary>
        /// <param name="search">Data for search</param>
        /// <param name="count">The search limit</param>
        public IEnumerable<LookupMaster> GetCountryCodes(string search, int count)
        {
            IQueryable<LookupMaster> query = Set.Where(x => x.Type == "CountryCode" && (x.DisplayValue.StartsWith(search) || x.Value.StartsWith(search))).OrderBy(x => x.DisplayValue);
            if (count > 0)
            {
                query = query.Take(count);
            }
            return query.ToList();
        }

        /// <summary>
        /// Gets list of Foreign Discharge Ports
        /// </summary>
        /// <param name="search">The search data</param>
        /// <param name="count">The search limit</param>
        public IEnumerable<LookupMaster> GetExportDischargePort(string search, int count = 10)
        {
            IQueryable<LookupMaster> query = Set.Where(x => x.Type == "Export_Discharge_Port");

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.DisplayValue.Contains(search) || x.Value.StartsWith(search));
            }
            
            return query.OrderBy(x => x.DisplayValue).Take(count).ToList();
        }
    }
}
