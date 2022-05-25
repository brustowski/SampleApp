using System;
using System.Data.Entity;

namespace Framework.DataLayer
{
    /// <summary>
    /// Contains additional context attributes
    /// </summary>
    public abstract class DbExtendedContext : DbContext
    {
        private readonly object _locker = new object();

        #region Constructor

        protected DbExtendedContext(string connectionString)
            : base(connectionString)
        {
        }

        protected DbExtendedContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model)
            : base(connectionString, model)
        {
        }

        protected DbExtendedContext(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        protected DbExtendedContext(System.Data.Common.DbConnection existingConnection, System.Data.Entity.Infrastructure.DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }
        #endregion

        public abstract string DefaultSchema { get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);
        }

        public void RunLongDatabaseOperation(Action action)
        {
            lock (_locker)
            {
                var timeout = Database.CommandTimeout;

                Database.CommandTimeout = 0;
                action.Invoke();

                Database.CommandTimeout = timeout;
            }
            
        }
    }
}
