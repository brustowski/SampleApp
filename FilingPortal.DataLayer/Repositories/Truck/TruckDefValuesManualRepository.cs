using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories.Truck;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.DTOs;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Truck
{
    /// <summary>
    /// Represents the repository of the <see cref="TruckDefValueManual"/>
    /// </summary>
    public class TruckDefValuesManualRepository : BaseDefValuesManualRepository<TruckDefValueManual>, ITruckDefValuesManualRepository
    {
        /// <summary>
        /// Gets the name of the update entry stored procedure
        /// </summary>
        protected override string UpdateEntryProcedureName => "sp_imp_truck_update_entry";

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDefValuesManualRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckDefValuesManualRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Recalculates values on form values change
        /// </summary>
        /// <param name="filingParameters">Current form values</param>
        public override InboundRecordFilingParameters ProcessChanges(InboundRecordFilingParameters filingParameters) =>
            filingParameters;
    }
}
