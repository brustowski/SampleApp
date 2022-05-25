using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Vessel;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.AgileSettings;

namespace FilingPortal.DataLayer.Repositories.VesselImport

{
    /// <summary>
    /// Class for repository of <see cref="VesselImportDefValue"/>
    /// </summary>
    internal class VesselImportDefValuesRepository : BaseDefValuesRepository<VesselImportDefValue, VesselImportSection>
        , IAgileConfiguration<VesselImportDefValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDefValuesRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselImportDefValuesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Get the columns configuration <see cref="AgileField"/> for the grid
        /// </summary>
        public new IEnumerable<AgileField> GetFields()
        {
            return Set
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
}
