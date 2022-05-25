﻿using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Commands;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Truck
{
    /// <summary>
    /// Class describing the Cancel action availability for Inbound Record list
    /// </summary>
    public class TruckInboundListCancelRule : IAvailableRule<List<TruckInboundReadModel>>
    {
        /// <summary>
        /// Defines required permissions
        /// </summary>
        private readonly Permissions[] _permissions = new[] { Permissions.TruckFileInboundRecord };

        /// <summary>
        /// Determines whether the Cancel action is available for the specified list of Inbound Record models
        /// </summary>
        /// <param name="models">The list of Inbound Record models</param>
        /// <param name="resourceRequestor">The Resource Requestor</param>
        public bool IsAvailable(List<TruckInboundReadModel> models, IPermissionChecker resourceRequestor)
        {
            return models.Any()
                && resourceRequestor.HasPermissions(_permissions.Cast<int>())
                && models.All(x => x.CanBeCanceled());
        }
    }
}