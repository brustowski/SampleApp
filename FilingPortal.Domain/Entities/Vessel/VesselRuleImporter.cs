namespace FilingPortal.Domain.Entities.Vessel
{
    using Framework.Domain;
    using System;

    /// <summary>
    /// Defines the Vessel Importer Rule
    /// </summary>
    public class VesselRuleImporter : Entity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the CargoWise Importer
        /// </summary>
        public string CWImporter { get; set; }

        /// <summary>
        /// Gets or sets the Rule Creation Date
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the Rule Creator
        /// </summary>
        public string CreatedUser { get; set; }
    }
}
