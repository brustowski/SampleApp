using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories.VesselImport;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.DataLayer.Repositories.VesselImport
{
    /// <summary>
    /// Class for repository of <see cref="VesselImportFilingHeader"/>
    /// </summary>
    public class VesselImportFilingHeadersRepository : BaseFilingHeadersRepository<VesselImportFilingHeader>, IVesselImportFilingHeadersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportFilingHeadersRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselImportFilingHeadersRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
        /// <summary>
        /// The name of the create entry record stored procedure
        /// </summary>
        protected override string CreateEntryProcedureName => "sp_imp_vessel_create_entry_records";
        /// <summary>
        /// The name of the delete entry record stored procedure
        /// </summary>
        protected override string DeleteEntryProcedureName => "sp_imp_vessel_delete_entry_records";
        /// <summary>
        /// Finds the VesselFilingHeaders by Vessel identifiers
        /// </summary>
        /// <param name="vesselIds">The Vessel identifiers</param>
        public override IEnumerable<VesselImportFilingHeader> FindByInboundRecordIds(IEnumerable<int> vesselIds) =>
            Set.Where(x => x.VesselInbounds.Select(r => r.Id).Intersect(vesselIds).Any());

        /// <summary>
        /// Gets the section by its name
        /// </summary>
        /// <param name="sectionName">The section name</param>
        protected override BaseSection GetSection(string sectionName)
        {
            var context = (FilingPortalContext)UnitOfWork.Context;
            return context.VesselImportSections.FirstOrDefault(x => x.Name == sectionName);
        }
    }
}