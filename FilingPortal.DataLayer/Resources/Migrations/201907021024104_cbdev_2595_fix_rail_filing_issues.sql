--- update fields ---
ALTER TABLE dbo.Rail_InvoiceHeaders
DROP COLUMN Invoice_Total
GO

CREATE FUNCTION dbo.rail_invoice_total (@invoiceHeaderId INT)
RETURNS NUMERIC(18, 6) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result NUMERIC(18, 6) = NULL;

  SELECT
    @result = SUM(ril.Customs_QTY * ril.PriceUnit)
  FROM dbo.Rail_InvoiceLines ril
  WHERE ril.InvoiceHeaders_FK = @invoiceHeaderId
  GROUP BY ril.InvoiceHeaders_FK

  RETURN @result
END
GO

ALTER TABLE dbo.Rail_InvoiceHeaders
ADD Invoice_Total AS (dbo.rail_invoice_total(id))
GO

-- remove unnecessary value
DELETE rdf
  FROM dbo.Rail_DEFValues rdf
  INNER JOIN rail_sections rs
    ON rdf.section_id = rs.id
WHERE rdf.ColName = 'Invoice_Total'
  AND rs.table_name = 'Rail_InvoiceHeaders'
GO

DELETE FROM dbo.Rail_DEFValues_Manual
WHERE column_name = 'Invoice_Total'
  AND table_name = 'Rail_InvoiceHeaders'
GO

