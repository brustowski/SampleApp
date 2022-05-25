namespace FilingPortal.Domain.Entities.Pipeline
{
    using Framework.Domain;
    using System;

    /// <summary>
    /// Provides the Pipeline Rule for facilities
    /// </summary>
    public class PipelineRuleFacility : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets the Facility
        /// </summary>
        public string Facility { get; set; }

        /// <summary>
        /// Gets or sets the Port
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Gets or sets the Destination
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets the Destination State
        /// </summary>
        public string DestinationState { get; set; }

        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public string FIRMsCode { get; set; }

        /// <summary>
        /// Gets or sets the Issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the Origin
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets the Pipeline
        /// </summary>
        public string Pipeline { get; set; }
    }
}
