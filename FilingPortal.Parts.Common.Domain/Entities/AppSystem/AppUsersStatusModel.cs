using System.Collections.Generic;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.AppSystem
{
    /// <summary>
    /// Defines the <see cref="AppUsersStatusModel" />
    /// </summary>
    public class AppUsersStatusModel : Entity
    {
        /// <summary>
        /// Gets or sets the AppUsers collection with corresponding status
        /// </summary>
        public virtual ICollection<AppUsersModel> AppUsers { get; set; } = new List<AppUsersModel>();

        /// <summary>
        /// Gets or sets the Status Name
        /// </summary>
        public string Name { get; set; }
    }
}
