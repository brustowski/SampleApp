using System;
using System.Collections.Generic;
using Framework.Domain;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Parts.Common.Domain.Entities.Clients
{
    /// <summary>
    /// Class describes Client
    /// </summary>
    public class Client : EntityWithTypedId<Guid>
    {
        /// <summary>
        /// Gets or sets Client Code
        /// </summary>
        public string ClientCode { get; set; }

        /// <summary>
        /// Gets or sets full name of the client
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets Client status
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets Importer Client Type
        /// </summary>
        public bool Importer { get; set; }

        /// <summary>
        /// Gets or sets Supplier Client Type
        /// </summary>
        public bool Supplier { get; set; }

        /// <summary>
        /// Gets or sets record's last updated time
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }

        /// <summary>
        /// Gets Client Type string representation
        /// </summary>
        public string ClientType
        {
            get
            {
                var result = new List<string>();
                if (Importer)
                {
                    result.Add(Enums.ClientType.Importer.GetDescription());
                }

                if (Supplier)
                {
                    result.Add(Enums.ClientType.Supplier.GetDescription());
                }

                return string.Join("; ", result);
            }
        }

        /// <summary>
        /// Corresponding contacts
        /// </summary>
        public virtual ICollection<ClientContact> Contacts { get; set; }
        /// <summary>
        /// Corresponding client numbers
        /// </summary>
        public virtual ICollection<ClientCode> ClientNumbers { get; set; }
    }

}
