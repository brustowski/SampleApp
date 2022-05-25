using System;
using System.Data.Entity;
using Framework.DataLayer;
using Framework.Infrastructure;

namespace FilingPortal.Parts.Common.DataLayer.Base
{
    /// <summary>
    /// Base class for context factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FpContextFactory<T>: IDbContextFactory<T>
    where T: DbContext, new()
    {
        public T Create()
        {
            var dbContext = new T();
            try
            {
                dbContext.Database.Initialize(false);
                dbContext.Database.Log = s =>
                {
                    AppLogger.Trace(s);
                };
            }
            catch (Exception)
            {
                throw;
            }
            return dbContext;
        }
    }
}
