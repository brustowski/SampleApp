using FilingPortal.Domain.Entities.Vessel;
using System;
using System.Collections.Generic;
using FilingPortal.Domain.Entities.VesselExport;
using Framework.Domain;

namespace FilingPortal.Domain.Entities.Handbooks
{
    /// <summary>
    /// Defines Tariff Entity
    /// </summary>
    public class HtsTariff : Entity
    {
        /// <summary>
        /// Gets or sets the Tariff
        /// </summary>
        public string USC_Tariff { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Short_Description { get; set; }
        /// <summary>
        /// Gets or sets record's valid from date 
        /// </summary>
        public DateTime FromDateTime { get; set; }

        /// <summary>
        /// Gets or sets record's valid todate 
        /// </summary>
        public DateTime ToDateTime { get; set; }

        /// <summary>
        /// Gets or sets Tariff_Type
        /// </summary>
        public string Tariff_Type { get; set; }

        /// <summary>
        /// Gets or sets UOM
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Gets or sets record's last updated time
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }

        /// <summary>
        /// List of corresponding vessel imports
        /// </summary>
        public virtual ICollection<VesselImportRecord> VesselImports { get; set; }

        /// <summary>
        /// List of corresponding product descriptions
        /// </summary>
        public virtual ICollection<ProductDescriptionHandbookRecord> ProductDescriptions { get; set; }
    }

}
