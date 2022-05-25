using System;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Describes base class for Field configuration
    /// </summary>
    public abstract class BaseDefValue : Entity
    {
        /// <summary>
        /// Gets or sets Column Name
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets Creation Date
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets Creator
        /// </summary>
        public string CreatedUser { get; set; }

        /// <summary>
        /// Gets or sets Display On UI
        /// </summary>
        public byte DisplayOnUI { get; set; }

        /// <summary>
        /// Gets or sets Manual
        /// </summary>
        public byte Manual { get; set; }

        /// <summary>
        /// Gets or sets Editable
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// Gets or sets Mandatory
        /// </summary>
        public bool Mandatory { get; set; }

        /// <summary>
        /// Gets or sets order and visibility on Single Filing form
        /// </summary>
        public byte? SingleFilingOrder { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Paired Field Table Name
        /// </summary>
        public string PairedFieldTable { get; set; }

        /// <summary>
        /// Paired Field Column name
        /// </summary>
        public string PairedFieldColumn { get; set; }

        /// <summary>
        /// Dropdown fields configuration
        /// </summary>
        public string HandbookName { get; set; }

        /// <summary>
        /// Gets or sets Has Default Value
        /// </summary>
        public bool HasDefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the Overridden Type
        /// </summary>
        public string OverriddenType { get; set; }
    }
}
