using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Commands;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.AppSystem
{
    /// <summary>
    /// Defines the User
    /// </summary>
    public class AppUsersModel : EntityWithTypedId<string>, IPermissionChecker
    {
        /// <summary>
        /// Gets or sets the Request from non-registered user
        /// </summary>
        public string RequestInfo { get; set; }

        /// <summary>
        /// Gets or sets the Status of user
        /// </summary>
        public virtual AppUsersStatusModel Status { get; set; }

        /// <summary>
        /// Gets or sets the StatusId - foreign key to status model
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Gets or sets the roles of the user
        /// </summary>
        public virtual ICollection<AppRole> Roles { get; set; }

        /// <summary>
        /// Checks if user has specified permissions
        /// </summary>
        /// <param name="permissions">Numeric value of permission</param>
        public bool HasPermissions(IEnumerable<int> permissions) => Roles.Any(x => x.Permissions.Any(p => permissions.Contains(p.Id)));
    }
}
