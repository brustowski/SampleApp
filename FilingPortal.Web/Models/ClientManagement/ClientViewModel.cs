using System;

namespace FilingPortal.Web.Models.ClientManagement
{
    /// <summary>
    /// Class representing Client in View model
    /// </summary>
    public class ClientViewModel
    {
        /// <summary>
        /// Gets or sets the Client identifier
        /// </summary>
        public Guid Id { get; set; }
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
        public string Status { get; set; }
        /// <summary>
        /// Gets or sets Client Type
        /// </summary>
        public string ClientType { get; set; }
    }
}