using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Represents the repository of the <see cref="PipelineDefValue"/>
    /// </summary>
    public class PipelineDefValuesRepository : BaseDefValuesRepository<PipelineDefValue, PipelineSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDefValuesRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineDefValuesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
    }
}
