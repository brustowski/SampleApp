using FilingPortal.PluginEngine.Models;

namespace FilingPortal.PluginEngine.GridConfigurations
{
    /// <summary>
    /// Class for rule grid configuration
    /// </summary>
    public abstract class RuleGridConfiguration<TModel> : GridConfiguration<TModel> where TModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected sealed override void ConfigureColumns()
        {
            ConfigureEditableColumns();

            AddColumn(x => x.CreatedDate).DisplayName("Last Modified Date");
            AddColumn(x => x.CreatedUser).DisplayName("Last Modified By");
        }

        /// <summary>
        /// Configures editable rule columns
        /// </summary>
        protected abstract void ConfigureEditableColumns();
    }
}