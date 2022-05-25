using System;
using Framework.DataLayer;
using Framework.Infrastructure;

namespace FilingPortal.Parts.Recon.DataLayer
{
    /// <summary>
    /// Class for <see cref="PluginContextFactory"/> creation
    /// </summary>
    public class PluginContextFactory : IDbContextFactory<PluginContext>
    {
        /// <summary>
        /// Creates instance of <see cref="PluginContextFactory"/>
        /// </summary>
        public PluginContext Create()
        {
            var dbContext = new PluginContext();

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