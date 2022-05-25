using System;
using System.Collections.Generic;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Clients
{
    /// <summary>
    /// Interface for repository of <see cref="Client"/>
    /// </summary>
    public interface IClientContactsRepository : IDataProviderEditableRepository<ClientContact, Guid>
    {
        /// <summary>
        /// Searches for contacts in the repository
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        /// <param name="clientId">Client id to be filtered by</param>
        IList<ClientContact> Search(string searchInfo, int limit, Guid clientId);
        /// <summary>
        /// Searches for contacts in the repository
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        /// <param name="clientCode">Client code to be filtered by</param>
        IList<ClientContact> Search(string searchInfo, int limit, string clientCode);
        /// <summary>
        /// Gets client contact by code
        /// </summary>
        /// <param name="code">Client contact code</param>
        ClientContact GetByCode(string code);
    }
}
