using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.AppSystem.Repositories
{
    /// <summary>
    /// Defines the <see cref="IAppUsersDataRepository" /> to define users additional data methods
    /// </summary>
    public interface IAppUsersDataRepository : ISearchRepository
    {
        /// <summary>
        /// Returns <see cref="AppUsersData"/> if already exists with specified user account, null otherwise
        /// </summary>
        /// <param name="userAccount">The userAccount<see cref="string"/></param>
        AppUsersData GetUserData(string userAccount);

        /// <summary>
        /// Gets list of user data specified by search request
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        /// <param name="limit">limit on the number of returned results</param>
        IList<AppUsersData> GetData(string searchRequest, int limit = 0);
    }
}
