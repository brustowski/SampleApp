using System.Data.Entity.ModelConfiguration.Conventions;

namespace FilingPortal.Parts.Common.DataLayer.Conventions
{
    public class KeyConvention : Convention
    {
        public KeyConvention()
        {
            Properties()
                .Where(prop => prop.Name == "Id")
                .Configure(config => config.IsKey().HasColumnName("id"));
        }
    }
}