namespace FilingPortal.Parts.Common.Domain.Validators
{
    /// <summary>
    /// Class for validation messages
    /// </summary>
    public class ValidationMessages
    {
        /// <summary>
        /// "Filtered records can`t be filed together" error message
        /// </summary>
        public const string RecordsCantBeFiled = "Filtered records can`t be filed together";
        /// <summary>
        /// The message for invalid records status
        /// </summary>
        public const string InvalidRecordsStatus = "At least one of selected records has incorrect status";

        /// <summary>
        /// The message for invalid inbound records that shall have the same Importer, Supplier and HTS
        /// </summary>
        public const string InvalidRulesImporterSupplierTrainPortAndHts = "Selected rows shall have the same Importer, Supplier, Train #, Port and HTS";

        /// <summary>
        /// "Document Type is required" error message
        /// </summary>
        public const string DocumentTypeIsRequired = "Document Type is required";

        /// <summary>
        /// "Document Name is required" error message
        /// </summary>
        public const string DocumentNameIsRequired = "Document Name is required";

        /// <summary>
        /// The message for document type that Filing Header identifier is required
        /// </summary>
        public const string FilingHeaderIdIsRequired = "Filing header identifier is required";

        /// <summary>
        /// "Field value is required" message
        /// </summary>
        public const string FieldValueIsRequired = "Field value is required";

        /// <summary>
        /// "Incorrect value. Only Date (mm/dd/yyyy) values are allowed" message
        /// </summary>
        public const string DateFieldValueType = "Incorrect value. Only Date (mm/dd/yyyy) values are allowed";

        /// <summary>
        /// "Incorrect value. Only Number (decimals are supported) values are allowed" message
        /// </summary>
        public const string NumberFieldValueType = "Incorrect value. Only Number (decimals are supported) values are allowed";

        /// <summary>
        /// The message for invalid inbound records that shall have the same Importer, Supplier, Port, Description1  attributes in manifests
        /// </summary>
        public const string InvalidImporterSupplierDescriptions =
            "Selected rows shall have the same Importer, Supplier and Description1 attributes in manifests";

        /// <summary>
        /// "Selected rows should have the same Importer value" message
        /// </summary>
        public const string InvalidRulesImporter = "Selected rows should have the same Importer value";

        /// <summary>
        /// "System can not perform File operation as record(s) used in Filing has status which differs from \"In Review\" or does not belong to current entity already" message 
        /// </summary>
        public const string FormatCannotProceedOperationStatusDiffersOrNotBelong =
            "System can not perform {0} operation as record(s) used in Filing has status which differs from \"{1}\" or does not belong to current entity already";

        /// <summary>
        /// "System can not perform specified operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"" message
        /// </summary>
        public const string FormatCannotProceedOperationStatusDiffersFromOpenError =
            "System can not perform {0} operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"";

        /// <summary>
        /// "System can not perform Cancel operation as record(s) used in Filing has status which differs from \"In Review / Error\"" message
        /// </summary>
        public const string SystemCanNotPerformCancelAction =
            "System can not perform Cancel operation as record(s) used in Filing has status which differs from \"In Review/Error\" ";

        /// <summary>
        /// "The system cannot perform an operation" error message
        /// </summary>
        public const string SystemCanNotPerformOperation = "The system cannot perform an operation";

        /// <summary>
        /// "Rule already exists for such values" message
        /// </summary>
        public const string RuleAlreadyExists = "Rule already exists for such values";

        /// <summary>
        /// "Filing headers do not exist" message
        /// </summary>
        public const string FilingHeadersDoNotExist = "Filing headers do not exist";

        /// <summary>
        /// "Provided value does not match field format - integer" message
        /// </summary>
        public const string ProvidedValueNotInteger = "Provided value does not match field format - integer";

        /// <summary>
        /// "Provided value does not match field format - decimal" message
        /// </summary>
        public const string ProvidedValueNotDecimal = "Provided value does not match field format - decimal";

        /// <summary>
        /// "Provided value does not match field format - byte
        /// </summary>
        public const string ProvidedValueNotByte = "Provided value does not match field format - byte";

        /// <summary>
        /// "Provided value does not match field format
        /// </summary>
        public const string ProvidedValueNotMatchFieldFormat = "Provided value does not match {0} format";

        /// <summary>
        /// "The field must be up to {0} characters long" message format
        /// </summary>
        public const string ValueExceedsSpecifiedLength = "The field must be up to {0} characters long";
        /// <summary>
        /// "The field must be equal to {0} characters long" message format
        /// </summary>
        public const string ValueNotEqualSpecifiedLength = "The field must be equal to {0} characters long";

        /// <summary>
        /// "Importer Name is required" message
        /// </summary>
        public const string ImporterNameIsRequired = "Importer Name is required";

        /// <summary>
        /// "Supplier Name is required" message
        /// </summary>
        public const string SupplierNameIsRequired = "Supplier Name is required";

