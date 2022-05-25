using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.DTOs;
using Framework.DataLayer;
using Newtonsoft.Json;

namespace FilingPortal.DataLayer.Repositories.TruckExport
{
    /// <summary>
    /// Class for repository of <see cref="TruckExportDefValueManual"/>
    /// </summary>
    public class TruckExportDefValuesManualRepository : BaseDefValuesManualRepository<TruckExportDefValueManual>, ITruckExportDefValuesManualRepository
    {
        /// <summary>
        /// Gets the name of the update entry stored procedure
        /// </summary>
        protected override string UpdateEntryProcedureName => "sp_exp_truck_update_entry";

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportDefValuesManualRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckExportDefValuesManualRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Recalculates values on form values change
        /// </summary>
        /// <param name="filingParameters">Current form values</param>
        public override InboundRecordFilingParameters ProcessChanges(InboundRecordFilingParameters filingParameters) =>
            filingParameters;
    }
}
