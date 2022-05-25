using Framework.Domain.Paging;
using Framework.Domain.Specifications;

namespace FilingPortal.Domain.Common
{
    internal interface ICustomSpecificationsRegistry
    {
        ISpecification<T> GetSpecification<T>(Filter filter) where T : class;
    }
}