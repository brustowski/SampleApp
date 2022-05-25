using AutoMapper;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Imports.Pipeline.RulePrice;
using FilingPortal.Domain.Repositories.Clients;
using System;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Domain.Mapping.Resolvers
{
    /// <summary>
    /// Resolves importer id on mapping between <see cref="ImportModel"/> and <see cref="PipelineRulePrice"/>
    /// </summary>
    public class ImporterIdResolver : IValueResolver<ImportModel, PipelineRulePrice, Guid>
    {
        /// <summary>
        /// Clients repository
        /// </summary>
        private readonly IClientRepository _repository;

        /// <summary>
        /// Creates a new instance of <see cref="ImporterIdResolver"/>
        /// </summary>
        /// <param name="repository">Clients repository</param>
        public ImporterIdResolver(IClientRepository repository)
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
        public Guid Resolve(ImportModel source, PipelineRulePrice destination, Guid destMember,
            ResolutionContext context)
        {
            Client client = _repository.GetClientByCode(source.ImporterCode);
            return client != null ? client.Id : default(Guid);
        }
    }
}