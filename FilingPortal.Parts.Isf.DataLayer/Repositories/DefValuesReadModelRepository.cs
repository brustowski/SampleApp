using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Isf.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Isf.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="DefValueReadModel"/>
    /// </summary>
    public class DefValuesReadModelRepository : SearchRepository<DefValueReadModel>, IDefValuesReadModelRepository<DefValueReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValuesReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public DefValuesReadModelRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
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