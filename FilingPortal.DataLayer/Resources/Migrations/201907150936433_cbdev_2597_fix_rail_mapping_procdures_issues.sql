--- update Rail Declaration tab table ---
IF OBJECT_ID(N'FK_DeclarationTab_BD_Parsed', 'F') IS NOT NULL
  ALTER TABLE dbo.Rail_DeclarationTab
  DROP CONSTRAINT FK_DeclarationTab_BD_Parsed
GO
IF EXISTS (SELECT
      1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Rail_DeclarationTab'
    AND COLUMN_NAME = 'BDP_FK'
    AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE dbo.Rail_DeclarationTab
  ALTER COLUMN BDP_FK INT NULL
END
GO

--- update Rail MISC table ---
IF OBJECT_ID(N'FK_RAIL_MISC_REFERENCE_RAIL_BD_', 'F') IS NOT NULL
  ALTER TABLE dbo.Rail_MISC
  DROP CONSTRAINT FK_RAIL_MISC_REFERENCE_RAIL_BD_
GO
IF EXISTS (SELECT
      1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Rail_MISC'
    AND COLUMN_NAME = 'BDP_FK'
    AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE dbo.Rail_MISC
  ALTER COLUMN BDP_FK INT NULL
END
GO

--- update Rail Invoice Line table ---
IF OBJECT_ID(N'FK_RAIL_INV_REFERENCE_RAIL_BD_', 'F') IS NOT NULL
  ALTER TABLE dbo.Rail_InvoiceLines
  DROP CONSTRAINT FK_RAIL_INV_REFERENCE_RAIL_BD_
GO
IF EXISTS (SELECT
      1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Rail_InvoiceLines'
    AND COLUMN_NAME = 'BDP_FK'
    AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE dbo.Rail_InvoiceLines
  ALTER COLUMN BDP_FK INT NULL
END
GO

--- update Rail Container tab table ---
ALTER TABLE dbo.Rail_ContainersTab
ADD Gross_Weight NUMERIC(18, 6) NULL
GO

ALTER TABLE dbo.Rail_ContainersTab
ADD Gross_Weight_Unit VARCHAR(2) NULL
GO

--- add missing functions ---
IF OBJECT_ID(N'dbo.rail_gross_weight', 'FN') IS NOT NULL
  DROP FUNCTION dbo.rail_gross_weight
GO
CREATE FUNCTION dbo.rail_gross_weight (@filingHeaderId INT)
RETURNS DECIMAL(18, 9) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result DECIMAL(18, 9) = NULL;
  DECLARE @count INT = 0;

  SELECT
    @count = COUNT(rct.BDP_FK)
  FROM dbo.Rail_ContainersTab rct
  WHERE rct.Filing_Headers_FK = @filingHeaderId

  SELECT
    @result = SUM(dbo.weightToTon(rct.Gross_Weight, rct.Gross_Weight_unit))
  FROM dbo.Rail_ContainersTab rct
  WHERE rct.Filing_Headers_FK = @filingHeaderId

  RETURN CASE
    WHEN @count > 1 THEN @result
    ELSE @result * 1000
  END
END
GO

IF OBJECT_ID(N'dbo.rail_gross_weight_unit', 'FN') IS NOT NULL
  DROP FUNCTION dbo.rail_gross_weight_unit
GO
CREATE FUNCTION dbo.rail_gross_weight_unit (@filingHeaderId INT)
RETURNS VARCHAR(2) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result VARCHAR(2) = NULL;
  DECLARE @count INT = 0;

  SELECT
    @count = COUNT(rct.BDP_FK)
  FROM dbo.Rail_ContainersTab rct
  WHERE rct.Filing_Headers_FK = @filingHeaderId

  SELECT
    @result =
    CASE
      WHEN @count > 1 THEN 'T'
      ELSE 'KG'
    END

  RETURN @result
END
GO

--- update Rail Invoice Lines table ---
-- drop Filing Header summary columns
IF EXISTS (SELECT
      1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Rail_Filing_Headers'
    AND COLUMN_NAME = 'GrossWeightSummaryUnit'
    AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE dbo.Rail_Filing_Headers
  DROP COLUMN GrossWeightSummaryUnit
END
GO

IF EXISTS (SELECT
      1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Rail_Filing_Headers'
    AND COLUMN_NAME = 'GrossWeightSummary'
    AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE dbo.Rail_Filing_Headers
  DROP COLUMN GrossWeightSummary
END
GO
-- drop functions
IF OBJECT_ID(N'dbo.fn_RailWeightSummary', 'FN') IS NOT NULL
  DROP FUNCTION dbo.fn_RailWeightSummary
GO

IF OBJECT_ID(N'dbo.fn_Rail_CalculateGrossWtUnit', 'FN') IS NOT NULL
  DROP FUNCTION dbo.fn_Rail_CalculateGrossWtUnit
GO
-- alter Invoice Lines columns
IF EXISTS (SELECT
      1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Rail_InvoiceLines'
    AND COLUMN_NAME = 'Gr_Weight_Tons'
    AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE dbo.Rail_InvoiceLines
  DROP COLUMN Gr_Weight_Tons
END
GO

IF EXISTS (SELECT
      1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Rail_InvoiceLines'
    AND COLUMN_NAME = 'Gr_Weight'
    AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE dbo.Rail_InvoiceLines
  DROP COLUMN Gr_Weight
END
GO

IF EXISTS (SELECT
      1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Rail_InvoiceLines'
    AND COLUMN_NAME = 'Gr_Weight_Unit'
    AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE dbo.Rail_InvoiceLines
  DROP COLUMN Gr_Weight_Unit
END
GO

ALTER TABLE dbo.Rail_InvoiceLines
ADD Gr_Weight AS (dbo.rail_gross_weight(Filing_Headers_FK))
GO

ALTER TABLE dbo.Rail_InvoiceLines
ADD Gr_Weight_Unit AS (dbo.rail_gross_weight_unit(Filing_Headers_FK))
GO

--- update Rail configuration ---
IF NOT EXISTS (SELECT
      1
    FROM dbo.Rail_DEFValues
    WHERE ColName = 'Gr_Weight_Unit')
BEGIN
  INSERT dbo.Rail_DEFValues (
    Display_on_UI
   ,ValueLabel
   ,ValueDesc
   ,DefValue
   ,ColName
   ,FManual
   ,FHasDefaultVal
   ,FEditable
   ,FMandatory
   ,CreatedDate
   ,CreatedUser
   ,SingleFilingOrder
   ,paired_field_table
   ,paired_field_column
   ,handbook_name
   ,section_id)
  VALUES (
    10
   ,'Gross Weight Unit'
   ,'Gross Weight Unit'
   ,NULL
   ,'Gr_Weight_Unit'
   ,DEFAULT
   ,DEFAULT
   ,0
   ,0
   ,DEFAULT
   ,DEFAULT
   ,NULL
   ,NULL
   ,NULL
   ,NULL
   ,5)
END
GO

IF NOT EXISTS (SELECT
      1
    FROM dbo.Rail_DEFValues
    WHERE ColName = 'Gr_Weight')
BEGIN
  INSERT dbo.Rail_DEFValues (
    Display_on_UI
   ,ValueLabel
   ,ValueDesc
   ,DefValue
   ,ColName
   ,FManual
   ,FHasDefaultVal
   ,FEditable
   ,FMandatory
   ,CreatedDate
   ,CreatedUser
   ,SingleFilingOrder
   ,paired_field_table
   ,paired_field_column
   ,handbook_name
   ,section_id)
  VALUES (
    11
   ,'Gross Weight'
   ,'Gross Weight'
   ,NULL
   ,'Gr_Weight'
   ,DEFAULT
   ,DEFAULT
   ,0
   ,0
   ,DEFAULT
   ,DEFAULT
   ,NULL
   ,NULL
   ,NULL
   ,NULL
   ,5)
END
GO

UPDATE dbo.Rail_DEFValues
SET FEditable = 0
WHERE ColName = 'Gr_Weight_Unit'
OR ColName = 'Gr_Weight'
GO

UPDATE dbo.Rail_DEFValues_Manual
SET FEditable = 0
WHERE column_name = 'Gr_Weight_Unit'
OR column_name = 'Gr_Weight'
GO

INSERT dbo.Rail_DEFValues (
  Display_on_UI
 ,ValueLabel
 ,ValueDesc
 ,DefValue
 ,ColName
 ,FManual
 ,FHasDefaultVal
 ,FEditable
 ,FMandatory
 ,CreatedDate
 ,CreatedUser
 ,SingleFilingOrder
 ,paired_field_table
 ,paired_field_column
 ,handbook_name
 ,section_id)
VALUES (
  10
 ,'Gross Weight Unit'
 ,'Gross Weight Unit'
 ,NULL
 ,'Gross_Weight_Unit'
 ,DEFAULT
 ,DEFAULT
 ,DEFAULT
 ,1
 ,DEFAULT
 ,DEFAULT
 ,NULL
 ,NULL
 ,NULL
 ,NULL
 ,6);
GO

INSERT dbo.Rail_DEFValues (
  Display_on_UI
 ,ValueLabel
 ,ValueDesc
 ,DefValue
 ,ColName
 ,FManual
 ,FHasDefaultVal
 ,FEditable
 ,FMandatory
 ,CreatedDate
 ,CreatedUser
 ,SingleFilingOrder
 ,paired_field_table
 ,paired_field_column
 ,handbook_name
 ,section_id)
VALUES (
  11
 ,'Gross Weight'
 ,'Gross Weight'
 ,NULL
 ,'Gross_Weight'
 ,DEFAULT
 ,DEFAULT
 ,DEFAULT
 ,1
 ,DEFAULT
 ,DEFAULT
 ,NULL
 ,NULL
 ,NULL
 ,NULL
 ,6);
GO
--- update Rail mapping procedures ---
-- add containers tab record --
ALTER PROCEDURE dbo.rail_add_container_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add container data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_ContainersTab pct
      WHERE pct.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Rail_ContainersTab (
        BDP_FK
       ,Bill_Issuer_SCAC
       ,Bill_Num
       ,Bill_Number
       ,Container_Number
       ,Gross_Weight
       ,Gross_Weight_Unit
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        details.BDP_FK
       ,p.IssuerCode
       ,p.BillofLading
       ,CONCAT('MB:', p.BillofLading)
       ,CONCAT(EquipmentInitial, EquipmentNumber)
       ,p.Weight
       ,p.WeightUnit
       ,details.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details details
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = details.BDP_FK
      WHERE details.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END;
GO
-- add declaration tab record --
ALTER PROCEDURE dbo.rail_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_DeclarationTab pdt
      WHERE pdt.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Rail_DeclarationTab (
        Carrier_SCAC
       ,Country_of_Export
       ,Description
       ,Destination
       ,Destination_State
       ,Discharge
       ,Entry_Port
       ,FIRMs_Code
       ,Importer
       ,Issuer
       ,Main_Supplier
       ,Origin
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        p.IssuerCode AS Carrier_SCAC
       ,rp.Export AS Country_of_Export
       ,Description1 AS Description
       ,rp.Destination AS Destination
       ,rn.Destination_State AS Destination_State
       ,p.PortOfUnlading AS Discharge
       ,p.PortOfUnlading AS Entry_Port
       ,rp.FIRMs_Code AS FIRMs_Code
       ,rn.Importer AS Importer
       ,p.IssuerCode AS Issuer
       ,rn.Main_Supplier AS Main_Supplier
       ,rp.Origin AS Origin
       ,d.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details d
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = d.BDP_FK
      LEFT JOIN dbo.Rail_Rule_Port rp
        ON p.PortOfUnlading = RP.Port
      LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
        ON RTRIM(LTRIM(p.Importer)) = RTRIM(LTRIM(rn.Importer_Name))
          AND (RTRIM(LTRIM(p.Supplier)) = RTRIM(LTRIM(rn.Supplier_Name))
            OR (p.Supplier IS NULL
              AND rn.Supplier_Name IS NULL))
      WHERE d.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END;
GO
-- add invoice header record --
ALTER PROCEDURE dbo.rail_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_InvoiceHeaders'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add invoice header data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_InvoiceHeaders pih
      WHERE pih.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Rail_InvoiceHeaders (
        Consignee
       ,Export
       ,Importer
       ,Manufacturer
       ,Origin
       ,Seller
       ,Ship_to_party
       ,Sold_to_party
       ,Supplier
       ,Transaction_Related
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        rn.Consignee AS Consignee
       ,rp.Export AS Export
       ,rn.Importer AS Importer
       ,rn.Manufacturer AS Manufacturer
       ,rn.CountryofOrigin AS Origin
       ,rn.Seller AS Seller
       ,rn.Ship_to_party AS Ship_to_party
       ,rn.Sold_to_party AS Sold_to_party
       ,rn.Main_Supplier AS Supplier
       ,rn.Relationship AS Transaction_Related
       ,d.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details d
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = d.BDP_FK
      LEFT JOIN dbo.Rail_Rule_Port rp
        ON p.PortOfUnlading = RP.Port
      LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
        ON p.Importer = rn.Importer_Name
          AND (p.Supplier = rn.Supplier_Name
            OR (p.Supplier IS NULL
              AND rn.Supplier_Name IS NULL))
      WHERE d.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.rail_add_invoice_line_record @filingHeaderId
                                         ,@recordId
                                         ,@filingUser

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END;
GO
-- add invoice line record --
ALTER PROCEDURE dbo.rail_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add invoice line data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_InvoiceLines pil
      WHERE pil.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Rail_InvoiceLines (
        InvoiceHeaders_FK
       ,Attribute_1
       ,Attribute_2
       ,Consignee
       ,Dest_State
       ,Export
       ,Goods_Description
       ,Manufacturer
       ,ORG
       ,Origin
       ,Prod_ID_1
       ,Tariff
       ,Transaction_Related
       ,Customs_QTY
       ,SPI
       ,UQ
       ,PriceUnit
       ,Invoice_Qty
       ,Invoice_Qty_Unit
       ,Amount
       ,Line_Price
       ,Description
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId AS InvoiceHeaders_FK
       ,rd.Attribute_1 AS Attribute_1
       ,rd.Attribute_2 AS Attribute_2
       ,rn.Consignee AS Consignee
       ,rn.Destination_State AS Dest_State
       ,rp.Export AS Export
       ,rd.Goods_Description AS Goods_Description
       ,rn.Manufacturer AS Manufacturer
       ,rn.CountryofOrigin AS ORG
       ,rn.CountryofOrigin AS Origin
       ,rd.Prod_ID_1 AS Prod_ID_1
       ,rd.Tariff AS Tariff
       ,rn.Relationship AS Transaction_Related
       ,rd.Template_Invoice_Quantity AS Customs_QTY
       ,rn.DFT AS SPI
       ,rd.Invoice_UOM AS UQ
       ,rn.Value AS PriceUnit
       ,rd.Template_Invoice_Quantity AS Invoice_Qty
       ,rd.Invoice_UOM AS Invoice_Qty_Unit
       ,rn.Freight AS Amount
       ,rn.Value * rd.Template_Invoice_Quantity AS Line_Price
       ,rd.Description AS Description
       ,d.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details d
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = d.BDP_FK
      LEFT JOIN dbo.Rail_Rule_Port rp
        ON p.PortOfUnlading = RP.Port
      LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
        ON p.Importer = rn.Importer_Name
          AND (p.Supplier = rn.Supplier_Name
            OR (p.Supplier IS NULL
              AND rn.Supplier_Name IS NULL))
      LEFT JOIN dbo.Rail_Rule_Desc1_Desc2 rd
        ON RTRIM(LTRIM(rd.Description1)) = RTRIM(LTRIM(p.Description1))
      WHERE d.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END;
GO
-- add misc record --
ALTER PROCEDURE dbo.rail_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );


  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add misc data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_MISC pm
      WHERE pm.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Rail_MISC (
        Recon_Issue
       ,FTA_Recon
       ,Payment_Type
       ,Broker_to_Pay
       ,Submitter
       ,Filing_Headers_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        ISNULL(rn.Value_Recon, 'N/A') AS Recon_Issue
       ,rn.FTA_Recon AS FTA_Recon
       ,rn.Payment_Type AS Payment_Type
       ,rn.Broker_to_Pay AS Broker_to_Pay
       ,rn.Importer AS Submitter
       ,d.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details d
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = d.BDP_FK
      LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
        ON RTRIM(LTRIM(p.Importer)) = RTRIM(LTRIM(rn.Importer_Name))
          AND (RTRIM(LTRIM(p.Supplier)) = RTRIM(LTRIM(rn.Supplier_Name))
            OR (p.Supplier IS NULL
              AND rn.Supplier_Name IS NULL))
      WHERE d.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    PRINT @recordId

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO
-- add filing records --
ALTER PROCEDURE dbo.rail_filing (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  -- add declaration
  EXEC dbo.rail_add_declaration_record @filingHeaderId
                                      ,@filingHeaderId
                                      ,@filingUser
  -- add container
  EXEC dbo.rail_add_container_record @filingHeaderId
                                    ,@filingHeaderId
                                    ,@filingUser
  -- add invoice header
  EXEC dbo.rail_add_invoice_header_record @filingHeaderId
                                         ,@filingHeaderId
                                         ,@filingUser
  -- add misc
  EXEC dbo.rail_add_misc_record @filingHeaderId
                               ,@filingHeaderId
                               ,@filingUser
END;
GO
-- update def values manual data --
ALTER PROCEDURE dbo.update_def_values_manual (@defValuesManualTableName VARCHAR(128) = 'truck_export_def_values_manual'
, @tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  /*
    Update values into def value manual table with data from result table
  */
  DECLARE @selectColumnsList VARCHAR(MAX);
  DECLARE @columnsList VARCHAR(MAX);
  DECLARE @selectStatment VARCHAR(MAX);
  DECLARE @mergeStatment VARCHAR(MAX);

  -- get table column names with type converion for select statment
  SET @selectColumnsList = STUFF((SELECT DISTINCT
      ',' + 'isnull(cast(' +
      CASE
        WHEN t.name LIKE 'date%' THEN 'format(' + QUOTENAME(c.name) + ', ''MM/dd/yyyy'')'
        WHEN t.name LIKE 'numeric' THEN 'format(' + QUOTENAME(c.name) + ', ''0.######'')'
        WHEN t.name LIKE 'decimal' THEN 'format(' + QUOTENAME(c.name) + ', ''0.######'')'
        ELSE QUOTENAME(c.name)
      END
      + ' as VARCHAR(MAX)),'''') AS ' + QUOTENAME(c.NAME)
    FROM sys.columns c
    INNER JOIN sys.types t
      ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(@tableName)
    AND UPPER(c.name) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID', 'CREATED_DATE', 'CREATED_USER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')
  --PRINT @selectColumnsList

  -- get table column names for UNPIVOT statment
  SET @columnsList = STUFF((SELECT DISTINCT
      ',' + QUOTENAME(NAME)
    FROM sys.columns
    WHERE objecT_id = OBJECT_ID(@tableName)
    AND UPPER(NAME) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID', 'CREATED_DATE', 'CREATED_USER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')
  --PRINT @columnsList

  -- set sselect statment
  SET @selectStatment =
  ' SELECT column_name, value
  FROM (SELECT top 1 ' + @selectColumnsList + ' FROM  ' + @tableName + ' where id=' + CAST(@recordId AS VARCHAR(32)) + ') p
  UNPIVOT (value FOR column_name IN (' + @columnsList + ')) as unpvt'
  --PRINT @selectStatment

  --set merge statment
  SET @mergeStatment = '
MERGE ' + @defValuesManualTableName + ' AS t 
USING (' + @selectStatment + ') AS s 
ON (upper(t.column_name) = upper(s.column_name) and upper(t.table_name) = ''' + UPPER(@tableName) + ''' AND t.record_id = ' + CAST(@recordId AS VARCHAR(32)) + ')
WHEN MATCHED THEN 
  UPDATE SET t.value = case when s.value='''' then null else s.value end  ;';
  --PRINT @mergeStatment

  EXEC (@mergeStatment)

END
GO

--- restore Rail Filing view ---
ALTER VIEW dbo.v_Rail_Filing_Data
AS
SELECT
  f.BDP_FK AS id
 ,h.id AS Filing_header_id
 ,p.BDP_EM AS Manifest_id
 ,d.Importer AS Importer
 ,d.Entry_Port AS Port_code
 ,d.Master_Bill AS Bill_of_lading
 ,c.Container_Number AS Container_number
 ,p.ReferenceNumber1 AS Train_number
 ,ISNULL(c.Gross_Weight, 0) AS Gross_weight
 ,ISNULL(c.Gross_Weight_Unit, 0) AS Gross_weight_unit

FROM dbo.Rail_Filing_Headers h
INNER JOIN dbo.Rail_Filing_Details f
  ON h.id = f.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed p
  ON f.BDP_FK = p.BDP_PK
LEFT OUTER JOIN dbo.Rail_DeclarationTab d
  ON d.Filing_Headers_FK = h.id
LEFT OUTER JOIN dbo.Rail_ContainersTab c
  ON c.Filing_Headers_FK = h.id
    AND f.BDP_FK = c.BDP_FK
GO

--- restore Rail Report View --- 
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
 ,containers.Gross_Weight AS Rail_Packing_Gross_Weight
 ,containers.Gross_Weight_Unit AS Rail_Packing_Gross_Weight_Unit

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
 ,invlines.Gr_Weight AS Rail_GrossWeightSummary
 ,invlines.Gr_Weight_Unit AS Rail_GrossWeightSummaryUnit

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

FROM dbo.Rail_Filing_Headers headers
INNER JOIN dbo.Rail_Filing_Details detailes
  ON headers.id = detailes.Filing_Headers_FK
LEFT JOIN dbo.Rail_DeclarationTab declaration
  ON declaration.Filing_Headers_FK = headers.id
LEFT JOIN dbo.Rail_ContainersTab containers
  ON containers.Filing_Headers_FK = headers.id
    AND detailes.BDP_FK = containers.BDP_FK
LEFT JOIN dbo.Rail_InvoiceLines invlines
  ON invlines.Filing_Headers_FK = headers.id
LEFT JOIN dbo.Rail_InvoiceHeaders invheaders
  ON invheaders.Filing_Headers_FK = headers.id
    AND invheaders.id = invlines.InvoiceHeaders_FK
LEFT JOIN dbo.Rail_MISC misc
  ON misc.Filing_Headers_FK = headers.id
WHERE headers.MappingStatus = 2
GO

-- rail inbound grid view 
ALTER VIEW dbo.Rail_Inbound_Grid
AS
SELECT DISTINCT
  bd_parsed.BDP_PK BD_Parsed_Id
 ,bd_parsed.BDP_EM BD_Parsed_EDIMessage_Id
 ,filing_headers.id Filing_Headers_id
 ,bd_parsed.Importer BD_Parsed_Importer
 ,bd_parsed.Supplier BD_Parsed_Supplier
 ,bd_parsed.PortOfUnlading BD_Parsed_PortOfUnlading
 ,bd_parsed.Description1 BD_Parsed_Description1
 ,bd_parsed.BillofLading BD_Parsed_BillofLading
 ,bd_parsed.IssuerCode BD_Parsed_Issuer_Code
 ,CONCAT(bd_parsed.EquipmentInitial, bd_parsed.EquipmentNumber) AS BD_Parsed_Container_Number
 ,bd_parsed.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,bd_parsed.CreatedDate BD_Parsed_CreatedDate
 ,bd_parsed.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(bd_parsed.DuplicateOf, 0)) BD_Parsed_Is_Duplicated
 ,importer_supplier.Importer Rule_ImporterSupplier_Importer
 ,importer_supplier.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,rail_description.Tariff Rule_Desc1_Desc2_Tariff
 ,rail_port.Port Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.name Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.name Filing_Headers_FilingStatus_Title
 ,rail_description.Description
 ,filing_headers.ErrorDescription

FROM dbo.Rail_BD_Parsed bd_parsed
LEFT JOIN dbo.Rail_Rule_Port rail_port
  ON bd_parsed.PortOfUnlading = rail_port.Port
LEFT JOIN dbo.Rail_Rule_ImporterSupplier importer_supplier
  ON bd_parsed.Importer = importer_supplier.Importer_Name
    AND (bd_parsed.Supplier = importer_supplier.Supplier_Name
      OR (bd_parsed.Supplier IS NULL
        AND importer_supplier.Supplier_Name IS NULL))
LEFT JOIN dbo.Rail_Rule_Desc1_Desc2 rail_description
  ON rail_description.Description1 = bd_parsed.Description1
LEFT JOIN dbo.Rail_Filing_Details filing_details
  ON filing_details.BDP_FK = bd_parsed.BDP_PK
LEFT JOIN dbo.Rail_Filing_Headers filing_headers
  ON filing_headers.id = filing_details.Filing_Headers_FK
    AND filing_headers.MappingStatus <> 0
LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_headers.MappingStatus, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_headers.FilingStatus, 0) = filing_status.id
WHERE NOT EXISTS (SELECT
    *
  FROM dbo.Rail_Filing_Headers filing_headers
  INNER JOIN dbo.Rail_Filing_Details filing_details
    ON filing_headers.id = filing_details.Filing_Headers_FK
  WHERE filing_headers.MappingStatus > 0
  AND bd_parsed.BDP_PK = filing_details.BDP_FK)

UNION ALL

SELECT
  filing_details.BDP_FK BD_Parsed_Id
 ,bd_parsed.BDP_EM BD_Parsed_EDIMessage_Id
 ,filing_headers.id Filing_Headers_id
 ,bd_parsed.Importer BD_Parsed_Importer
 ,bd_parsed.Supplier BD_Parsed_Supplier
 ,declaration_tab.Entry_Port BD_Parsed_PortOfUnlading
 ,declaration_tab.Description BD_Parsed_Description1
 ,declaration_tab.Master_Bill BD_Parsed_BillofLading
 ,bd_parsed.IssuerCode BD_Parsed_Issuer_Code
 ,containers_tab.Container_Number AS BD_Parsed_Container_Number
 ,bd_parsed.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,bd_parsed.CreatedDate BD_Parsed_CreatedDate
 ,bd_parsed.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(bd_parsed.DuplicateOf, 0)) BD_Parsed_Is_Duplicated
 ,declaration_tab.Importer Rule_ImporterSupplier_Importer
 ,declaration_tab.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,invoice_lines.Tariff Rule_Desc1_Desc2_Tariff
 ,declaration_tab.Entry_Port Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.name Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.name Filing_Headers_FilingStatus_Title
 ,invoice_lines.Description
 ,filing_headers.ErrorDescription

FROM dbo.Rail_Filing_Headers filing_headers
INNER JOIN dbo.Rail_Filing_Details filing_details
  ON filing_headers.id = filing_details.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed bd_parsed
  ON filing_details.BDP_FK = bd_parsed.BDP_PK
LEFT JOIN dbo.MappingStatus mapping_status
  ON filing_headers.MappingStatus = mapping_status.id
LEFT JOIN dbo.FilingStatus filing_status
  ON filing_headers.FilingStatus = filing_status.id
LEFT JOIN dbo.Rail_DeclarationTab declaration_tab
  ON declaration_tab.Filing_Headers_FK = filing_headers.id
LEFT JOIN dbo.Rail_ContainersTab containers_tab
  ON containers_tab.Filing_Headers_FK = filing_headers.id
    AND filing_details.BDP_FK = containers_tab.BDP_FK
LEFT JOIN dbo.Rail_InvoiceLines invoice_lines
  ON invoice_lines.Filing_Headers_FK = filing_headers.id
WHERE filing_headers.MappingStatus > 0
GO
