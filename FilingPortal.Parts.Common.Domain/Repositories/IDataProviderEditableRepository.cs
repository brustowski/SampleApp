using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Describes data provider editable repository
    /// </summary>
    public interface IDataProviderEditableRepository<TData, in TId> : IDataProviderRepository<TData, TId>
        where TData : EntityWithTypedId<TId>
    {
        void Add(TData entity);
        void Save();
    }
}
