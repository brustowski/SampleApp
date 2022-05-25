using System.Collections.Generic;
using FilingPortal.Domain.Entities.Vessel;
using Framework.Domain;

namespace FilingPortal.Domain.Entities.Handbooks
{
    /// <summary>
    /// Product description handbook record
    /// </summary>
    public class ProductDescriptionHandbookRecord : Entity
    {
        /// <summary>
        /// Product description
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Corresponding tariff
        /// </summary>
        public virtual HtsTariff Tariff { get; set; }

        /// <summary>
        /// Corresponding Tariff's ID
        /// </summary>
        public int TariffId { get; set; }

        /// <summary>
        /// List of associated vessel imports
        /// </summary>
        public virtual ICollection<VesselImportRecord> VesselImports { get; set; }
    }
}
