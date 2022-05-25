--- Pipeline ---
ALTER TABLE dbo.Pipeline_InvoiceLines
DROP COLUMN LNO
GO

IF OBJECT_ID('dbo.pipeline_invoice_line_number', 'FN') IS NOT NULL
  DROP FUNCTION dbo.pipeline_invoice_line_number
GO

CREATE FUNCTION dbo.pipeline_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      pil.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY pil.id)
    FROM dbo.Pipeline_InvoiceLines pil
    WHERE pil.InvoiceHeaders_FK = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO

ALTER TABLE dbo.Pipeline_InvoiceLines
ADD invoice_line_number AS (dbo.pipeline_invoice_line_number(InvoiceHeaders_FK, id))
GO

ALTER VIEW dbo.Pipeline_Report
AS
SELECT
  header.id AS Pipeline_Filing_Headers_id
 ,details.Pipeline_Inbounds_FK AS PI_ID
 ,declaration.Arr AS Pipeline_DeclarationTab_Arr
 ,declaration.Arr_2 AS Pipeline_DeclarationTab_Arr_2
 ,declaration.Carrier_SCAC AS Pipeline_DeclarationTab_Carrier_SCAC
 ,declaration.Centralized_Exam_Site AS Pipeline_DeclarationTab_Centralized_Exam_Site
 ,declaration.Certify_Cargo_Release AS Pipeline_DeclarationTab_Certify_Cargo_Release
 ,declaration.Check_Local_Client AS Pipeline_DeclarationTab_Check_Local_Client
 ,declaration.Container AS Pipeline_DeclarationTab_Container
 ,declaration.Country_of_Export AS Pipeline_DeclarationTab_Country_of_Export
 ,declaration.Dep AS Pipeline_DeclarationTab_Dep
 ,declaration.Description AS Pipeline_DeclarationTab_Description
 ,declaration.Destination AS Pipeline_DeclarationTab_Destination
 ,declaration.Destination_State AS Pipeline_DeclarationTab_Destination_State
 ,declaration.Discharge AS Pipeline_DeclarationTab_Discharge
 ,declaration.Enable_Entry_Sum AS Pipeline_DeclarationTab_Enable_Entry_Sum
 ,declaration.Entry_Port AS Pipeline_DeclarationTab_Entry_Port
 ,declaration.Entry_Type AS Pipeline_DeclarationTab_Entry_Type
 ,declaration.ETA AS Pipeline_DeclarationTab_ETA
 ,declaration.Export_Date AS Pipeline_DeclarationTab_Export_Date
 ,declaration.FIRMs_Code AS Pipeline_DeclarationTab_FIRMs_Code
 ,declaration.HMF AS Pipeline_DeclarationTab_HMF
 ,declaration.Importer AS Pipeline_DeclarationTab_Importer
 ,declaration.Importer_of_record AS Pipeline_DeclarationTab_Importer_of_record
 ,declaration.INCO AS Pipeline_DeclarationTab_INCO
 ,declaration.Issuer AS Pipeline_DeclarationTab_Issuer
 ,declaration.Main_Supplier AS Pipeline_DeclarationTab_Main_Supplier
 ,declaration.Manual_Entry AS Pipeline_DeclarationTab_Manual_Entry
 ,declaration.Master_Bill AS Pipeline_DeclarationTab_Master_Bill
 ,declaration.No_Packages AS Pipeline_DeclarationTab_No_Packages
 ,declaration.Origin AS Pipeline_DeclarationTab_Origin
 ,declaration.Owner_Ref AS Pipeline_DeclarationTab_Owner_Ref
 ,declaration.Purchased AS Pipeline_DeclarationTab_Purchased
 ,declaration.RLF AS Pipeline_DeclarationTab_RLF
 ,declaration.Service AS Pipeline_DeclarationTab_Service
 ,declaration.Shipment_Type AS Pipeline_DeclarationTab_Shipment_Type
 ,declaration.Split_Shipment_Release AS Pipeline_DeclarationTab_Split_Shipment_Release
 ,declaration.Total_Volume AS Pipeline_DeclarationTab_Total_Volume
 ,declaration.Total_Weight AS Pipeline_DeclarationTab_Total_Weight
 ,declaration.Transport AS Pipeline_DeclarationTab_Transport
 ,declaration.Type AS Pipeline_DeclarationTab_Type
 ,containers.Bill_Issuer_SCAC AS Pipeline_Packing_Bill_Issuer_SCAC
 ,containers.Bill_Number AS Pipeline_Packing_Bill_Number
 ,containers.Bill_Type AS Pipeline_Packing_Bill_Type
 ,containers.Container_Number AS Pipeline_Packing_Container_Number
 ,containers.Is_Split AS Pipeline_Packing_Is_Split
 ,containers.IT_Number AS Pipeline_Packing_IT_Number
 ,containers.Manifest_QTY AS Pipeline_Packing_Manifest_QTY
 ,containers.Marks_and_Numbers AS Pipeline_Packing_Marks_and_Numbers
 ,containers.Pack_QTY AS Pipeline_Packing_Pack_QTY
 ,containers.Pack_Type AS Pipeline_Packing_Pack_Type
 ,containers.Shipping_Symbol AS Pipeline_Packing_Shipping_Symbol
 ,containers.UQ AS Pipeline_Packing_UQ
 ,containers.Packing_UQ AS Pipeline_Packing_Container_Packing_UQ
 ,containers.Seal_Number AS Pipeline_Packing_Seal_Number
 ,containers.Type AS Pipeline_Packing_Type
 ,containers.Mode AS Pipeline_Packing_Mode
 ,containers.Goods_Weight AS Pipeline_Packing_Goods_Weight
 ,containers.Bill_Num AS Pipeline_Packing_Bill_Num
 ,invheaders.Agreed_Place AS Pipeline_InvoiceHeaders_Agreed_Place
 ,invheaders.Broker_PGA_Contact_Email AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Email
 ,invheaders.Broker_PGA_Contact_Name AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Name
 ,invheaders.Broker_PGA_Contact_Phone AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Phone
 ,invheaders.Consignee AS Pipeline_InvoiceHeaders_Consignee
 ,invheaders.Consignee_Address AS Pipeline_InvoiceHeaders_Consignee_Address
 ,invheaders.Curr AS Pipeline_InvoiceHeaders_Curr
 ,invheaders.EPA_PST_Cert_Date AS Pipeline_InvoiceHeaders_EPA_PST_Cert_Date
 ,invheaders.EPA_TSCA_Cert_Date AS Pipeline_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,invheaders.EPA_VNE_Cert_Date AS Pipeline_InvoiceHeaders_EPA_VNE_Cert_Date
 ,invheaders.Export AS Pipeline_InvoiceHeaders_Export
 ,invheaders.Export_Date AS Pipeline_InvoiceHeaders_Export_Date
 ,invheaders.First_Sale AS Pipeline_InvoiceHeaders_First_Sale
 ,invheaders.FSIS_Cert_Date AS Pipeline_InvoiceHeaders_FSIS_Cert_Date
 ,invheaders.FWS_Cert_Date AS Pipeline_InvoiceHeaders_FWS_Cert_Date
 ,invheaders.Importer AS Pipeline_InvoiceHeaders_Importer
 ,invheaders.INCO AS Pipeline_InvoiceHeaders_INCO
 ,invheaders.Inv_Date AS Pipeline_InvoiceHeaders_Inv_Date
 ,invheaders.Inv_Gross_Weight AS Pipeline_InvoiceHeaders_Inv_Gross_Weight
 ,invheaders.Invoice_No AS Pipeline_InvoiceHeaders_Invoice_No
 ,invheaders.Invoice_Total AS Pipeline_InvoiceHeaders_Invoice_Total
 ,invheaders.LACEY_ACT_Cert_Date AS Pipeline_InvoiceHeaders_LACEY_ACT_Cert_Date
 ,invheaders.Landed_Costing_Ex_Rate AS Pipeline_InvoiceHeaders_Landed_Costing_Ex_Rate
 ,invheaders.Manufacturer AS Pipeline_InvoiceHeaders_Manufacturer
 ,invheaders.Net_Weight AS Pipeline_InvoiceHeaders_Net_Weight
 ,invheaders.NHTSA_Cert_Date AS Pipeline_InvoiceHeaders_NHTSA_Cert_Date
 ,invheaders.Origin AS Pipeline_InvoiceHeaders_Origin
 ,invheaders.Packages AS Pipeline_InvoiceHeaders_Packages
 ,invheaders.Payment_Date AS Pipeline_InvoiceHeaders_Payment_Date
 ,invheaders.Seller AS Pipeline_InvoiceHeaders_Seller
 ,invheaders.Ship_to_party AS Pipeline_InvoiceHeaders_Ship_to_party
 ,invheaders.Sold_to_party AS Pipeline_InvoiceHeaders_Sold_to_party
 ,invheaders.Supplier AS Pipeline_InvoiceHeaders_Supplier
 ,invheaders.Supplier_Address AS Pipeline_InvoiceHeaders_Supplier_Address
 ,invheaders.Transaction_Related AS Pipeline_InvoiceHeaders_Transaction_Related
 ,invlines.Attribute_1 AS Pipeline_InvoiceLines_Attribute_1
 ,invlines.Attribute_2 AS Pipeline_InvoiceLines_Attribute_2
 ,invlines.Certifying_Individual AS Pipeline_InvoiceLines_Certifying_Individual
 ,invlines.CIF_Component AS Pipeline_InvoiceLines_CIF_Component
 ,invlines.Code AS Pipeline_InvoiceLines_Code
 ,invlines.Consignee AS Pipeline_InvoiceLines_Consignee
 ,invlines.Curr AS Pipeline_InvoiceLines_Curr
 ,invlines.Customs_QTY AS Pipeline_InvoiceLines_Customs_QTY
 ,invlines.Dest_State AS Pipeline_InvoiceLines_Dest_State
 ,invlines.EPA_TSCA_Toxic_Substance_Control_Act_Indicator AS Pipeline_InvoiceLines_EPA_TSCA_Toxic_Indicator
 ,invlines.Export AS Pipeline_InvoiceLines_Export
 ,invlines.Goods_Description AS Pipeline_InvoiceLines_Goods_Description
 ,invlines.Gr_Weight AS Pipeline_InvoiceLines_Gr_Weight
 ,invlines.Invoice_No AS Pipeline_InvoiceLines_Invoice_No
 ,invlines.Invoice_Qty AS Pipeline_InvoiceLines_Invoice_Qty
 ,invlines.Invoice_Qty_Unit AS Pipeline_InvoiceLines_Invoice_Qty_Unit
 ,invlines.Line_Price AS Pipeline_InvoiceLines_Line_Price
 ,invlines.invoice_line_number AS Pipeline_InvoiceLines_LNO
 ,invlines.Manufacturer AS Pipeline_InvoiceLines_Manufacturer
 ,invlines.ORG AS Pipeline_InvoiceLines_ORG
 ,invlines.Origin AS Pipeline_InvoiceLines_Origin
 ,invlines.PGA_Contact_Email AS Pipeline_InvoiceLines_PGA_Contact_Email
 ,invlines.PGA_Contact_Name AS Pipeline_InvoiceLines_PGA_Contact_Name
 ,invlines.PGA_Contact_Phone AS Pipeline_InvoiceLines_PGA_Contact_Phone
 ,invlines.PriceUnit AS Pipeline_InvoiceLines_PriceUnit
 ,invlines.Prod_ID_1 AS Pipeline_InvoiceLines_Prod_ID_1
 ,invlines.Sold_to_party AS Pipeline_InvoiceLines_Sold_To_Party
 ,invlines.SPI AS Pipeline_InvoiceLines_SPI
 ,invlines.Tariff AS Pipeline_InvoiceLines_Tariff
 ,invlines.Transaction_Related AS Pipeline_InvoiceLines_Transaction_Related
 ,invlines.TSCA_Indicator AS Pipeline_InvoiceLines_TSCA_Indicator
 ,invlines.UQ AS Pipeline_InvoiceLines_UQ
 ,invlines.Amount AS Pipeline_InvoiceLines_Amount
 ,misc.Bond_Type AS Pipeline_MISC_Bond_Type
 ,misc.Branch AS Pipeline_MISC_Branch
 ,misc.Broker AS Pipeline_MISC_Broker
 ,misc.Broker_to_Pay AS Pipeline_MISC_Broker_to_Pay
 ,misc.FTA_Recon AS Pipeline_MISC_FTA_Recon
 ,misc.Merge_By AS Pipeline_MISC_Merge_By
 ,misc.Payment_Type AS Pipeline_MISC_Payment_Type
 ,misc.Prelim_Statement_Date AS Pipeline_MISC_Prelim_Statement_Date
 ,misc.Preparer_Dist_Port AS Pipeline_MISC_Preparer_Dist_Port
 ,misc.Recon_Issue AS Pipeline_MISC_Recon_Issue
 ,misc.Submitter AS Pipeline_MISC_Submitter
 ,misc.Tax_Deferrable_Ind AS Pipeline_MISC_Tax_Deferrable_Ind
 ,declaration.Pipeline AS Pipeline_DeclarationTab_Pipeline
 ,invlines.Attribute_3 AS Pipeline_InvoiceLines_Attribute_3
 ,invheaders.Manufacturer_Address AS Pipeline_InvoiceHeaders_Manufacturer_Address