        /// <summary>
        /// "Id is required" message
        /// </summary>
        public const string IdIsRequired = "Id is required";

        /// <summary>
        /// "Manual is required" message
        /// </summary>
        public const string ManualIsRequired = "Manual is required";

        /// <summary>
        /// "'Display On UI' is required" message 
        /// </summary>
        public const string DisplayOnUIIsRequired = "'Display On UI' is required";

        /// <summary>
        /// "Has Default Value is required" message
        /// </summary>
        public const string HasDefaultValueIsRequired = "Has Default Value is required";

        /// <summary>
        /// "Editable is required" message
        /// </summary>
        public const string EditableIsRequired = "Editable is required";

        /// <summary>
        /// "Mandatory is required" message
        /// </summary>
        public const string MandatoryIsRequired = "Mandatory is required";

        /// <summary>
        /// "Port is required" message
        /// </summary>
        public const string PortIsRequired = "Port is required";

        /// <summary>
        /// "Facility is required" message
        /// </summary>
        public const string FacilityIsRequired = "Facility is required";
        /// <summary>
        /// "Description 1 is required" message
        /// </summary>
        public const string Description1IsRequired = "Description 1 is required";

        /// <summary>
        /// "Sheet is missing or renamed" message
        /// </summary>
        public const string SheetNotFound = "Sheet is missing or renamed";

        /// <summary>
        /// "Importer is required" message
        /// </summary>
        public static readonly string ImporterIsRequired = "Importer is required";

        /// <summary>
        /// "Supplier is required" message
        /// </summary>
        public static readonly string SupplierIsRequired = "Supplier is required";

        /// <summary>
        /// Valid or N/A required ,message 
        /// </summary>
        public const string ValidOrNaRequired = "Please enter valid input or 'N/A'";

        /// <summary>
        /// "Batch is required" message
        /// </summary>
        public const string BatchIsRequired = "Batch is required";

        /// <summary>
        /// "Ticket Number is required" message
        /// </summary>
        public const string TicketNumberIsRequired = "Ticket Number is required";

        /// <summary>
        /// "Quantity is required" message
        /// </summary>
        public const string QuantityIsRequired = "Quantity is required";

        /// <summary>
        /// "API is required" message
        /// </summary>
        public const string APIIsRequired = "API is required";

        /// <summary>
        /// "Export Date is required" message
        /// </summary>
        public const string ExportDateIsRequired = "Export Date is required";

        /// <summary>
        /// "Import Date is required" message
        /// </summary>
        public const string ImportDateIsRequired = "Import Date is required";

        /// <summary>      
        /// "Invalid Port" message
        /// </summary>
        public const string InvalidPort = "Invalid Port";

        /// <summary>
        /// "Invalid Imp code" message
        /// </summary>
        public const string InvalidImporter = "Invalid Importer Code";

        /// <summary>
        /// "Invalid Supplier" message
        /// </summary>
        public const string InvalidSupplier = "Invalid Supplier Code";

        /// <summary>
        /// "Invalid Supplier Address" message
        /// </summary>
        public const string InvalidSupplierAddress = "Invalid Supplier Address Code";

        /// <summary>
        /// "Invalid USPPI code" message
        /// </summary>
        public const string InvalidUsppiCode = "Invalid USPPI Code";

        /// <summary>
        /// "Invalid Consignee" message
        /// </summary>
        public const string InvalidConsignee = "Invalid Consignee";

        /// <summary>
        /// "Invalid Manufacturer" message
        /// </summary>
        public const string InvalidManufacturer = "Invalid Manufacturer";

        /// <summary>
        /// "Invalid Manufacturer Address" message
        /// </summary>
        public const string InvalidManufacturerAddress = "Invalid Manufacturer Address";

        /// <summary>
        /// "Invalid USPPI" message
        /// </summary>
        public const string InvalidUSPPI = "Invalid USPPI";

        /// <summary>
        /// "Invalid Address" message
        /// </summary>
        public const string InvalidAddress = "Invalid Address";

        /// <summary>
        /// "Invalid Contact" message
        /// </summary>
        public const string InvalidContact = "Invalid Contact";

        /// <summary>
        /// "Invalid Seller" message
        /// </summary>
        public const string InvalidSeller = "Invalid Seller";

        /// <summary>
        /// "Invalid Sold To Party" message
        /// </summary>
        public const string InvalidSoldToParty = "Invalid Sold To Party";

        /// <summary>
        /// "Invalid Ship To Party" message
        /// </summary>
        public const string InvalidShipToParty = "Invalid Ship To Party";

        /// <summary>
        /// "Invalid FIRMs" message
        /// </summary>
        public const string InvalidFIRMs = "Invalid FIRMs Code";

        /// <summary>
        /// "Invalid Origin" message
        /// </summary>
        public const string InvalidOrigin = "Invalid Origin ";

        /// <summary>
        /// "Invalid Destination" message
        /// </summary>
        public const string InvalidDestination = "Invalid Destination";

