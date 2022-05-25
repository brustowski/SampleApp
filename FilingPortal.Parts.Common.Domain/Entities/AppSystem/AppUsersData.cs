using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.AppSystem
{
    /// <summary>
    /// Defines the User additional data
    /// </summary>
    public class AppUsersData : EntityWithTypedId<string>
    {
        /// <summary>
        /// App user Branch
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// App user Broker
        /// </summary>
        public string Broker { get; set; }

        /// <summary>
        /// App user Location
        /// </summary>
        public string Location { get; set; }
    }
}
