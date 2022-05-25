using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.PluginEngine.FieldConfigurations;

namespace FilingPortal.Web.FieldConfigurations
{
    /// <summary>
    /// Implements repository-driven dropdown rule
    /// </summary>
    /// <typeparam name="TDefValuesManualReadModel">Configuration field type</typeparam>
    public class AgileComplexFieldRule<TDefValuesManualReadModel> : IComplexFieldsRule<TDefValuesManualReadModel> where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Returns true if field is complex
        /// </summary>
        /// <param name="model">Configuration field description</param>
        public bool IsComplexField(TDefValuesManualReadModel model) => !string.IsNullOrWhiteSpace(model.PairedFieldTable) && !string.IsNullOrWhiteSpace(model.PairedFieldColumn);
        /// <summary>
        /// Returns true if field is paired with any other
        /// </summary>
        /// <param name="model">Configuration field description</param>
        /// <param name="allModels">All available models</param>
        public bool IsPairedField(TDefValuesManualReadModel model, IEnumerable<TDefValuesManualReadModel> allModels)
        {
            return allModels?.Any(x =>
                       x.FilingHeaderId == model.FilingHeaderId
                       && String.Compare(x.TableName, model.TableName, StringComparison.OrdinalIgnoreCase) == 0
                       && String.Compare(x.PairedFieldColumn, model.ColumnName, StringComparison.OrdinalIgnoreCase) == 0) ?? false;
        }
    }
}