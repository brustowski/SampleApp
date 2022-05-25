using System;
using Framework.DataLayer;
using Framework.Infrastructure;

namespace FilingPortal.Parts.Inbond.DataLayer
{
    /// <summary>
    /// Class for <see cref="InbondContextFactory"/> creation
    /// </summary>
    public class InbondContextFactory : IDbContextFactory<InbondContext>
    {
        /// <summary>
        /// Creates instance of <see cref="InbondContextFactory"/>
        /// </summary>
        public InbondContext Create()
        {
            var dbContext = new InbondContext();

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