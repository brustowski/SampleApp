using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.AgileSettings;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    /// <summary>
    /// Class for repository of <see cref="RailDefValuesManualReadModel"/>
    /// </summary>
    public class RailDefValuesReadModelRepository : SearchRepositoryWithTypedId<RailDefValuesReadModel, int>, IRailDefValuesReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailDefValuesManualReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public RailDefValuesReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

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