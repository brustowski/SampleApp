﻿using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Vessel
{
    /// <summary>
    /// Class describing the Edit action availability for Inbound Record
    /// </summary>
    public class VesselImportEditRule : IAvailableRule<VesselImportReadModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.VesselFileImportRecord };

        /// <summary>
        /// Determines whether the Edit action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(VesselImportReadModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && model.CanBeEdited();
        }
    }
}