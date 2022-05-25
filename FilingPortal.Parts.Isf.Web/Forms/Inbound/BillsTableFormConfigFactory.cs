using FilingPortal.Parts.Isf.Web.Configs;
using FilingPortal.Parts.Isf.Web.Models.AddInbound;
using FilingPortal.PluginEngine.FieldConfigurations.Common;

namespace FilingPortal.Parts.Isf.Web.Forms.Inbound
{
    /// <summary>
    /// Factory that provides Bills table configuration for add inbound record form
    /// </summary>
    public class BillsTableFormConfigFactory : FormConfigFactory<BillsRecordEditModel>
    {
        /// <summary>
        /// Creates configuration for bills table
        /// </summary>
        protected override void Setup()
        {
            AddField(x => x.BillType).Title("Bill Type").Mandatory().Lookup(DataProviderNames.BillTypes);
            AddField(x => x.BillNumber).Title("Bill Number").Mandatory();
        }
    }
}