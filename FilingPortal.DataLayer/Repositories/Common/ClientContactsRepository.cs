using FilingPortal.Domain.Repositories.Clients;
using Framework.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Represents the repository of the <see cref="ClientContact" />
    /// </summary>
    public class ClientContactsRepository : RepositoryWithTypedId<ClientContact, Guid>, IClientContactsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientContactsRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public ClientContactsRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }


        /// <summary>
        /// Searches for client contacts in the repository
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IList<ClientContact> Search(string searchInfo, int limit)
        {
            IQueryable<ClientContact> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.ContactName.StartsWith(searchInfo));
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return query.OrderBy(x => x.ContactName).ToList();
        }

        /// <summary>
        /// Searches for client contacts in the repository
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        /// <param name="clientId">Client identifier to be filtered by</param>
        public IList<ClientContact> Search(string searchInfo, int limit, Guid clientId)
        {
            IQueryable<ClientContact> query = Set.Where(x => x.ClientId == clientId);

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.ContactName.StartsWith(searchInfo));
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return query.OrderBy(x => x.ContactName).ToList();
        }

        /// <summary>
        /// Searches for contacts in the repository
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        /// <param name="clientCode">Client code to be filtered by</param>
        public IList<ClientContact> Search(string searchInfo, int limit, string clientCode)
        {
            Guid clientId = GetSet<Client>().Where(x => x.ClientCode == clientCode).Select(x => x.Id).FirstOrDefault();
            return Search(searchInfo, limit, clientId);
        }

        /// <summary>
        /// Gets client contact by code
        /// </summary>
        /// <param name="code">Client contact code</param>
        public ClientContact GetByCode(string code)
        {
            return Set.FirstOrDefault(x => x.ContactName == code);
        }
    }
}
