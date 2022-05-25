using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories.Common;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain.Paging;

namespace FilingPortal.Web.FieldConfigurations
{
    /// <summary>
    /// Implements repository-driven dropdown rule
    /// </summary>
    /// <typeparam name="TDefValuesManualReadModel">Configuration field type</typeparam>
    public class AgileDropdownFieldRule<TDefValuesManualReadModel> : IDropdownFieldRule<TDefValuesManualReadModel> where TDefValuesManualReadModel : BaseDefValuesManualReadModel
    {
        private readonly IHandbookRepository _repository;

        /// <summary>
        /// Creates a new instance of <see cref="AgileDropdownFieldRule{TDefValuesManualReadModel}"/> 
        /// </summary>
        /// <param name="repository">Fields configuration repository</param>
        public AgileDropdownFieldRule(IHandbookRepository repository) => _repository = repository;

        /// <summary>
        /// Determines if field is dropdown
        /// </summary>
        /// <param name="model">Field</param>
        public bool IsDropdownField(TDefValuesManualReadModel model) => !string.IsNullOrWhiteSpace(model.HandbookName);

        /// <summary>
        /// Return available options for this field
        /// </summary>
        /// <param name="model">Field</param>
        public IEnumerable<LookupItem> GetOptions(TDefValuesManualReadModel model) => IsDropdownField(model) ? _repository.GetOptions(model.HandbookName) : new List<LookupItem<string>>();
    }
}