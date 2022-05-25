using System.Collections.Generic;
using FilingPortal.Cargowise.Domain.Entities;
using Framework.Domain.Repositories;

namespace FilingPortal.Cargowise.Domain.Repositories
{
    /// <summary>
    /// Describes methods working with ports of clearance
    /// </summary>
    public interface IPortsOfClearanceRepository : ISearchRepository<PortOfClearance>
    {
        /// <summary>
        /// Searches for ports in repository
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        IList<PortOfClearance> Search(string searchInfoSearch, int searchInfoLimit);

        /// <summary>
        /// Checks whether record with specified port code exists in the table
        /// </summary>
        /// <param name="portCode">Port code</param>
        bool IsExist(string portCode);

        /// <summary>
        /// Returns port code if exists
        /// </summary>
        /// <param name="portCode">The port code</param>
        PortOfClearance GetByCode(string portCode);
    }
}
