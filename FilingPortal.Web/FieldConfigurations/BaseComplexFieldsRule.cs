using System.Collections.Generic;
using FilingPortal.Domain.Common;
using FilingPortal.Parts.Common.Domain.Common;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.PluginEngine.FieldConfigurations;

namespace FilingPortal.Web.FieldConfigurations
{
    /// <summary>
    /// Defines the abstract complex field rule
    /// </summary>
    public abstract class BaseComplexFieldsRule<TDefValuesManualReadModel> : IComplexFieldsRule<TDefValuesManualReadModel>
    where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Gets the main field's table name
        /// </summary>
        protected abstract string MainFieldColumnName { get; }

        /// <summary>
        /// Gets the main field's columns name
        /// </summary>
        protected abstract string MainFieldTable { get; }

        /// <summary>
        /// Gets the paired field table name
        /// </summary>
        protected abstract string PairedFieldColumnName { get; }

        /// <summary>
        /// Gets the paired field's columns name
        /// </summary>
        protected abstract string PairedFieldTable { get; }

        /// <summary>
        /// Returns if model is main complex field
        /// </summary>
        /// <param name="model">The model <see cref="BaseDefValuesManualReadModel"/></param>
        public bool IsComplexField(TDefValuesManualReadModel model) =>
            model.GetUniqueData() == new DefValuesUniqueData(MainFieldTable, MainFieldColumnName);

        /// <summary>
        /// Returns true if field is paired with any other
        /// </summary>
        /// <param name="model">Configuration field description</param>
        /// <param name="allModels">All available models</param>
        public bool IsPairedField(TDefValuesManualReadModel model, IEnumerable<TDefValuesManualReadModel> allModels) =>
            model.GetUniqueData() == new DefValuesUniqueData(PairedFieldTable, PairedFieldColumnName);
    }
}
