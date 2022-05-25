using System;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of DefValuesManual
    /// </summary>
    public interface IDefValuesManualRepository<TDefValuesManual> : IRepositoryWithTypeId<TDefValuesManual, int>
        where TDefValuesManual : BaseDefValuesManual
    {
        /// <summary>
        /// Updates filing parameters
        /// </summary>
        /// <param name="filingParameters">The filing parameters</param>
        void UpdateValues(InboundRecordFilingParameters filingParameters);

        /// <summary>
        /// Updates filing parameters
        /// </summary>
        /// <param name="filingParameters">The filing parameters</param>
        (int inserted, int updated) ImportValues(IEnumerable<ImportFormParameter> filingParameters);

        /// <summary>
        /// Recalculates values on form values change
        /// </summary>
        /// <param name="filingParameters">Current form values</param>
        InboundRecordFilingParameters ProcessChanges(InboundRecordFilingParameters filingParameters);
    }
}