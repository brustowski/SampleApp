using System;
using AutoMapper;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Domain.Import.Inbound;

namespace FilingPortal.Parts.Rail.Export.Domain.Mapping.Resolvers
{
    /// <summary>
    /// Resolves Client id on mapping between <see cref="InboundImportModel"/> and <see cref="InboundRecord"/>
    /// </summary>
    public class ClientIdResolver : IValueResolver<InboundImportModel, InboundRecord, Guid>
    {
        /// <summary>
        /// Clients repository
        /// </summary>
        private readonly IClientRepository _repository;

        /// <summary>
        /// Creates a new instance of <see cref="ClientIdResolver"/> class.
        /// </summary>
        /// <param name="repository">Clients repository</param>
        public ClientIdResolver(IClientRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Implementors use source object to provide a destination object.
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object, if exists</param>
        /// <param name="destMember">Destination member</param>
        /// <param name="context">The context of the mapping</param>
        public Guid Resolve(InboundImportModel source, InboundRecord destination, Guid destMember,
            ResolutionContext context)
        {
            Client client = _repository.GetClientByCode(source.Exporter);
            return client != null ? client.Id : default(Guid);
        }
    }
}