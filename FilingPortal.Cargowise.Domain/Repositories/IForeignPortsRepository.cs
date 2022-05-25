using System.Collections.Generic;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Cargowise.Domain.Repositories
{
    /// <summary>
    /// Describes Foreign Port repository
    /// </summary>
    public interface IForeignPortsRepository : IDataProviderRepository<ForeignPort, int>
    {
        /// <summary>
        /// Gets Foreign port by port code
        /// </summary>
        /// <param name="portCode">The port code</param>
        ForeignPort GetByCode(string portCode);
        /// <summary>
        /// Gets the collection of the ports specified by country code
        /// </summary>
        /// <param name="countryCode">Country code</param>
        IEnumerable<ForeignPort> GetCountryPorts(string countryCode);
    }
}