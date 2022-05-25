﻿
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.PageConfigs.Configuration
{
    /// <summary>
    /// Class describing the Delete action availability for specified model
    /// </summary>
    public class DefValuesDeleteRule : IAvailableRule<DefValuesViewModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.DeleteConfiguration };

        /// <summary>
        /// Determines whether the Delete action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(DefValuesViewModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>());
        }
    }
}