using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Framework.Domain;
using Framework.Domain.Repositories;

namespace Framework.DataLayer
{
    public abstract class RepositoryWithTypedId<TEntity, TId> : IRepositoryWithTypeId<TEntity, TId>
        where TEntity : EntityWithTypedId<TId>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkfactory;
        protected DbSet<TEntity> Set { get { return UnitOfWork.Context.Set<TEntity>(); } }
        protected IUnitOfWorkDbContext UnitOfWork { get { return _unitOfWorkfactory.Create(); } }

        protected RepositoryWithTypedId(IUnitOfWorkFactory unitOfWorkfactory)
        {
            _unitOfWorkfactory = unitOfWorkfactory;
            Debug.WriteLine("UoW - {0} in - {1} ", UnitOfWork.GetHashCode(), this.GetType().Name);
        }

        public virtual TEntity Get(TId id)
        {
            return Set.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        public virtual void AddOrUpdate(TEntity entity)
        {
            if (Equals(entity.Id, default(TId)))
                Add(entity);
            else
                Update(entity);
        }

        public virtual void Save()
        {
            UnitOfWork.Save();
        }

        public virtual Task SaveAsync()
        {
            return UnitOfWork.SaveAsync();
        }

        public virtual Task SaveAsync(CancellationToken cancellationToken)
        {
            return UnitOfWork.SaveAsync(cancellationToken);
        }

        public virtual void Delete(TEntity entity)
        {
            Set.Remove(entity);
        }

        public virtual void DeleteById(TId id)
        {
            Delete(Get(id));
        }

        /// <summary>
        /// Mass delete action
        /// </summary>
        /// <param name="ids">Ids of records to delete</param>
        public virtual void Delete(IEnumerable<TId> ids)
        {
            Set.RemoveRange(Set.Where(x => ids.Contains(x.Id)));
            UnitOfWork.Save();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Set.ToList();
        }

        public IEnumerable<TEntity> GetList(IEnumerable<TId> idsList)
        {
            return GetByIdList(idsList).ToList();
        }

        protected IQueryable<TEntity> GetByIdList(IEnumerable<TId> idsList)
        {
            return Set.Where(x => idsList.Contains(x.Id));
        }

        public virtual IEnumerable<TEntity> GetByIdList(TId[] idList)
        {
            return Set
                .Where(entity => idList.Contains(entity.Id))
                .ToArray();
        }

        public virtual void Update(TEntity entity)
        {
            UnitOfWork.Context.Entry((object)entity)
                .State = EntityState.Modified;
        }

        protected DbSet<T> GetSet<T>() where T : class
        {
            return UnitOfWork.Context.Set<T>();
        }

        protected void Update(object entity)
        {
            UnitOfWork.Context.Entry(entity)
                .State = EntityState.Modified;
        }
    }
}