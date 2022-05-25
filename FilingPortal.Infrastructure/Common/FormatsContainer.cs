namespace FilingPortal.Infrastructure.Common
{
    /// <summary>
    /// Formats for dates, numbers and currency used in UI
    /// </summary>
    public static class FormatsContainer
    {
        /// <summary>
        /// The "MM/yyyy" datetime format
        /// </summary>
        public const string UI_MONTH_DATETIME_FORMAT = "MM/yyyy";
        /// <summary>
        /// The "MM/dd/yyyy" datetime format
        /// </summary>
        public const string UI_DATETIME_FORMAT = "M/d/yyyy";
        /// <summary>
        /// The "MM/dd/yy" datetime format
        /// </summary>
        public const string UI_DATETIME_FORMAT_SHORT_YEAR = "M/d/yy";
        /// <summary>
        /// The "M/d/yyyy" datetime short format
        /// </summary>
        public const string UI_DATETIME_SHORT_FORMAT = "M/d/yyyy";
        /// <summary>
        /// The "MM/yyyy" datetime format
        /// </summary>
        public const string UI_DATETIME_FORMAT_MONTH_YEAR = "MM/yyyy";
        /// <summary>
        /// The "MM/dd/yyyy hh:mm tt" datetime format
        /// </summary>
        public const string UI_DATETIME_FORMAT_FULL = "MM/dd/yyyy hh:mm tt";
        /// <summary>
        /// The "MMddyyyy_hhmm" datetime format for filename
        /// </summary>
        public const string UI_DATETIME_FORMAT_FOR_FILENAME = "MMddyyyy_hhmm";
        /// <summary>
        /// The "yyyyMMdd-hhmmss" datetime format for report filename
        /// </summary>
        public const string UI_DATETIME_FORMAT_FOR_REPORT_FILENAME = "yyyyMMdd-hhmmss";
        /// <summary>
        /// The currency format
        /// </summary>
        public const string UI_MONEY_FORMAT = "C";
        /// <summary>
        /// The numeric format
        /// </summary>
        public const string UI_NUMBER_FORMAT = "N";
    }
}