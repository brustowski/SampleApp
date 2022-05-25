using Framework.Domain;
using System;

namespace FilingPortal.Domain.Entities
{
    /// <summary>
    /// Defines LookupMaster Entity
    /// </summary>
    public class LookupMaster: Entity
    {
        /// <summary>
        /// Gets or sets the DisplayValue
        /// </summary>
        public string DisplayValue { get; set; }
        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets record's last updated time
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }

    }
    
}