FROM dbo.Pipeline_Filing_Headers AS header
INNER JOIN dbo.Pipeline_Filing_Details AS details
  ON header.id = details.Filing_Headers_FK
LEFT OUTER JOIN dbo.Pipeline_DeclarationTab AS declaration
  ON declaration.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = declaration.PI_FK
LEFT OUTER JOIN dbo.Pipeline_ContainersTab AS containers
  ON containers.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = containers.PI_FK
LEFT OUTER JOIN dbo.Pipeline_InvoiceLines AS invlines
  ON invlines.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = invlines.PI_FK
LEFT OUTER JOIN dbo.Pipeline_InvoiceHeaders AS invheaders
  ON invheaders.Filing_Headers_FK = header.id
    AND invheaders.id = invlines.InvoiceHeaders_FK
LEFT OUTER JOIN dbo.Pipeline_MISC AS misc
  ON misc.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = misc.PI_FK
WHERE (header.MappingStatus = 2)
GO

DELETE FROM dbo.Pipeline_DEFValues
WHERE ColName = 'LNO'
DELETE FROM dbo.Pipeline_DEFValues_Manual
WHERE column_name = 'LNO'
GO

--- Truck ---
ALTER TABLE dbo.Truck_InvoiceLines
DROP COLUMN LNO
GO

