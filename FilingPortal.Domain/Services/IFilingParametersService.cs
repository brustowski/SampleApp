using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Describes methods to work with filing parameters 
    /// </summary>
    /// <typeparam name="TDefValuesManual">Form configuration type</typeparam>
    /// <typeparam name="TDefValuesManualReadModel">Form configuration with value type</typeparam>
    public interface IFilingParametersService<TDefValuesManual, TDefValuesManualReadModel>
        where TDefValuesManual : BaseDefValuesManual
        where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Updates the filing parameters
        /// </summary>
        /// <param name="parameters">The filing parameters</param>
        void UpdateFilingParameters(InboundRecordFilingParameters parameters);

        /// <summary>
        /// Recalculates values on form values change
        /// </summary>
        /// <param name="filingParameters">Current form values</param>
        InboundRecordFilingParameters ProcessChanges(InboundRecordFilingParameters filingParameters);
    }
}
