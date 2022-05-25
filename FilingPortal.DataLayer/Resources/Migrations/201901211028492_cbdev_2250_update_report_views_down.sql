IF OBJECT_ID(N'dbo.Rail_Report', 'V') IS NOT NULL
  DROP VIEW dbo.Rail_Report
GO

CREATE VIEW dbo.Rail_Report 
AS select
   h.id as Rail_Filing_Headers_id,
   
   f.[BDP_FK] as BDP_PK,

   d.Arr as Rail_DeclarationTab_Arr,
   d.Arr_2 as Rail_DeclarationTab_Arr_2,
   d.Carrier_SCAC as Rail_DeclarationTab_Carrier_SCAC,
   d.Centralized_Exam_Site as Rail_DeclarationTab_Centralized_Exam_Site,
   d.Certify_Cargo_Release as Rail_DeclarationTab_Certify_Cargo_Release,
   d.Check_Local_Client as Rail_DeclarationTab_Check_Local_Client,
   d.Container as Rail_DeclarationTab_Container,
   d.Country_of_Export as Rail_DeclarationTab_Country_of_Export,
   d.Dep as Rail_DeclarationTab_Dep,
   d.Description as Rail_DeclarationTab_Description,
   d.Destination as Rail_DeclarationTab_Destination,
   d.Destination_State as Rail_DeclarationTab_Destination_State,
   d.Discharge as Rail_DeclarationTab_Discharge,
   d.Enable_Entry_Sum as Rail_DeclarationTab_Enable_Entry_Sum,
   d.Entry_Port as Rail_DeclarationTab_Entry_Port,
   d.Entry_Type as Rail_DeclarationTab_Entry_Type,
   d.ETA as Rail_DeclarationTab_ETA,
   d.Export_Date as Rail_DeclarationTab_Export_Date,
   d.FIRMs_Code as Rail_DeclarationTab_FIRMs_Code,
   d.HMF as Rail_DeclarationTab_HMF,
   d.Importer as Rail_DeclarationTab_Importer,
   d.Importer_of_record as Rail_DeclarationTab_Importer_of_record,
   d.INCO as Rail_DeclarationTab_INCO,
   d.Issuer as Rail_DeclarationTab_Issuer,
   d.Main_Supplier as Rail_DeclarationTab_Main_Supplier,
   d.Manual_Entry as Rail_DeclarationTab_Manual_Entry,
   d.Master_Bill as Rail_DeclarationTab_Master_Bill,
   d.No_Packages as Rail_DeclarationTab_No_Packages,
   d.Origin as Rail_DeclarationTab_Origin,
   d.Owner_Ref as Rail_DeclarationTab_Owner_Ref,
   d.Purchased as Rail_DeclarationTab_Purchased,
   d.RLF as Rail_DeclarationTab_RLF,
   d.Service as Rail_DeclarationTab_Service,
   d.Shipment_Type as Rail_DeclarationTab_Shipment_Type,
   d.Split_Shipment_Release as Rail_DeclarationTab_Split_Shipment_Release,
   d.Total_Volume as Rail_DeclarationTab_Total_Volume,
   d.Total_Weight as Rail_DeclarationTab_Total_Weight,
   d.Transport as Rail_DeclarationTab_Transport,
   d.Type as Rail_DeclarationTab_Type,

   p.Bill_Issuer_SCAC as Rail_Packing_Bill_Issuer_SCAC,
   p.Bill_Number as Rail_Packing_Bill_Number,
   p.Bill_Type as Rail_Packing_Bill_Type,
   p.Container_Number as Rail_Packing_Container_Number,
   p.Is_Split as Rail_Packing_Is_Split,
   p.IT_Number as Rail_Packing_IT_Number,
   p.Manifest_QTY as Rail_Packing_Manifest_QTY,
   p.Marks_and_Numbers as Rail_Packing_Marks_and_Numbers,
   p.Pack_QTY as Rail_Packing_Pack_QTY,
   p.Pack_Type as Rail_Packing_Pack_Type,
   p.Shipping_Symbol as Rail_Packing_Shipping_Symbol,
   p.UQ as Rail_Packing_UQ,
   p.Packing_UQ as Rail_Packing_Container_Packing_UQ,
   p.Seal_Number as Rail_Packing_Seal_Number,
   p.Type as Rail_Packing_Type,
   p.Mode as Rail_Packing_Mode,
   p.Goods_Weight as Rail_Packing_Goods_Weight,
   p.Bill_Num as Rail_Packing_Bill_Num,
   
   i.Agreed_Place as Rail_InvoiceHeaders_Agreed_Place,
   i.Broker_PGA_Contact_Email as Rail_InvoiceHeaders_Broker_PGA_Contact_Email,
   i.Broker_PGA_Contact_Name as Rail_InvoiceHeaders_Broker_PGA_Contact_Name,
   i.Broker_PGA_Contact_Phone as Rail_InvoiceHeaders_Broker_PGA_Contact_Phone,
   i.Consignee as Rail_InvoiceHeaders_Consignee,
   i.Consignee_Address as Rail_InvoiceHeaders_Consignee_Address,
   i.Curr as Rail_InvoiceHeaders_Curr,
   i.EPA_PST_Cert_Date as Rail_InvoiceHeaders_EPA_PST_Cert_Date,
   i.EPA_TSCA_Cert_Date as Rail_InvoiceHeaders_EPA_TSCA_Cert_Date,
   i.EPA_VNE_Cert_Date as Rail_InvoiceHeaders_EPA_VNE_Cert_Date,
   i.Export as Rail_InvoiceHeaders_Export,
   i.Export_Date as Rail_InvoiceHeaders_Export_Date,
   i.First_Sale as Rail_InvoiceHeaders_First_Sale,
   i.FSIS_Cert_Date as Rail_InvoiceHeaders_FSIS_Cert_Date,
   i.FWS_Cert_Date as Rail_InvoiceHeaders_FWS_Cert_Date,
   i.Importer as Rail_InvoiceHeaders_Importer,
   i.INCO as Rail_InvoiceHeaders_INCO,
   i.Inv_Date as Rail_InvoiceHeaders_Inv_Date,
   i.Inv_Gross_Weight as Rail_InvoiceHeaders_Inv_Gross_Weight,
   i.Invoice_No as Rail_InvoiceHeaders_Invoice_No,
   i.Invoice_Total as Rail_InvoiceHeaders_Invoice_Total,
   i.LACEY_ACT_Cert_Date as Rail_InvoiceHeaders_LACEY_ACT_Cert_Date,
   i.Landed_Costing_Ex_Rate as Rail_InvoiceHeaders_Landed_Costing_Ex_Rate,
   i.Manufacturer as Rail_InvoiceHeaders_Manufacturer,
   i.Net_Weight as Rail_InvoiceHeaders_Net_Weight,
   i.NHTSA_Cert_Date as Rail_InvoiceHeaders_NHTSA_Cert_Date,
   i.Origin as Rail_InvoiceHeaders_Origin,
   i.Packages as Rail_InvoiceHeaders_Packages,
   i.Payment_Date as Rail_InvoiceHeaders_Payment_Date,
   i.Seller as Rail_InvoiceHeaders_Seller,
   i.Ship_to_party as Rail_InvoiceHeaders_Ship_to_party,
   i.Sold_to_party as Rail_InvoiceHeaders_Sold_to_party,
   i.Supplier as Rail_InvoiceHeaders_Supplier,
   i.Supplier_Address as Rail_InvoiceHeaders_Supplier_Address,
   i.Transaction_Related as Rail_InvoiceHeaders_Transaction_Related,

   l.Attribute_1 as Rail_InvoiceLines_Attribute_1,
   l.Attribute_2 as Rail_InvoiceLines_Attribute_2,
   l.Certifying_Individual as Rail_InvoiceLines_Certifying_Individual,
   l.CIF_Component as Rail_InvoiceLines_CIF_Component,
   l.Code as Rail_InvoiceLines_Code,
   l.Consignee as Rail_InvoiceLines_Consignee,
   l.Curr as Rail_InvoiceLines_Curr,
   l.Customs_QTY as Rail_InvoiceLines_Customs_QTY,
   l.Dest_State as Rail_InvoiceLines_Dest_State,
   l.EPA_TSCA_Toxic_Substance_Control_Act_Indicator as Rail_InvoiceLines_EPA_TSCA_Toxic_Substance_Control_Act_Indicator,
   l.Export as Rail_InvoiceLines_Export,
   l.Goods_Description as Rail_InvoiceLines_Goods_Description,
   l.Gr_Weight as Rail_InvoiceLines_Gr_Weight,
   l.Invoice_No as Rail_InvoiceLines_Invoice_No,
   l.Invoice_Qty as Rail_InvoiceLines_Invoice_Qty,
   l.Invoice_Qty_Unit as Rail_InvoiceLines_Invoice_Qty_Unit,
   l.Line_Price as Rail_InvoiceLines_Line_Price,
   l.LNO as Rail_InvoiceLines_LNO,
   l.Manufacturer as Rail_InvoiceLines_Manufacturer,
   l.ORG as Rail_InvoiceLines_ORG,
   l.Origin as Rail_InvoiceLines_Origin,
   l.PGA_Contact_Email as Rail_InvoiceLines_PGA_Contact_Email,
   l.PGA_Contact_Name as Rail_InvoiceLines_PGA_Contact_Name,
   l.PGA_Contact_Phone as Rail_InvoiceLines_PGA_Contact_Phone,
   l.PriceUnit as Rail_InvoiceLines_PriceUnit,
   l.Prod_ID_1 as Rail_InvoiceLines_Prod_ID_1,
   l.Sold_To_Party as Rail_InvoiceLines_Sold_To_Party,
   l.SPI as Rail_InvoiceLines_SPI,
   l.Tariff as Rail_InvoiceLines_Tariff,
   l.Transaction_Related as Rail_InvoiceLines_Transaction_Related,
   l.TSCA_Indicator as Rail_InvoiceLines_TSCA_Indicator,
   l.UQ as Rail_InvoiceLines_UQ,
   l.[Amount] as Rail_InvoiceLines_Amount,

   m.Bond_Type as Rail_MISC_Bond_Type,
   m.Branch as Rail_MISC_Branch,
   m.Broker as Rail_MISC_Broker,
   m.Broker_to_Pay as Rail_MISC_Broker_to_Pay,
   m.FTA_Recon as Rail_MISC_FTA_Recon,
   m.Merge_By as Rail_MISC_Merge_By,
   m.Payment_Type as Rail_MISC_Payment_Type,
   m.Prelim_Statement_Date as Rail_MISC_Prelim_Statement_Date,
   m.Preparer_Dist_Port as Rail_MISC_Preparer_Dist_Port,
   m.Recon_Issue as Rail_MISC_Recon_Issue,
   m.Submitter as Rail_MISC_Submitter,
   m.Tax_Deferrable_Ind as Rail_MISC_Tax_Deferrable_Ind 
