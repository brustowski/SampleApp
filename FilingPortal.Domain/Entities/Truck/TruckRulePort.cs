namespace FilingPortal.Domain.Entities.Truck
{
    using Framework.Domain;
    using System;

    /// <summary>
    /// Defines the Port Truck Rule
    /// </summary>
    public class TruckRulePort : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets the Entry Port
        /// </summary>
        public string EntryPort { get; set; }

        /// <summary>
        /// Gets or sets the Arrival Port
        /// </summary>
        public string ArrivalPort { get; set; }

        /// <summary>
        /// Gets or sets the FIRMsCode
        /// </summary>
        public string FIRMsCode { get; set; }
    }
}
