using System.Collections.Generic;
using Framework.Domain;

namespace FilingPortal.Cargowise.Domain.Entities.CargoWise
{
    /// <summary>
    /// Defines Country
    /// </summary>
    public class Country : Entity
    {
        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the FIRMs Codes for this Country
        /// </summary>
        public virtual ICollection<CargowiseFirmsCodes> FirmsCodes { get; set; }
    }
}