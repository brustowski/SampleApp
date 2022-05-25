﻿using System.Linq;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.PageConfigs.Audit.Rail
{
    /// <summary>
    /// Class describing the Import action availability for the Audit Rail Train consist sheet
    /// </summary>
    public class AuditRailTrainConsistSheetPageImport : IAvailableRule<PageConfigurationModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.AuditRailImportTrainConsistSheet };

        /// <summary>
        /// Determines whether the action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(PageConfigurationModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>());
        }
    }
}