using AutoMapper;
using FilingPortal.Domain.Mapping.Converters;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using System;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Mapping.Converters
{
    /// <summary>
    /// Resolves Product id by its code for specified member on mapping between models
    /// </summary>
    public class ProductIdByCodeNullableResolver : IMemberValueResolver<object, object, string, Guid?>
    {
        /// <summary>
        /// Clients repository
        /// </summary>
        private readonly IDataProviderRepository<ProductCode, Guid> _repository;

        /// <summary>
        /// Creates a new instance of <see cref="ClientIdByCodeNullableResolver"/> class.
        /// </summary>
        /// <param name="repository">Clients repository</param>
        public ProductIdByCodeNullableResolver(IDataProviderRepository<ProductCode, Guid> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Implementors use source object to provide a destination object.
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object, if exists</param>
        /// <param name="sourceMember">Source member</param>
        /// <param name="destMember">Destination member</param>
        /// <param name="context">The context of the mapping</param>
        public Guid? Resolve(object source, object destination, string sourceMember, Guid? destMember,
            ResolutionContext context)
        {
            ProductCode productCode = _repository.Search(sourceMember, 1).FirstOrDefault();
            return productCode?.Id;
        }
    }
}
