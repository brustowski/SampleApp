using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.TruckExport
{
    /// <summary>
    /// Class for repository of <see cref="TruckExportFilingHeader"/>
    /// </summary>
    public class TruckExportFilingHeadersRepository : BaseFilingHeadersRepository<TruckExportFilingHeader>, ITruckExportFilingHeadersRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportFilingHeadersRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckExportFilingHeadersRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// The name of the create entry record stored procedure
        /// </summary>
        protected override string CreateEntryProcedureName => "sp_exp_truck_create_entry_records";

        /// <summary>
        /// The name of the delete entry record stored procedure
        /// </summary>
        protected override string DeleteEntryProcedureName => "sp_exp_truck_delete_entry_records";

        /// <summary>
        /// The name of the refile entry record stored procedure
        /// </summary>
        protected override string RefileEntryProcedureName => "sp_exp_truck_refile_entry";

        /// <summary>
        /// Finds the Filing Headers by record identifiers
        /// </summary>
        /// <param name="ids">The record identifiers</param>
        public override IEnumerable<TruckExportFilingHeader> FindByInboundRecordIds(IEnumerable<int> ids)
        {
            return Set.Where(x => x.TruckExports.Select(r => r.Id).Intersect(ids).Any());
        }

        /// <summary>
        /// Gets the section by its name
        /// </summary>
        /// <param name="sectionName">The section name</param>
        protected override BaseSection GetSection(string sectionName)
        {
            var context = (FilingPortalContext)UnitOfWork.Context;
            return context.TruckExportSections.FirstOrDefault(x => x.Name == sectionName);
        }
    }
}