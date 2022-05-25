//     This code was generated by a Reinforced.Typings tool. 
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.

export enum IsfPermissions { 
	ViewInboundRecord = 22001, 
	DeleteInboundRecord = 22002, 
	FileInboundRecord = 22003, 
	AddInboundRecord = 22004
}
export class IsfGridNames
{
	public static InboundRecords: string = `isf_inbound`;
	public static DefaultValues: string = `isf_default_values`;
	public static FilingGrid: string = `isf_filing_grid`;
}
export interface IsfInboundEditModel
{
	Id: number;
	ImporterId: string;
	BuyerId: string;
	BuyerAppAddress: any;
	ConsigneeId: string;
	MblScacCode: string;
	Eta: any;
	Etd: any;
	SellerId: string;
	SellerAppAddress: any;
	ShipToId: string;
	ShipToAppAddress: any;
	ContainerStuffingLocationId: string;
	ContainerStuffingLocationAppAddress: any;
	ConsolidatorId: string;
	ConsolidatorAppAddress: any;
	OwnerRef: string;
	Manufacturers: IsfInboundManufacturerRecordEditModel[];
	Bills: IsfInboundBillRecordEditModel[];
	Containers: IsfContainersRecordEditModel[];
}
export interface IsfContainersRecordEditModel
{
	Id: number;
	ContainerType: string;
	ContainerNumber: string;
}
export interface IsfInboundBillRecordEditModel
{
	Id: number;
	BillType: string;
	BillNumber: string;
}
export interface IsfInboundManufacturerRecordEditModel
{
	Id: number;
	InboundRecordId: number;
	ManufacturerId: string;
	Address: any;
	CountryOfOrigin: string;
	HtsNumbers: string;
}
export abstract class IsfDataProviderNames
{
	public static TableColumns: string = `isf_TableColumns`;
	public static TableNames: string = `isf_TableNames`;
	public static BillTypes: string = `isf_BillTypes`;
	public static FormConfiguration: string = `isf_FormConfiguration`;
}
export class IsfPageConfigNames
{
	public static InboundActions: string = `isf_InboundActionsConfiguration`;
	public static InboundListActions: string = `isf_InboundListActionsConfiguration`;
	public static InboundViewPageActions: string = `isf_InboundViewPageActionsConfiguration`;
}