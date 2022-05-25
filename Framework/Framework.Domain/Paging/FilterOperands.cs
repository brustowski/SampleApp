namespace Framework.Domain.Paging
{
    /// <summary>
    /// Set of known filter operands
    /// </summary>
    public static class FilterOperands
    {
        /// <summary>
        /// Field contains filter value
        /// </summary>
        public const string Contains = "contains";

        /// <summary>
        /// Custom value processing
        /// </summary>
        public const string Custom = "custom";

        /// <summary>
        /// Field value between filter values
        /// </summary>
        public const string DateBetween = "dateBetween";

        /// <summary>
        /// Field value is date and greater then filter value
        /// </summary>
        public const string DateFrom = "dateFrom";

        /// <summary>
        /// Field value is date and less then filter value
        /// </summary>
        public const string DateTo = "dateTo";

        /// <summary>
        /// Field value is equal to filter value
        /// </summary>
        public const string Equal = "equals";

        /// <summary>
        /// Field value is not equal to filter value
        /// </summary>
        public const string NotEqual = "notequals";
    }
}
