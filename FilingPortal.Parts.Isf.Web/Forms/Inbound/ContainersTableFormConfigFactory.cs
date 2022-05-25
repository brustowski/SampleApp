using FilingPortal.Parts.Isf.Web.Models.AddInbound;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.FieldConfigurations.Common;

namespace FilingPortal.Parts.Isf.Web.Forms.Inbound
{
    /// <summary>
    /// Factory that provides Containers table configuration for add inbound record form
    /// </summary>
    public class ContainersTableFormConfigFactory : FormConfigFactory<ContainersRecordEditModel>
    {
        /// <summary>
        /// Creates configuration for bills table
        /// </summary>
        protected override void Setup()
        {
            AddField(x => x.ContainerType).Lookup(DataProviderNames.ContainerTypes).Title("Container Type"); ;
            AddField(x => x.ContainerNumber).Title("Container Number");
        }
    }
}