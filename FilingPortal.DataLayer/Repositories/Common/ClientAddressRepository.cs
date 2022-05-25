using FilingPortal.Domain.Repositories.Clients;
using Framework.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Represents the repository of the <see cref="ClientAddress" />
    /// </summary>
    public class ClientAddressRepository : RepositoryWithTypedId<ClientAddress, Guid>, IClientAddressRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAddress"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public ClientAddressRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }


        /// <summary>
        /// Searches for address in the repository
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IList<ClientAddress> Search(string searchInfo, int limit)
        {
            IQueryable<ClientAddress> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.Code.StartsWith(searchInfo));
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return query.OrderBy(x => x.Code).ToList();
        }

        /// <summary>
        /// Searches for address in the repository
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        /// <param name="clientId">Client identifier to be filtered by</param>
        public IList<ClientAddress> Search(string searchInfo, int limit, Guid clientId)
        {
            IQueryable<ClientAddress> query = Set.Where(x => x.ClientId == clientId);

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.Code.StartsWith(searchInfo));
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return query.OrderBy(x => x.Code).ToList();
        }

        /// <summary>
        /// Searches for address for the specified client in the repository
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        /// <param name="clientCode">Client code to be filtered by</param>
        public IList<ClientAddress> Search(string searchInfo, int limit, string clientCode)
        {
            Client client = UnitOfWork.Context.Set<Client>().FirstOrDefault(x => x.ClientCode == clientCode);

            return client == null 
                ? new List<ClientAddress>()
                : Search(searchInfo, limit, client.Id);
        }

        /// <summary>
        /// Gets the Client Address by the specified code
        /// </summary>
        /// <param name="addressCode">The client address code</param>
        public ClientAddress GetByCode(string addressCode)
        {
            return Set.FirstOrDefault(x => x.Code == addressCode);
        }
    }
}
