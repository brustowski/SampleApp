using FilingPortal.Domain.Repositories.Clients;
using Framework.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.DataLayer.Repositories.Common
{

    /// <summary>
    /// Class for repository of ClientReadModel , for validating and lookup of Client codes
    /// </summary>
    public class ClientRepository : SearchRepositoryWithTypedId<Client, Guid>, IClientRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public ClientRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Get all client codes based on the searchRequest data
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        /// <param name="limit">The limit on the number of returned results</param>
        public IList<Client> GetAll(string searchRequest, int limit = 10)
        {
            IQueryable<Client> query = Set;
            if (!string.IsNullOrWhiteSpace(searchRequest))
            {
                query = query.Where(x => x.ClientCode.StartsWith(searchRequest) || x.ClientName.StartsWith(searchRequest));
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return query.OrderBy(x => x.ClientCode).ToList();
        }

        /// <summary>
        /// Gets clients list specified by client code type
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        /// <param name="clientCodeType">Client code type</param>
        /// <param name="limit">Limits the number of returned records</param>
        public IList<Client> GetAll(string searchRequest, string clientCodeType, int limit = 10)
        {
            IQueryable<Client> query = Set.Where(x=>x.ClientNumbers.Any(y=>y.CodeType == clientCodeType));

            if (!string.IsNullOrWhiteSpace(searchRequest))
            {
                query = query.Where(x => x.ClientCode.StartsWith(searchRequest) 
                                         || x.ClientName.StartsWith(searchRequest) 
                                         || x.ClientNumbers.Any(y=>y.RegNumber.StartsWith(searchRequest)));
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }
            return query.OrderBy(x => x.ClientCode).ToList();
        }

        /// <summary>
        /// Gets clients by its reg number
        /// </summary>
        /// <param name="regNumber">Reg number</param>
        /// <param name="codeType">Code type</param>
        public IList<Client> GetByRegNumber(string regNumber, string codeType = null)
        {
            return Set.Where(x =>
                x.ClientNumbers.Any(c => c.RegNumber == regNumber && (codeType == null || c.CodeType == codeType))).ToList();
        }

        /// <summary>
        /// Get importer codes based on the searchRequest data
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        /// <param name="limit">The limit on the number of returned results</param>
        public IList<Client> GetImporters(string searchRequest, int limit = 10)
        {
            IQueryable<Client> query = Set.Where(x => x.Status && x.Importer);
            if (!string.IsNullOrWhiteSpace(searchRequest))
            {
                query = query.Where(x => x.ClientCode.StartsWith(searchRequest) || x.ClientName.StartsWith(searchRequest));
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return query.OrderBy(x => x.ClientCode).ToList();
        }

        /// <summary>
        /// Get supplier codes based on the searchRequest data
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        /// <param name="limit">limit on the number of returned results</param>
        public IList<Client> GetSuppliers(string searchRequest, int limit = 10)
        {
            IQueryable<Client> query = Set.Where(x => x.Status && x.Supplier);
            if (!string.IsNullOrWhiteSpace(searchRequest))
            {
                query = query.Where(x => x.ClientCode.StartsWith(searchRequest) || x.ClientName.StartsWith(searchRequest));
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return query.OrderBy(x => x.ClientCode).ToList();
        }

        /// <summary>
        /// Check if importer is valid
        /// </summary>
        /// <param name="clientCode">The client code</param>
        public bool IsImporterValid(string clientCode)
        {
            return Set.Any(x => x.ClientCode == clientCode && x.Importer);
        }
        /// <summary>
        /// Check if Supplier is valid
        /// </summary>
        /// <param name="clientCode">The client code</param>
        public bool IsSupplierValid(string clientCode)
        {
            return Set.Any(x => x.ClientCode == clientCode && x.Supplier);
        }

        /// <summary>
        /// Returns client if exists
        /// </summary>
        /// <param name="clientCode">The client code</param>
        public Client GetClientByCode(string clientCode) =>
            Set.FirstOrDefault(x =>
                x.ClientCode.Equals(clientCode, StringComparison.InvariantCultureIgnoreCase));

        /// <summary>
        /// Checks whether record with specified Client code exists in the table
        /// </summary>
        /// <param name="clientCode">The Client code</param>
        public bool IsExist(string clientCode)
        {
            return Set.Any(x=>x.ClientCode.Equals(clientCode));
        }
    }
}
