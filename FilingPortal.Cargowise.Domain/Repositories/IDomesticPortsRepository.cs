using System.Collections.Generic;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Cargowise.Domain.Repositories
{
    /// <summary>
    /// Describes repository of the <see cref="DomesticPort"/>
    /// </summary>
    public interface IDomesticPortsRepository : IDataProviderRepository<DomesticPort, int>
    {
        /// <summary>
        /// Searches for UNLOCO in the repository specified by search request
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        IEnumerable<string> SearchUNLOCO(string searchInfo, int limit);
        /// <summary>
        /// Checks whether specified port code exists in the table
        /// </summary>
        /// <param name="port">The Port code</param>
        bool IsExist(string port);
    }
}
