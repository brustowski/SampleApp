namespace FilingPortal.Domain.Common.Validation
{
    /// <summary>
    /// Provides various constants for validator
    /// </summary>
    public class ValidatorConstants
    {
        /// <summary>
        /// Regex expression for decimal format validation
        /// </summary>
        public static readonly string Decimal = @"^(([0-9]{0,12}\.[0-9]*)|([0-9]{0,12}))$";
    }
}
