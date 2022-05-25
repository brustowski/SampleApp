using System.Data.Entity.ModelConfiguration.Conventions;

namespace FilingPortal.Parts.Common.DataLayer.Conventions
{
    public class DecimalConvention : Convention
    {
        public DecimalConvention()
        {
            Properties<decimal>()
                .Configure(config => config.HasPrecision(18, 6));
        }
    }
}