IF OBJECT_ID('dbo.truck_invoice_line_number', 'FN') IS NOT NULL
  DROP FUNCTION dbo.truck_invoice_line_number
GO

CREATE FUNCTION dbo.truck_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      til.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY til.id)
    FROM dbo.Truck_InvoiceLines til
    WHERE til.InvoiceHeaders_FK = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO

ALTER TABLE dbo.Truck_InvoiceLines
ADD invoice_line_number AS (dbo.truck_invoice_line_number(InvoiceHeaders_FK, id))
GO

ALTER VIEW dbo.Truck_Report
AS
SELECT
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
 ,containers.UQ AS Containers_UQ
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
 ,invlines.invoice_line_number AS LNO
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
 ,invlines.Manufacturer AS Truck_InvoiceLines_Manufacturer
 ,invheaders.Transaction_Related AS Truck_InvoiceHeaders_Transaction_Related
 ,invlines.Transaction_Related AS Truck_InvoiceLines_Transaction_Related
 ,invheaders.EPA_TSCA_Cert_Date AS Truck_InvoiseHeaders_EPA_TSCA_Cert_Date
 ,declaration.Container AS Truck_DeclarationTab_Container
 ,invheaders.Supplier AS Truck_InvoiceHeaders_Supplier
 ,invheaders.Origin AS Truck_InvoiceHeaders_Origin
