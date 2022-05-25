using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Service describing File Procedure with option to file consolidated
    /// </summary>
    public interface IConsolidatedFilingWorkflow<TFilingHeader, TDefValuesManual>: IFilingWorkflow<TFilingHeader, TDefValuesManual>
        where TFilingHeader: FilingHeaderOld
        where TDefValuesManual : BaseDefValuesManual
    {
        /// <summary>
        /// Creates initial data for unit trade filing
        /// </summary>
        /// <param name="header">Selected header</param>
        OperationResultWithValue<int> StartUnitTradeFiling(TFilingHeader header);
    }
}