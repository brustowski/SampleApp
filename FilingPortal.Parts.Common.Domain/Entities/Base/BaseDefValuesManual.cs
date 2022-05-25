using System;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Defines the Base Def Values Manual entity
    /// </summary>
    public abstract class BaseDefValuesManual : Entity
    {
        /// <summary>
        /// Gets or Sets the Filing Header Id
        /// </summary>
        public int FilingHeaderId { get; set; }

        /// <summary>
        /// Gets or sets Section Title
        /// </summary>
        public string SectionTitle { get; set; }

        /// <summary>
        /// Gets or sets Table Name
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets Column Name
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets Field Value Modification Date
        /// </summary>
        public DateTime ModificationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets field visibility and order on UI
        /// </summary>
        public byte DisplayOnUI { get; set; }

        /// <summary>
        /// Gets or sets field visibility and order on form
        /// </summary>
        public byte Manual { get; set; }

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
        /// Gets or sets Parent Record Id
        /// </summary>
        public int ParentRecordId { get; set; }

        /// <summary>
        /// Gets or sets Section Name
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        /// Gets or sets Record Id 
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether field is Editable
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether field Has a Default Value
        /// </summary>
        public bool HasDefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether field is Mandatory
        /// </summary>
        public bool Mandatory { get; set; }
        /// <summary>
        /// Gets or sets whether this field requires validation
        /// </summary>
        public bool ConfirmationNeeded { get; set; }

    }
}
