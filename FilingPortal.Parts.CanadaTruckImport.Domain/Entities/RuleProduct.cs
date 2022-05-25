using System;
using FilingPortal.Domain.Entities;
using Framework.Domain;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Entities
{
    /// <summary>
    /// Defines Product rule
    /// </summary>
    public class RuleProduct : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets Product Code Id
        /// </summary>
        public Guid ProductCodeId { get; set; }
        /// <summary>
        /// Gets or sets Product Code
        /// </summary>
        public virtual ProductCode ProductCode { get; set; }
        /// <summary>
        /// Gets or sets Gross Weight Unit
        /// </summary>
        public string GrossWeightUnit { get; set; }
        /// <summary>
        /// Gets or sets Packages Unit
        /// </summary>
        public string PackagesUnit { get; set; }
        /// <summary>
        /// Gets or sets the Invoice UQ
        /// </summary>
        public string InvoiceUQ { get; set; }
    }
}
