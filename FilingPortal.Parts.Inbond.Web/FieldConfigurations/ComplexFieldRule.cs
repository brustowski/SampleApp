using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Repositories;
using FilingPortal.PluginEngine.FieldConfigurations;

namespace FilingPortal.Parts.Inbond.Web.FieldConfigurations
{
    /// <summary>
    /// Implements repository-driven dropdown rule
    /// </summary>
    public class ComplexFieldRule : IComplexFieldsRule<DefValueManualReadModel>
    {
        private readonly IFilingHeaderRepository _filingHeadersRepository;

        public ComplexFieldRule(IFilingHeaderRepository filingHeadersRepository)
        {
            _filingHeadersRepository = filingHeadersRepository;
        }

        /// <summary>
        /// Returns true if field is complex
        /// </summary>
        /// <param name="model">Configuration field description</param>
        public bool IsComplexField(DefValueManualReadModel model)
        {
            var filingHeader = _filingHeadersRepository.Get(model.FilingHeaderId);

            return !string.IsNullOrWhiteSpace(model.PairedFieldTable) &&
                   !string.IsNullOrWhiteSpace(model.PairedFieldColumn) && (
                       filingHeader.ConfirmationNeeded && model.ConfirmationNeeded || !model.ConfirmationNeeded
                   );
        }

        /// <summary>
        /// Returns true if field is paired with any other
        /// </summary>
        /// <param name="model">Configuration field description</param>
        /// <param name="allModels">All available models</param>
        public bool IsPairedField(DefValueManualReadModel model, IEnumerable<DefValueManualReadModel> allModels)
        {
            var filingHeader = _filingHeadersRepository.Get(model.FilingHeaderId);

            return allModels?.Any(x =>
                       x.FilingHeaderId == model.FilingHeaderId
                       && (filingHeader.ConfirmationNeeded && x.ConfirmationNeeded || !x.ConfirmationNeeded)
                       && String.Compare(x.TableName, model.TableName, StringComparison.OrdinalIgnoreCase) == 0
                       && String.Compare(x.PairedFieldColumn, model.ColumnName, StringComparison.OrdinalIgnoreCase) == 0) ?? false;
        }
    }
}