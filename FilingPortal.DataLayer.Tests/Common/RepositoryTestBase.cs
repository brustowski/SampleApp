using System.Data.Entity;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Common
{
    [TestClass]
    public abstract class RepositoryTestBase
    {
        protected readonly IUnitOfWorkFactory UnitOfWorkFactory;
        protected readonly FilingPortalContext DbContext;
        private DbContextTransaction _transaction;
        protected AppUsersModel CurrentUser { get; set; }

        protected RepositoryTestBase()
        {
            UnitOfWorkFactory = CreateTestUnitOfWorkFactory();
            DbContext = (FilingPortalContext)UnitOfWorkFactory.Create().Context;
            DbContext.Database.CreateIfNotExists();
        }


        [TestInitialize]
        public void TestInitialize()
        {
            _transaction = DbContext.Database.BeginTransaction();
            CurrentUser = new AppUsersModel() { Id = "sa", StatusId = 1 };
            TestInit();
        }

        protected virtual void TestInit()
        {
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            DbContext.Dispose();
        }

        protected IUnitOfWorkFactory CreateTestUnitOfWorkFactory()
        {
            return new TestUnitOfWorkFactory();
        }

        protected void DetachEntities()
        {
            foreach (System.Data.Entity.Infrastructure.DbEntityEntry entity in UnitOfWorkFactory.Create().Context.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;
            }
        }

        protected void DetachEntities<T>() where T : class
        {
            foreach (System.Data.Entity.Infrastructure.DbEntityEntry<T> entity in UnitOfWorkFactory.Create().Context.ChangeTracker.Entries<T>())
            {
                entity.State = EntityState.Detached;
            }
        }
    }
}

