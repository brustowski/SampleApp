using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories.VesselImport;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.AgileSettings;

namespace FilingPortal.DataLayer.Repositories.VesselImport
{
    /// <summary>
    /// Represents the repository of the <see cref="VesselImportDefValueReadModel"/>
    /// </summary>
    internal class VesselImportDefValuesReadModelRepository : SearchRepositoryWithTypedId<VesselImportDefValueReadModel, int>
        , IVesselImportDefValuesReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDefValuesReadModelRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselImportDefValuesReadModelRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

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
