using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.AgileSettings;

namespace FilingPortal.DataLayer.Repositories.TruckExport
{
    /// <summary>
    /// Represents the repository of the <see cref="TruckExportDefValueReadModel"/>
    /// </summary>
    internal class TruckExportDefValuesReadModelRepository : SearchRepositoryWithTypedId<TruckExportDefValueReadModel, int>, ITruckExportDefValuesReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportDefValuesReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckExportDefValuesReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
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
