using System;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Clients
{
    /// <summary>
    /// Defines Client Address model
    /// </summary>
    public class ClientAddress : EntityWithTypedId<Guid>
    {
        /// <summary>
        /// Gets or sets the Client identifier
        /// </summary>
        public Guid ClientId { get; set; }
        /// <summary>
        /// Gets or sets the Address Short Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the record's last updated time
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }
    }
}
