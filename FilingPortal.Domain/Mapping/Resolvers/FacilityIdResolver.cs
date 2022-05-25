﻿using AutoMapper;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Imports.Pipeline.RulePrice;
using FilingPortal.Domain.Repositories.Pipeline;

namespace FilingPortal.Domain.Mapping.Resolvers
{
    /// <summary>
    /// Resolves Facility Id on mapping between <see cref="ImportModel"/> and <see cref="PipelineRulePrice"/>
    /// </summary>
    public class FacilityIdResolver : IValueResolver<ImportModel, PipelineRulePrice, int?>
    {
        /// <summary>
        /// Pipeline facility rule repository
        /// </summary>
        private readonly IPipelineRuleFacilityRepository _repository;

        /// <summary>
        /// Creates a new instance of <see cref="FacilityIdResolver"/>
        /// </summary>
        /// <param name="repository">Pipeline batch code rule repository</param>
        public FacilityIdResolver(IPipelineRuleFacilityRepository repository)
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
        public int? Resolve(ImportModel source, PipelineRulePrice destination, int? destMember,
            ResolutionContext context)
        {
            PipelineRuleFacility facility = _repository.GetByFacilityName(source.Facility);
            return facility != null ? facility.Id : (int?)null;
        }
    }
}