--- update Rail filing procedures ---
-- apply def values --
ALTER PROCEDURE dbo.rail_filing_param (@Filing_Headers_id INT)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = ''

  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = ' +
    CASE
      WHEN v.value IS NULL THEN +'NULL '
      ELSE 'try_cast(''' + ISNULL(v.value, '') + ''' as ' +
        data_type +
        CASE
          WHEN data_type IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
            CASE
              WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
              ELSE CONVERT(VARCHAR(4),
                CASE
                  WHEN data_type IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
                  ELSE CHARACTER_MAXIMUM_LENGTH
                END)
            END + ')'
          WHEN data_type IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
            + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
          ELSE ''
        END
        + ') '
    END
    + 'where id = ' + CAST(v.record_id AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM information_schema.columns i
  LEFT JOIN dbo.Rail_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(v.table_name)
  WHERE i.table_schema = 'dbo'
  AND v.Filing_Headers_FK = @Filing_Headers_id

  EXEC (@str);
END
GO

-- apply def values for specified table --
ALTER PROCEDURE dbo.rail_apply_def_values (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = ''
  SELECT
    @str = @str + 'update  ' + dfm.table_name + ' set ' + dfm.column_name + ' = ' +
    CASE
      WHEN dfm.[value] IS NULL THEN 'NULL '
      ELSE 'try_cast(''' + ISNULL(dfm.[value], '') + ''' as ' +
        i.DATA_TYPE +
        CASE
          WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
            CASE
              WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
              ELSE CONVERT(VARCHAR(4),
                CASE
                  WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
                  ELSE CHARACTER_MAXIMUM_LENGTH
                END)
            END + ')'
          WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
            + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
          ELSE ''
        END
        + ') '
    END
    + 'where id = ' + CAST(@recordId AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM dbo.Rail_DEFValues_Manual dfm
  LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(dfm.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(dfm.table_name)
  WHERE UPPER(dfm.table_name) = UPPER(@tableName)
  AND i.TABLE_SCHEMA = 'dbo'
  AND dfm.FHasDefaultVal = 1
  AND dfm.record_id = @recordId

  EXEC (@str);
END
GO

-- add containers tab record --
ALTER PROCEDURE dbo.rail_add_container_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

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
    WITH RichData
    AS
      (SELECT
          details.BDP_FK
         ,p.IssuerCode AS Bill_Issuer_SCAC
         ,p.BillofLading AS Bill_Num
         ,CONCAT('MB:', p.BillofLading) AS Bill_Number
         ,CONCAT(EquipmentInitial, EquipmentNumber) AS Container_Number
         ,details.FILING_HEADERS_FK
        FROM dbo.Rail_Filing_Details details
        INNER JOIN dbo.Rail_BD_Parsed p
          ON p.BDP_PK = details.BDP_FK
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Rail_ContainersTab (
        BDP_FK
       ,Bill_Issuer_SCAC
       ,Bill_Num
       ,Bill_Number
       ,Container_Number
       ,FILING_HEADERS_FK)
      SELECT
        BDP_FK
       ,Bill_Issuer_SCAC
       ,Bill_Num
       ,Bill_Number
       ,Container_Number
       ,FILING_HEADERS_FK
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Rail_ContainersTab pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

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

  RETURN @recordId
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
  DECLARE @recordId INT;

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
    WITH RichData
    AS
      (SELECT
          d.BDP_FK
         ,p.IssuerCode AS Carrier_SCAC
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
         ,p.BillofLading AS Master_Bill
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
        WHERE d.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Rail_DeclarationTab (
        BDP_FK
       ,Carrier_SCAC
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
       ,Master_Bill
       ,Origin
       ,FILING_HEADERS_FK)
      SELECT
        BDP_FK
       ,Carrier_SCAC
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
       ,Master_Bill
       ,Origin
       ,FILING_HEADERS_FK
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Rail_DeclarationTab pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

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

  RETURN @recordId
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
  DECLARE @recordId INT;

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
    WITH RichData
    AS
      (SELECT
          d.BDP_FK
         ,ISNULL(rn.Value_Recon, 'N/A') AS Recon_Issue
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
        WHERE d.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Rail_MISC (
        BDP_FK
       ,Recon_Issue
       ,FTA_Recon
       ,Payment_Type
       ,Broker_to_Pay
       ,Submitter
       ,Filing_Headers_FK)
      SELECT
        BDP_FK
       ,Recon_Issue
       ,FTA_Recon
       ,Payment_Type
       ,Broker_to_Pay
       ,Submitter
       ,Filing_Headers_FK
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Rail_MISC pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

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

  RETURN @recordId
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
  DECLARE @recordId INT;

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
    WITH RichData
    AS
      (SELECT
          d.BDP_FK
         ,@parentId AS InvoiceHeaders_FK
         ,rd.Attribute_1 AS Attribute_1
         ,rd.Attribute_2 AS Attribute_2
         ,p.Weight AS Gr_Weight
         ,p.WeightUnit AS Gr_Weight_Unit
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
        WHERE d.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Rail_InvoiceLines (
        BDP_FK
       ,InvoiceHeaders_FK
       ,Attribute_1
       ,Attribute_2
       ,Gr_Weight
       ,Gr_Weight_Unit
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
      SELECT
        BDP_FK
       ,InvoiceHeaders_FK
       ,Attribute_1
       ,Attribute_2
       ,Gr_Weight
       ,Gr_Weight_Unit
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
       ,FILING_HEADERS_FK
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Rail_InvoiceLines pil
    WHERE pil.Filing_Headers_FK = @parentId
  END

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

  RETURN @recordId
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
  DECLARE @recordId INT;

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
    WITH RichData
    AS
      (SELECT DISTINCT
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
        WHERE d.Filing_Headers_FK = @filingHeaderId)

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
      SELECT
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
       ,FILING_HEADERS_FK
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Rail_InvoiceHeaders pih
    WHERE pih.Filing_Headers_FK = @parentId
  END

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

  -- add invoice line
  EXEC dbo.rail_add_invoice_line_record @filingHeaderId
                                       ,@recordId
                                       ,@filingUser
  RETURN @recordId
END;
GO


--- update Pipeline filing procedures ---
-- apply def values for specified table --
ALTER PROCEDURE dbo.pipeline_apply_def_values (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = ''
  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = try_cast(''' + ISNULL(v.[value], '') + ''' as ' +
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
    + ') ' + 'where id = ' + CAST(@recordId AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM dbo.Pipeline_DEFValues_Manual v  
  LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
  WHERE UPPER(v.table_name) = UPPER(@tableName)
  AND i.TABLE_SCHEMA = 'dbo'
  AND v.FHasDefaultVal = 1

  EXEC (@str);
END
GO

-- add containers tab record --
ALTER PROCEDURE dbo.pipeline_add_container_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

  --Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add container data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_ContainersTab pct
      WHERE pct.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK
         ,details.Pipeline_Inbounds_FK AS PI_FK
         ,ROUND(inbound.Quantity, 0) AS Manifest_QTY
         ,REPLACE(inbound.TicketNumber, '-', '') AS Bill_Num
         ,rulePorts.Issuer AS Bill_Issuer_SCAC
        FROM Pipeline_Filing_Details details
        INNER JOIN Pipeline_Inbound inbound
          ON details.Pipeline_Inbounds_FK = inbound.id
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        LEFT JOIN Pipeline_Rule_PortImporter rulePorts
          ON ( --we have both tofacility-port and port-importer rule associated with record
              (
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND RTRIM(LTRIM(ruleFacility.port)) = RTRIM(LTRIM(rulePorts.port))
              )
              OR (-- we have tofacility-port rule but no port-importer rule associated with record 
                RTRIM(LTRIM(rulePorts.port)) = RTRIM(LTRIM(ruleFacility.port))
                AND (LOWER(RTRIM(LTRIM(rulePorts.Importer))) = LOWER('N/A')
                  AND @ImporterCode NOT IN (SELECT
                      Importer
                    FROM Pipeline_Rule_PortImporter
                    WHERE port = ruleFacility.port)

                )
              )
              OR (--we dont have tofacility-port rule but port-importer rule exist for the record 
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND ISNULL(RTRIM(LTRIM(ruleFacility.port)), '') = ''
              )
            )
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Pipeline_ContainersTab (
        Filing_Headers_FK
       ,PI_FK
       ,Manifest_QTY
       ,Bill_Num
       ,Bill_Issuer_SCAC)
      SELECT
        Filing_Headers_FK
       ,PI_FK
       ,Manifest_QTY
       ,Bill_Num
       ,Bill_Issuer_SCAC
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_ContainersTab pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.pipeline_add_def_values_manual @tableName
                                         ,@filingHeaderId
                                         ,@parentId
                                         ,@recordId

  -- apply default values
  EXEC dbo.pipeline_apply_def_values @tableName
                                    ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

-- add declaration tab record --
ALTER PROCEDURE dbo.pipeline_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

  --Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_DeclarationTab pdt
      WHERE pdt.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK AS Filing_Headers_FK
         ,details.Pipeline_Inbounds_FK AS PI_FK
         ,ruleImporters.Supplier AS Main_Supplier
         ,@ImporterCode AS Importer
         ,rulePorts.Issuer AS Issuer
         ,REPLACE(inbound.TicketNumber, '-', '') AS Batch_Ticket
         ,rulePorts.Pipeline AS Pipeline
         ,rulePorts.Issuer AS Carrier_SCAC
         ,rulePorts.port AS Discharge
         ,rulePorts.port AS Entry_Port
         ,inbound.ExportDate AS Dep
         ,inbound.ImportDate AS Arr
         ,inbound.ImportDate AS Arr_2
         ,rulePorts.Origin AS Origin
         ,rulePorts.Destination AS Destination
         ,rulePorts.Destination_State AS Destination_State
         ,inbound.ImportDate AS ETA
         ,inbound.ImportDate AS Export_Date
         ,CONCAT(rulePorts.description, ': ', inbound.Batch) AS Description
         ,inbound.TicketNumber AS Owner_Ref
         ,rulePorts.FIRMs_Code AS FIRMs_Code
         ,REPLACE(inbound.TicketNumber, '-', '') AS Master_Bill
        FROM Pipeline_Filing_Details details
        INNER JOIN Pipeline_Inbound inbound
          ON details.Pipeline_Inbounds_FK = inbound.id
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        LEFT JOIN Pipeline_Rule_PortImporter rulePorts
          ON (
              (--we have both tofacility-port and port-importer rule associated with record
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND RTRIM(LTRIM(ruleFacility.port)) = RTRIM(LTRIM(rulePorts.port))
              )
              OR (-- we have tofacility-port rule but no port-importer rule associated with record 
                RTRIM(LTRIM(rulePorts.port)) = RTRIM(LTRIM(ruleFacility.port))
                AND (
                  LOWER(RTRIM(LTRIM(rulePorts.Importer))) = LOWER('N/A')
                  AND @ImporterCode NOT IN (SELECT
                      Importer
                    FROM Pipeline_Rule_PortImporter
                    WHERE port = ruleFacility.port)
                )
              )
              OR (--we dont have tofacility-port rule but port-importer rule exist for the record 
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND ISNULL(RTRIM(LTRIM(ruleFacility.port)), '') = ''
              )
            )
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Pipeline_DeclarationTab (
        Filing_Headers_FK
       ,PI_FK
       ,Main_Supplier
       ,Importer
       ,Issuer
       ,Batch_Ticket
       ,Pipeline
       ,Carrier_SCAC
       ,Discharge
       ,Entry_Port
       ,Dep
       ,Arr
       ,Arr_2
       ,Origin
       ,Destination
       ,Destination_State
       ,ETA
       ,Export_Date
       ,Description
       ,Owner_Ref
       ,FIRMs_Code
       ,Master_Bill
       ,Importer_of_record)
      SELECT
        Filing_Headers_FK
       ,PI_FK
       ,Main_Supplier
       ,Importer
       ,Issuer
       ,Batch_Ticket
       ,Pipeline
       ,Carrier_SCAC
       ,Discharge
       ,Entry_Port
       ,Dep
       ,Arr
       ,Arr_2
       ,Origin
       ,Destination
       ,Destination_State
       ,ETA
       ,Export_Date
       ,Description
       ,Owner_Ref
       ,FIRMs_Code
       ,Master_Bill
       ,@ImporterCode
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_DeclarationTab pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.pipeline_add_def_values_manual @tableName
                                         ,@filingHeaderId
                                         ,@parentId
                                         ,@recordId

  -- apply default values
  EXEC dbo.pipeline_apply_def_values @tableName
                                    ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

-- add misc record --
ALTER PROCEDURE dbo.pipeline_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

  -- get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)


  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add misc data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_MISC pm
      WHERE pm.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Pipeline_Inbounds_FK AS PI_FK
         ,details.Filing_Headers_FK
         ,userData.Branch
         ,userData.Broker
         ,userData.Location
         ,ruleImporters.recon_issue
         ,ruleImporters.fta_recon
         ,ruleImporters.payment_type
         ,ruleImporters.broker_to_pay
        FROM dbo.Pipeline_Filing_Details details
        INNER JOIN dbo.Pipeline_Inbound inbound
          ON inbound.id = details.Pipeline_Inbounds_FK
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        LEFT JOIN app_users_data userData
          ON userData.UserAccount = @filingUser
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Pipeline_MISC (
        Filing_Headers_FK
       ,PI_FK
       ,Branch
       ,Broker
       ,Preparer_Dist_Port
       ,recon_issue
       ,fta_recon
       ,payment_type
       ,broker_to_pay)
      SELECT
        Filing_Headers_FK
       ,PI_FK
       ,Branch
       ,Broker
       ,Location
       ,recon_issue
       ,fta_recon
       ,payment_type
       ,broker_to_pay
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_MISC pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.pipeline_add_def_values_manual @tableName
                                         ,@filingHeaderId
                                         ,@parentId
                                         ,@recordId

  -- apply default values
  EXEC dbo.pipeline_apply_def_values @tableName
                                    ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

-- add invoice line record --
ALTER PROCEDURE dbo.pipeline_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

  -- Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- get current tariff ---------------
  DECLARE @tariffs TABLE (
    pi INT NOT NULL
   ,tariff INT NOT NULL
  )

  INSERT INTO @tariffs (
      pi
     ,tariff)
    SELECT
      details.Pipeline_Inbounds_FK
     ,TariffId =
      CASE
        WHEN pi.API < 25 THEN 1
        WHEN pi.API >= 25 THEN 2
      END
    FROM Pipeline_Filing_Details details
    INNER JOIN Pipeline_Inbound pi
      ON details.Pipeline_Inbounds_FK = pi.id
    WHERE details.Filing_Headers_FK = @filingHeaderId

  -- add invoice line data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_InvoiceLines pil
      WHERE pil.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK
         ,@parentId AS InvoiceHeaders_FK
         ,details.Pipeline_Inbounds_FK AS PI_FK
         ,inbound.Batch AS Invoice_No -- ?? is it ok?
         ,ruleImporters.Transaction_Related AS Transaction_Related
         ,ruleAPI.Tariff AS Tariff
         ,inbound.Quantity AS Customs_QTY
         ,CONCAT(ruleBatch.product, ' - ', ruleBatch.batch_code) AS Goods_Description
         ,ruleImporters.SPI AS SPI
         ,CONVERT(DECIMAL(18, 3), dbo.fn_pipeline_weight(inbound.Quantity, inbound.API)) AS Gr_weight
         ,ruleImporters.value AS PriceUnit
         ,inbound.Batch AS Attribute_1
         ,'API @ 60° F = ' + CONVERT(VARCHAR(128), inbound.API) AS Attribute_2
         ,rulePorts.customs_attribute3 AS Attribute_3
         ,inbound.Quantity AS Invoice_Qty
         ,ruleImporters.origin AS ORG
         ,inbound.Quantity * ruleImporters.value AS Line_Price
         ,inbound.Quantity * ruleImporters.freight AS Amount
         ,ruleImporters.supplier AS Manufacturer
         ,ruleImporters.Consignee AS Consignee
         ,ruleImporters.Consignee AS Sold_To_Party
         ,ruleImporters.origin AS Origin
         ,ruleImporters.country_of_export AS Export
         ,rulePorts.Destination_State AS Destination_State
        FROM dbo.Pipeline_Filing_Details details
        INNER JOIN dbo.Pipeline_Inbound inbound
          ON inbound.id = details.Pipeline_Inbounds_FK
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        LEFT JOIN Pipeline_Rule_PortImporter rulePorts
          ON (
              --we have both tofacility-port and port-importer rule associated with record
              (
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND RTRIM(LTRIM(ruleFacility.port)) = RTRIM(LTRIM(rulePorts.port))
              )
              OR (-- we have tofacility-port rule but no port-importer rule associated with record 
                RTRIM(LTRIM(rulePorts.port)) = RTRIM(LTRIM(ruleFacility.port))
                AND (LOWER(RTRIM(LTRIM(rulePorts.Importer))) = LOWER('N/A')
                  AND @ImporterCode NOT IN (SELECT
                      Importer
                    FROM Pipeline_Rule_PortImporter
                    WHERE port = ruleFacility.port)
                )
              )
              OR (--we dont have tofacility-port rule but port-importer rule exist for the record
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND ISNULL(RTRIM(LTRIM(ruleFacility.port)), '') = ''
              )
            )
        LEFT JOIN Pipeline_Rule_BatchCode ruleBatch
          ON dbo.extractBatchCode(inbound.Batch) = ruleBatch.batch_code
        LEFT JOIN @tariffs t
          ON inbound.id = t.pi
        LEFT JOIN Pipeline_Rule_API ruleAPI
          ON t.Tariff = ruleAPI.id
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Pipeline_InvoiceLines (
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,PI_FK
       ,Invoice_No
       ,Transaction_Related
       ,tariff
       ,Customs_QTY
       ,Goods_Description
       ,spi
       ,Gr_Weight
       ,PriceUnit
       ,Attribute_1
       ,Attribute_2
       ,Attribute_3
       ,Invoice_Qty
       ,ORG
       ,Line_Price
       ,Amount
       ,Manufacturer
       ,Consignee
       ,Sold_to_party
       ,Origin
       ,Export
       ,Dest_State)
      SELECT
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,PI_FK
       ,Invoice_No
       ,Transaction_Related
       ,Tariff
       ,Customs_QTY
       ,Goods_Description
       ,SPI
       ,Gr_weight
       ,PriceUnit
       ,Attribute_1
       ,Attribute_2
       ,Attribute_3
       ,Invoice_Qty
       ,ORG
       ,Line_Price
       ,Amount
       ,Manufacturer
       ,Consignee
       ,Sold_To_Party
       ,Origin
       ,Export
       ,Destination_State
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_InvoiceLines pil
    WHERE pil.Filing_Headers_FK = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.pipeline_add_def_values_manual @tableName
                                         ,@filingHeaderId
                                         ,@parentId
                                         ,@recordId

  -- apply default values
  EXEC dbo.pipeline_apply_def_values @tableName
                                    ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

-- add invoice header record --
ALTER PROCEDURE dbo.pipeline_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_InvoiceHeaders'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

  --Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add invoice header data and apply rules
  -- inbound.Quantity * ruleImporters.value - invLines.PriceUnit AS Invoice_Total -- replace with computed field
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_InvoiceHeaders pih
      WHERE pih.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT DISTINCT
          details.Filing_Headers_FK AS Filing_Headers_FK
         ,inbound.Batch AS Invoice_No
         ,ruleImporters.supplier AS supplier
         ,inbound.Quantity * ruleImporters.value AS Invoice_Total
         ,ruleImporters.Origin AS Origin
         ,ruleImporters.Consignee AS Consignee
         ,ruleImporters.transaction_related AS transaction_related
         ,ruleImporters.supplier AS Manufacturer
         ,ruleImporters.supplier AS Seller
         ,@ImporterCode AS Importer
         ,ruleImporters.Consignee AS Sold_To_Party
         ,ruleImporters.Consignee AS Ship_To_Party
         ,ruleImporters.seller AS Seller_Address
         ,ruleImporters.country_of_export AS Export
         ,ruleImporters.mid AS Manufacturer_Address
        FROM dbo.Pipeline_Filing_Details details
        INNER JOIN dbo.Pipeline_Inbound inbound
          ON inbound.Id = details.Pipeline_Inbounds_FK
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Pipeline_InvoiceHeaders (
        Filing_Headers_FK
       ,Invoice_No
       ,Supplier
       ,Invoice_Total
       ,Origin
       ,Consignee
       ,Transaction_Related
       ,Manufacturer
       ,Seller
       ,Importer
       ,Sold_to_party
       ,Ship_to_party
       ,Supplier_Address
       ,Export
       ,Manufacturer_Address)
      SELECT
        Filing_Headers_FK
       ,Invoice_No
       ,supplier
       ,Invoice_Total
       ,Origin
       ,Consignee
       ,transaction_related
       ,Manufacturer
       ,Seller
       ,Importer
       ,Sold_To_Party
       ,Ship_To_Party
       ,Seller_Address
       ,Export
       ,Manufacturer_Address
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_InvoiceHeaders pih
    WHERE pih.Filing_Headers_FK = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.pipeline_add_def_values_manual @tableName
                                         ,@filingHeaderId
                                         ,@parentId
                                         ,@recordId

  -- apply default values
  EXEC dbo.pipeline_apply_def_values @tableName
                                    ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  -- add invoice line
  EXEC dbo.pipeline_add_invoice_line_record @filingHeaderId
                                           ,@recordId
                                           ,@filingUser
  RETURN @recordId
END;
GO

--- update Truck filing procedures ---
-- apply def values for specified table --
ALTER PROCEDURE dbo.truck_apply_def_values (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = ''
  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = try_cast(''' + ISNULL(v.[value], '') + ''' as ' +
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
    + ') ' + 'where id = ' + CAST(@recordId AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM dbo.truck_def_values_manual v  
  LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
  WHERE UPPER(v.table_name) = UPPER(@tableName)
  AND i.TABLE_SCHEMA = 'dbo'
  AND v.has_default_value = 1

  EXEC (@str);
END
GO

-- add containers tab record --
ALTER PROCEDURE dbo.truck_add_container_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add сontainersTab data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Truck_ContainersTab сontainersTab
      WHERE сontainersTab.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.BDP_FK AS TI_FK
         ,details.FILING_HEADERS_FK AS Filing_Headers_FK
        FROM Truck_Filing_Details details
        INNER JOIN Truck_Inbound inbound
          ON details.BDP_FK = inbound.id
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    INSERT INTO dbo.Truck_ContainersTab (
        Filing_Headers_FK
       ,TI_FK)
      SELECT Filing_Headers_FK, TI_FK FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Truck_ContainersTab сontainersTab
    WHERE сontainersTab.Filing_Headers_FK = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.truck_add_def_values_manual @tableName
                                      ,@filingHeaderId
                                      ,@parentId
                                      ,@recordId

  -- apply default values
  EXEC dbo.truck_apply_def_values @tableName
                                 ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

-- add declaration tab record --
ALTER PROCEDURE dbo.truck_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add declarationTab data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Truck_DeclarationTab declarationTab
      WHERE declarationTab.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK AS Filing_Headers_FK
         ,details.BDP_FK AS TI_FK
         ,ruleImporters.cw_supplier AS Main_Supplier
         ,ruleImporters.cw_ior AS Importer
         ,SUBSTRING(inbound.PAPs, 1, 4) AS Issuer
         ,SUBSTRING(inbound.PAPs, 5, LEN(inbound.PAPs)) AS Master_Bill
         ,SUBSTRING(inbound.PAPs, 1, 4) AS Carrier_SCAC
         ,ruleImporters.arrival_port AS Discharge
         ,ruleImporters.Entry_Port AS Entry_Port
         ,ruleImporters.Destination_State AS Destination_State
         ,ruleImporters.Goods_Description AS Description
         ,rulePorts.FIRMs_Code
        FROM Truck_Filing_Details details
        INNER JOIN Truck_Inbound inbound
          ON details.BDP_FK = inbound.id
        LEFT JOIN Truck_Rule_Importers ruleImporters
          ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
        LEFT JOIN Truck_Rule_Ports rulePorts
          ON (RTRIM(LTRIM(ruleImporters.arrival_port)) = RTRIM(LTRIM(rulePorts.arrival_port))
              AND RTRIM(LTRIM(ruleImporters.Entry_Port)) = RTRIM(LTRIM(rulePorts.Entry_Port)))
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    INSERT INTO dbo.Truck_DeclarationTab (
        Filing_Headers_FK
       ,TI_FK
       ,Main_Supplier
       ,Importer
       ,Issuer
       ,Master_Bill
       ,Carrier_SCAC
       ,Discharge
       ,Entry_Port
       ,Destination_State
       ,Description
       ,FIRMs_Code)
      SELECT
        Filing_Headers_FK
       ,TI_FK
       ,Main_Supplier
       ,Importer
       ,Issuer
       ,Master_Bill
       ,Carrier_SCAC
       ,Discharge
       ,Entry_Port
       ,Destination_State
       ,Description
       ,firms_code
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Truck_DeclarationTab declarationTab
    WHERE declarationTab.Filing_Headers_FK = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.truck_add_def_values_manual @tableName
                                      ,@filingHeaderId
                                      ,@parentId
                                      ,@recordId

  -- apply default values
  EXEC dbo.truck_apply_def_values @tableName
                                 ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

-- add misc record --
ALTER PROCEDURE dbo.truck_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add misc data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Truck_MISC misc
      WHERE misc.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK
         ,details.BDP_FK AS TI_FK
         ,userData.Branch
         ,userData.Broker
         ,userData.Location
         ,ruleImporters.Recon_Issue
         ,ruleImporters.nafta_recon
        FROM dbo.Truck_Filing_Details details
        INNER JOIN dbo.Truck_Inbound inbound
          ON inbound.id = details.BDP_FK
        LEFT JOIN Truck_Rule_Importers ruleImporters
          ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
        LEFT JOIN app_users_data userData
          ON userData.UserAccount = @FilingUser
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    INSERT INTO dbo.Truck_MISC (
        Filing_Headers_FK
       ,TI_FK
       ,Branch
       ,Broker
       ,Preparer_Dist_Port
       ,Recon_Issue
       ,FTA_Recon)
      SELECT
        Filing_Headers_FK
       ,TI_FK
       ,Branch
       ,Broker
       ,Location
       ,recon_issue
       ,nafta_recon
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Truck_MISC misc
    WHERE misc.Filing_Headers_FK = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.truck_add_def_values_manual @tableName
                                      ,@filingHeaderId
                                      ,@parentId
                                      ,@recordId

  -- apply default values
  EXEC dbo.truck_apply_def_values @tableName
                                 ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

-- add invoice line record --
ALTER PROCEDURE dbo.truck_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add Invoice Lines data and apply rules  
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Truck_InvoiceLines invoiceLines
      WHERE invoiceLines.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK
         ,@parentId AS InvoiceHeaders_FK
         ,details.BDP_FK AS TI_FK
         ,inbound.PAPs AS Invoice_No
         ,ruleImporters.transactions_related AS Transaction_Related
         ,ruleImporters.Tariff AS Tariff
         ,ruleImporters.custom_quantity AS Customs_QTY
         ,ruleImporters.Goods_Description AS Goods_Description
         ,ruleImporters.SPI AS SPI
         ,ruleImporters.co AS ORG
         ,ruleimporters.ce AS Export
         ,ruleImporters.Gross_Weight AS Gr_Weight
         ,ruleImporters.custom_uq AS UQ
         ,ruleImporters.Line_Price AS PriceUnit
         ,ruleImporters.product_id AS Prod_ID_1
         ,ruleImporters.custom_attrib1 AS Attribute_1
         ,ruleImporters.custom_attrib2 AS Attribute_2
         ,ruleImporters.Invoice_Qty AS Invoice_Qty
         ,ruleImporters.invoice_uq AS Invoice_Qty_Unit
         ,ruleImporters.charges AS Amount
         ,ruleImporters.invoice_qty * ruleImporters.line_price AS Line_Price
        FROM dbo.Truck_Filing_Details details
        INNER JOIN dbo.Truck_Inbound inbound
          ON inbound.id = details.BDP_FK
        LEFT JOIN Truck_Rule_Importers ruleImporters
          ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    INSERT INTO dbo.Truck_InvoiceLines (
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,TI_FK
       ,Invoice_No
       ,Transaction_Related
       ,Tariff
       ,Customs_QTY
       ,Goods_Description
       ,SPI
       ,ORG
       ,Export
       ,Gr_Weight
       ,UQ
       ,PriceUnit
       ,Prod_ID_1
       ,Attribute_1
       ,Attribute_2
       ,Invoice_Qty
       ,Invoice_Qty_Unit
       ,Amount
       ,Line_Price)
      SELECT
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,TI_FK
       ,Invoice_No
       ,Transaction_Related
       ,Tariff
       ,Customs_QTY
       ,Goods_Description
       ,SPI
       ,ORG
       ,Export
       ,Gr_Weight
       ,UQ
       ,PriceUnit
       ,Prod_ID_1
       ,Attribute_1
       ,Attribute_2
       ,Invoice_Qty
       ,Invoice_Qty_Unit
       ,Amount
       ,Line_Price
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Truck_InvoiceLines invoiceLines
    WHERE invoiceLines.Filing_Headers_FK = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.truck_add_def_values_manual @tableName
                                      ,@filingHeaderId
                                      ,@parentId
                                      ,@recordId

  -- apply default values
  EXEC dbo.truck_apply_def_values @tableName
                                 ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

-- add invoice header record --
ALTER PROCEDURE dbo.truck_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_InvoiceHeaders'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add invoiceHeaders data and apply rules
  -- invLines.Invoice_Qty * invLines.PriceUnit AS Invoice_Total -- replace with computed field
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Truck_InvoiceHeaders invoiceHeaders
      WHERE invoiceHeaders.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK AS Filing_Headers_FK
         ,inbound.PAPs AS Invoice_No
         ,ruleImporters.cw_supplier AS Supplier
         ,ruleImporters.cw_ior AS Consignee
         ,ruleImporters.transactions_related AS Transaction_Related
         ,ruleImporters.cw_supplier AS Manufacturer
         ,ruleImporters.cw_supplier AS Seller
         ,ruleImporters.cw_ior AS Importer
         ,ruleImporters.cw_ior AS Sold_to_party
         ,ruleImporters.cw_ior AS Ship_to_party
         ,ruleimporters.ce AS Export
        FROM dbo.Truck_Filing_Details details
        INNER JOIN dbo.Truck_Inbound inbound
          ON inbound.id = details.BDP_FK
        LEFT JOIN Truck_Rule_Importers ruleImporters
          ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    INSERT INTO dbo.Truck_InvoiceHeaders (
        Filing_Headers_FK
       ,Invoice_No
       ,Supplier
       ,Consignee
       ,Transaction_Related
       ,Manufacturer
       ,Seller
       ,Importer
       ,Sold_to_party
       ,Ship_to_party
       ,Export)
      SELECT
        Filing_Headers_FK
       ,Invoice_No
       ,Supplier
       ,Consignee
       ,Transaction_Related
       ,Manufacturer
       ,Seller
       ,Importer
       ,Sold_to_party
       ,Ship_to_party
       ,Export
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Truck_InvoiceHeaders invoiceHeaders
    WHERE invoiceHeaders.Filing_Headers_FK = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.truck_add_def_values_manual @tableName
                                      ,@filingHeaderId
                                      ,@parentId
                                      ,@recordId

  -- apply default values
  EXEC dbo.truck_apply_def_values @tableName
                                 ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                   ,@tableName
                                   ,@recordId
  -- add invoice line
  EXEC dbo.truck_add_invoice_line_record @filingHeaderId
                                        ,@recordId
                                        ,@filingUser

  RETURN @recordId
END;
GO

--- update Truck Export filing procedures ---
-- apply def values for specified table --
ALTER PROCEDURE dbo.truck_export_apply_def_values (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = ''
  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = try_cast(''' + ISNULL(v.[value], '') + ''' as ' +
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
    + ') ' + 'where id = ' + CAST(@recordId AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM dbo.truck_export_def_values_manual v  
  LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
  WHERE UPPER(v.table_name) = UPPER(@tableName)
  AND i.TABLE_SCHEMA = 'dbo'
  AND v.has_default_value = 1

  EXEC (@str);
END
GO

-- add declaration tab record --
ALTER PROCEDURE dbo.truck_export_add_declaration_record (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.truck_export_declarations ted
      WHERE ted.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          te.routed_tran
         ,te.eccn
         ,te.export
         ,te.importer
         ,te.tariff_type
         ,te.sold_en_route
         ,te.master_bill
         ,te.origin
         ,terc.destination
         ,terc.country
         ,terec.tran_related
         ,te.exporter
         ,te.export_date
         ,te.hazardous
         ,rulePort.origin_code
         ,rulePort.state_of_origin
        FROM truck_export_filing_details tefd
        JOIN truck_exports te
          ON tefd.truck_export_id = te.id
        LEFT JOIN Truck_Export_Rule_Consignee terc
          ON te.importer = terc.consignee_code
        LEFT JOIN Truck_Export_Rule_Exporter_Consignee terec
          ON te.importer = terec.consignee_code
            AND te.exporter = terec.exporter
        LEFT JOIN truck_export_rule_port rulePort
          ON (RTRIM(LTRIM(te.origin)) = RTRIM(LTRIM(rulePort.port)))
        WHERE tefd.filing_header_id = @filingHeaderId)
    INSERT INTO dbo.truck_export_declarations (
        filing_header_id
       ,destination
       ,country_of_dest
       ,tran_related
       ,routed_tran
       ,eccn
       ,export
       ,importer
       ,tariff_type
       ,sold_en_route
       ,master_bill
       ,owner_ref
       ,port_of_loading
       ,transport_ref
       ,main_supplier
       ,dep
       ,exp_date
       ,hazardous
       ,origin
       ,state_of_origin)
      SELECT
        @parentId
       ,destination
       ,country
       ,tran_related
       ,routed_tran
       ,eccn
       ,export
       ,importer
       ,tariff_type
       ,sold_en_route
       ,master_bill
       ,master_bill
       ,origin
       ,master_bill
       ,exporter
       ,export_date
       ,export_date
       ,hazardous
       ,origin_code
       ,state_of_origin
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM truck_export_declarations ted
    WHERE ted.filing_header_id = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.truck_export_add_def_values_manual @tableName
                                             ,@filingHeaderId
                                             ,@parentId
                                             ,@recordId

  -- apply default values
  EXEC dbo.truck_export_apply_def_values @tableName
                                        ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

-- add invoice line record --
ALTER PROCEDURE dbo.truck_export_add_invoice_line_record (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_invoice_lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT

  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add invoice line data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.truck_export_invoice_lines t
      WHERE t.invoice_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          te.tariff
         ,te.customs_qty
         ,te.price
         ,te.gross_weight
         ,te.gross_weight_uom
         ,te.goods_description
         ,te.tariff_type
         ,[dbo].[fn_getUnitByTariff](te.tariff, te.tariff_type) AS invoice_qty_unit
        FROM truck_export_filing_details tefd
        JOIN truck_exports te
          ON tefd.truck_export_id = te.id
        WHERE tefd.filing_header_id = @filingHeaderId)
    INSERT INTO dbo.truck_export_invoice_lines (
        invoice_header_id
       ,tariff
       ,customs_qty
       ,price
       ,gross_weight
       ,gross_weight_unit
       ,goods_description
       ,tariff_type
       ,invoice_qty_unit)
      SELECT
        @parentId
       ,tariff
       ,customs_qty
       ,price
       ,gross_weight
       ,gross_weight_uom
       ,goods_description
       ,tariff_type
       ,invoice_qty_unit
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM truck_export_invoice_lines t
    WHERE t.invoice_header_id = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.truck_export_add_def_values_manual @tableName
                                             ,@filingHeaderId
                                             ,@parentId
                                             ,@recordId

  -- apply default values
  EXEC dbo.truck_export_apply_def_values @tableName
                                        ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

-- add invoice header record --
ALTER PROCEDURE dbo.truck_export_add_invoice_header_record (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_invoice_headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT

  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.truck_export_invoice_headers t
      WHERE t.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          terec.address
         ,terec.contact
         ,terec.phone
         ,terc.ultimate_consignee_type
         ,te.exporter
         ,te.price
         ,te.importer
        FROM truck_export_filing_details tefd
        JOIN truck_exports te
          ON tefd.truck_export_id = te.id
        LEFT JOIN Truck_Export_Rule_Consignee terc
          ON te.importer = terc.consignee_code
        LEFT JOIN Truck_Export_Rule_Exporter_Consignee terec
          ON te.importer = terec.consignee_code
            AND te.exporter = terec.exporter
        WHERE tefd.filing_header_id = @filingHeaderId)
    INSERT INTO dbo.truck_export_invoice_headers (
        filing_header_id
       ,usppi_address
       ,usppi_contact
       ,usppi_phone
       ,ultimate_consignee_type
       ,usppi
       ,invoice_total_amount
       ,ultimate_consignee)
      SELECT
        @parentId
       ,address
       ,contact
       ,phone
       ,ultimate_consignee_type
       ,exporter
       ,price
       ,importer
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM truck_export_invoice_headers t
    WHERE t.filing_header_id = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.truck_export_add_def_values_manual @tableName
                                             ,@filingHeaderId
                                             ,@parentId
                                             ,@recordId

  -- apply default values
  EXEC dbo.truck_export_apply_def_values @tableName
                                        ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  -- add invoice line
  EXEC dbo.truck_export_add_invoice_line_record @filingHeaderId
                                               ,@recordId

  RETURN @recordId
END
GO