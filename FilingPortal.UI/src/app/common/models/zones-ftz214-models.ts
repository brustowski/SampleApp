//     This code was generated by a Reinforced.Typings tool. 
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.

export enum ZonesFtz214Permissions { 
	ViewInboundRecord = 21200, 
	DeleteInboundRecord = 21201, 
	FileInboundRecord = 21202, 
	ImportInboundRecord = 21203, 
	ViewRules = 21204, 
	EditRules = 21205, 
	DeleteRules = 21206
}
export class ZonesFtz214GridNames
{
	public static InboundRecords: string = `zones_vtz_214_inbound`;
	public static DefaultValues: string = `zones_vtz_214_default_values`;
	public static FilingGrid: string = `zones_vtz_214_filing_grid`;
	public static ImporterRuleGrid: string = `zones_vtz_214_importer_rule_grid`;
}
export interface ApplicantChangeModel
{
	ClientId: any;
}
export abstract class ZonesFtz214DataProviderNames
{
	public static TableColumns: string = `zones_ftz_214_TableColumns`;
	public static TableNames: string = `zones_ftz_214_TableNames`;
	public static FormConfiguration: string = `zones_ftz_214_FormConfiguration`;
}
export class ZonesFtz214PageConfigNames
{
	public static InboundActions: string = `zones_ftz_214_InboundActionsConfiguration`;
	public static InboundViewPageActions: string = `zones_ftz_214_InboundViewPageActionsConfiguration`;
	public static RulesPageActions: string = `zones_ftz_214_RulesPageActionsConfiguration`;
	public static RulesRecordActions: string = `zones_ftz_214_RulesRecordActionsConfiguration`;
}
