using Framework.Domain;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Describes service that returns information about unique fields in database
    /// </summary>
    public interface IKeyFieldsService
    {
        /// <summary>
        /// Checks if property of entity has Unique constraint in database
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="propertyName">Property name</param>
        bool IsKeyField<TEntity>(string propertyName) where TEntity : Entity;
    }
}
