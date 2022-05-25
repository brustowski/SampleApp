using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.AppSystem
{
    /// <summary>
    /// Defines the Application Administrator
    /// </summary>
    public class AppAdmin : Entity
    {
        /// <summary>
        /// Gets or sets Administrator Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Administrator Full Name
        /// </summary>
        public string FullName { get; set; }
    }
}
