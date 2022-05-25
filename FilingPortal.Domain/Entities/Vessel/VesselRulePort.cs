namespace FilingPortal.Domain.Entities.Vessel
{
    using FilingPortal.Cargowise.Domain.Entities.CargoWise;
    using Framework.Domain;

    /// <summary>
    /// Defines the Vessel Port Rule
    /// </summary>
    public class VesselRulePort : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets the Discharge Name
        /// </summary>
        public string DischargeName { get; set; }

        /// <summary>
        /// Gets or sets the Entry Port
        /// </summary>
        public string EntryPort { get; set; }

        /// <summary>
        /// Gets or sets the Discharge Port
        /// </summary>
        public string DischargePort { get; set; }

        /// <summary>
        /// Gets or sets the FIRMs Code Id
        /// </summary>
        public int FirmsCodeId { get; set; }

        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public virtual CargowiseFirmsCodes FirmsCode { get; set; }

        /// <summary>
        /// Gets or sets the HMF
        /// </summary>
        public string HMF { get; set; }
        /// <summary>
        /// Gets or sets the Destination Code
        /// </summary>
        public string DestinationCode { get; set; }
    }
}
