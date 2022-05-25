﻿using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Vessel
{
    /// <summary>
    /// Class describing the Add action availability for Vessel Rules Page Configuration
    /// </summary>
    public class VesselRulesPageAddRule : IAvailableRule<PageConfigurationModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.VesselEditImportRecordRules};

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