using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Migrations
{
    internal sealed class Configuration : FpMigrationConfiguration<PluginContext>
    {
        protected override string[] GetIgnoreSchemas()
        {
            return new[] { "common.", "dbo." };
        }
    }
}
