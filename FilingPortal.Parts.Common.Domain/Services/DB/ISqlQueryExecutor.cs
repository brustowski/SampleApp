using System.Collections.Generic;
using System.Data;

namespace FilingPortal.Parts.Common.Domain.Services.DB
{
    /// <summary>
    /// Describes methods to execute SQL queries against a database
    /// </summary>
    public interface ISqlQueryExecutor
    {
        /// <summary>
        /// Executes a SQL command against the database and returns the number of rows affected
        /// </summary>
        /// <param name="sqlCommand">SQL command text</param>
        int ExecuteSqlCommand(string sqlCommand);
        /// <summary>
        /// Executes a SQL query against the database and returns <see cref="DataTable"/> result
        /// </summary>
        /// <param name="sqlQuery">SQL query text</param>
        DataTable ExecuteSqlQuery(string sqlQuery);
        /// <summary>
        /// Executes bulk insert of provided records into specified data tabel and mappings
        /// </summary>
        /// <typeparam name="TDto">Type of the records</typeparam>
        /// <param name="entities">Set of records to insert</param>
        /// <param name="tableName">Table name</param>
        /// <param name="mappings">record property to table column mapping</param>
        void ExecuteSqlBulkInsert<TDto>(IEnumerable<TDto> entities, string tableName, Dictionary<string, string> mappings);
        /// <summary>
        /// Close connection
        /// </summary>
        void Close();
    }
}
