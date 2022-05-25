using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.Web.Models.VesselExport;

namespace FilingPortal.Web.FieldConfigurations.VesselExport
{
    /// <summary>
    /// Factory that provides Vessel Form configuration
    /// </summary>
    public class VesselExportFormConfigFactory : FormConfigFactory<VesselExportEditModel>
    {
        /// <summary>
        /// Creates configuration for add new vessel form
        /// </summary>
        protected override void Setup()
        {
            AddField(x => x.UsppiId).Title("USPPI").Mandatory().Lookup(DataProviderNames.Suppliers);
            AddField(x => x.AddressId).Title("Address").Lookup(DataProviderNames.ClientAddresses).DependsOn("UsppiId");
            AddField(x => x.ContactId).Title("Contact").Separator().Lookup(DataProviderNames.ClientContacts, true).DependsOn("UsppiId");
            AddField(x => x.Phone).Separator().Lookup(DataProviderNames.ContactPhones, true).DependsOn("ContactId");
            AddField(x => x.ImporterId).Title("Importer").Mandatory().Separator()
                .Lookup(DataProviderNames.Importers);
            AddField(x => x.VesselId).Title("Vessel").Mandatory().Separator()
                .Lookup(DataProviderNames.Vessels, true);

            AddField(x => x.ExportDate).Type(FieldType.Date).Title("Export Date").Mandatory();
            AddField(x => x.LoadPort).Title("Load Port").Mandatory()
                .Lookup(DataProviderNames.DomesticPorts);
            AddField(x => x.DischargePort).Title("Discharge Port").Mandatory()
                .Lookup(DataProviderNames.DischargePorts);
            AddField(x => x.CountryOfDestinationId).Title("Country Of Destination").Mandatory()
                .DependsOn("DischargePort").Lookup(DataProviderNames.DischargePortCountries);
            AddField(x => x.TariffType).Title("Tariff Type").Mandatory()
                .Lookup(DataProviderNames.TariffTypes);
            AddField(x => x.Tariff).Title("Tariff").Mandatory()
                .Lookup(DataProviderNames.TariffCodes).DependsOn("TariffType");
            AddField(x => x.Quantity).Type(FieldType.Number).Mandatory();
            AddField(x => x.OriginIndicator).DefaultValue("D").Title("Origin Indicator").Mandatory()
                .Lookup(DataProviderNames.OriginIndicator);
            AddField(x => x.Weight).Type(FieldType.Number);
            AddField(x => x.InBond).DefaultValue("70").Lookup(DataProviderNames.InBond).Title("In-Bond").Mandatory();
            AddField(x => x.TransportRef).Title("Transport Ref").Mandatory();
            AddField(x => x.Container).DefaultValue("BLK").Lookup(DataProviderNames.Containers).Mandatory();
            AddField(x => x.Value).Type(FieldType.Number).Mandatory();
            AddField(x => x.SoldEnRoute).DefaultValue("N").Title("Sold En Route").Mandatory()
                .Lookup(DataProviderNames.YesNoLookup);
            AddField(x => x.ReferenceNumber).Title("Reference Number").Mandatory();
            AddField(x => x.OriginalItn).Title("Original ITN")
                .DependsOn("SoldEnRoute", new[] { "Y" });
            AddField(x => x.RoutedTransaction).DefaultValue("N").Title("Routed Transaction").Mandatory()
                .Lookup(DataProviderNames.YesNoLookup);
            AddField(x => x.ExportAdjustmentValue).DefaultValue("P").Title("Export Adjustment Value").Mandatory()
                .Lookup(DataProviderNames.ExportAdjustmentValue);


            AddField(x => x.GoodsDescription).Title("Goods Description").Mandatory();
            AddField(x => x.Description).Mandatory().Mandatory();
        }
    }
}