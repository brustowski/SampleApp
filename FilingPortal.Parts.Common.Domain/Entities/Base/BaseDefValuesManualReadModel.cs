using System;
using FilingPortal.Parts.Common.Domain.Common;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Defines base DefValue Manual Read model entity
    /// </summary>

    public abstract class BaseDefValuesManualReadModel : Entity
    {
        /// <summary>
        /// Gets or sets whether this model should be displayed on UI (0-false, 1..n-true, serial number)
        /// </summary>
        public byte DisplayOnUI { get; set; }
        /// <summary>
        /// Gets or sets the Filing Header id
        /// </summary>
        public int FilingHeaderId { get; set; }
        /// <summary>
        /// Gets or sets Column Name
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Gets or sets value for this model
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets field UI label
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Gets or sets whether this model is mandatory
        /// </summary>
        public bool Mandatory { get; set; }
        /// <summary>
        /// Gets or sets field database type
        /// </summary>
        public string ValueType { get; set; }
        /// <summary>
        /// Gets or sets field max length
        /// </summary>
        public int? ValueMaxLength { get; set; }
        /// <summary>
        /// Gets or sets Table Name
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Gets or sets whether this model is editable
        /// </summary>
        public bool Editable { get; set; }
        /// <summary>
        /// Gets or sets whether this model has default value
        /// </summary>
        public bool HasDefaultValue { get; set; }
        /// <summary>
        /// Gets or sets whether this model should be displayed on 7501 form (0-false, 1..n-true, serial number)
        /// </summary>
        public byte Manual { get; set; }
        /// <summary>
        /// Gets or sets the modification date of model
        /// </summary>
        public DateTime ModificationDate { get; set; } = DateTime.Now;
        /// <summary>
        /// Gets or sets the section name, where field is located
        /// </summary>
        public string SectionTitle { get; set; }
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
        /// Gets or sets the Depends On Column
        /// </summary>
        public string DependsOn { get; set; }
        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets whether this field requires confirmation
        /// </summary>
        public bool ConfirmationNeeded { get; set; }
        /// <summary>
        /// Gets def value unique data
        /// </summary>
        public virtual DefValuesUniqueData GetUniqueData() => new DefValuesUniqueData(TableName, ColumnName);
        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        protected bool Equals(BaseDefValuesManualReadModel other)
        {
            return TableName.Equals(other.TableName) && ColumnName.Equals(other.ColumnName) && RecordId.Equals(other.RecordId);
        }
        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && Equals((BaseDefValuesManualReadModel)obj);
        }
        /// <summary>
        /// Gets object hash code
        /// </summary>
        public override int GetHashCode()
        {
            return RecordId.GetHashCode() ^ Id.GetHashCode();
        }
        /// <summary>
        /// Check if operands are equal or not
        /// </summary>
        /// <param name="left">Left operand to check</param>
        /// <param name="right">Right First operand to check</param>
        public static bool operator ==(BaseDefValuesManualReadModel left, BaseDefValuesManualReadModel right)
        {
            return Equals(left, right);
        }
        /// <summary>
        /// Check if operands are equal or not
        /// </summary>
        /// <param name="left">Left operand to check</param>
        /// <param name="right">Right First operand to check</param>
        public static bool operator !=(BaseDefValuesManualReadModel left, BaseDefValuesManualReadModel right)
        {
            return !Equals(left, right);
        }
    }
}