FROM dbo.Truck_Filing_Headers headers
LEFT OUTER JOIN dbo.Truck_ContainersTab containers
  ON containers.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_DeclarationTab declaration
  ON declaration.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_InvoiceHeaders invheaders
  ON invheaders.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_InvoiceLines invlines
  ON invlines.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_MISC misc
  ON misc.Filing_Headers_FK = headers.id
WHERE headers.MappingStatus = 2
GO

DELETE FROM dbo.truck_def_values
WHERE column_name = 'LNO'
DELETE FROM dbo.truck_def_values_manual
WHERE column_name = 'LNO'
GO

--- Rail ---
ALTER TABLE dbo.Rail_InvoiceLines
DROP COLUMN LNO
GO

IF OBJECT_ID('dbo.rail_invoice_line_number', 'FN') IS NOT NULL
  DROP FUNCTION dbo.rail_invoice_line_number
GO

CREATE FUNCTION dbo.rail_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      ril.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY ril.id)
    FROM dbo.Rail_InvoiceLines ril
    WHERE ril.InvoiceHeaders_FK = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO

ALTER TABLE dbo.Rail_InvoiceLines
ADD invoice_line_number AS (dbo.rail_invoice_line_number(InvoiceHeaders_FK, id))
GO

ALTER VIEW dbo.Rail_Report
AS
SELECT
  headers.id AS Rail_Filing_Headers_id
 ,detailes.BDP_FK AS BDP_PK
 ,declaration.Arr AS Rail_DeclarationTab_Arr
 ,declaration.Arr_2 AS Rail_DeclarationTab_Arr_2
 ,declaration.Carrier_SCAC AS Rail_DeclarationTab_Carrier_SCAC
 ,declaration.Centralized_Exam_Site AS Rail_DeclarationTab_Centralized_Exam_Site
 ,declaration.Certify_Cargo_Release AS Rail_DeclarationTab_Certify_Cargo_Release
 ,declaration.Check_Local_Client AS Rail_DeclarationTab_Check_Local_Client
 ,declaration.Container AS Rail_DeclarationTab_Container
 ,declaration.Country_of_Export AS Rail_DeclarationTab_Country_of_Export
 ,declaration.Dep AS Rail_DeclarationTab_Dep
 ,declaration.Description AS Rail_DeclarationTab_Description
 ,declaration.Destination AS Rail_DeclarationTab_Destination
 ,declaration.Destination_State AS Rail_DeclarationTab_Destination_State
 ,declaration.Discharge AS Rail_DeclarationTab_Discharge
 ,declaration.Enable_Entry_Sum AS Rail_DeclarationTab_Enable_Entry_Sum
 ,declaration.Entry_Port AS Rail_DeclarationTab_Entry_Port
 ,declaration.Entry_Type AS Rail_DeclarationTab_Entry_Type
 ,declaration.ETA AS Rail_DeclarationTab_ETA
 ,declaration.Export_Date AS Rail_DeclarationTab_Export_Date
 ,declaration.FIRMs_Code AS Rail_DeclarationTab_FIRMs_Code
 ,declaration.HMF AS Rail_DeclarationTab_HMF
 ,declaration.Importer AS Rail_DeclarationTab_Importer
 ,declaration.Importer_of_record AS Rail_DeclarationTab_Importer_of_record
 ,declaration.INCO AS Rail_DeclarationTab_INCO
 ,declaration.Issuer AS Rail_DeclarationTab_Issuer
 ,declaration.Main_Supplier AS Rail_DeclarationTab_Main_Supplier
 ,declaration.Manual_Entry AS Rail_DeclarationTab_Manual_Entry
 ,declaration.Master_Bill AS Rail_DeclarationTab_Master_Bill
 ,declaration.No_Packages AS Rail_DeclarationTab_No_Packages
 ,declaration.Origin AS Rail_DeclarationTab_Origin
 ,declaration.Owner_Ref AS Rail_DeclarationTab_Owner_Ref
 ,declaration.Purchased AS Rail_DeclarationTab_Purchased
 ,declaration.RLF AS Rail_DeclarationTab_RLF
 ,declaration.Service AS Rail_DeclarationTab_Service
 ,declaration.Shipment_Type AS Rail_DeclarationTab_Shipment_Type
 ,declaration.Split_Shipment_Release AS Rail_DeclarationTab_Split_Shipment_Release
 ,declaration.Total_Volume AS Rail_DeclarationTab_Total_Volume
 ,declaration.Total_Weight AS Rail_DeclarationTab_Total_Weight
 ,declaration.Transport AS Rail_DeclarationTab_Transport
 ,declaration.Type AS Rail_DeclarationTab_Type

 ,containers.Bill_Issuer_SCAC AS Rail_Packing_Bill_Issuer_SCAC
 ,containers.Bill_Number AS Rail_Packing_Bill_Number
 ,containers.Bill_Type AS Rail_Packing_Bill_Type
 ,containers.Container_Number AS Rail_Packing_Container_Number
 ,containers.Is_Split AS Rail_Packing_Is_Split
 ,containers.IT_Number AS Rail_Packing_IT_Number
 ,containers.Manifest_QTY AS Rail_Packing_Manifest_QTY
 ,containers.Marks_and_Numbers AS Rail_Packing_Marks_and_Numbers
 ,containers.Pack_QTY AS Rail_Packing_Pack_QTY
 ,containers.Pack_Type AS Rail_Packing_Pack_Type
 ,containers.Shipping_Symbol AS Rail_Packing_Shipping_Symbol
 ,containers.UQ AS Rail_Packing_UQ
 ,containers.Packing_UQ AS Rail_Packing_Container_Packing_UQ
 ,containers.Seal_Number AS Rail_Packing_Seal_Number
 ,containers.Type AS Rail_Packing_Type
 ,containers.Mode AS Rail_Packing_Mode
 ,containers.Goods_Weight AS Rail_Packing_Goods_Weight
 ,containers.Bill_Num AS Rail_Packing_Bill_Num

 ,invheaders.Agreed_Place AS Rail_InvoiceHeaders_Agreed_Place
 ,invheaders.Broker_PGA_Contact_Email AS Rail_InvoiceHeaders_Broker_PGA_Contact_Email
 ,invheaders.Broker_PGA_Contact_Name AS Rail_InvoiceHeaders_Broker_PGA_Contact_Name
 ,invheaders.Broker_PGA_Contact_Phone AS Rail_InvoiceHeaders_Broker_PGA_Contact_Phone
 ,invheaders.Consignee AS Rail_InvoiceHeaders_Consignee
 ,invheaders.Consignee_Address AS Rail_InvoiceHeaders_Consignee_Address
 ,invheaders.Curr AS Rail_InvoiceHeaders_Curr
 ,invheaders.EPA_PST_Cert_Date AS Rail_InvoiceHeaders_EPA_PST_Cert_Date
 ,invheaders.EPA_TSCA_Cert_Date AS Rail_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,invheaders.EPA_VNE_Cert_Date AS Rail_InvoiceHeaders_EPA_VNE_Cert_Date
 ,invheaders.Export AS Rail_InvoiceHeaders_Export
 ,invheaders.Export_Date AS Rail_InvoiceHeaders_Export_Date
 ,invheaders.First_Sale AS Rail_InvoiceHeaders_First_Sale
 ,invheaders.FSIS_Cert_Date AS Rail_InvoiceHeaders_FSIS_Cert_Date
 ,invheaders.FWS_Cert_Date AS Rail_InvoiceHeaders_FWS_Cert_Date
 ,invheaders.Importer AS Rail_InvoiceHeaders_Importer
 ,invheaders.INCO AS Rail_InvoiceHeaders_INCO
 ,invheaders.Inv_Date AS Rail_InvoiceHeaders_Inv_Date
 ,invheaders.Inv_Gross_Weight AS Rail_InvoiceHeaders_Inv_Gross_Weight
 ,invheaders.Invoice_No AS Rail_InvoiceHeaders_Invoice_No
 ,invheaders.Invoice_Total AS Rail_InvoiceHeaders_Invoice_Total
 ,invheaders.LACEY_ACT_Cert_Date AS Rail_InvoiceHeaders_LACEY_ACT_Cert_Date
 ,invheaders.Landed_Costing_Ex_Rate AS Rail_InvoiceHeaders_Landed_Costing_Ex_Rate
 ,invheaders.Manufacturer AS Rail_InvoiceHeaders_Manufacturer
 ,invheaders.Net_Weight AS Rail_InvoiceHeaders_Net_Weight
 ,invheaders.NHTSA_Cert_Date AS Rail_InvoiceHeaders_NHTSA_Cert_Date
 ,invheaders.Origin AS Rail_InvoiceHeaders_Origin
 ,invheaders.Packages AS Rail_InvoiceHeaders_Packages
 ,invheaders.Payment_Date AS Rail_InvoiceHeaders_Payment_Date
 ,invheaders.Seller AS Rail_InvoiceHeaders_Seller
 ,invheaders.Ship_to_party AS Rail_InvoiceHeaders_Ship_to_party
 ,invheaders.Sold_to_party AS Rail_InvoiceHeaders_Sold_to_party
 ,invheaders.Supplier AS Rail_InvoiceHeaders_Supplier
 ,invheaders.Supplier_Address AS Rail_InvoiceHeaders_Supplier_Address
 ,invheaders.Transaction_Related AS Rail_InvoiceHeaders_Transaction_Related

 ,invlines.Attribute_1 AS Rail_InvoiceLines_Attribute_1
 ,invlines.Attribute_2 AS Rail_InvoiceLines_Attribute_2
 ,invlines.Certifying_Individual AS Rail_InvoiceLines_Certifying_Individual
 ,invlines.CIF_Component AS Rail_InvoiceLines_CIF_Component
 ,invlines.Code AS Rail_InvoiceLines_Code
 ,invlines.Consignee AS Rail_InvoiceLines_Consignee
 ,invlines.Curr AS Rail_InvoiceLines_Curr
 ,(dbo.fn_GetRailCarCount(invlines.Filing_Headers_FK) * invlines.Customs_QTY) AS Rail_InvoiceLines_Customs_QTY
 ,invlines.Dest_State AS Rail_InvoiceLines_Dest_State
 ,invlines.EPA_TSCA_Toxic_Substance_Control_Act_Indicator AS Rail_InvoiceLines_EPA_TSCA_Toxic_Indicator
 ,invlines.Export AS Rail_InvoiceLines_Export
 ,invlines.Goods_Description AS Rail_InvoiceLines_Goods_Description
 ,invlines.Gr_Weight AS Rail_InvoiceLines_Gr_Weight
 ,invlines.Invoice_No AS Rail_InvoiceLines_Invoice_No
 ,(dbo.fn_GetRailCarCount(invlines.Filing_Headers_FK) * invlines.Invoice_Qty) AS Rail_InvoiceLines_Invoice_Qty
 ,invlines.Invoice_Qty_Unit AS Rail_InvoiceLines_Invoice_Qty_Unit
 ,invlines.Line_Price AS Rail_InvoiceLines_Line_Price
 ,invlines.invoice_line_number AS Rail_InvoiceLines_LNO
 ,invlines.Manufacturer AS Rail_InvoiceLines_Manufacturer
 ,invlines.ORG AS Rail_InvoiceLines_ORG
 ,invlines.Origin AS Rail_InvoiceLines_Origin
 ,invlines.PGA_Contact_Email AS Rail_InvoiceLines_PGA_Contact_Email
 ,invlines.PGA_Contact_Name AS Rail_InvoiceLines_PGA_Contact_Name
 ,invlines.PGA_Contact_Phone AS Rail_InvoiceLines_PGA_Contact_Phone
 ,invlines.PriceUnit AS Rail_InvoiceLines_PriceUnit
 ,invlines.Prod_ID_1 AS Rail_InvoiceLines_Prod_ID_1
 ,invlines.Sold_To_Party AS Rail_InvoiceLines_Sold_To_Party
 ,invlines.SPI AS Rail_InvoiceLines_SPI
 ,invlines.Tariff AS Rail_InvoiceLines_Tariff
 ,invlines.Transaction_Related AS Rail_InvoiceLines_Transaction_Related
 ,invlines.TSCA_Indicator AS Rail_InvoiceLines_TSCA_Indicator
 ,invlines.UQ AS Rail_InvoiceLines_UQ
 ,(dbo.fn_GetRailCarCount(invlines.Filing_Headers_FK) * invlines.Amount) AS Rail_InvoiceLines_Amount
 ,invlines.Gr_Weight_Unit

 ,misc.Bond_Type AS Rail_MISC_Bond_Type
 ,misc.Branch AS Rail_MISC_Branch
 ,misc.Broker AS Rail_MISC_Broker
 ,misc.Broker_to_Pay AS Rail_MISC_Broker_to_Pay
 ,misc.FTA_Recon AS Rail_MISC_FTA_Recon
 ,misc.Merge_By AS Rail_MISC_Merge_By
 ,misc.Payment_Type AS Rail_MISC_Payment_Type
 ,misc.Prelim_Statement_Date AS Rail_MISC_Prelim_Statement_Date
 ,misc.Preparer_Dist_Port AS Rail_MISC_Preparer_Dist_Port
 ,misc.Recon_Issue AS Rail_MISC_Recon_Issue
 ,misc.Submitter AS Rail_MISC_Submitter
 ,misc.Tax_Deferrable_Ind AS Rail_MISC_Tax_Deferrable_Ind

 ,headers.GrossWeightSummary AS Rail_GrossWeightSummary
 ,headers.GrossWeightSummaryUnit AS Rail_GrossWeightSummaryUnit

