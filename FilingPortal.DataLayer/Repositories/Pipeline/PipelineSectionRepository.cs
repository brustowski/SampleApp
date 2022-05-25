using FilingPortal.Domain.Entities.Pipeline;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Represents repository of the Pipeline Section entity
    /// </summary>
    class PipelineSectionRepository : SearchRepository<PipelineSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineSectionRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineSectionRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
    }
}