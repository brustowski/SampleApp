using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Truck
{

    /// <summary>
    /// Defines The Truck Section Entity
    /// </summary>
    public class TruckSection : BaseSection
    {
        /// <summary>
        /// Gets or sets the Parent
        /// </summary>
        public virtual TruckSection Parent { get; set; }

        /// <summary>
        /// Gets or sets the Descendants
        /// </summary>
        public virtual ICollection<TruckSection> Descendants { get; set; }

        /// <summary>
        /// Gets or sets the field configurations
        /// </summary>
        public virtual ICollection<TruckDefValue> Fields { get; set; }
    }
}
