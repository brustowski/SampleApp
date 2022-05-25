using System;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Service describing File Procedure with option to file consolidated
    /// </summary>
    /// <typeparam name="TFilingHeader">Filing header type</typeparam>
    /// <typeparam name="TDefValuesManual">DefValues_Manual type</typeparam>
    public class ConsolidatedFilingWorkflow<TFilingHeader, TDefValuesManual> : FilingWorkflow<TFilingHeader, TDefValuesManual>, IConsolidatedFilingWorkflow<TFilingHeader, TDefValuesManual>
        where TFilingHeader : FilingHeaderOld, new()
        where TDefValuesManual : BaseDefValuesManual
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsolidatedFilingWorkflow{TFilingHeader, TDefValuesManual}"/> class.
        /// </summary>
        /// <param name="repository">The filing headers repository</param>
        /// <param name="defValuesRepository">The defValues_Manual Repository</param>
        public ConsolidatedFilingWorkflow(
            IFilingHeaderRepository<TFilingHeader> repository,
            IDefValuesManualRepository<TDefValuesManual> defValuesRepository) : base(repository, defValuesRepository)
        {
        }

        /// <summary>
        /// Creates the initial filing header by specified inbound record ids
        /// </summary>
        /// <param name="header">The header<see cref="TFilingHeader"/></param>
        public OperationResultWithValue<int> StartUnitTradeFiling(TFilingHeader header)
        {
            try
            {
                FilingRepository.Add(header);
                FilingRepository.Save();

                return RunFilingProcedure(header.Id, header.CreatedUser);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "An error occurred during preparing set for filing.");
                var result = new OperationResultWithValue<int>();
                result.AddErrorMessage(ErrorMessages.CreateInitialFilingHeaderError);
                return result;
            }
        }
    }
}
