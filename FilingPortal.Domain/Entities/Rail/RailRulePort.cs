using Framework.Domain;
using System;

namespace FilingPortal.Domain.Entities.Rail
{
    /// <summary>
    /// Describes Port Rail Rule
    /// </summary>
    public class RailRulePort: AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets Port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Gets or sets Origin
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// Gets or sets Destination
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// Gets or sets FIRMs Code
        /// </summary>
        public string FIRMsCode { get; set; }
        /// <summary>
        /// Gets or sets Export
        /// </summary>
        public string Export { get; set; }

        /// <summary>
        /// Gets or sets Rule Creator
        /// </summary>
        public override string CreatedUser { get; set; } = "sa";
    }
}
