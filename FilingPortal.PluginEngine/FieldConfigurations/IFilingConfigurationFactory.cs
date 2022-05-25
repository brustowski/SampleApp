using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using System;

namespace FilingPortal.PluginEngine.FieldConfigurations
{
    /// <summary>
    /// Describes filing configuration factory
    /// </summary>
    /// <typeparam name="TModel">Type of the source type of the fields</typeparam>
    public interface IFilingConfigurationFactory<TModel>
    where TModel : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Create Filing configuration for specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">Filing header identifier</param>
        FilingConfiguration CreateConfiguration(int filingHeaderId);

        /// <summary>
        /// Create Filing configuration for specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">Filing header identifier</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="recordId">Record Id</param>
        FilingConfiguration CreateConfigurationForSection(int filingHeaderId, string sectionName, int recordId);
        /// <summary>
        /// Create Filing configuration for specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">Filing header identifier</param>
        /// <param name="sectionName">Section name</param>
        /// <param name="operationId">Unique operation id</param>
        FilingConfiguration CreateConfigurationForSection(int filingHeaderId, string sectionName, Guid operationId);
    }
}