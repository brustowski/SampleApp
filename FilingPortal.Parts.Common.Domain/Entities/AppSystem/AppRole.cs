using System.Collections.Generic;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.AppSystem
{
    /// <summary>
    /// Defines the Application User Role
    /// </summary>
    public class AppRole : Entity
    {
        /// <summary>
        /// Gets or sets the Users of the Role
        /// </summary>
        public virtual ICollection<AppUsersModel> AppUsers { get; set; }

        /// <summary>
        /// Gets or sets the Permissions of the Role
        /// </summary>
        public virtual ICollection<AppPermission> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }
    }
}
