using AutoMapper;
using FilingPortal.Domain.Repositories.Clients;
using System;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Domain.Mapping.Converters
{
    /// <summary>
    /// Resolves Client Address id by its code for specified member on mapping between models
    /// </summary>
    public class ClientAddressIdByCodeNullableResolver : IMemberValueResolver<object, object, string, Guid?>
    {
        /// <summary>
        /// Clients address repository
        /// </summary>
        private readonly IClientAddressRepository _repository;

        /// <summary>
        /// Creates a new instance of <see cref="ClientAddressIdByCodeNullableResolver"/> class.
        /// </summary>
        /// <param name="repository">Clients Address repository</param>
        public ClientAddressIdByCodeNullableResolver(IClientAddressRepository repository)
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
        public Guid? Resolve(object source, object destination, string sourceMember, Guid? destMember, ResolutionContext context)
        {
            ClientAddress address = _repository.GetByCode(sourceMember);
            return address?.Id;
        }
    }
}
