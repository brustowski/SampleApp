using System;
using System.Collections.Generic;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Clients
{
    /// <summary>
    /// Defines Client Contact model
    /// </summary>
    public class ClientContact : EntityWithTypedId<Guid>
    {
        /// <summary>
        /// Gets or sets the Client identifier
        /// </summary>
        public Guid ClientId { get; set; }
        /// <summary>
        /// Gets or sets Contact name
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// Gets or sets Work Phone
        /// </summary>
        public string WorkPhone { get; set; }
        /// <summary>
        /// Gets or sets Mobile Phone
        /// </summary>
        public string MobilePhone { get; set; }
        /// <summary>
        /// Gets or sets Home Phone
        /// </summary>
        public string HomePhone { get; set; }
        /// <summary>
        /// Gets or sets Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets Client
        /// </summary>
        public virtual Client Client { get; set; }
        /// <summary>
        /// Gets or sets additional phones
        /// </summary>
        public virtual ICollection<ClientContactAdditionalPhone> AdditionalPhones { get; set; }
    }
}
