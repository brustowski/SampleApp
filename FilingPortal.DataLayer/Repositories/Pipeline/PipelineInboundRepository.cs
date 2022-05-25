using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories.Pipeline;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Class for repository of <see cref="PipelineInbound"/>
    /// </summary>
    public class PipelineInboundRepository : SearchRepository<PipelineInbound>, IPipelineInboundRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineInboundRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Gets inbound records by filing header id
        /// </summary>
        /// <param name="filingHeaderId">Filing header id</param>
        public IEnumerable<PipelineInbound> GetByFilingId(int filingHeaderId)
        {
            return Set.Where(x => x.FilingHeaders.Select(f => f.Id).Contains(filingHeaderId));
        }
    }
}
