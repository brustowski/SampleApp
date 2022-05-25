using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.Web.Models.Rail;

namespace FilingPortal.Web.FieldConfigurations.Rail
{
    /// <summary>
    /// Factory that provides Rail Form configuration
    /// </summary>
    public class RailInboundFormConfigFactory : FormConfigFactory<RailInboundEditModel>
    {
        /// <summary>
        /// Creates configuration for add new vessel form
        /// </summary>
        protected override void Setup()
        {
            AddField(x => x.Consignee).Mandatory().Lookup(DataProviderNames.ImporterLongTitles);
            AddField(x => x.Supplier).Mandatory().Lookup(DataProviderNames.SuppliersLongTitles);
            AddField(x => x.Importer).Lookup(DataProviderNames.ImporterLongTitles).Separator();
            AddField(x => x.Description).Type(FieldType.MultilineText).Mandatory().Long().Multiline().Separator();
            AddField(x => x.EquipmentInitial).Title("Equipment Initial").Mandatory();
            AddField(x => x.EquipmentNumber).Title("Equipment Number").Mandatory();
            AddField(x => x.IssuerCode).Title("Issuer Code").Lookup(DataProviderNames.IssuerCodes).Mandatory();
            AddField(x => x.BillOfLading).Title("Bill of Lading").Mandatory();
            AddField(x => x.PortOfUnlading).Title("Port of Unlading").Lookup(DataProviderNames.DomesticPorts).Mandatory();
            AddField(x => x.Destination).Lookup(DataProviderNames.StateNames);
            AddField(x => x.Weight).Type(FieldType.Number).Mandatory();
            AddField(x => x.WeightUnit).Title("Weight Unit").Lookup(DataProviderNames.Units).Mandatory();
            AddField(x => x.ReferenceNumber).Title("Reference Number").Mandatory();
            AddField(x => x.ManifestUnits).Title("Manifest Units").Lookup(DataProviderNames.Units).Mandatory();
        }
    }
}