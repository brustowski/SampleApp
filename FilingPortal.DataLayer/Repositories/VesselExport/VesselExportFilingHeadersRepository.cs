using System;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories.VesselExport;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Infrastructure;

namespace FilingPortal.DataLayer.Repositories.VesselExport
{
    /// <summary>
    /// Class for repository of <see cref="VesselExportFilingHeader"/>
    /// </summary>
    public class VesselExportFilingHeadersRepository : BaseFilingHeadersRepository<VesselExportFilingHeader>, IVesselExportFilingHeadersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportFilingHeadersRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselExportFilingHeadersRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
        /// <summary>
        /// The name of the create entry record stored procedure
        /// </summary>
        protected override string CreateEntryProcedureName => "sp_exp_vessel_create_entry_records";
        /// <summary>
        /// The name of the delete entry record stored procedure
        /// </summary>
        protected override string DeleteEntryProcedureName => "sp_exp_vessel_delete_entry_records";
        /// <summary>
        /// Finds the VesselFilingHeaders by Vessel identifiers
        /// </summary>
        /// <param name="vesselIds">The Vessel identifiers</param>
        public override IEnumerable<VesselExportFilingHeader> FindByInboundRecordIds(IEnumerable<int> vesselIds) =>
            Set.Where(x => x.VesselExports.Select(r => r.Id).Intersect(vesselIds).Any());

        /// <summary>
        /// Gets the section by its name
        /// </summary>
        /// <param name="sectionName">The section name</param>
        protected override BaseSection GetSection(string sectionName)
        {
            var context = (FilingPortalContext)UnitOfWork.Context;
            return context.VesselExportSections.FirstOrDefault(x => x.Name == sectionName);
        }
    }
}