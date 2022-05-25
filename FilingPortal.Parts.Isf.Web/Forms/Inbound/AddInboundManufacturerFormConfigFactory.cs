using FilingPortal.Parts.Isf.Web.Models.AddInbound;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.FieldConfigurations.Common;

namespace FilingPortal.Parts.Isf.Web.Forms.Inbound
{
    /// <summary>
    /// Factory that provides Add inbound record configuration
    /// </summary>
    public class AddInboundManufacturerFormConfigFactory : FormConfigFactory<InboundManufacturerRecordEditModel>
    {
        /// <summary>
        /// Creates configuration for add new vessel form
        /// </summary>
        protected override void Setup()
        {
            AddField(x => x.ManufacturerId).Title("Name").Mandatory().Lookup(DataProviderNames.AllClients);
            AddField(x => x.Address).Title("Address").Address().DependsOn("ManufacturerId");
            AddField(x => x.CountryOfOrigin).Title("Country of Origin").Lookup(DataProviderNames.CountryCodes);
            AddField(x => x.HtsNumbers).Title("HTS Numbers").Lookup(DataProviderNames.HtsNumbers);
        }
    }
}