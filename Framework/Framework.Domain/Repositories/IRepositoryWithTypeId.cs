using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Domain.Repositories
{
    public interface IRepositoryWithTypeId<TEntity, in TId> where TEntity : class
    {
        TEntity Get(TId id);

        void Add(TEntity entity);

        void AddOrUpdate(TEntity entity);

        void Save();
        Task SaveAsync();
        Task SaveAsync(CancellationToken cancellationToken);

        void Delete(TEntity entity);

        void DeleteById(TId id);
        /// <summary>
        /// Mass delete action
        /// </summary>
        /// <param name="ids">Ids of records to delete</param>
        void Delete(IEnumerable<TId> ids);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetList(IEnumerable<TId> idsList);

        void Update(TEntity entity);
    }
}