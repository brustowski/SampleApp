using System.Collections.Generic;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.PluginEngine.FieldConfigurations
{
    /// <summary>
    /// Defines rules for complex fields
    /// </summary>
    public interface IComplexFieldsRule<in TDefValuesManualReadModel> where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Returns true if field is complex
        /// </summary>
        /// <param name="model">Configuration field description</param>
        bool IsComplexField(TDefValuesManualReadModel model);

        /// <summary>
        /// Returns true if field is paired with any other
        /// </summary>
        /// <param name="model">Configuration field description</param>
        /// <param name="allModels">All available models</param>
        bool IsPairedField(TDefValuesManualReadModel model, IEnumerable<TDefValuesManualReadModel> allModels);
    }
}