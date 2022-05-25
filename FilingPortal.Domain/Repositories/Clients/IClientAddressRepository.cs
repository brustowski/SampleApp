using FilingPortal.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Clients
{
    /// <summary>
    /// Interface for repository of <see cref="Client"/>
    /// </summary>
    public interface IClientAddressRepository : IDataProviderRepository<ClientAddress, Guid>
    {
        /// <summary>
        /// Searches for address for the specified client in the repository
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        /// <param name="clientId">Client Identifier to be filtered by</param>
        IList<ClientAddress> Search(string searchInfo, int limit, Guid clientId);

        /// <summary>
        /// Searches for address for the specified client in the repository
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        /// <param name="clientCode">Client code to be filtered by</param>
        IList<ClientAddress> Search(string searchInfo, int limit, string clientCode);

        /// <summary>
        /// Gets the Client Address by the specified code
        /// </summary>
        /// <param name="addressCode">The client address code</param>
        ClientAddress GetByCode(string addressCode);
    }
}