FROM dbo.Rail_Filing_Headers headers
INNER JOIN dbo.Rail_Filing_Details detailes
  ON headers.id = detailes.Filing_Headers_FK
LEFT JOIN dbo.Rail_DeclarationTab declaration
  ON declaration.Filing_Headers_FK = headers.id
    AND detailes.BDP_FK = declaration.BDP_FK
LEFT JOIN dbo.Rail_ContainersTab containers
  ON containers.Filing_Headers_FK = headers.id
    AND detailes.BDP_FK = containers.BDP_FK
LEFT JOIN dbo.Rail_InvoiceLines invlines
  ON invlines.Filing_Headers_FK = headers.id
    AND detailes.BDP_FK = invlines.BDP_FK
LEFT JOIN dbo.Rail_InvoiceHeaders invheaders
  ON invheaders.Filing_Headers_FK = headers.id
    AND invheaders.id = invlines.InvoiceHeaders_FK
LEFT JOIN dbo.Rail_MISC misc
  ON misc.Filing_Headers_FK = headers.id
    AND detailes.BDP_FK = misc.BDP_FK
WHERE headers.MappingStatus = 2
GO

DELETE FROM dbo.Rail_DEFValues
WHERE ColName = 'LNO'
DELETE FROM dbo.Rail_DEFValues_Manual
WHERE column_name = 'LNO'
GO