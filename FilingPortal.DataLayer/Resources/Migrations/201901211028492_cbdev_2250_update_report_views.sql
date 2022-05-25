IF OBJECT_ID(N'dbo.Rail_Report', 'V') IS NOT NULL
  DROP VIEW dbo.Rail_Report
GO

CREATE VIEW dbo.Rail_Report 
AS select
   headers.id as Rail_Filing_Headers_id
  ,detailes.BDP_FK as BDP_PK
  ,declaration.Arr as Rail_DeclarationTab_Arr
  ,declaration.Arr_2 as Rail_DeclarationTab_Arr_2
  ,declaration.Carrier_SCAC as Rail_DeclarationTab_Carrier_SCAC
  ,declaration.Centralized_Exam_Site as Rail_DeclarationTab_Centralized_Exam_Site
  ,declaration.Certify_Cargo_Release as Rail_DeclarationTab_Certify_Cargo_Release
  ,declaration.Check_Local_Client as Rail_DeclarationTab_Check_Local_Client
  ,declaration.Container as Rail_DeclarationTab_Container
  ,declaration.Country_of_Export as Rail_DeclarationTab_Country_of_Export
  ,declaration.Dep as Rail_DeclarationTab_Dep
  ,declaration.Description as Rail_DeclarationTab_Description
  ,declaration.Destination as Rail_DeclarationTab_Destination
  ,declaration.Destination_State as Rail_DeclarationTab_Destination_State
  ,declaration.Discharge as Rail_DeclarationTab_Discharge
  ,declaration.Enable_Entry_Sum as Rail_DeclarationTab_Enable_Entry_Sum
  ,declaration.Entry_Port as Rail_DeclarationTab_Entry_Port
  ,declaration.Entry_Type as Rail_DeclarationTab_Entry_Type
  ,declaration.ETA as Rail_DeclarationTab_ETA
  ,declaration.Export_Date as Rail_DeclarationTab_Export_Date
  ,declaration.FIRMs_Code as Rail_DeclarationTab_FIRMs_Code
  ,declaration.HMF as Rail_DeclarationTab_HMF
  ,declaration.Importer as Rail_DeclarationTab_Importer
  ,declaration.Importer_of_record as Rail_DeclarationTab_Importer_of_record
  ,declaration.INCO as Rail_DeclarationTab_INCO
  ,declaration.Issuer as Rail_DeclarationTab_Issuer
  ,declaration.Main_Supplier as Rail_DeclarationTab_Main_Supplier
  ,declaration.Manual_Entry as Rail_DeclarationTab_Manual_Entry
  ,declaration.Master_Bill as Rail_DeclarationTab_Master_Bill
  ,declaration.No_Packages as Rail_DeclarationTab_No_Packages
  ,declaration.Origin as Rail_DeclarationTab_Origin
  ,declaration.Owner_Ref as Rail_DeclarationTab_Owner_Ref
  ,declaration.Purchased as Rail_DeclarationTab_Purchased
  ,declaration.RLF as Rail_DeclarationTab_RLF
  ,declaration.Service as Rail_DeclarationTab_Service
  ,declaration.Shipment_Type as Rail_DeclarationTab_Shipment_Type
  ,declaration.Split_Shipment_Release as Rail_DeclarationTab_Split_Shipment_Release
  ,declaration.Total_Volume as Rail_DeclarationTab_Total_Volume
  ,declaration.Total_Weight as Rail_DeclarationTab_Total_Weight
  ,declaration.Transport as Rail_DeclarationTab_Transport
  ,declaration.Type as Rail_DeclarationTab_Type

  ,containers.Bill_Issuer_SCAC as Rail_Packing_Bill_Issuer_SCAC
  ,containers.Bill_Number as Rail_Packing_Bill_Number
  ,containers.Bill_Type as Rail_Packing_Bill_Type
  ,containers.Container_Number as Rail_Packing_Container_Number
  ,containers.Is_Split as Rail_Packing_Is_Split
  ,containers.IT_Number as Rail_Packing_IT_Number
  ,containers.Manifest_QTY as Rail_Packing_Manifest_QTY
  ,containers.Marks_and_Numbers as Rail_Packing_Marks_and_Numbers
  ,containers.Pack_QTY as Rail_Packing_Pack_QTY
  ,containers.Pack_Type as Rail_Packing_Pack_Type
  ,containers.Shipping_Symbol as Rail_Packing_Shipping_Symbol
  ,containers.UQ as Rail_Packing_UQ
  ,containers.Packing_UQ as Rail_Packing_Container_Packing_UQ
  ,containers.Seal_Number as Rail_Packing_Seal_Number
  ,containers.Type as Rail_Packing_Type
  ,containers.Mode as Rail_Packing_Mode
  ,containers.Goods_Weight as Rail_Packing_Goods_Weight
  ,containers.Bill_Num as Rail_Packing_Bill_Num

  ,invheaders.Agreed_Place as Rail_InvoiceHeaders_Agreed_Place
  ,invheaders.Broker_PGA_Contact_Email as Rail_InvoiceHeaders_Broker_PGA_Contact_Email
  ,invheaders.Broker_PGA_Contact_Name as Rail_InvoiceHeaders_Broker_PGA_Contact_Name
  ,invheaders.Broker_PGA_Contact_Phone as Rail_InvoiceHeaders_Broker_PGA_Contact_Phone
  ,invheaders.Consignee as Rail_InvoiceHeaders_Consignee
  ,invheaders.Consignee_Address as Rail_InvoiceHeaders_Consignee_Address
  ,invheaders.Curr as Rail_InvoiceHeaders_Curr
  ,invheaders.EPA_PST_Cert_Date as Rail_InvoiceHeaders_EPA_PST_Cert_Date
  ,invheaders.EPA_TSCA_Cert_Date as Rail_InvoiceHeaders_EPA_TSCA_Cert_Date
  ,invheaders.EPA_VNE_Cert_Date as Rail_InvoiceHeaders_EPA_VNE_Cert_Date
  ,invheaders.Export as Rail_InvoiceHeaders_Export
  ,invheaders.Export_Date as Rail_InvoiceHeaders_Export_Date
  ,invheaders.First_Sale as Rail_InvoiceHeaders_First_Sale
  ,invheaders.FSIS_Cert_Date as Rail_InvoiceHeaders_FSIS_Cert_Date
  ,invheaders.FWS_Cert_Date as Rail_InvoiceHeaders_FWS_Cert_Date
  ,invheaders.Importer as Rail_InvoiceHeaders_Importer
  ,invheaders.INCO as Rail_InvoiceHeaders_INCO
  ,invheaders.Inv_Date as Rail_InvoiceHeaders_Inv_Date
  ,invheaders.Inv_Gross_Weight as Rail_InvoiceHeaders_Inv_Gross_Weight
  ,invheaders.Invoice_No as Rail_InvoiceHeaders_Invoice_No
  ,invheaders.Invoice_Total as Rail_InvoiceHeaders_Invoice_Total
  ,invheaders.LACEY_ACT_Cert_Date as Rail_InvoiceHeaders_LACEY_ACT_Cert_Date
  ,invheaders.Landed_Costing_Ex_Rate as Rail_InvoiceHeaders_Landed_Costing_Ex_Rate
  ,invheaders.Manufacturer as Rail_InvoiceHeaders_Manufacturer
  ,invheaders.Net_Weight as Rail_InvoiceHeaders_Net_Weight
  ,invheaders.NHTSA_Cert_Date as Rail_InvoiceHeaders_NHTSA_Cert_Date
  ,invheaders.Origin as Rail_InvoiceHeaders_Origin
  ,invheaders.Packages as Rail_InvoiceHeaders_Packages
  ,invheaders.Payment_Date as Rail_InvoiceHeaders_Payment_Date
  ,invheaders.Seller as Rail_InvoiceHeaders_Seller
  ,invheaders.Ship_to_party as Rail_InvoiceHeaders_Ship_to_party
  ,invheaders.Sold_to_party as Rail_InvoiceHeaders_Sold_to_party
  ,invheaders.Supplier as Rail_InvoiceHeaders_Supplier
  ,invheaders.Supplier_Address as Rail_InvoiceHeaders_Supplier_Address
  ,invheaders.Transaction_Related as Rail_InvoiceHeaders_Transaction_Related

  ,invlines.Attribute_1 as Rail_InvoiceLines_Attribute_1
  ,invlines.Attribute_2 as Rail_InvoiceLines_Attribute_2
  ,invlines.Certifying_Individual as Rail_InvoiceLines_Certifying_Individual
  ,invlines.CIF_Component as Rail_InvoiceLines_CIF_Component
  ,invlines.Code as Rail_InvoiceLines_Code
  ,invlines.Consignee as Rail_InvoiceLines_Consignee
  ,invlines.Curr as Rail_InvoiceLines_Curr
  ,invlines.Customs_QTY as Rail_InvoiceLines_Customs_QTY
  ,invlines.Dest_State as Rail_InvoiceLines_Dest_State
  ,invlines.EPA_TSCA_Toxic_Substance_Control_Act_Indicator as Rail_InvoiceLines_EPA_TSCA_Toxic_Substance_Control_Act_Indicator
  ,invlines.Export as Rail_InvoiceLines_Export
  ,invlines.Goods_Description as Rail_InvoiceLines_Goods_Description
  ,invlines.Gr_Weight as Rail_InvoiceLines_Gr_Weight
  ,invlines.Invoice_No as Rail_InvoiceLines_Invoice_No
  ,invlines.Invoice_Qty as Rail_InvoiceLines_Invoice_Qty
  ,invlines.Invoice_Qty_Unit as Rail_InvoiceLines_Invoice_Qty_Unit
  ,invlines.Line_Price as Rail_InvoiceLines_Line_Price
  ,invlines.LNO as Rail_InvoiceLines_LNO
  ,invlines.Manufacturer as Rail_InvoiceLines_Manufacturer
  ,invlines.ORG as Rail_InvoiceLines_ORG
  ,invlines.Origin as Rail_InvoiceLines_Origin
  ,invlines.PGA_Contact_Email as Rail_InvoiceLines_PGA_Contact_Email
  ,invlines.PGA_Contact_Name as Rail_InvoiceLines_PGA_Contact_Name
  ,invlines.PGA_Contact_Phone as Rail_InvoiceLines_PGA_Contact_Phone
  ,invlines.PriceUnit as Rail_InvoiceLines_PriceUnit
  ,invlines.Prod_ID_1 as Rail_InvoiceLines_Prod_ID_1
  ,invlines.Sold_To_Party as Rail_InvoiceLines_Sold_To_Party
  ,invlines.SPI as Rail_InvoiceLines_SPI
  ,invlines.Tariff as Rail_InvoiceLines_Tariff
  ,invlines.Transaction_Related as Rail_InvoiceLines_Transaction_Related
  ,invlines.TSCA_Indicator as Rail_InvoiceLines_TSCA_Indicator
  ,invlines.UQ as Rail_InvoiceLines_UQ
  ,invlines.[Amount] as Rail_InvoiceLines_Amount

  ,misc.Bond_Type as Rail_MISC_Bond_Type
  ,misc.Branch as Rail_MISC_Branch
  ,misc.Broker as Rail_MISC_Broker
  ,misc.Broker_to_Pay as Rail_MISC_Broker_to_Pay
  ,misc.FTA_Recon as Rail_MISC_FTA_Recon
  ,misc.Merge_By as Rail_MISC_Merge_By
  ,misc.Payment_Type as Rail_MISC_Payment_Type
  ,misc.Prelim_Statement_Date as Rail_MISC_Prelim_Statement_Date
  ,misc.Preparer_Dist_Port as Rail_MISC_Preparer_Dist_Port
  ,misc.Recon_Issue as Rail_MISC_Recon_Issue
  ,misc.Submitter as Rail_MISC_Submitter
  ,misc.Tax_Deferrable_Ind as Rail_MISC_Tax_Deferrable_Ind 
