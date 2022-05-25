using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter for MappingStatus from long
    /// </summary>
    public class LongToJobStatusConverter : LongToEnumConverter<JobStatus>
    {
    }
}