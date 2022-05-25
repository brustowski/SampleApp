SET ARITHABORT OFF
go
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'fn_GetRailCarCount') AND type IN ('IF', 'FN', 'TF'))
  DROP FUNCTION dbo.fn_GetRailCarCount
GO
Create FUNCTION [dbo].[fn_GetRailCarCount](
  @filingHeaderId INT
)
RETURNS INT
WITH SCHEMABINDING
AS 
  BEGIN
    DECLARE @result INT=null ;    
    SELECT
      @result = COUNT(d.BDP_FK) FROM dbo.Rail_Filing_Details d		 
    WHERE d.Filing_Headers_FK = @filingHeaderId
	GROUP BY d.Filing_Headers_FK  
    RETURN @result
  END 
  GO
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'fn_Rail_CalculateGrossWtUnit') AND type IN ('IF', 'FN', 'TF'))
  DROP FUNCTION dbo.fn_Rail_CalculateGrossWtUnit
GO
CREATE function [dbo].[fn_Rail_CalculateGrossWtUnit](
  @filingHeaderId INT
)
RETURNS VARCHAR(2)
WITH SCHEMABINDING
AS 
  BEGIN
     DECLARE @result varchar(2)=null ;  	
	 DECLARE @count int = 0; 
	
	 Select  @count = (dbo.fn_GetRailCarCount( @filingHeaderId)) 
    
	 Select Top(1) 
	 @result = case when @count > 1 then 'T' else ril.Gr_Weight_Unit end      
	FROM dbo.Rail_InvoiceLines ril
    WHERE ril.Filing_Headers_FK = @filingHeaderId
    RETURN @result
  END 
  GO
  ALTER TABLE dbo.Rail_Filing_Headers
	DROP COLUMN GrossWeightTonsSummary
  go
IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.fn_RailWeightSummary') AND type IN ('IF', 'FN', 'TF'))
  DROP FUNCTION dbo.fn_RailWeightSummary
GO
CREATE FUNCTION [dbo].[fn_RailWeightSummary](
  @filingHeaderId INT
)
RETURNS DECIMAL(18, 9)
WITH SCHEMABINDING
AS 
 BEGIN
    DECLARE @result DECIMAL(18, 9) = NULL;
	DECLARE @calcwt DECIMAL(18, 9) = NULL;
	DECLARE @count int = 0;
	
	Select  @count = (dbo.fn_GetRailCarCount( @filingHeaderId))
    
    SELECT
      @calcwt = SUM(ril.Gr_Weight)		  
	FROM dbo.Rail_InvoiceLines ril
    WHERE ril.Filing_Headers_FK = @filingHeaderId

	 Select Top(1)
	 @result = case when @count > 1 then dbo.weightToTon(@calcwt, ril.Gr_Weight_Unit) else ril.Gr_Weight end
	   
	FROM dbo.Rail_InvoiceLines ril
    WHERE ril.Filing_Headers_FK = @filingHeaderId
    RETURN @result
  END
GO

ALTER TABLE dbo.Rail_Filing_Headers
	ADD GrossWeightSummary as ([dbo].[fn_RailWeightSummary]([id]))
GO

ALTER TABLE dbo.Rail_Filing_Headers
	ADD GrossWeightSummaryUnit as ([dbo].[fn_Rail_CalculateGrossWtUnit]([id]))
GO

ALTER VIEW [dbo].[Rail_Report]
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
 ,(dbo.fn_GetRailCarCount(invlines.Filing_Headers_FK)*invlines.Customs_QTY) AS Rail_InvoiceLines_Customs_QTY
 ,invlines.Dest_State AS Rail_InvoiceLines_Dest_State
 ,invlines.EPA_TSCA_Toxic_Substance_Control_Act_Indicator AS Rail_InvoiceLines_EPA_TSCA_Toxic_Indicator
 ,invlines.Export AS Rail_InvoiceLines_Export
 ,invlines.Goods_Description AS Rail_InvoiceLines_Goods_Description
 ,invlines.Gr_Weight AS Rail_InvoiceLines_Gr_Weight
 ,invlines.Invoice_No AS Rail_InvoiceLines_Invoice_No
 ,(dbo.fn_GetRailCarCount(invlines.Filing_Headers_FK)*invlines.Invoice_Qty) AS Rail_InvoiceLines_Invoice_Qty
 ,invlines.Invoice_Qty_Unit AS Rail_InvoiceLines_Invoice_Qty_Unit
 ,invlines.Line_Price AS Rail_InvoiceLines_Line_Price
 ,invlines.LNO AS Rail_InvoiceLines_LNO
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
 ,(dbo.fn_GetRailCarCount(invlines.Filing_Headers_FK)*invlines.Amount) AS Rail_InvoiceLines_Amount
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

ALTER VIEW [dbo].[v_Rail_Filing_Data] 
AS SELECT
  f.BDP_FK AS id
 ,h.id AS Filing_header_id
 ,p.BDP_EM AS Manifest_id
 ,d.Importer AS Importer
 ,d.Entry_Port AS Port_code
 ,d.Master_Bill AS Bill_of_lading
 ,c.Container_Number AS Container_number
 ,p.ReferenceNumber1 AS Train_number
 ,l.Gr_Weight AS Gross_weight
 ,l.Gr_Weight_Unit AS Gross_weight_unit

FROM dbo.Rail_Filing_Headers h
INNER JOIN dbo.Rail_Filing_Details f
  ON h.id = f.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed p
  ON f.BDP_FK = p.BDP_PK
LEFT OUTER JOIN dbo.Rail_DeclarationTab d
  ON d.Filing_Headers_FK = h.id
    AND f.BDP_FK = d.BDP_FK
LEFT OUTER JOIN dbo.Rail_ContainersTab c
  ON c.Filing_Headers_FK = h.id
    AND f.BDP_FK = c.BDP_FK
LEFT OUTER JOIN dbo.Rail_InvoiceLines l
  ON l.Filing_Headers_FK = h.id
    AND f.BDP_FK = l.BDP_FK
GO
