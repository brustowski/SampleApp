using System.Collections.Generic;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.AppSystem
{
    /// <summary>
    /// Defines the Application Permission
    /// </summary>
    public class AppPermission : Entity
    {
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Roles
        /// </summary>
        public virtual ICollection<AppRole> Roles { get; set; }
    }
}
