namespace FilingPortal.Parts.Common.Domain.Common
{
    /// <summary>
    /// Defines unique data for def values field
    /// </summary>
    public class DefValuesUniqueData
    {
        private readonly string _table;
        public readonly string Column;

        /// <summary>
        /// Creates new instance of field unique data
        /// </summary>
        /// <param name="table">Table name</param>
        /// <param name="column">Column name</param>
        public DefValuesUniqueData(string table, string column)
        {
            _table = table;
            Column = column;
        }

        /// <summary>
        /// Overrides equality operator
        /// </summary>
        public static bool operator ==(DefValuesUniqueData obj1, DefValuesUniqueData obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return (obj1.Equals(obj2));
        }
        /// <summary>
        /// Overrides unequality operator
        /// </summary>
        public static bool operator !=(DefValuesUniqueData obj1, DefValuesUniqueData obj2) => !(obj1 == obj2);

        /// <summary>
        /// Overrides equality operator
        /// </summary>
        public override bool Equals(object obj) => Equals(obj as DefValuesUniqueData);

        /// <summary>
        /// Return equality between two different objects
        /// </summary>
        private bool Equals(DefValuesUniqueData other) => string.Equals(_table, other._table) && string.Equals(Column, other.Column);

        /// <summary>
        /// Returns hash code for the object
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((_table != null ? _table.GetHashCode() : 0) * 397) ^ (Column != null ? Column.GetHashCode() : 0);
            }
        }
    }
}
