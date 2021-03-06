using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Import;
using System;
using System.Collections.Generic;
using FilingPortal.Domain.Entities.Truck;

namespace FilingPortal.Domain.Imports.Truck.RuleImporter
{
    /// <summary>
    /// Represents the import configuration
    /// </summary>
    internal class Configuration : IImportConfiguration
    {
        /// <summary>
        /// Gets the template name
        /// </summary>
        public string Name => GridNames.TruckRuleImporter;

        /// <summary>
        /// Gets the template File Name
        /// </summary>
        public string FileName { get; set; } = $"{GridNames.TruckRuleImporter}_template";

        /// <summary>
        /// Gets the template underlying model type
        /// </summary>
        public Type ModelType => typeof(ImportModel);
        /// <summary>
        /// Gets the result entity model
        /// </summary>
        public Type ResultType => typeof(TruckRuleImporter);
        /// <summary>
        /// Gets the required permission collection
        /// </summary>
        public IEnumerable<int> Permissions => new[] { (int)Enums.Permissions.TruckEditInboundRecordRules };
    }
}
