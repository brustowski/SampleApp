using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using FilingPortal.Web.Models.ClientManagement;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.ClientManagement
{
    /// <summary>
    /// Class describing the configuration for the Clients grid
    /// </summary>
    public class ClientsGridConfig : GridConfiguration<ClientViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.Clients;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.ClientName).DisplayName("Client Name").MinWidth(100).DefaultSorted();
            AddColumn(x => x.ClientType).DisplayName("Client Type").NotSortable();
            AddColumn(x => x.ClientCode).DisplayName("Client Code");
            AddColumn(x => x.Status).DisplayName("Status");
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.ClientName).Title("Client Name");
            TextFilterFor(x => x.ClientCode).Title("Client Code");
            SelectFilterFor(x => x.Status).Title("Status").DataSourceFrom<ClientStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.ClientType).Title("Client Type").SetOperand(FilterOperands.Custom).DataSourceFrom<ClientTypeFilterDataProvider>().NotSearch();

        }
    }
}