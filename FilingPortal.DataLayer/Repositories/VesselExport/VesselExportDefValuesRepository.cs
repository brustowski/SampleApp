using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.VesselExport;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.AgileSettings;

namespace FilingPortal.DataLayer.Repositories.VesselExport
{
    /// <summary>
    /// Class for repository of <see cref="VesselExportDefValue"/>
    /// </summary>
    internal class VesselExportDefValuesRepository : BaseDefValuesRepository<VesselExportDefValue, VesselExportSection>
        , IAgileConfiguration<VesselExportDefValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportDefValuesRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselExportDefValuesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Get the columns configuration <see cref="AgileField"/> for the grid
        /// </summary>
        public new IEnumerable<AgileField> GetFields() => Set
                 .Where(x => x.SingleFilingOrder.HasValue)
                 .OrderBy(x => x.SingleFilingOrder)
                 .Select(x => new AgileField
                 {
                     ColumnName = x.ColumnName,
                     TableName = x.Section.TableName,
                     DisplayName = x.Label
                 }).ToList();
    }
}
