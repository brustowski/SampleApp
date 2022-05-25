using System;
using System.Collections.Generic;
using Framework.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Domain.Repositories.Clients
{
    /// <summary>
    /// Interface for repository of <see cref="Client"/>
    /// </summary>
    public interface IClientRepository : ISearchRepository, IRepositoryWithTypeId<Client, Guid>
    {
        /// <summary>
        /// Gets list of Importer clients 
        /// </summary>
        /// <param name="searchRequest">The searchRequest data</param>
        /// <param name="limit">Request limit</param>
        IList<Client> GetImporters(string searchRequest, int limit);

        /// <summary>
        /// Gets list of Supplier clients 
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        /// <param name="limit">The limit on the number of returned results</param>
        IList<Client> GetSuppliers(string searchRequest, int limit);

        /// <summary>
        /// Validate Importer clients 
        /// </summary>
        /// <param name="clientCode">ClientCode to be validated</param>
        bool IsImporterValid(string clientCode);

        /// <summary>
        /// Validate Supplier clients 
        /// </summary>
        /// <param name="clientCode">ClientCode to be validated</param>
        bool IsSupplierValid(string clientCode);

        /// <summary>
        /// Returns client if exists
        /// </summary>
        /// <param name="clientCode">Client code</param>
        Client GetClientByCode(string clientCode);

        /// <summary>
        /// Checks whether record with specified Client code exists in the table
        /// </summary>
        /// <param name="clientCode">The Client code</param>
        bool IsExist(string clientCode);
        /// <summary>
        /// Gets list of Importer clients 
        /// </summary>
        /// <param name="searchRequest">The searchRequest data</param>
        /// <param name="limit">Request limit</param>
        IList<Client> GetAll(string searchRequest, int limit);
        /// <summary>
        /// Gets clients list specified by client code type
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        /// <param name="clientCodeType">Client code type</param>
        /// <param name="limit">Limits the number of returned records</param>
        IList<Client> GetAll(string searchRequest, string clientCodeType, int limit);

        /// <summary>
        /// Gets client by its reg number
        /// </summary>
        /// <param name="regNumber">Reg number</param>
        /// <param name="codeType">Code type</param>
        IList<Client> GetByRegNumber(string regNumber, string codeType = null);
    }
}
