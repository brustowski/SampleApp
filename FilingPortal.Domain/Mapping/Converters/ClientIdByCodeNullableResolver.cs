using System;
using AutoMapper;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Domain.Mapping.Converters
{
    /// <summary>
    /// Resolves Client id by its code for specified member on mapping between models
    /// </summary>
    public class ClientIdByCodeNullableResolver : IMemberValueResolver<object, object, string, Guid?>
    {
        /// <summary>
        /// Clients repository
        /// </summary>
        private readonly IClientRepository _repository;

        /// <summary>
        /// Creates a new instance of <see cref="ClientIdByCodeNullableResolver"/> class.
        /// </summary>
        /// <param name="repository">Clients repository</param>
        public ClientIdByCodeNullableResolver(IClientRepository repository)
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
            Client client = _repository.GetClientByCode(sourceMember);
            return client?.Id;
        }
    }
}
