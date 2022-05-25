CREATE TABLE dbo.Truck_ContainersTab (
  id INT IDENTITY PRIMARY KEY
 ,Filing_Headers_FK INT NOT NULL
 ,TI_FK INT NULL
 ,Bill_Type VARCHAR(128) NULL
 ,Manifest_QTY VARCHAR(128) NULL
 ,UQ VARCHAR(128) NULL
 ,Bill_Issuer_SCAC VARCHAR(128) NULL
 ,IT_Number VARCHAR(128) NULL
 ,Is_Split VARCHAR(128) NULL
 ,Bill_Number VARCHAR(128) NULL
 ,Container_Number VARCHAR(128) NULL
 ,Pack_QTY VARCHAR(128) NULL
 ,Pack_Type VARCHAR(128) NULL
 ,Marks_and_Numbers VARCHAR(128) NULL
 ,Shipping_Symbol VARCHAR(128) NULL
 ,Seal_Number VARCHAR(128) NULL
 ,Type VARCHAR(128) NULL
 ,Mode VARCHAR(128) NULL
 ,Goods_Weight VARCHAR(128) NULL
 ,Bill_Num VARCHAR(128) NULL
 ,Packing_UQ VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
)
GO

ALTER TABLE dbo.Truck_ContainersTab
ADD CONSTRAINT [FK_dbo.Truck_ContainersTab_dbo.Truck_Inbound_TI_FK] FOREIGN KEY (TI_FK) REFERENCES dbo.Truck_Inbound (id)
GO

ALTER TABLE dbo.Truck_ContainersTab
ADD CONSTRAINT [FK_dbo.Truck_ContainersTab_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

CREATE TABLE dbo.Truck_DeclarationTab (
  id INT IDENTITY PRIMARY KEY
 ,Filing_Headers_FK INT NOT NULL
 ,TI_FK INT NOT NULL
 ,Main_Supplier VARCHAR(128) NULL
 ,Importer VARCHAR(128) NULL
 ,Shipment_Type VARCHAR(128) NULL
 ,Transport VARCHAR(128) NULL
 ,Container VARCHAR(128) NULL
 ,Entry_Type VARCHAR(128) NULL
 ,RLF VARCHAR(128) NULL
 ,Enable_Entry_Sum VARCHAR(128) NULL
 ,Type VARCHAR(128) NULL
 ,Certify_Cargo_Release VARCHAR(128) NULL
 ,Service VARCHAR(128) NULL
 ,Issuer VARCHAR(128) NULL
 ,Master_Bill VARCHAR(128) NULL
 ,Carrier_SCAC VARCHAR(128) NULL
 ,Discharge VARCHAR(128) NULL
 ,Entry_Port VARCHAR(128) NULL
 ,Dep DATE NULL
 ,Arr DATE NULL
 ,Arr_2 DATE NULL
 ,HMF VARCHAR(128) NULL
 ,Origin VARCHAR(128) NULL
 ,Destination VARCHAR(128) NULL
 ,Destination_State VARCHAR(128) NULL
 ,Country_of_Export VARCHAR(128) NULL
 ,ETA DATE NULL
 ,Export_Date DATE NULL
 ,Description VARCHAR(128) NULL
 ,Owner_Ref VARCHAR(128) NULL
 ,INCO VARCHAR(128) NULL
 ,Total_Weight VARCHAR(128) NULL
 ,Total_Volume VARCHAR(128) NULL
 ,No_Packages VARCHAR(128) NULL
 ,FIRMs_Code VARCHAR(128) NULL
 ,Centralized_Exam_Site VARCHAR(128) NULL
 ,Purchased VARCHAR(128) NULL
 ,Manual_Entry VARCHAR(128) NULL
 ,Importer_of_record VARCHAR(128) NULL
 ,Split_Shipment_Release VARCHAR(128) NULL
 ,Check_Local_Client VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME()) 
)
GO

ALTER TABLE dbo.Truck_DeclarationTab
ADD CONSTRAINT [FK_dbo.Truck_DeclarationTab_dbo.Truck_Inbound_TI_FK] FOREIGN KEY (TI_FK) REFERENCES dbo.Truck_Inbound (Id)
GO

