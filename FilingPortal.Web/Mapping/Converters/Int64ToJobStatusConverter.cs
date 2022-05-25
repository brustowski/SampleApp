using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter for JobStatus from long
    /// </summary>
    public class Int64ToJobStatusConverter : LongToNullableEnumConverter<JobStatus>
    {

    }
}