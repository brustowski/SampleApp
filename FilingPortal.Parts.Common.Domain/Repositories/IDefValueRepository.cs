using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Defines the generic form configuration entities repository
    /// </summary>
    /// <typeparam name="TDefValueEntity">DefValue entity type</typeparam>
    public interface IDefValueRepository<TDefValueEntity>: IRuleRepository<TDefValueEntity> where TDefValueEntity : BaseDefValue
    {
        /// <summary>
        /// Creates entity and sets up default value for corresponding resulting table column
        /// </summary>
        /// <param name="entity">Form configuration param</param>
        /// <param name="value">Default value for this configuration parameter</param>
        void UpdateColumnConstraint(TDefValueEntity entity, string value);
    }
}
