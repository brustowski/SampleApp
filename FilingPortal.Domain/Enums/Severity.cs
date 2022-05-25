using Reinforced.Typings.Attributes;

namespace FilingPortal.Domain.Enums
{
    /// <summary>
    /// Validation result severity
    /// </summary>
    [TsEnum(FlattenHierarchy = true, IncludeNamespace = false)]
    public enum Severity
    {
        Error, Warning, Info
    }
}