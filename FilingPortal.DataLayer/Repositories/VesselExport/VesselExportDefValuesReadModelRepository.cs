using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories.VesselExport;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.AgileSettings;

namespace FilingPortal.DataLayer.Repositories.VesselExport
{
    /// <summary>
    /// Represents the repository of the <see cref="VesselExportDefValueReadModel"/>
    /// </summary>
    internal class VesselExportDefValuesReadModelRepository : SearchRepositoryWithTypedId<VesselExportDefValueReadModel, int>
        , IVesselExportDefValuesReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportDefValuesReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselExportDefValuesReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
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
