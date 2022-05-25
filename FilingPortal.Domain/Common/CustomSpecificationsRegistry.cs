using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;

namespace FilingPortal.Domain.Common
{
    internal class CustomSpecificationsRegistry : ICustomSpecificationsRegistry
    {
        private readonly ILifetimeScope _scope;

        public CustomSpecificationsRegistry(ILifetimeScope scope)
        {
            _scope = scope;
        }
        public ISpecification<T> GetSpecification<T>(Filter filter) where T : class
        {
            var specification = _scope.ResolveOptional<ICustomSpecification<T>>();
            if (specification == null) throw new KeyNotFoundException($"Specification for type {typeof(T).Name} not found");

            specification.SetValue(filter.FieldName, filter.Values.Select(x => x.Value).ToArray());

            return specification;
        }
    }
}