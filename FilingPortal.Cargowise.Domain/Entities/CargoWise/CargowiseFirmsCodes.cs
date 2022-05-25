namespace FilingPortal.Cargowise.Domain.Entities.CargoWise
{
    /// <summary>
    /// Syncronizable with CargoWise entity that stores FIRMs Codes - information about discharge terminals
    /// </summary>
    public class CargowiseFirmsCodes : BaseSyncEntity
    {
        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public string FirmsCode { get; set; }
        /// <summary>
        /// Gets or sets the Discharge terminal name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets whether this record is active
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the country ID
        /// </summary>
        public int? CountryId { get; set; }
        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        public Country Country { get; set; }
        /// <summary>
        /// Gets or sets the state Id
        /// </summary>
        public int? StateId { get; set; }
        /// <summary>
        /// Gets or sets the State
        /// </summary>
        public USStates State { get; set; }
    }
}