ALTER TABLE dbo.Truck_DeclarationTab
ADD CONSTRAINT [FK_dbo.Truck_DeclarationTab_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

CREATE TABLE dbo.Truck_InvoiceHeaders (
  id INT NOT NULL PRIMARY KEY
 ,Filing_Headers_FK INT NOT NULL
 ,Invoice_No VARCHAR(128) NULL
 ,Supplier VARCHAR(128) NULL
 ,Supplier_Address VARCHAR(128) NULL
 ,INCO VARCHAR(128) NULL
 ,Invoice_Total NUMERIC(18, 6) NULL
 ,Curr VARCHAR(128) NULL
 ,Origin VARCHAR(128) NULL
 ,Payment_Date VARCHAR(128) NULL
 ,Consignee VARCHAR(128) NULL
 ,Consignee_Address VARCHAR(128) NULL
 ,Inv_Date VARCHAR(128) NULL
 ,Agreed_Place VARCHAR(128) NULL
 ,Inv_Gross_Weight VARCHAR(128) NULL
 ,Net_Weight VARCHAR(128) NULL
 ,Export VARCHAR(128) NULL
 ,Export_Date DATE NULL
 ,First_Sale VARCHAR(128) NULL
 ,Transaction_Related VARCHAR(128) NULL
 ,Packages VARCHAR(128) NULL
 ,Manufacturer VARCHAR(128) NULL
 ,Seller VARCHAR(128) NULL
 ,Importer VARCHAR(128) NULL
 ,Sold_to_party VARCHAR(128) NULL
 ,Ship_to_party VARCHAR(128) NULL
 ,Broker_PGA_Contact_Name VARCHAR(128) NULL
 ,Broker_PGA_Contact_Phone VARCHAR(128) NULL
 ,Broker_PGA_Contact_Email VARCHAR(128) NULL
 ,EPA_PST_Cert_Date VARCHAR(128) NULL
 ,EPA_TSCA_Cert_Date VARCHAR(128) NULL
 ,EPA_VNE_Cert_Date VARCHAR(128) NULL
 ,FSIS_Cert_Date VARCHAR(128) NULL
 ,FWS_Cert_Date VARCHAR(128) NULL
 ,LACEY_ACT_Cert_Date VARCHAR(128) NULL
 ,NHTSA_Cert_Date VARCHAR(128) NULL
 ,Landed_Costing_Ex_Rate VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
)
GO

ALTER TABLE dbo.Truck_InvoiceHeaders
ADD CONSTRAINT FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

CREATE TABLE dbo.Truck_InvoiceLines (
  id INT IDENTITY PRIMARY KEY
 ,Filing_Headers_FK INT NOT NULL
 ,InvoiceHeaders_FK INT NOT NULL
 ,TI_FK INT NULL
 ,LNO VARCHAR(128) NULL
 ,Invoice_No VARCHAR(128) NULL
 ,Tariff VARCHAR(128) NULL
 ,Customs_QTY NUMERIC(18, 6) NULL
 ,Line_Price NUMERIC(18, 6) NULL
 ,Goods_Description VARCHAR(128) NULL
 ,ORG VARCHAR(128) NULL
 ,SPI VARCHAR(128) NULL
 ,Gr_Weight NUMERIC(18, 6) NULL
 ,UQ VARCHAR(128) NULL
 ,PriceUnit NUMERIC(18, 6) NULL
 ,Prod_ID_1 VARCHAR(128) NULL
 ,Attribute_1 VARCHAR(128) NULL
 ,Attribute_2 VARCHAR(128) NULL
 ,Export VARCHAR(128) NULL
 ,Origin VARCHAR(128) NULL
 ,Dest_State VARCHAR(128) NULL
 ,Transaction_Related VARCHAR(128) NULL
 ,Invoice_Qty NUMERIC(18, 6) NULL
 ,Invoice_Qty_Unit VARCHAR(128) NULL
 ,Manufacturer VARCHAR(128) NULL
 ,Consignee VARCHAR(128) NULL
 ,Sold_to_party VARCHAR(128) NULL
 ,Code VARCHAR(128) NULL
 ,Curr VARCHAR(128) NULL
 ,CIF_Component VARCHAR(128) NULL
 ,EPA_TSCA_Toxic_Substance_Control_Act_Indicator VARCHAR(128) NULL
 ,TSCA_Indicator VARCHAR(128) NULL
 ,Certifying_Individual VARCHAR(128) NULL
 ,PGA_Contact_Name VARCHAR(128) NULL
 ,PGA_Contact_Phone VARCHAR(128) NULL
 ,PGA_Contact_Email VARCHAR(128) NULL
 ,Amount INT NULL
 ,Description VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,Gr_Weight_Unit VARCHAR(2) NULL
 ,Gr_Weight_Tons AS ([dbo].[weightToTon]([Gr_Weight], [Gr_Weight_Unit]))
)
GO

ALTER TABLE dbo.Truck_InvoiceLines
ADD CONSTRAINT [FK_dbo.Truck_InvoiceLines.Truck_Inbound_TI_FK] FOREIGN KEY (TI_FK) REFERENCES dbo.Truck_Inbound (Id)
GO

ALTER TABLE dbo.Truck_InvoiceLines
ADD CONSTRAINT [FK_dbo.Truck_InvoiceLines.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

CREATE TABLE dbo.Truck_MISC (
  id INT IDENTITY PRIMARY KEY
 ,Filing_Headers_FK INT NOT NULL
 ,TI_FK INT NULL
 ,Branch VARCHAR(128) NULL
 ,Broker VARCHAR(128) NULL
 ,Merge_By VARCHAR(128) NULL
 ,Tax_Deferrable_Ind VARCHAR(128) NULL
 ,Preparer_Dist_Port VARCHAR(128) NULL
 ,Recon_Issue VARCHAR(128) NULL
 ,FTA_Recon VARCHAR(128) NULL
 ,Bond_Type VARCHAR(128) NULL
 ,Payment_Type VARCHAR(128) NULL
 ,Broker_to_Pay VARCHAR(128) NULL
 ,Prelim_Statement_Date VARCHAR(128) NULL
 ,Submitter VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
)
GO

ALTER TABLE dbo.Truck_MISC
ADD CONSTRAINT [FK_dbo.Truck_MISC.Truck_Inbound_TI_FK] FOREIGN KEY (TI_FK) REFERENCES dbo.Truck_Inbound (Id)
GO

ALTER TABLE dbo.Truck_MISC
ADD CONSTRAINT [FK_dbo.Truck_MISC.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

ALTER VIEW dbo.v_Truck_Filing_Data 
AS SELECT
  ti.Id AS id
  ,tfh.id AS Filing_header_id 
  ,tdt.Importer AS Importer
  ,ti.PAPs
FROM dbo.Truck_Filing_Headers tfh
LEFT OUTER JOIN dbo.Truck_DeclarationTab tdt
  ON tdt.FILING_HEADERS_FK = tfh.id
  INNER JOIN Truck_Inbound ti
  ON ti.Id = tdt.TI_FK
GO