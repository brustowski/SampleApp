using Framework.Domain;
using System;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Entities
{
    /// <summary>
    /// Represents the Product Code entity
    /// </summary>
    public class ProductCode : EntityWithTypedId<Guid>
    {
        /// <summary>
        /// Gets or sets the Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the Last Updated date
        /// </summary>
        public DateTime Updated { get; set; }
    }
}
