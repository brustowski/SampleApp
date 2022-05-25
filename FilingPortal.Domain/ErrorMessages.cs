namespace FilingPortal.Domain
{
    /// <summary>
    /// Class for error messages
    /// </summary>
    public static class ErrorMessages
    {
        /// <summary>
        /// The message for unknown error during processing the record
        /// </summary>
        public static readonly string ProcessingRecordErrorMessage = "Error occured during processing your records. Please try again later or contact support team.";
        /// <summary>
        /// The message for unknown error during the filing process
        /// </summary>
        public static readonly string FilingProcessErrorMessage = "An error occurred during the filing process. Please contact the support team.";
        /// <summary>
        /// "Some filing parameters are invalid" error message
        /// </summary>
        public static readonly string InvalidFilingParameterValue = "Some filing parameters are invalid";
        /// <summary>
        /// The message for error of initial filing header creation process
        /// </summary>
        public static readonly string CreateInitialFilingHeaderError = "System has encountered unknown error during preparing data set for filing. Please check Rules in Configuration - Default Values table";
        /// <summary>
        /// The message for error if an action is performed on a nonexistent rule
        /// </summary>
        public static readonly string RuleDoesNotExist = "The specified rule does not exist.";
        /// <summary>
        /// The message for unknown error during rule delete process
        /// </summary>
        public static readonly string RuleCommonDeleteError = "Error occurred during delete rule process.";
        /// <summary>
        /// The message for unknown error during rule add process
        /// </summary>
        public static readonly string RuleCommonAddError = "Error occurred during add new rule process.";
        /// <summary>
        /// The message for unknown error during rule update process
        /// </summary>
        public static readonly string RuleCommonUpdateError = "Error occurred during updating rule process.";

        /// <summary>
        /// "Unknown request content" message
        /// </summary>
        public static readonly string UnknownRequestContent = "Unknown request content";
        /// <summary>
        /// "File is empty" message
        /// </summary>
        public static readonly string FileIsEmpty = "File is empty";
        /// <summary>
        /// "The request does not contain any files" message
        /// </summary>
        public static readonly string NoFilesInRequest = "The request does not contain any files";
        /// <summary>
        /// The message for parsing datasheet in Excel file error
        /// </summary>
        public static readonly string ExcelFileParsingError = "Error occurred during parsing the datasheet in Excel file";
        /// <summary>
        /// The message for unhandled error in validation process
        /// </summary>
        public static readonly string UnexpectedValidationError = "Unexpected error occurred during validation process";
        /// <summary>
        /// The message for error when deleting a file
        /// </summary>
        public static readonly string FileDeletingError = "Error occurred while deleting the file";
        /// <summary>
        /// The message for error if action is performed on nonexistent record
        /// </summary>
        public static readonly string RecordDoesNotExistError = "Record does not exist.";
        /// <summary>
        /// The message for unknown error during email sending process
        /// </summary>
        public static readonly string EmailSendingError = "An unexpected error occurred during sending the email message";
        /// <summary>
        /// The message for Insufficient Permissions
        /// </summary>
        public static readonly string InsufficientPermissions = "Insufficient Permissions";
    }
}
