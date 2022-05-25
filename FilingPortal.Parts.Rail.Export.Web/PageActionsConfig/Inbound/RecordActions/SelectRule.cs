﻿using System.Linq;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Domain.Enums;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Rail.Export.Web.PageActionsConfig.Inbound.RecordActions
{
    /// <summary>
    /// Class describing the Select action availability for Inbound Record
    /// </summary>
    public class SelectRule : IAvailableRule<InboundReadModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.ViewInboundRecord };

        /// <summary>
        /// Determines whether the Select action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(InboundReadModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && model.CanBeSelected();
        }
    }
}