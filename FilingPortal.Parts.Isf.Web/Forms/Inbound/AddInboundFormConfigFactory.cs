using FilingPortal.Parts.Isf.Web.Models.AddInbound;
using FilingPortal.Parts.Isf.Web.Models.Inbound;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.FieldConfigurations.Common;

namespace FilingPortal.Parts.Isf.Web.Forms.Inbound
{
    /// <summary>
    /// Factory that provides Add inbound record configuration
    /// </summary>
    public class AddInboundFormConfigFactory : FormConfigFactory<InboundRecordEditModel>
    {
        /// <summary>
        /// Manufacturers table configuration builder
        /// </summary>
        private readonly IFormConfigFactory<InboundManufacturerRecordEditModel> _manufacturersFactory;
        /// <summary>
        /// Bills table configuration builder
        /// </summary>
        private readonly IFormConfigFactory<BillsRecordEditModel> _billsTableFactory;

        /// <summary>
        /// Containers table configuration builder
        /// </summary>
        private readonly IFormConfigFactory<ContainersRecordEditModel> _containersTableFactory;

        /// <summary>
        /// Creates a new instance of <see cref="AddInboundFormConfigFactory"/>
        /// </summary>
        /// <param name="manufacturersFactory">Manufacturers table configuration builder</param>
        /// <param name="billsTableFactory">Bills table configuration builder</param>
        /// <param name="containersTableFactory">Containers table configuration builder</param>
        public AddInboundFormConfigFactory(IFormConfigFactory<InboundManufacturerRecordEditModel> manufacturersFactory,
            IFormConfigFactory<BillsRecordEditModel> billsTableFactory,
            IFormConfigFactory<ContainersRecordEditModel> containersTableFactory
            )
        {
            _manufacturersFactory = manufacturersFactory;
            _billsTableFactory = billsTableFactory;
            _containersTableFactory = containersTableFactory;
        }

        /// <summary>
        /// Creates configuration for add new vessel form
        /// </summary>
        protected override void Setup()
        {
            AddField(x => x.ImporterId).Title("Importer").Mandatory().Lookup(DataProviderNames.AllClients);
            AddField(x => x.SellerId).Title("Seller").Lookup(DataProviderNames.AllClients);

            AddField(x => x.ConsigneeId).Title("Consignee").Lookup(DataProviderNames.AllClients).Separator();
            AddField(x => x.SellerAppAddress).Title("Address").Address()
                .DependsOn("SellerId").Separator();

            AddField(x => x.BuyerId).Title("Buyer").Lookup(DataProviderNames.AllClients);
            AddField(x => x.ShipToId).Title("Ship To").Lookup(DataProviderNames.AllClients);

            AddField(x => x.BuyerAppAddress).Title("Address").Address()
                .DependsOn("BuyerId").Separator();
            AddField(x => x.ShipToAppAddress).Title("Address").Address()
                .DependsOn("ShipToId").Separator();

            AddField(x => x.ContainerStuffingLocationId).Title("Container Stuffing Location")
                .Lookup(DataProviderNames.AllClients);
            AddField(x => x.ConsolidatorId).Title("Consolidator/Forwarder").Lookup(DataProviderNames.AllClients);


            AddField(x => x.ContainerStuffingLocationAppAddress).Title("Address")
                .Address().DependsOn("ContainerStuffingLocationId").Separator();
            AddField(x => x.ConsolidatorAppAddress).Title("Address")
                .Address().DependsOn("ConsolidatorId").Separator();

            AddField(x => x.MblScacCode).Title("MBL SCAC code").Lookup(DataProviderNames.IssuerCodes);
            AddField(x => x.Etd).Title("ETD").Type(FieldType.Date);
            AddField(x => x.OwnerRef).Title("Owner Ref").Separator();
            AddField(x => x.Eta).Title("ETA").Type(FieldType.Date);

            AddField(x => x.Bills).Table(_billsTableFactory.CreateFormConfig()).FullLineControl().Separator();
            AddField(x => x.Containers).Table(_containersTableFactory.CreateFormConfig()).FullLineControl().Separator();


            AddField(x => x.Manufacturers).Table(_manufacturersFactory.CreateFormConfig()).Long().FullLineControl();
        }
    }
}