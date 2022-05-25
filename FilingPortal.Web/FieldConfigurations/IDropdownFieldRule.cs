using FilingPortal.Domain.Entities;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain.Paging;

namespace FilingPortal.Web.FieldConfigurations
{
    /// <summary>
    /// Describes rule for dropdown fields
    /// </summary>
    /// <typeparam name="TDefValuesManualReadModel">Field type</typeparam>
    public interface IDropdownFieldRule<in TDefValuesManualReadModel>
        where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Determines if field is dropdown
        /// </summary>
        /// <param name="model">Field</param>
        bool IsDropdownField(TDefValuesManualReadModel model);

        /// <summary>
        /// Return available options for this field
        /// </summary>
        /// <param name="model">Field</param>
        IEnumerable<LookupItem> GetOptions(TDefValuesManualReadModel model);
    }
}
