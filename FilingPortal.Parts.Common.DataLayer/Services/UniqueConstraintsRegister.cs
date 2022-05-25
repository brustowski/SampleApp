using System.Collections.Generic;
using System.Data;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Services.DB;

namespace FilingPortal.Parts.Common.DataLayer.Services
{
    /// <summary>
    /// Implements unique constraints register
    /// </summary>
    public class UniqueConstraintsRegister : IUniqueConstraintsRegister
    {
        /// <summary>
        /// Stores information about table-column association
        /// </summary>
        private class UniqueField
        {
            /// <summary>
            /// Gets or sets table name
            /// </summary>
            public string TableName { get; set; }
            /// <summary>
            /// Gets or sets column name
            /// </summary>
            public string ColumnName { get; set; }
        }

        private readonly List<UniqueField> _register;
        /// <summary>
        /// Creates a new instance of <see cref="UniqueConstraintsRegister"/>
        /// </summary>
        /// <param name="executor">SQL code executor</param>
        public UniqueConstraintsRegister(ISqlQueryExecutor executor)
        {
            DataTable dataTable = executor.ExecuteSqlQuery(
                @"SELECT sh.name AS schema_name,
                   t.name AS table_name,
                   c.name AS column_name
                FROM sys.indexes i
                   INNER JOIN sys.index_columns ic
                      ON i.index_id = ic.index_id AND i.object_id = ic.object_id
                   INNER JOIN sys.tables AS t 
                      ON t.object_id = i.object_id
                   INNER JOIN sys.columns c
                      ON t.object_id = c.object_id AND ic.column_id = c.column_id
                   INNER JOIN sys.objects AS syso 
                      ON syso.object_id = t.object_id AND syso.is_ms_shipped = 0 
                   INNER JOIN sys.schemas AS sh
                      ON sh.schema_id = t.schema_id 
                   INNER JOIN information_schema.schemata sch
                      ON sch.schema_name = sh.name
                WHERE i.is_unique = 1");
            executor.Close();
            _register = dataTable.AsEnumerable().Select(x => new UniqueField
            { TableName = $"{x[0]}.{x[1]}", ColumnName = x[2].ToString() }).ToList();
        }

        /// <summary>
        /// Returns whether column in table is part of unique index
        /// </summary>
        /// <param name="tableName">DB table name</param>
        /// <param name="columnName">DB column name</param>
        /// <returns></returns>
        public bool IsUnique(string tableName, string columnName)
        {
            return _register.Any(x => x.TableName == tableName && x.ColumnName == columnName);
        }
    }
}
