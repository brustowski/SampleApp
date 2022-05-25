using System;
using System.Collections.Generic;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Parts.CanadaTruckImport.Domain.Config;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.RuleVendor
{
    /// <summary>
    /// Represents the import configuration
    /// </summary>
    internal class Configuration : IImportConfiguration
    {
        /// <summary>
        /// Gets the template name
        /// </summary>
        public string Name => GridNames.RuleVendor;

        /// <summary>
        /// Gets the template File Name
        /// </summary>
        public string FileName { get; set; } = $"{GridNames.RuleVendor}_template";

        /// <summary>
        /// Gets the template underlying model type
        /// </summary>
        public Type ModelType => typeof(ImportModel);
        /// <summary>
        /// Gets the result entity model
        /// </summary>
        public Type ResultType => typeof(Entities.RuleVendor);
        /// <summary>
        /// Gets the required permission collection
        /// </summary>
        public IEnumerable<int> Permissions => new[] { (int)Enums.Permissions.EditRules };
    }
}
