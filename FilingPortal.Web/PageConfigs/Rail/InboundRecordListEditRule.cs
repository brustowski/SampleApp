﻿using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Web.PageConfigs.Common;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Class describing the Edit action availability for Inbound Record list
    /// </summary>
    public class InboundRecordListEditRule : IAvailableRule<List<RailInboundReadModel>>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.RailFileInboundRecord };

        /// <summary>
        /// Determines whether the Edit action is available for the specified list of Inbound Record models
        /// </summary>
        /// <param name="models">The list of Inbound Record models</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(List<RailInboundReadModel> models, IPermissionChecker resourceRequestor)
        {
            return models.Any()
                && resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && models.All(x => x.CanBeEdited());
        }
    }
}