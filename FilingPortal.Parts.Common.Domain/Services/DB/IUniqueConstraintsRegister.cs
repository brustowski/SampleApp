namespace FilingPortal.Parts.Common.Domain.Services.DB
{
    /// <summary>
    /// Describes register of unique constraints
    /// </summary>
    public interface IUniqueConstraintsRegister
    {
        /// <summary>
        /// Returns whether column in table is part of unique index
        /// </summary>
        /// <param name="tableName">DB table name</param>
        /// <param name="columnName">DB column name</param>
        /// <returns></returns>
        bool IsUnique(string tableName, string columnName);
    }
}