from
   dbo.Rail_Filing_Headers headers
   inner join dbo.Rail_Filing_Details detailes on  headers.id = detailes.Filing_Headers_FK
   left join dbo.Rail_DeclarationTab declaration on  declaration.Filing_Headers_FK = headers.id and detailes.BDP_FK = declaration.BDP_FK
   left join dbo.Rail_ContainersTab containers on  containers.Filing_Headers_FK = headers.id and detailes.BDP_FK = containers.BDP_FK
   left join dbo.Rail_InvoiceLines invlines on  invlines.Filing_Headers_FK = headers.id and detailes.BDP_FK = invlines.BDP_FK
   left join dbo.Rail_InvoiceHeaders invheaders on invheaders.Filing_Headers_FK = headers.id and invheaders.id = invlines.InvoiceHeaders_FK 
   left join dbo.Rail_MISC misc on  misc.Filing_Headers_FK = headers.id and detailes.BDP_FK = misc.BDP_FK
where headers.MappingStatus=2
GO

IF OBJECT_ID(N'dbo.Truck_Report', 'V') IS NOT NULL
  DROP VIEW dbo.Truck_Report
GO


CREATE VIEW dbo.Truck_Report 
AS SELECT
  headers.id

 ,declaration.Main_Supplier
 ,declaration.Importer
 ,declaration.Shipment_Type
 ,declaration.Transport
 ,declaration.Entry_Type
 ,declaration.RLF
 ,declaration.Enable_Entry_Sum
 ,declaration.Type
 ,declaration.Certify_Cargo_Release
 ,declaration.Service
 ,declaration.Issuer
 ,declaration.Master_Bill
 ,declaration.Carrier_SCAC
 ,declaration.Discharge
 ,declaration.Entry_Port
 ,declaration.Dep
 ,declaration.Arr
 ,declaration.Arr_2
 ,declaration.HMF
 ,declaration.Origin
 ,declaration.Destination
 ,declaration.Destination_State
 ,declaration.Country_of_Export
 ,declaration.ETA
 ,declaration.Export_Date
 ,declaration.Description
 ,declaration.Owner_Ref
 ,declaration.INCO
 ,declaration.Total_Weight
 ,declaration.Total_Volume
 ,declaration.No_Packages
 ,declaration.FIRMs_Code
 ,declaration.Centralized_Exam_Site
 ,declaration.Purchased
 ,declaration.Manual_Entry
 ,declaration.Importer_of_record
 ,declaration.Split_Shipment_Release
 ,declaration.Check_Local_Client

 ,containers.Bill_Type
 ,containers.Bill_Num
 ,containers.Bill_Number
 ,containers.UQ as Containers_UQ
 ,containers.Manifest_QTY
 ,containers.Packing_UQ
 ,containers.Bill_Issuer_SCAC

 ,invheaders.Invoice_No
 ,invheaders.Consignee_Address
 ,invheaders.Invoice_Total
 ,invheaders.Curr
 ,invheaders.Payment_Date
 ,invheaders.Consignee
 ,invheaders.Inv_Date
 ,invheaders.Agreed_Place
 ,invheaders.Inv_Gross_Weight
 ,invheaders.Net_Weight
 ,invheaders.Manufacturer
 ,invheaders.Seller
 ,invheaders.Sold_to_party
 ,invheaders.Ship_to_party
 ,invheaders.Broker_PGA_Contact_Name
 ,invheaders.Broker_PGA_Contact_Phone
 ,invheaders.Broker_PGA_Contact_Email

 ,invlines.LNO
 ,invlines.Tariff
 ,invlines.Customs_QTY
 ,invlines.Line_Price
 ,invlines.Goods_Description
 ,invlines.ORG
 ,invlines.SPI
 ,invlines.Gr_Weight
 ,invlines.UQ
 ,invlines.PriceUnit
 ,invlines.Prod_ID_1
 ,invlines.Attribute_1
 ,invlines.Attribute_2
 ,invlines.Export
 ,invlines.Invoice_Qty
 ,invlines.Invoice_Qty_Unit
 ,invlines.Code
 ,invlines.Amount
 ,invlines.CIF_Component
 ,invlines.EPA_TSCA_Toxic_Substance_Control_Act_Indicator
 ,invlines.TSCA_Indicator
 ,invlines.Certifying_Individual

 ,misc.Branch
 ,misc.Broker
 ,misc.Merge_By
 ,misc.Tax_Deferrable_Ind
 ,misc.Preparer_Dist_Port
 ,misc.Recon_Issue
 ,misc.FTA_Recon
 ,misc.Bond_Type
 ,misc.Payment_Type
 ,misc.Broker_to_Pay
 ,misc.Prelim_Statement_Date
 ,misc.Submitter
FROM dbo.Truck_Filing_Headers headers
LEFT JOIN dbo.Truck_ContainersTab containers
  ON containers.Filing_Headers_FK = headers.id
LEFT JOIN dbo.Truck_DeclarationTab declaration
  ON declaration.Filing_Headers_FK = headers.id
LEFT JOIN dbo.Truck_InvoiceHeaders invheaders
  ON invheaders.Filing_Headers_FK = headers.id
LEFT JOIN dbo.Truck_InvoiceLines invlines
  ON invlines.Filing_Headers_FK = headers.id
LEFT JOIN dbo.Truck_MISC misc
  ON misc.Filing_Headers_FK = headers.id
WHERE headers.MappingStatus = 2
GO
