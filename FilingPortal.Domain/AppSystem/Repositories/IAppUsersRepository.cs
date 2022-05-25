using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.AppSystem.Repositories
{
    /// <summary>
    /// Defines the <see cref="IAppUsersRepository" /> to define users methods
    /// </summary>
    public interface IAppUsersRepository : ISearchRepository
    {
        /// <summary>
        /// Returns <see cref="AppUsersModel"/> if already exists with specified user account, null otherwise
        /// </summary>
        /// <param name="userAccount">The userAccount<see cref="string"/></param>
        AppUsersModel GetUserInfo(string userAccount);

        /// <summary>
        /// Adds user in Waiting status to AppUsers model with corresponding comment
        /// </summary>
        /// <param name="userAccount">The userAccount <see cref="string"/></param>
        /// <param name="requestInfo">The requestInfo <see cref="string"/> - comment to request</param>
        void SendAccessRequest(string userAccount, string requestInfo);

        /// <summary>
        /// Gets list of user logins specified by search request
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        /// <param name="limit">limit on the number of returned results</param>
        IList<string> GetLogins(string searchRequest, int limit = 0);
    }
}
