using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories.Pipeline;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Provides the repository of <see cref="PipelineFilingData"/>
    /// </summary>
    public class PipelineFilingDataRepository : SearchRepository<PipelineFilingData>, IPipelineFilingDataRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineFilingDataRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineFilingDataRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Gets unique data for selected filing headers
        /// </summary>
        /// <param name="ids">Filing Headers ids</param>
        public IList<PipelineFilingData> GetByFilingNumbers(params int[] ids) =>
            Set.Where(x => ids.Contains(x.FilingHeaderId)).ToList();
    }
}
