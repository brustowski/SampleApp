using System.Collections.Generic;
using FilingPortal.Domain.AppSystem.Helpers;
using FilingPortal.Domain.AppSystem.Repositories;
using Framework.DataLayer;
using System.Linq;
using System.Data.Entity;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.DataLayer.Repositories.AppSystem
{
    /// <summary>
    /// Defines the Users repository
    /// </summary>
    public class AppUsersRepository : SearchRepositoryWithTypedId<AppUsersModel, string>, IAppUsersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppUsersRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unitOfWork<see cref="IUnitOfWorkFactory"/></param>
        public AppUsersRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Returns <see cref="AppUsersModel"/> if already exists with specified user account, null otherwise
        /// </summary>
        /// <param name="userAccount">The userAccount<see cref="string"/></param>
        public AppUsersModel GetUserInfo(string userAccount) 
            => Set.Where(x =>x.Id == userAccount).Include(x => x.Roles.Select(r => r.Permissions)).FirstOrDefault();

        /// <summary>
        /// Adds user in Waiting status to AppUsers model with corresponding comment
        /// </summary>
        /// <param name="userAccount">The userAccount <see cref="string"/></param>
        /// <param name="requestInfo">The requestInfo <see cref="string"/> - comment to request</param>
        public void SendAccessRequest(string userAccount, string requestInfo)
        {
            AppUsersModel usersModel = GetUserInfo(userAccount);

            if (usersModel != null) return;

            var result = new AppUsersModel()
            {
                Id = userAccount,
                RequestInfo = requestInfo,
                StatusId = (int)AppUsersStatusHelper.Waiting
            };

            Set.Add(result);
            Save();
        }

        /// <summary>
        /// Gets list of user logins specified by search request
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        /// <param name="limit">limit on the number of returned results</param>
        public IList<string> GetLogins(string searchRequest, int limit = 0)
        {
            IQueryable<AppUsersModel> result = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchRequest))
            {
                result = result.Where(x => x.Id.Contains(searchRequest));
            }

            if (limit > 0)
            {
                result = result.Take(limit);
            }

            return result.Select(x => x.Id).ToList();
        }
    }
}
