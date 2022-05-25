ALTER VIEW dbo.Truck_Report 
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