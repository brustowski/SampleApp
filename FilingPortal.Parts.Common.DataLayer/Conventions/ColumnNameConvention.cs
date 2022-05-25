using System.Data.Entity.ModelConfiguration.Conventions;
using Inflector;

namespace FilingPortal.Parts.Common.DataLayer.Conventions
{
    public class ColumnNameConvention : Convention
    {
        public ColumnNameConvention()
        {
            Properties()
                .Configure(config => config.HasColumnName(config.ClrPropertyInfo.Name.Underscore()));
        }
    }
}