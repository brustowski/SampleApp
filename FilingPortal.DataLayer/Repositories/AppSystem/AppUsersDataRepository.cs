using System.Collections.Generic;
using Framework.DataLayer;
using System.Linq;
using FilingPortal.Domain.AppSystem.Repositories;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.DataLayer.Repositories.AppSystem
{
    /// <summary>
    /// Defines the Users additional data repository
    /// </summary>
    public class AppUsersDataRepository : SearchRepositoryWithTypedId<AppUsersData, string>, IAppUsersDataRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppUsersDataRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unitOfWork<see cref="IUnitOfWorkFactory"/></param>
        public AppUsersDataRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Returns <see cref="AppUsersData"/> if already exists with specified user account, null otherwise
        /// </summary>
        /// <param name="userAccount">The userAccount<see cref="string"/></param>
        public AppUsersData GetUserData(string userAccount)
            => Set.FirstOrDefault(x => x.Id == userAccount);

        /// <summary>
        /// Gets list of user logins specified by search request
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        /// <param name="limit">limit on the number of returned results</param>
        public IList<AppUsersData> GetData(string searchRequest, int limit = 0)
        {
            IQueryable<AppUsersData> result = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchRequest))
            {
                result = result.Where(x => x.Id.Contains(searchRequest));
            }

            if (limit > 0)
            {
                result = result.Take(limit);
            }

            return result.ToList();
        }
    }
}
