using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Cargowise.DataLayer.Repositories
{
    /// <summary>
    /// Represents the repository of the <see cref="USStates" />
    /// </summary>
    public class USStatesRepository : SearchRepository<USStates>, IUSStatesRepository<USStates>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="USStates"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public USStatesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Search if the record already exist
        /// </summary>
        /// <param name="lookupData">us state lookup data</param>
        public bool IsDublicated(USStates lookupData)
        {
            return false;
        }

        /// <summary>
        /// Search for the lookupData 
        /// </summary>
        /// <param name="searchData">Data for check</param>
        public bool IsExist(string searchData)
        {
            return Set.Any(x => x.StateCode == searchData);
        }
        /// <summary>
        /// Gets list of State codes
        /// </summary>
        /// <param name="search">Data for search</param>
        /// <param name="count">The search limit</param>
        public IEnumerable<USStates> GetStateData(string search, int count)
        {
            IQueryable<USStates> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.StateCode.Contains(search) || x.StateName.Contains(search));
            }

            if (count > 0)
            {
                query = query.Take(count);
            }

            return query.OrderBy(x => x.StateName).ToList();
        }
    }
}
