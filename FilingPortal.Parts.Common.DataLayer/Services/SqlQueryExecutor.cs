using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using FilingPortal.Parts.Common.Domain.Services.DB;
using Framework.DataLayer;
using Framework.Infrastructure;

namespace FilingPortal.Parts.Common.DataLayer.Services
{
    /// <summary>
    /// Provides methods to execute SQL queries against a database
    /// </summary>
    public class SqlQueryExecutor : ISqlQueryExecutor, IDisposable
    {
        /// <summary>
        /// Database connection string
        /// </summary>
        private readonly string _connectionString;
        /// <summary>
        /// <see cref="SqlConnection"/> object
        /// </summary>
        private SqlConnection _connection;

        /// <summary>
        /// Gets an open connection to database
        /// </summary>
        private SqlConnection GetOpenConnection()
        {

            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }

            return _connection;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQueryExecutor" /> class
        /// </summary>
        /// <param name="unitOfWorkfactory">Factory to create <see cref="IUnitOfWorkDbContext"/> object</param>
        public SqlQueryExecutor(IUnitOfWorkFactory unitOfWorkfactory)
        {
            IUnitOfWorkDbContext uow = unitOfWorkfactory.Create();
            _connectionString = uow.Context.Database.Connection.ConnectionString;
        }
        /// <summary>
        /// Executes a SQL command against the database and returns the number of rows affected
        /// </summary>
        /// <param name="sqlQuery">SQL command text</param>
        public int ExecuteSqlCommand(string sqlQuery)
        {
            using (new MonitoredScope("Execute Sql Command"))
            {
                var command = new SqlCommand(sqlQuery, GetOpenConnection());
                var result = command.ExecuteNonQuery();
                return result;
            }
        }
        /// <summary>
        /// Executes a SQL query against the database and returns <see cref="DataTable"/> result
        /// </summary>
        /// <param name="sqlQuery">SQL query text</param>
        public DataTable ExecuteSqlQuery(string sqlQuery)
        {
            using (new MonitoredScope("Execute Sql Query"))
            {
                var command = new SqlCommand(sqlQuery, GetOpenConnection());
                SqlDataReader reader = command.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable;
            }
        }
        /// <summary>
        /// Executes bulk insert of provided records into specified data tabel and mappings
        /// </summary>
        /// <typeparam name="TDto">Type of the records</typeparam>
        /// <param name="records">Set of records to insert</param>
        /// <param name="tableName">Table name</param>
        /// <param name="mappings">record property to table column mapping</param>
        public void ExecuteSqlBulkInsert<TDto>(IEnumerable<TDto> records, string tableName, Dictionary<string, string> mappings)
        {
            using (new MonitoredScope("Execute Sql Bulk Insert"))
            {
                Type t = typeof(TDto);
                PropertyInfo[] properties = t.GetProperties().ToArray();

                var table = new DataTable(tableName);
                using (var bulkCopy = new SqlBulkCopy(GetOpenConnection()))
                {
                    bulkCopy.DestinationTableName = tableName;
                    SetColumnMappings(properties, table, mappings, bulkCopy);
                    AddAllEntitiesToDataTable(records, properties, table);
                    bulkCopy.WriteToServer(table);
                }
            }
        }
        /// <summary>
        /// Fills the data table with values from the specified properties of objects
        /// </summary>
        /// <typeparam name="TDto">Object type</typeparam>
        /// <param name="records">Set of objects to get values</param>
        /// <param name="properties">Set of properties whose values need to be added to the data table</param>
        /// <param name="table"><see cref="DataTable"/> object to add values</param>
        private void AddAllEntitiesToDataTable<TDto>(IEnumerable<TDto> records, PropertyInfo[] properties, DataTable table)
        {
            foreach (TDto entity in records)
            {
                var columns = properties.Select(property => GetPropertyValue(property.GetValue(entity, null))).ToArray();
                table.Rows.Add(columns);
            }
        }
        /// <summary>
        /// Gets property value or DBNull in case of null value
        /// </summary>
        /// <param name="o">Value</param>
        private static object GetPropertyValue(object o)
        {
            if (o == null)
            {
                return DBNull.Value;
            }

            return o;
        }
        /// <summary>
        /// Sets source data table columns and column mappings
        /// </summary>
        /// <param name="properties">Property to map on</param>
        /// <param name="table">Data tabel to set</param>
        /// <param name="mappings">Object property on column mapping discionary</param>
        /// <param name="bulkCopy"><see cref="SqlBulkCopy"/> object to add column mapping</param>
        private void SetColumnMappings(PropertyInfo[] properties, DataTable table, Dictionary<string, string> mappings, SqlBulkCopy bulkCopy)
        {
            foreach (PropertyInfo property in properties)
            {
                // Since we cannot trust the CLR type properties to be in the same order as
                // the table columns we use the SqlBulkCopy column mappings.
                if (property.PropertyType == typeof(DateTime))
                {
                    table.Columns.Add(property.Name, property.PropertyType);
                }
                else
                {
                    table.Columns.Add(new DataColumn(property.Name));
                }

                var clrPropertyName = property.Name;
                if (!mappings.ContainsKey(property.Name))
                {
                    throw new KeyNotFoundException("Key with value " + property.Name + " not found");
                }
                var tableColumnName = mappings[property.Name];
                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(clrPropertyName, tableColumnName));
            }
        }

        /// <summary>
        /// Close connection
        /// </summary>
        public void Close()
        {
            try
            {
                _connection?.Close();
            }
            catch
            {
                // ignored
            }
        }
        /// <summary>
        /// <see cref="IDisposable.Dispose"/> method
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}
