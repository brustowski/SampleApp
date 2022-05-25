using System.Data.Entity;
using FilingPortal.Parts.Common.DataLayer.Conventions;
using Framework.DataLayer;

namespace FilingPortal.Parts.Common.DataLayer.Base
{
    public abstract class FpContext : DbExtendedContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Add(new KeyConvention());
            modelBuilder.Conventions.Add(new ColumnNameConvention());
            modelBuilder.Conventions.Add(new DecimalConvention());
            modelBuilder.Conventions.Add(new StringConvention());
            modelBuilder.Conventions.Add(new DatetimeConvention());
            modelBuilder.Configurations.AddFromAssembly(typeof(CommonContext).Assembly);
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
        }

        #region Constructor

        protected FpContext()
            : base("Name=FilingPortalContext")
        {
        }
        #endregion
    }
}
