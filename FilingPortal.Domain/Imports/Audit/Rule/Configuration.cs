using System;
using System.Collections.Generic;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Entities.Audit.Rail;

namespace FilingPortal.Domain.Imports.Audit.Rule
{
    /// <summary>
    /// Represents the import configuration
    /// </summary>
    internal class Configuration : IImportConfiguration
    {
        /// <summary>
        /// Gets the template name
        /// </summary>
        public string Name => GridNames.AuditRailDailyAuditRules;

        /// <summary>
        /// Gets the template File Name
        /// </summary>
        public string FileName { get; set; } = $"{GridNames.AuditRailDailyAuditRules}_template";

        /// <summary>
        /// Gets the template underlying model type
        /// </summary>
        public Type ModelType => typeof(DailyAuditRuleImportModel);

        /// <summary>
        /// Gets the result entity model
        /// </summary>
        public Type ResultType => typeof(AuditRailDailyRule);

        /// <summary>
        /// Gets the required permission collection
        /// </summary>
        public IEnumerable<int> Permissions => new[] { (int)Enums.Permissions.AuditRailDailyAudit };
    }
}
