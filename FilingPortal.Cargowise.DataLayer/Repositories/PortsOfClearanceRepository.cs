using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities;
using FilingPortal.Cargowise.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Cargowise.DataLayer.Repositories
{
    /// <summary>
    /// Implements methods working with ports of clearance
    /// </summary>
    internal class PortsOfClearanceRepository : SearchRepository<PortOfClearance>, IPortsOfClearanceRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="PortsOfClearanceRepository"/>
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        public PortsOfClearanceRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Searches for ports in repository
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        public IList<PortOfClearance> Search(string searchInfoSearch, int searchInfoLimit)
        {
            IQueryable<PortOfClearance> query = Set;
            if (!string.IsNullOrWhiteSpace(searchInfoSearch))
            {
                query = query.Where(x => x.Office.Contains(searchInfoSearch) || x.Code.Contains(searchInfoSearch));
            }

            return query.Take(searchInfoLimit).OrderBy(x => x.Code).ToList();
        }

        /// <summary>
        /// Checks whether record with specified port code exists in the table
        /// </summary>
        /// <param name="portCode">Port code</param>
        public bool IsExist(string portCode)
        {
            return Set.Any(x => x.Code.Equals(portCode));
        }

        /// <summary>
        /// Returns port code if exists
        /// </summary>
        /// <param name="portCode">The port code</param>
        public PortOfClearance GetByCode(string portCode) =>
            Set.FirstOrDefault(x =>
                x.Code.Equals(portCode, StringComparison.InvariantCultureIgnoreCase));
    }
}
