using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="BaseTable"/>
    /// </summary>
    public interface ITablesRepository<out TTable> : ISearchRepository
    where TTable : BaseTable
    {
        /// <summary>
        /// Gets list of table names
        /// </summary>
        IQueryable<string> GetTableNames();

        /// <summary>
        /// Gets list of table and column names for specified table name
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        IQueryable<TTable> GetByTableName(string tableName);

        /// <summary>
        /// Gets list of table and column names
        /// </summary>
        IQueryable<TTable> GetAllAsQueryable();
        /// <summary>
        /// Gets section id by the table name
        /// </summary>
        /// <param name="tableName">The table name</param>
        int GetSectionIdByTableName(string tableName);
    }
}
