using AutoMapper;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon;
using System.Collections.Generic;

namespace FilingPortal.Parts.Recon.Domain.Mapping.Resolvers
{
    /// <summary>
    /// Provides method to convert <see cref="InboundRecordReadModel"/> to <see cref="AllMismatchesModel"/>
    /// </summary>
    public class ReconValidationErrorsResolver : IValueResolver<InboundRecordReadModel, AllMismatchesModel, string>
    {
        /// <summary>
        /// The record validator
        /// </summary>
        private readonly ISingleRecordValidator<InboundRecordReadModel> _validator;

        /// <summary>
        /// Initialize a new instance of the <see cref="ReconValidationErrorsResolver"/> class.
        /// </summary>
        /// <param name="validator">The record validator</param>
        public ReconValidationErrorsResolver(ISingleRecordValidator<InboundRecordReadModel> validator) => _validator = validator;

        /// <summary>
        /// Implementors use source object to provide a destination object.
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object, if exists</param>
        /// <param name="destMember">Destination member</param>
        /// <param name="context">The context of the mapping</param>
        public string Resolve(InboundRecordReadModel source, AllMismatchesModel destination, string destMember,
            ResolutionContext context)
        {
            List<string> errors = _validator.GetErrors(source);

            return string.Join("; ", errors);
        }
    }
}
