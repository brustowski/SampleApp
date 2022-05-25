using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using Framework.Domain;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    public interface IInboundRecordsRepository<TInboundType> : ISearchRepository<TInboundType> where TInboundType : Entity, IInboundType
    {
        /// <summary>
        /// Gets inbound records by filing header id
        /// </summary>
        /// <param name="filingHeaderId">Filing header id</param>
        IEnumerable<TInboundType> GetByFilingId(int filingHeaderId);
    }
}
