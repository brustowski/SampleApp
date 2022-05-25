﻿using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Class describing the Restore action availability for the list of the Rail Inbound Records
    /// </summary>
    public class RailListRestoreRule : IAvailableRule<List<RailInboundReadModel>>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.RailDeleteInboundRecord };

        /// <summary>
        /// Determines whether the Restore action is available for the specified list of Inbound Record models
        /// </summary>
        /// <param name="models">The list of Inbound Record models</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(List<RailInboundReadModel> models, IPermissionChecker resourceRequestor)
        {
            return models.Any()
                && resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && models.All(x => x.CanBeRestored());
        }
    }
}