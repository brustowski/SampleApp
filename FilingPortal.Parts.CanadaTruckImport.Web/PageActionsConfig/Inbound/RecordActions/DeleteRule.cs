﻿using System.Linq;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.CanadaTruckImport.Web.PageActionsConfig.Inbound.RecordActions
{
    /// <summary>
    /// Class describing the Delete action availability for Inbound Record
    /// </summary>
    public class DeleteRule : IAvailableRule<InboundReadModel>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = { Permissions.DeleteInboundRecord };

        /// <summary>
        /// Determines whether the Delete action is available for the specified model
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(InboundReadModel model, IPermissionChecker resourceRequestor)
        {
            return resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && model.CanBeDeleted();
        }
    }
}