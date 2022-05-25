using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Represents the repository of the <see cref="TransportMode" />
    /// </summary>
    public class TransportModeRepository : SearchRepository<TransportMode>, ITransportModeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransportModeRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public TransportModeRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Searches for the Transport Mode for US
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IEnumerable<TransportMode> SearchForUS(string searchInfo, int limit)
        {
            IQueryable<TransportMode> query = Set.Where(x => x.Country.Equals("US"));

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.CodeNumber.StartsWith(searchInfo) || x.Code.StartsWith(searchInfo) || x.Description.Contains(searchInfo));
            }

            return query.OrderBy(x=>x.CodeNumber).Take(limit).ToList();
        }

        /// <summary>
        /// Gets the Transport Mode specified by number
        /// </summary>
        /// <param name="transportModeNumber">The Transport Mode Number</param>
        public TransportMode GetByNumber(string transportModeNumber)
        {
            return Set.FirstOrDefault(x => x.CodeNumber.Equals(transportModeNumber));
        }

        /// <summary>
        /// Searches for the Canada Transport Mode
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IEnumerable<TransportMode> SearchForCanada(string searchInfo, int limit)
        {
            IQueryable<TransportMode> query = Set.Where(x => x.Country.Equals("CA"));

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.Code.StartsWith(searchInfo) || x.Description.Contains(searchInfo));
            }

            return query.OrderBy(x => x.CodeNumber).Take(limit).ToList();
        }

        /// <summary>
        /// Gets the Canada Transport Mode specified by code 
        /// </summary>
        /// <param name="transportModeCode">The Transport Mode Code</param>
        public TransportMode GetByCodeForCanada(string transportModeCode)
        {
            return Set.FirstOrDefault(x => x.Country.Equals("CA") && x.Code.Equals(transportModeCode));
        }

        /// <summary>
        /// Returns all codes without duplicates
        /// </summary>
        public IEnumerable<string> GetCodes()
        {
            return Set.Select(x => x.Code).Distinct();
        }
    }
}
