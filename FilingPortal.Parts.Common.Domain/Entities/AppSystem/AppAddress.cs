using System;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.AppSystem
{
    /// <summary>
    /// Defines System Address model
    /// </summary>
    public class AppAddress : Entity
    {
        /// <summary>
        /// Gets or sets CargoWise client address
        /// </summary>
        public ClientAddress CwAddress { get; set; }
        /// <summary>
        /// Gets or sets the CargoWise address identifier
        /// </summary>
        public Guid? CwAddressId { get; set; }
        /// <summary>
        /// Gets or sets whether CargoWise address is overriden by manually entered address
        /// </summary>
        public bool IsOverriden { get; set; }
        /// <summary>
        /// Gets or sets company name
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// Gets or sets first part of address
        /// </summary>
        public string Address1 { get; set; }
        /// <summary>
        /// Gets or sets second part of address
        /// </summary>
        public string Address2 { get; set; }
        /// <summary>
        /// Gets or sets Country Code
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Gets or sets City
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets Postal Code
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// Gets or sets State Code
        /// </summary>
        public string StateCode { get; set; }
    }
}
