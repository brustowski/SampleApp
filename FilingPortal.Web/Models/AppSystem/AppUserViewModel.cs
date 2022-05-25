using FilingPortal.Domain.Enums;
using Reinforced.Typings.Attributes;
using System.Collections.Generic;

namespace FilingPortal.Web.Models.AppSystem
{
    /// <summary>
    /// Application User view model file
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class AppUserViewModel
    {
        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the UserAccount
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// Gets or sets user permissions
        /// </summary>
        public IEnumerable<int> Permissions { get; set; }
    }
}