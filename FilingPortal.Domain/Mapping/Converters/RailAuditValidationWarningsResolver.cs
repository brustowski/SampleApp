using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using AutoMapper;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.GridExport.Models.Audit.Rail;
using FilingPortal.Domain.Validators;

namespace FilingPortal.Domain.Mapping.Converters
{
    /// <summary>
    /// Provides method to convert <see cref="AuditRailDaily"/> to <see cref="AuditRailDailyAuditReportModel"/>
    /// </summary>
    public class RailAuditValidationWarningsResolver : IValueResolver<AuditRailDaily, AuditRailDailyAuditReportModel, string>
    {
        private readonly ISingleRecordTypedValidator<FieldsValidationResult, AuditRailDaily> _validator;

        public RailAuditValidationWarningsResolver(
            ISingleRecordTypedValidator<FieldsValidationResult, AuditRailDaily> validator) => _validator = validator;

        /// <summary>
        /// Implementors use source object to provide a destination object.
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object, if exists</param>
        /// <param name="destMember">Destination member</param>
        /// <param name="context">The context of the mapping</param>
        /// <returns>Result, typically build from the source resolution result</returns>
        public string Resolve(AuditRailDaily source, AuditRailDailyAuditReportModel destination, string destMember,
            ResolutionContext context)
        {
            List<FieldsValidationResult> errors = Task.Run(async () => await _validator.GetErrorsAsync(source)).Result;

            return string.Join("; ", errors.Where(x => x.Severity == Severity.Warning).Select(x => x.Message));
        }
    }
}
