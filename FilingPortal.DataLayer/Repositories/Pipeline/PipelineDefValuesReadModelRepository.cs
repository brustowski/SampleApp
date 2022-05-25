using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Pipeline;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.AgileSettings;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Class for repository of <see cref="RailDefValuesManualReadModel"/>
    /// </summary>
    public class PipelineDefValuesReadModelRepository : SearchRepositoryWithTypedId<PipelineDefValueReadModel, int>, IPipelineDefValuesReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDefValuesReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineDefValuesReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }
        /// <summary>
        /// Get the columns configuration <see cref="AgileField"/> for the grid
        /// </summary>
        public IEnumerable<AgileField> GetFields()
        {
            return Set
                .Where(x => x.SingleFilingOrder.HasValue)
                .OrderBy(x => x.SingleFilingOrder)
                .Select(x => new AgileField
                {
                    ColumnName = x.ColumnName,
                    TableName = x.TableName,
                    DisplayName = x.Label,
                    TypeName = x.ValueType
                }).ToList();
        }
    }
}