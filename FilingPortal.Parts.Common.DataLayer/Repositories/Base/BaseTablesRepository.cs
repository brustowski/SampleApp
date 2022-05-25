using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.DataLayer;

namespace FilingPortal.Parts.Common.DataLayer.Repositories.Base
{
    /// <summary>
    /// Class for repository of <see cref="BaseTablesRepository{TTable}"/>
    /// </summary>
    public class BaseTablesRepository<TTable> : SearchRepository<TTable>, ITablesRepository<TTable>
    where TTable : BaseTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTablesRepository{TTable}"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public BaseTablesRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        { }

        /// <summary>
        /// Gets list of table names
        /// </summary>
        public IQueryable<string> GetTableNames()
        {
            return Set.Select(x => x.TableName).Distinct();
        }

        /// <summary>
        /// Gets section id by the table name
        /// </summary>
        /// <param name="tableName">The table name</param>
        public int GetSectionIdByTableName(string tableName)
        {
            TTable result = Set.First(x => x.TableName == tableName);
            return result.SectionId;
        }

        /// <summary>
        /// Gets list of table and column names for specified table name
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        public IQueryable<TTable> GetByTableName(string tableName)
        {
            return Set.Where(x => x.TableName == tableName);
        }

        /// <summary>
        /// Gets list of table and column names
        /// </summary>
        public IQueryable<TTable> GetAllAsQueryable()
        {
            return Set.AsQueryable();
        }
    }
}
