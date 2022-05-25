using System.Collections.Generic;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.Common.Domain.Common;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain.Paging;

namespace FilingPortal.Web.FieldConfigurations
{
    /// <summary>
    /// Implements rule for dropdown fields
    /// </summary>
    /// <typeparam name="TDefValuesManualReadModel">Inbound record type</typeparam>
    public abstract class BaseDropdownFieldRule<TDefValuesManualReadModel> : IDropdownFieldRule<TDefValuesManualReadModel> where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Handbooks repository
        /// </summary>
        private readonly IHandbookRepository _repository;

        /// <summary>
        /// Creates a new instance for dropdown field builder
        /// </summary>
        /// <param name="repository">Handbooks repository</param>
        protected BaseDropdownFieldRule(IHandbookRepository repository) => _repository = repository;

        /// <summary>
        /// Defines the table name
        /// </summary>
        protected abstract string FieldTable { get; }

        /// <summary>
        /// Defines the Column name
        /// </summary>
        protected abstract string FieldColumnName { get; }

        /// <summary>
        /// Defines the handbook name
        /// </summary>
        protected abstract string HandbookName { get; }

        /// <summary>
        /// Determines if field is dropdown
        /// </summary>
        /// <param name="model">Field</param>
        public bool IsDropdownField(TDefValuesManualReadModel model) => model.GetUniqueData() == new DefValuesUniqueData(FieldTable, FieldColumnName);

        /// <summary>
        /// Return available options for this field
        /// </summary>
        /// <param name="model">Field</param>
        public IEnumerable<LookupItem> GetOptions(TDefValuesManualReadModel model) => _repository.GetOptions(HandbookName);
    }
}