from
   dbo.Rail_Filing_Headers h
   inner join dbo.Rail_Filing_Details f on  h.id = f.Filing_Headers_FK
   left join dbo.Rail_DeclarationTab d on  d.Filing_Headers_FK = h.id and f.BDP_FK = d.BDP_FK
   left join dbo.Rail_ContainersTab p on  p.Filing_Headers_FK = h.id and f.BDP_FK = p.BDP_FK
   left join dbo.Rail_InvoiceLines l on  l.Filing_Headers_FK = h.id and f.BDP_FK = l.BDP_FK
   left join dbo.Rail_InvoiceHeaders i on i.Filing_Headers_FK = h.id and i.id = l.InvoiceHeaders_FK 
   left join dbo.Rail_MISC m on  m.Filing_Headers_FK = h.id and f.BDP_FK = m.BDP_FK
where h.MappingStatus=2
GO

IF OBJECT_ID(N'dbo.Truck_Report', 'V') IS NOT NULL
  DROP VIEW dbo.Truck_Report
GO

CREATE VIEW dbo.Truck_Report 
AS SELECT
  tfh.id
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
 --,tfd.Attribute_3
 ,invlines.Export
 ,invlines.Invoice_Qty
 ,invlines.Invoice_Qty_Unit
 --,tfd.Inv_Price
 --,tfd.Gross_Weight
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
 --,tfd.MID
 --,tfd.Manufacturer_Code
 --,tfd.Surety_Code
FROM dbo.Truck_Filing_Headers tfh
LEFT JOIN dbo.Truck_ContainersTab containers
  ON containers.Filing_Headers_FK = tfh.id
LEFT JOIN dbo.Truck_DeclarationTab declaration
  ON declaration.Filing_Headers_FK = tfh.id
LEFT JOIN dbo.Truck_InvoiceHeaders invheaders
  ON invheaders.Filing_Headers_FK = tfh.id
LEFT JOIN dbo.Truck_InvoiceLines invlines
  ON invlines.Filing_Headers_FK = tfh.id
LEFT JOIN dbo.Truck_MISC misc
  ON misc.Filing_Headers_FK = tfh.id
WHERE tfh.MappingStatus = 2
GO