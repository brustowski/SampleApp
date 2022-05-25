using System;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Clients
{
    /// <summary>
    /// Defines Client Contact additional phone model
    /// </summary>
    public class ClientContactAdditionalPhone : EntityWithTypedId<int>
    {
        /// <summary>
        /// Gets or sets the Contact identifier
        /// </summary>
        public Guid ContactId { get; set; }
        /// <summary>
        /// Gets or sets Phone Number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets Contact
        /// </summary>
        public ClientContact Contact { get; set; }
    }
}
