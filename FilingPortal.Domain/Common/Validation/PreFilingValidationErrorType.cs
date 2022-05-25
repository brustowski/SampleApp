using Reinforced.Typings.Attributes;

namespace FilingPortal.Domain.Common.Validation
{
    [TsEnum(IncludeNamespace = false)]
    public enum PreFilingValidationErrorType
    {
        None,
        ValidationFailed,
        InvalidStatus,
        MissingJobNumber
    }
}