        /// <summary>
        /// "Invalid Export" message
        /// </summary>
        public const string InvalidExport = "Invalid Export";

        /// <summary>
        /// "Invalid data" message
        /// </summary>
        public const string InvalidData = "Invalid Data";

        /// <summary>
        /// "Entry number is required" message
        /// </summary>
        public static readonly string EntryNumberIsRequired = "Entry number is required";

        /// <summary>
        /// "PAPs is required" message
        /// </summary>
        public static readonly string PAPsIsRequired = "PAPs is required";

        /// <summary>
        /// "Provided value must be number with up to 12 digits before and up to 6 digits after decimal point" message 
        /// </summary>
        public static readonly string DecimalFormatMismatch = "Provided value must be number with up to 12 digits before and up to 6 digits after decimal point";
        /// <summary>
        /// "Provided value must be valid date" message
        /// </summary>
        public static readonly string DateFormatMismatch = "Wrong date format. Please provide date in M/d/yyyy or M/d/yy format";
        /// <summary>
        /// "Provided value must be valid ID" message
        /// </summary>
        public static readonly string IdFormatMismatch = "Provided value must be valid ID";
        /// <summary>
        /// "Duplicate record in file with line number: {0}" message
        /// </summary>
        public static readonly string DuplicatesRecordInFile = "Duplicate record in file with line number: {0}";
        /// <summary>
        /// "Duplicate records in DB" message
        /// </summary>
        public static readonly string DuplicatesRecordInDB = "Duplicate record in the DB";

        /// <summary>
        /// "Entry Port is required" message
        /// </summary>
        public static readonly string EntryPortIsRequired = "Entry Port is required";

        /// <summary>
        /// "Discharge Name is required" message
        /// </summary>
        public static readonly string DischargeNameIsRequired = "Discharge Name is required";

        /// <summary>
        /// "Tariff is required" message
        /// </summary>
        public static readonly string TariffIsRequired = "Tariff is required";

        /// <summary>
        /// "Exporter is required" message
        /// </summary>
        public const string ExporterIsRequired = "Exporter is required";

        /// <summary>
        /// "USPPI is required" message
        /// </summary>
        public const string USPPIIsRequired = "USPPI is required";

        /// <summary>
        /// "Tariff Type is required" message
        /// </summary>
        public const string TariffTypeIsRequired = "Tariff Type is required";

        /// <summary>
        /// "Sold En Route is required" message
        /// </summary>
        public const string SoldEnRouteIsRequired = "Sold en route is required";

        /// <summary>
        /// "Master Bill is required" message
        /// </summary>
        public const string MasterBillRequired = "Master Bill is required";

        /// <summary>
        /// "Origin is required" message
        /// </summary>
        public const string OriginIsRequired = "Origin is required";

        /// <summary>
        /// "Export is required" message
        /// </summary>
        public const string ExportIsRequired = "Export is required";

        /// <summary>
        /// "Routed Tran is required" message
        /// </summary>
        public const string RoutedTranIsRequired = "Routed Tran is required";

        /// <summary>
        /// "Hazardous is required" message
        /// </summary>
        public const string HazardousIsRequired = "Hazardous is required";

        /// <summary>
        /// "Customs Qty is required" message
        /// </summary>
        public const string CustomsQtyIsRequired = "Customs Qty is required";

        /// <summary>
        /// "Price is required" message
        /// </summary>
        public const string PriceIsRequired = "Price is required";

        /// <summary>
        /// "ECCN is required" message
        /// </summary>
        public const string ECCNIsRequired = "ECCN is required";

        /// <summary>
        /// "Gross Weight is required" message
        /// </summary>
        public const string GrossWeightIsRequired = "Gross Weight is required";

        /// <summary>
        /// "Gross Weight UOM is required" message
        /// </summary>
        public const string GrossWeightUOMIsRequired = "Gross Weight UOM is required";

        /// <summary>
        /// "Goods Description is required" message
        /// </summary>
        public const string GoodsDescriptionIsRequired = "Goods Description is required";

        /// <summary>
        /// "Consignee Code is required" message
        /// </summary>
        public const string ConsigneeCodeIsRequired = "Consignee Code is required";

        /// <summary>
        /// "Consolidated filing is not available for records with \"In Review\" status"
        /// </summary>
        public const string ConsolidatedFilingIsNotAvailbleForInReviewStatus =
            "Consolidated filing is not available for records with \"In Review\" status";

        /// <summary>
        /// "{0} is invalid" template
        /// </summary>
        public const string PropertyInvalid = "{0} is invalid";

        /// <summary>
        /// "{0} is required" template
        /// </summary>
        public const string PropertyRequired = "{0} is required";

        /// <summary>
        /// "Invalid integer format" message
        /// </summary>
        public const string IntegerFormatMismatch = "Integer format mismatch";

        /// <summary>
        /// "Incorrect value. Only [...] values are allowed" message
        /// </summary>
        public static readonly string IncorrectValueType = "Incorrect value. Only [{0}] values are allowed";
    }
}
