using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Import;
using System;
using System.Collections.Generic;
using FilingPortal.Domain.Entities.TruckExport;

namespace FilingPortal.Domain.Imports.TruckExport.RuleConsignee
{
    /// <summary>
    /// Represents the import configuration
    /// </summary>
    internal class Configuration : IImportConfiguration
    {
        /// <summary>
        /// Gets the template name
        /// </summary>
        public string Name => GridNames.TruckExportRuleConsignee;

        /// <summary>
        /// Gets the template File Name
        /// </summary>
        public string FileName { get; set; } = $"{GridNames.TruckExportRuleConsignee}_template";

        /// <summary>
        /// Gets the template underlying model type
        /// </summary>
        public Type ModelType => typeof(ImportModel);
        /// <summary>
        /// Gets the result entity model
        /// </summary>
        public Type ResultType => typeof(TruckExportRuleConsignee);
        /// <summary>
        /// Gets the required permission collection
        /// </summary>
        public IEnumerable<int> Permissions => new[] { (int)Enums.Permissions.TruckEditExportRecordRules };
    }
}
