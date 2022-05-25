using System.Collections.Generic;
using FilingPortal.Domain.Entities.Handbooks;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Common
{
    /// <summary>
    /// Describes the Transport Mode repository
    /// </summary>
    public interface ITransportModeRepository : ISearchRepository<TransportMode>
    {
        /// <summary>
        /// Searches for the Transport Mode for US
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        IEnumerable<TransportMode> SearchForUS(string searchInfo, int limit);

        /// <summary>
        /// Gets the Transport Mode specified by number
        /// </summary>
        /// <param name="transportModeNumber">The Transport Mode Number</param>
        TransportMode GetByNumber(string transportModeNumber);

        /// <summary>
        /// Searches for the Canada Transport Mode
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        IEnumerable<TransportMode> SearchForCanada(string searchInfo, int limit);

        /// <summary>
        /// Gets the Canada Transport Mode specified by code 
        /// </summary>
        /// <param name="transportModeCode">The Transport Mode Code</param>
        TransportMode GetByCodeForCanada(string transportModeCode);

        /// <summary>
        /// Returns all codes without duplicates
        /// </summary>
        IEnumerable<string> GetCodes();
    }
}
