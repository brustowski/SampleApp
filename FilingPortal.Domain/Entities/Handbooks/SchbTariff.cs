using System;
using Framework.Domain;

namespace FilingPortal.Domain.Entities.Handbooks
{
    /// <summary>
    /// Defines Tariff Entity
    /// </summary>
    public class SchbTariff : Entity
    {
        /// <summary>
        /// Gets or sets the Tariff
        /// </summary>
        public string UB_Tariff { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Short_Description { get; set; }
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
    }

}
