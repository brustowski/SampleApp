using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.Models.Vessel;

namespace FilingPortal.Web.FieldConfigurations.Vessel
{
    /// <summary>
    /// Factory that provides Vessel Form configuration
    /// </summary>
    public class VesselFormConfigFactory : FormConfigFactory<VesselImportEditModel>
    {
        /// <summary>
        /// Creates configuration for add new vessel form
        /// </summary>
        protected override void Setup()
        {
            AddField(x => x.ImporterId).Title("Importer").Long().Mandatory().Lookup(DataProviderNames.Importers);
            AddField(x => x.SupplierId).Title("Supplier").Long().Lookup(DataProviderNames.Suppliers);
            AddField(x => x.VesselId).Title("Vessel").Long().Mandatory().Separator().Lookup(DataProviderNames.Vessels, true);
            AddField(x => x.CustomsQty).Title("Customs Qty").Type(FieldType.Number).Mandatory();
            AddField(x => x.UnitPrice).Title("Unit Price").Type(FieldType.Number).Mandatory();
            AddField(x => x.OwnerRef).Title("Owner Ref").Mandatory();
            AddField(x => x.CountryOfOriginId).Title("Country of Origin").Mandatory().Lookup(DataProviderNames.Countries);
            AddField(x => x.StateId).Title("State").Mandatory().Lookup(DataProviderNames.VesselImportsStateIds);
            AddField(x => x.FirmsCodeId).Title("FIRMs Code").Mandatory()
                .Lookup(DataProviderNames.CargowiseFirmsCodes).DependsOn("StateId");
            AddField(x => x.ClassificationId).Title("Classification").Mandatory().Lookup(DataProviderNames.TariffIds);
            AddField(x => x.ProductDescriptionId).Title("Product Description").Mandatory()
                .Lookup(DataProviderNames.VesselProductDescriptions, true).DependsOn("ClassificationId");
            AddField(x => x.Eta).Type(FieldType.Date).Title("ETA");
            AddField(x => x.FilerId).Title("Filer ID").Mandatory().Lookup(DataProviderNames.ApplicationUserData);
            AddField(x => x.Container, "BLK").Mandatory().Lookup(DataProviderNames.Containers);
            AddField(x => x.EntryType, "01").Title("Entry Type").Mandatory().Lookup(DataProviderNames.EntryTypes);
        }
    }
}