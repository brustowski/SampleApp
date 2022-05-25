//     This code was generated by a Reinforced.Typings tool. 
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.

export enum Severity { 
	Error = 0, 
	Warning = 1, 
	Info = 2
}
export enum RailInboundRecordStatus { 
	Open = 0, 
	Duplicates = 1, 
	Archived = 2, 
	Deleted = 3
}
export enum Permissions { 
	RailViewInboundRecord = 1, 
	RailDeleteInboundRecord = 2, 
	RailViewManifest = 3, 
	RailFileInboundRecord = 4, 
	PipelineImportInboundRecord = 5, 
	PipelineViewInboundRecord = 6, 
	PipelineDeleteInboundRecord = 7, 
	PipelineFileInboundRecord = 8, 
	TruckImportInboundRecord = 9, 
	TruckViewInboundRecord = 10, 
	TruckDeleteInboundRecord = 11, 
	TruckFileInboundRecord = 12, 
	ViewClients = 13, 
	RailViewInboundRecordRules = 14, 
	RailEditInboundRecordRules = 15, 
	RailDeleteInboundRecordRules = 16, 
	PipelineViewInboundRecordRules = 17, 
	PipelineEditInboundRecordRules = 18, 
	PipelineDeleteInboundRecordRules = 19, 
	TruckViewInboundRecordRules = 20, 
	TruckEditInboundRecordRules = 21, 
	TruckDeleteInboundRecordRules = 22, 
	ViewConfiguration = 23, 
	EditConfiguration = 24, 
	DeleteConfiguration = 25, 
	ExecuteSingleFiling = 26, 
	VesselViewImportRecordRules = 27, 
	VesselEditImportRecordRules = 28, 
	VesselDeleteImportRecordRules = 29, 
	VesselViewImportRecord = 30, 
	VesselDeleteImportRecord = 31, 
	VesselFileImportRecord = 32, 
	VesselAddImportRecord = 33, 
	TruckImportExportRecord = 34, 
	TruckViewExportRecord = 35, 
	TruckDeleteExportRecord = 36, 
	TruckFileExportRecord = 37, 
	TruckViewExportRecordRules = 38, 
	TruckEditExportRecordRules = 39, 
	TruckDeleteExportRecordRules = 40, 
	VesselAddExportRecord = 41, 
	VesselViewExportRecord = 42, 
	VesselDeleteExportRecord = 43, 
	VesselFileExportRecord = 44, 
	VesselViewExportRecordRules = 45, 
	VesselEditExportRecordRules = 46, 
	VesselDeleteExportRecordRules = 47, 
	AuditRailImportTrainConsistSheet = 48, 
	AuditRailDailyAudit = 49, 
	AdminAutoCreateConfiguration = 50
}
export interface FieldsValidationResult
{
	Fields: string;
	Message: string;
	OverrideId: string;
	Severity: Severity;
}
export class ReviewSectionExportModel
{
	public Values: ReviewSectionField[][];
	public SectionName: string;
	public Columns: ReviewSectionField[];
	public GetDynamicData() : any[]
	{
		return null;
	}
	public GetColumnInfos() : any[]
	{
		return null;
	}
	public GetImportConfiguration() : any
	{
		return null;
	}
}
export class ReviewSectionField
{
	public Id: string;
	public Value: string;
}
export class GeneratorFileNames
{
	public static PipelineApiCalculatorFileName: string = `api_calculator.xlsx`;
}
export class GridNames
{
	public static InboundRecords: string = `inbound_records`;
	public static InboundRecordsUniqueData: string = `inbound_records_unique_data`;
	public static RailRuleImporterSupplier: string = `rail_rule_importer_supplier`;
	public static RailRuleDescription: string = `rail_rule_description`;
	public static RailRulePort: string = `rail_rule_port`;
	public static RailDefaultValues: string = `rail_default_values`;
	public static RailSingleFilingGrid: string = `rail_single_filing`;
	public static RailManifestDataGrid: string = `rail_manifest_data`;
	public static PipelineInboundRecords: string = `pipeline_inbound_records`;
	public static PipelineRuleBatchCode: string = `pipeline_rule_batch_code`;
	public static PipelineRuleImporter: string = `pipeline_rule_importer`;
	public static PipelineRuleFacility: string = `pipeline_rule_facility`;
	public static PipelineDefaultValues: string = `pipeline_default_values`;
	public static PipelineInboundUniqueDataGrid: string = `pipeline_inbound_records_unique_data`;
	public static PipelineSingleFilingGrid: string = `pipeline_single_filing`;
	public static PipelineRuleConsigneeImporter: string = `pipeline_rule_consignee_importer`;
	public static PipelineRulePrice: string = `pipeline_rule_price`;
	public static TruckInboundRecords: string = `truck_inbound_records`;
	public static TruckRuleImporter: string = `truck_rule_importer`;
	public static TruckRulePort: string = `truck_rule_port`;
	public static TruckInboundUniqueDataGrid: string = `truck_inbound_records_unique_data`;
	public static TruckDefaultValues: string = `truck_default_values`;
	public static TruckSingleFilingGrid: string = `truck_single_filing`;
	public static TruckExportRuleConsignee: string = `truck_export_rule_consignee`;
	public static TruckExportRuleExporterConsignee: string = `truck_export_rule_exporter_consignee`;
	public static TruckExportRecords: string = `truck_export`;
	public static TruckExportDefaultValues: string = `truck_export_default_values`;
	public static TruckExportSingleFilingGrid: string = `truck_export_single_filing`;
	public static VesselRuleImporter: string = `vessel_rule_importer`;
	public static VesselRulePort: string = `vessel_rule_port`;
	public static VesselRuleProduct: string = `vessel_rule_product`;
	public static VesselImportRecords: string = `vessel_import_records`;
	public static VesselDefaultValues: string = `vessel_default_values`;
	public static VesselSingleFilingGrid: string = `vessel_single_filing`;
	public static VesselExportRuleUsppiConsignee: string = `vessel_export_rule_usppi_consignee`;
	public static VesselExportRecords: string = `vessel_export`;
	public static VesselExportDefaultValues: string = `vessel_export_default_values`;
	public static VesselExportSingleFilingGrid: string = `vessel_export_single_filing`;
	public static Clients: string = `clients`;
	public static AuditRailTrainConsistSheet: string = `rail_train_consist_sheet`;
	public static AuditRailDailyAudit: string = `rail_daily_audit`;
	public static AuditRailDailyAuditRules: string = `rail_daily_audit_rules`;
	public static AuditRailDailyAuditSpiRules: string = `rail_daily_audit_spi_rules`;
	public static AutoCreateRecords: string = `admin_auto_create_records`;
}
export enum PreFilingValidationErrorType { 
	None = 0, 
	ValidationFailed = 1, 
	InvalidStatus = 2, 
	MissingJobNumber = 3
}
export class PreFilingValidationResult
{
	public Id: number;
	public IsValid: boolean;
	public Error: string;
	public ErrorType: PreFilingValidationErrorType;
}
export interface XmlFileValidationErrorServer
{
	ErrorLevel: number;
	Tag: string;
	Error: string;
}
export interface ExcelFileValidationErrorServer
{
	ErrorLevel: number;
	Sheet: string;
	StringNumber: string;
	FieldName: string;
	Error: string;
}
export interface FieldErrorViewModel
{
	FieldName: string;
	Message: string;
}
export interface FileProcessingResultViewModelServer<TValidationError>
{
	ParsingErrors: TValidationError[];
	ValidationErrors: TValidationError[];
	CommonErrors: string[];
	FileName: string;
	Count: number;
}
export interface PageConfigurationModel
{
	Actions: { [key:string]: boolean };
}
export interface ValidationResultViewModel
{
	IsValid: boolean;
	CommonError: string;
}
export interface ValidationResultWithFieldsErrorsViewModel extends ValidationResultViewModel
{
	FieldsErrors: FieldErrorViewModel[];
	IsValid: boolean;
}
export interface ValidationResultWithFieldsErrorsViewModelTyped<T> extends ValidationResultWithFieldsErrorsViewModel
{
	Data: T;
}
export interface FilingResultViewModel
{
	ErrorMessage: string;
	FilingHeaderId: number;
	IsValid: boolean;
}
export interface AddressFieldEditModel
{
	Id: number;
	AddressId: string;
	AddressCode: string;
	Override: boolean;
	CompanyName: string;
	Addr1: string;
	Addr2: string;
	CountryCode: string;
	City: string;
	PostalCode: string;
	StateCode: string;
}
export enum HighlightingType { 
	NoHighlighting = 0, 
	Error = 1, 
	Warning = 2, 
	Success = 3, 
	Info = 4
}
export interface BaseInboundRecordField
{
	Title: string;
	Type: string;
	MarkedForFiltering: boolean;
}
export interface AddressInboundRecordField extends InboundRecordField
{
	ProviderName: string;
	IsDynamicProvider: boolean;
}
export interface ComplexInboundRecordField extends BaseInboundRecordField
{
	Fields: InboundRecordField[];
}
export interface DropdownInboundRecordField extends InboundRecordField
{
	ProviderName: string;
	IsDynamicProvider: boolean;
}
export interface InboundRecordField extends BaseInboundRecordField
{
	Id: number;
	RecordId: number;
	ParentRecordId: number;
	FilingHeaderId: number;
	DefaultValue: string;
	MaxLength: number;
	IsMandatory: boolean;
	IsDisabled: boolean;
	Prefix: string;
	DependOn: string;
	ConfirmationNeeded: boolean;
	Class: string;
}
export abstract class DataProviderNames
{
	public static ApplicationUser: string = `AppUser`;
	public static ApplicationUserData: string = `AppUserData`;
	public static CountryCodes: string = `CountryCodes`;
	public static Units: string = `Units`;
	public static IssuerCodes: string = `IssuerCodes`;
	public static DestinationCodes: string = `DestinationCodes`;
	public static FIRMsCodes: string = `FIRMsCodes`;
	public static Handbooks: string = `Handbooks`;
	public static ImporterCodes: string = `ImporterCodes`;
	public static RailProductDescriptions: string = `RailProductDescriptions`;
	public static RailRuleImporterNames: string = `RailRuleImporterNames`;
	public static RailRuleSupplierNames: string = `RailRuleSupplierNames`;
	public static Importers: string = `Importers`;
	public static ImporterLongTitles: string = `ImporterLongTitles`;
	public static Suppliers: string = `Suppliers`;
	public static SuppliersLongTitles: string = `SuppliersLongTitles`;
	public static OriginCodes: string = `OriginCodes`;
	public static PaymentTypes: string = `PaymentTypes`;
	public static PipelineTableColumns: string = `PipelineTableColumns`;
	public static PipelineTableNames: string = `PipelineTableNames`;
	public static PipelineFormConfiguration: string = `PipelineFormConfiguration`;
	public static PipelineRuleBatchCodes: string = `PipelineRuleBatchCodes`;
	public static PipelineRuleFacilities: string = `PipelineRuleFacilities`;
	public static PortCodes: string = `PortCodes`;
	public static RailTableColumns: string = `RailTableColumns`;
	public static RailTableNames: string = `RailTableNames`;
	public static RailFormConfiguration: string = `RailFormConfiguration`;
	public static StateNames: string = `StateNames`;
	public static StateIds: string = `StateIds`;
	public static SupplierCodes: string = `SupplierCodes`;
	public static TariffCodes: string = `TariffCodes`;
	public static HtsNumbers: string = `HtsNumbers`;
	public static TariffIds: string = `TariffIds`;
	public static TruckExportTableColumns: string = `TruckExportTableColumns`;
	public static TruckExportTableNames: string = `TruckExportTableNames`;
	public static TruckExportFormConfiguration: string = `TruckExportFormConfiguration`;
	public static TruckTableColumns: string = `TruckTableColumns`;
	public static TruckTableNames: string = `TruckTableNames`;
	public static TruckFormConfiguration: string = `TruckFormConfiguration`;
	public static UOMValues: string = `UOM Values`;
	public static ValueRecons: string = `Value Recons`;
	public static YesNoLookup: string = `YesNoLookup`;
	public static Vessels: string = `Vessels`;
	public static Containers: string = `Containers`;
	public static EntryTypes: string = `EntryTypes`;
	public static VesselProductDescriptions: string = `VesselProductDescriptions`;
	public static VesselTableColumns: string = `VesselTableColumns`;
	public static VesselTableNames: string = `VesselTableNames`;
	public static VesselFormConfiguration: string = `VesselFormConfiguration`;
	public static VesselExportTableColumns: string = `VesselExportTableColumns`;
	public static VesselExportTableNames: string = `VesselExportTableNames`;
	public static VesselExportFormConfiguration: string = `VesselExportFormConfiguration`;
	public static TariffTypes: string = `TariffTypes`;
	public static DomesticPorts: string = `DomesticPorts`;
	public static UNLOCOs: string = `UNLOCOs`;
	public static DischargePorts: string = `DischargePorts`;
	public static CountryNames: string = `CountryNames`;
	public static Countries: string = `Countries`;
	public static DischargePortCountries: string = `DischargePortCountries`;
	public static OriginIndicator: string = `OriginIndicator`;
	public static InBond: string = `InBond`;
	public static ExportAdjustmentValue: string = `ExportAdjustmentValue`;
	public static ConsigneeTypeLookup: string = `ConsigneeTypeLookup`;
	public static ClientAddresses: string = `ClientAddresses`;
	public static ClientAddressCode: string = `ClientAddressCode`;
	public static ClientContacts: string = `ClientContacts`;
	public static ClientContactCode: string = `ClientContactCode`;
	public static ContactPhones: string = `ContactPhones`;
	public static CargowiseFirmsCodes: string = `CargowiseFirmsCodes`;
	public static CargowiseUnlocoDictionary: string = `CargowiseUnlocoDictionary`;
	public static CargowisePortsOfClearance: string = `CargowisePortsOfClearance`;
	public static VesselImportsStateIds: string = `VesselImportsStateIds`;
	public static AllClients: string = `AllClients`;
	public static ClientCode: string = `ClientCode`;
	public static CargowiseFinalDestinations: string = `CargowiseFinalDestinations`;
	public static CargowisePortsOfArrival: string = `CargowisePortsOfArrival`;
	public static CargowiseSubLocation: string = `CargowiseSubLocation`;
	public static PacksUnitOfMeasure: string = `PacksUnitOfMeasure`;
	public static TransportModeNumber: string = `TransportModeNumber`;
	public static TransportModeCode: string = `TransportModeCode`;
	public static ContainerTypes: string = `ContainerTypes`;
	public static FieldType: string = `FieldType`;
	public static RuleErrorStatus: string = `RuleErrorStatus`;
	public static ShipmentTypes: string = `ShipmentTypes`;
	public static FreightTypes: string = `FreightTypes`;
	public static ReconIssue: string = `ReconIssue`;
	public static CertifyingIndividual: string = `CertifyingIndividual`;
	public static EpaTsca: string = `EpaTsca`;
	public static EntryDateElectionCode: string = `EntryDateElectionCode`;
}
export interface FilingResultBuilder
{
	IsValid: boolean;
	Messages: string[];
	Results: FilingResultViewModel[];
}
export enum FilingStatus { 
	Open = 0, 
	InProgress = 1, 
	Filed = 2, 
	Error = 3, 
	Updated = 4
}
export enum JobStatus { 
	Open = 0, 
	InReview = 1, 
	InProgress = 2, 
	Created = 3, 
	MappingError = 4, 
	CreatingError = 5, 
	WaitingUpdate = 6, 
	Updated = 7, 
	UpdatingError = 8
}
export enum MappingStatus { 
	Open = 0, 
	InReview = 1, 
	InProgress = 2, 
	Mapped = 3, 
	Error = 4, 
	Updated = 5
}
export class PageConfigNames
{
	public static RailRulesPageActions: string = `RailRulesPageActions`;
	public static RailViewPageActions: string = `RailViewPageActions`;
	public static TruckRuleConfigName: string = `TruckRuleActionsConfiguration`;
	public static TruckRulesPageActions: string = `TruckRulesPageActions`;
	public static TruckViewPageActions: string = `TruckViewPageActions`;
	public static ConfigurationPageActions: string = `ConfigurationPageActions`;
	public static PipelineViewPageActions: string = `PipelineViewPageActions`;
	public static PipelineRuleConfigName: string = `PipelineRuleActionsConfiguration`;
	public static PipelineRulesPageActions: string = `PipelineRulesPageActions`;
	public static PipelineInboundActions: string = `PipelineInboundActionsConfiguration`;
	public static PipelineListInboundActions: string = `PipelineInboundListActionsConfiguration`;
	public static VesselRuleConfigName: string = `VesselRuleActionsConfiguration`;
	public static VesselRulesPageActions: string = `VesselRulePageAction`;
	public static VesselImportActions: string = `VesselImportActionsConfiguration`;
	public static VesselListImportActions: string = `VesselImportListActionsConfiguration`;
	public static VesselViewPageActions: string = `VesselViewPageActions`;
	public static TruckExportActions: string = `TruckExportActionsConfiguration`;
	public static TruckExportViewPageActions: string = `TruckExportViewPageActions`;
	public static TruckExportRulesPageActions: string = `TruckExportRulesPageActions`;
	public static DefValueActionsConfigName: string = `DefValueActionsConfiguration`;
	public static VesselExportActions: string = `VesselExportActionsConfiguration`;
	public static VesselExportListActions: string = `VesselExportListActionsConfiguration`;
	public static VesselExportViewPageActions: string = `VesselExportViewPageActions`;
	public static VesselExportRuleConfigName: string = `VesselExportRuleActionsConfiguration`;
	public static VesselExportRulesPageActions: string = `VesselExportRulePageAction`;
	public static AuditRailTrainConsistSheetPageActions: string = `AuditRailTrainConsistSheetPageActions`;
	public static AuditRailDailyAuditRulesPageActions: string = `AuditRailDailyAuditRulesPageActions`;
	public static AdminRulesPageActions: string = `AdminRulesPageActions`;
}
export interface LookupItemEditModel
{
	ProviderName: string;
	Value: string;
	DependValue: any;
}
export interface VesselImportEditModel
{
	Id: number;
	ImporterId: string;
	SupplierId: string;
	VesselId: number;
	StateId: number;
	FirmsCodeId: number;
	ClassificationId: number;
	ProductDescriptionId: number;
	Eta: string;
	FilerId: string;
	Container: string;
	EntryType: string;
	CustomsQty: number;
	UnitPrice: number;
	OwnerRef: string;
	CountryOfOriginId: number;
}
export interface VesselImportViewModel
{
	ImporterCode: string;
	SupplierCode: string;
	Vessel: string;
	State: string;
	FirmsCode: string;
	Classification: string;
	ProductDescription: string;
	Eta: string;
	FilerId: string;
	Container: string;
	EntryType: string;
	CustomsQty: string;
	UnitPrice: string;
	OwnerRef: string;
	CountryOfOrigin: string;
	Errors: string[];
	MappingStatus: number;
	MappingStatusTitle: string;
	FilingStatus: number;
	FilingStatusTitle: string;
	EntryStatus: string;
	EntryStatusDescription: string;
	HasAllRequiredRules: boolean;
	CreatedDate: string;
	FilingNumber: string;
	JobLink: string;
	FilingHeaderId: string;
	Id: number;
	HighlightingType: HighlightingType;
}
export interface VesselExportEditModel
{
	Id: number;
	UsppiId: string;
	AddressId: string;
	ContactId: string;
	Phone: string;
	ImporterId: string;
	VesselId: number;
	ExportDate: string;
	LoadPort: string;
	DischargePort: string;
	CountryOfDestinationId: number;
	TariffType: string;
	Tariff: string;
	GoodsDescription: string;
	OriginIndicator: string;
	Quantity: number;
	Weight: number;
	Value: number;
	TransportRef: string;
	Container: string;
	InBond: string;
	SoldEnRoute: string;
	ExportAdjustmentValue: string;
	OriginalItn: string;
	RoutedTransaction: string;
	ReferenceNumber: string;
	Description: string;
}
export interface RailInboundEditModel
{
	Id: number;
	Consignee: string;
	Importer: string;
	Supplier: string;
	Description: string;
	EquipmentInitial: string;
	EquipmentNumber: string;
	IssuerCode: string;
	BillOfLading: string;
	PortOfUnlading: string;
	ManifestUnits: string;
	Weight: string;
	WeightUnit: string;
	ReferenceNumber: string;
	Destination: string;
}
export interface AppUserViewModel
{
	Status: string;
	UserAccount: string;
	Permissions: number[];
}
export interface FileProcessingDetailedResultViewModelServer<TValidationError> extends FileProcessingResultViewModelServer<TValidationError>
{
	Inserted: number;
	Updated: number;
	Payload: any;
}
export interface DocumentViewModelServer
{
	Id: number;
	Name: string;
	Description: string;
	Type: string;
	Status: number;
	IsManifest: boolean;
}
export enum TreeNodeType { 
	Container = 0, 
	Field = 1, 
	Document = 2
}
export interface TreeNodeServer
{
	Id: number;
	Name: string;
	Title: string;
	NodeType: TreeNodeType;
	Actions: { [key:string]: boolean };
	ParentId: number;
	Children: TreeNodeServer[];
}
export interface FieldTreeNodeServer extends TreeNodeServer
{
	Fields: BaseInboundRecordField[];
}
export interface DocumentTreeNodeServer extends TreeNodeServer
{
	Documents: DocumentViewModelServer[];
}
export interface FilingConfigurationFieldServer
{
	Id: number;
	FilingHeaderId: number;
	RecordId: number;
	ParentRecordId: number;
	SectionName: string;
	SectionTitle: string;
	Order: number;
	IsVisibleOn7501: boolean;
	IsVisibleOnRuleDrivenData: boolean;
	Title: string;
	Name: string;
	Value: string;
	Description: string;
	Type: string;
	MaxLength: number;
	IsMandatory: boolean;
	IsDisabled: boolean;
	Field: BaseInboundRecordField;
}
export interface FilingConfigurationSectionServer
{
	Id: number;
	Name: string;
	Title: string;
	ParentId: number;
	IsSingleSection: boolean;
	DisplayAsGrid: boolean;
}
export interface FilingConfigurationServer
{
	FilingHeaderId: number;
	Fields: FilingConfigurationFieldServer[];
	Sections: FilingConfigurationSectionServer[];
	Documents: DocumentViewModelServer[];
}