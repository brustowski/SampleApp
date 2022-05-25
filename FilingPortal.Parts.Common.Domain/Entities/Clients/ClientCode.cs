using System;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Clients
{
    /// <summary>
    /// Represents the Client Code
    /// </summary>
    public class ClientCode : EntityWithTypedId<Guid>
    {
        /// <summary>
        /// Gets or sets the Client Identifier
        /// </summary>
        public Guid ClientId { get; set; }
        /// <summary>
        /// Gets or sets the Client
        /// </summary>
        public Client Client { get; set; }
        /// <summary>
        /// Gets or sets the Code Type
        /// </summary>
        public string CodeType { get; set; }
        /// <summary>
        /// Gets or sets the Reg. Number
        /// </summary>
        public string RegNumber { get; set; }
    }
}
