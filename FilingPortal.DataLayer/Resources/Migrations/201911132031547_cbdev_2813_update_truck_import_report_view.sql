ALTER VIEW dbo.v_imp_truck_report 
AS SELECT
  header.id

 ,declaration.main_supplier as Main_Supplier
 ,declaration.importer as Importer
 ,declaration.shipment_type	as Shipment_Type
 ,declaration.transport as Transport
 ,declaration.entry_type as Entry_Type
 ,declaration.rlf as RLF
 ,declaration.enable_entry_sum as Enable_Entry_Sum
 ,declaration.[type] as Type
 ,declaration.certify_cargo_release as Certify_Cargo_Release
 ,declaration.[service] as Service
 ,declaration.issuer as Issuer
 ,declaration.master_bill as Master_Bill
 ,declaration.carrier_scac as Carrier_SCAC
 ,declaration.discharge as Discharge
 ,declaration.entry_port as Entry_Port
 ,declaration.dep as Dep
 ,declaration.arr as Arr
 ,declaration.arr2 AS Arr_2
 ,declaration.hmf as HMF
 ,declaration.origin as Origin
 ,declaration.destination as Destination
 ,declaration.destination_state as Destination_State
 ,declaration.country_of_export as Country_of_Export
 ,declaration.eta as ETA
 ,declaration.export_date as Export_Date
 ,declaration.[description] as Description
 ,declaration.owner_ref as Owner_Ref
 ,declaration.inco as INCO
 ,declaration.total_weight AS Total_Weight
 ,declaration.total_volume as Total_Volume
 ,declaration.no_packages as No_Packages
 ,declaration.firms_code as FIRMs_Code
 ,declaration.centralized_exam_site AS Centralized_Exam_Site
 ,declaration.purchased as Purchased
 ,declaration.manual_entry as Manual_Entry
 ,declaration.importer_of_record as Importer_of_record
 ,declaration.split_shipment_release as Split_Shipment_Release
 ,declaration.check_local_client as Check_Local_Client
 ,declaration.container AS Truck_DeclarationTab_Container

 ,container.bill_type as Bill_Type
 ,container.bill_num as Bill_Num
 ,container.bill_number as Bill_Number
 ,container.uq AS Containers_UQ
 ,container.manifest_qty as Manifest_QTY
 ,container.packing_uq as Packing_UQ
 ,container.bill_issuer_scac as Bill_Issuer_SCAC

 ,invoice.invoice_no as Invoice_No
 ,invoice.consignee_address as Consignee_Address
 ,invoice.invoice_total as Invoice_Total
 ,invoice.curr as Curr
 ,invoice.payment_date as Payment_Date
 ,invoice.consignee as Consignee
 ,invoice.inv_date as Inv_Date
 ,invoice.agreed_place as Agreed_Place
 ,invoice.inv_gross_weight as Inv_Gross_Weight
 ,invoice.net_weight as Net_Weight
 ,invoice.manufacturer as Manufacturer
 ,invoice.seller as Seller
 ,invoice.sold_to_party as Sold_to_party
 ,invoice.ship_to_party as Ship_to_party
 ,invoice.broker_pga_contact_name as Broker_PGA_Contact_Name
 ,invoice.broker_pga_contact_phone as Broker_PGA_Contact_Phone
 ,invoice.broker_pga_contact_email as Broker_PGA_Contact_Email
 ,invoice.transaction_related AS Truck_InvoiceHeaders_Transaction_Related
 ,invoice.epa_tsca_cert_date AS Truck_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,invoice.supplier AS Truck_InvoiceHeaders_Supplier
 ,invoice.origin AS Truck_InvoiceHeaders_Origin

 ,line.invoice_line_number AS LNO
 ,line.tariff as Tariff
 ,line.customs_qty as Customs_QTY
 ,line.line_price as Line_Price
 ,line.goods_description as Goods_Description
 ,line.org as ORG
 ,line.spi as SPI
 ,line.gr_weight as Gr_Weight
 ,line.gr_weight_unit as Gr_Weight_Unit
 ,line.gr_weight_tons as Gr_Weight_Tons
 ,line.uq as UQ
 ,line.price_unit AS PriceUnit
 ,line.prod_id1 AS Prod_ID_1
 ,line.attribute1 AS Attribute_1
 ,line.attribute2 AS Attribute_2
 ,line.export as Export
 ,line.invoice_qty as Invoice_Qty
 ,line.invoice_qty_unit as Invoice_Qty_Unit
 ,line.code as Code
 ,line.amount as Amount
 ,line.cif_component as CIF_Component
 ,line.epa_tsca_toxic_substance_control_act_indicator as EPA_TSCA_Toxic_Substance_Control_Act_Indicator
 ,line.tsca_indicator as TSCA_Indicator
 ,line.certifying_individual as Certifying_Individual
 ,line.manufacturer AS Truck_InvoiceLines_Manufacture
 ,line.transaction_related AS Truck_InvoiceLines_Transaction_Related

 ,misc.branch as Branch
 ,misc.[broker] as Broker
 ,misc.merge_by as Merge_By
 ,misc.tax_deferrable_ind as Tax_Deferrable_Ind
 ,misc.preparer_dist_port as Preparer_Dist_Port
 ,misc.recon_issue as Recon_Issue
 ,misc.fta_recon as FTA_Recon
 ,misc.bond_type as Bond_Type
 ,misc.payment_type as Payment_Type
 ,misc.broker_to_pay as Broker_to_Pay
 ,misc.prelim_statement_date as Prelim_Statement_Date
 ,misc.submitter as Submitter
FROM dbo.imp_truck_filing_header header
LEFT JOIN dbo.imp_truck_container container
  ON container.filing_header_id = header.id
LEFT JOIN dbo.imp_truck_declaration declaration
  ON declaration.filing_header_id = header.id
LEFT JOIN dbo.imp_truck_invoice_header invoice
  ON invoice.filing_header_id = header.id
LEFT JOIN dbo.imp_truck_invoice_line line
  ON line.parent_record_id = invoice.id
LEFT JOIN dbo.imp_truck_misc misc
  ON misc.filing_header_id = header.id
WHERE header.mapping_status = 2
GO