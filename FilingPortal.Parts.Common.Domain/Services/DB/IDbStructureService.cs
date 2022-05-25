using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Services.DB
{
    /// <summary>
    /// Describes methods to get database structure information
    /// </summary>
    public interface IDbStructureService
    {
        /// <summary>
        /// Gets DB column name by entity and property name
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="propertyName">Property name</param>
        string GetDbColumnName<TEntity>(string propertyName) where TEntity : Entity;

        /// <summary>
        /// Gets DB Table name by entity
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        string GetDbTableName<TEntity>() where TEntity : Entity;

        /// <summary>
        /// Returns TRUE if db contains entity
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        bool ContainsEntity<TEntity>() where TEntity : Entity;
    }
}
