using System.Collections.Generic;
using Framework.Domain;

namespace FilingPortal.Cargowise.Domain.Entities.CargoWise
{
    /// <summary>
    /// Defines US States
    /// </summary>
    public class USStates: Entity
    {
        /// <summary>
        /// Gets or sets the State Code 
        /// </summary>
        public string StateCode { get; set; }
        /// <summary>
        /// Gets or sets the StateName 
        /// </summary>
        public string StateName  { get; set; }

        /// <summary>
        /// Gets or sets collection of FIRMs Codes for current state
        /// </summary>
        public virtual ICollection<CargowiseFirmsCodes> FirmsCodes { get; set; }
    }
}
