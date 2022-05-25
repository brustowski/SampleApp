using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.Web.Mapping.Converters;

namespace FilingPortal.Parts.Recon.Web.Mappings.Converters
{
    /// <summary>
    /// Converter for FTA Recon Status from long
    /// </summary>
    public class Int64ToFtaReconStatusConverter : LongToEnumConverter<FtaReconStatusValue>
    {

    }
}