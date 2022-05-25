using System.Collections.Generic;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Services.DB;
using Framework.Domain;

namespace FilingPortal.Domain.Common
{
    /// <summary>
    /// Implements service that returns information about unique fields in database
    /// </summary>
    public class KeyFieldsService : IKeyFieldsService
    {
        /// <summary>
        /// Database Structure service
        /// </summary>
        private readonly IEnumerable<IDbStructureService> _dbStructureServices;
        /// <summary>
        /// Unique constraints register
        /// </summary>
        private readonly IUniqueConstraintsRegister _uniqueConstraintsRegister;

        /// <summary>
        /// Creates a new instance of <see cref="KeyFieldsService"/>
        /// </summary>
        /// <param name="dbStructureServices">Database Structure service</param>
        /// <param name="uniqueConstraintsRegister">Unique constraints register</param>
        public KeyFieldsService(IEnumerable<IDbStructureService> dbStructureServices,
            IUniqueConstraintsRegister uniqueConstraintsRegister)
        {
            _dbStructureServices = dbStructureServices;
            _uniqueConstraintsRegister = uniqueConstraintsRegister;
        }

        /// <summary>
        /// Checks if property of entity has Unique constraint in database
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="propertyName">Property name</param>
        public bool IsKeyField<TEntity>(string propertyName) where TEntity : Entity
        {
            foreach (var dbStructureService in _dbStructureServices)
            {
                if (dbStructureService.ContainsEntity<TEntity>())
                {
                    string tableName = dbStructureService.GetDbTableName<TEntity>();
                    if (tableName != null)
                    {
                        string columnName = dbStructureService.GetDbColumnName<TEntity>(propertyName);
                        if (columnName != null)
                        {
                            return _uniqueConstraintsRegister.IsUnique(tableName, columnName);
                        }
                    }
                }
            }

            return false;
        }
    }
}