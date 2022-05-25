using Framework.Domain.Paging;
using Framework.Domain.Specifications;

namespace FilingPortal.Domain.Common
{
    public interface ISpecificationFactory
    {
        ISpecification<T> Create<T>(Filter filter) where T : class;
    }
}