using System.Data.Entity.ModelConfiguration.Conventions;

namespace FilingPortal.Parts.Common.DataLayer.Conventions
{
    public class StringConvention : Convention
    {
        public StringConvention()
        {
            Properties<string>()
                .Configure(config => config.HasColumnType("varchar").HasMaxLength(128));
        }
    }
}