using System.Collections.Generic;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.PluginEngine.FieldConfigurations
{
    /// <summary>
    /// Builds inbound record fields
    /// </summary>
    public interface IInboundRecordFieldBuilder<TDefValuesManualReadModel> where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Creates fields description from def values
        /// </summary>
        /// <param name="models">Def values</param>
        IEnumerable<BaseInboundRecordField> CreateFrom(IEnumerable<TDefValuesManualReadModel> models);

        /// <summary>
        /// Creates fields description from def value
        /// </summary>
        /// <param name="model">Def value</param>
        /// <param name="allModels">Def values pool</param>
        BaseInboundRecordField CreateFrom(TDefValuesManualReadModel model,
            IList<TDefValuesManualReadModel> allModels);
    }
}