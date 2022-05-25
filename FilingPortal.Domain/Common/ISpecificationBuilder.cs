using Framework.Domain.Paging;
using Framework.Domain.Specifications;

namespace FilingPortal.Domain.Common
{
    /// <summary>
    /// Defines specification builder
    /// </summary>
    public interface ISpecificationBuilder
    {
        /// <summary>
        /// Build specification from filters set
        /// </summary>
        /// <typeparam name="TModel">Type of the model corresponding to the filters set</typeparam>
        /// <param name="filtersSet">Filters set</param>
        ISpecification Build<TModel>(FiltersSet filtersSet) where TModel: class;
    }
}