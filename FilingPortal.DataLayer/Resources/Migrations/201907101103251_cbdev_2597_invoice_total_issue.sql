--- Rail ---
DELETE FROM Rail_DEFValues WHERE ColName = 'Invoice_Total' AND section_id=4

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
  19
 ,'Invoice Total'
 ,'Invoice Total'
 ,NULL
 ,'Invoice_Total'
 ,0
 ,0
 ,0
 ,0
 ,GETDATE()
 ,SUSER_NAME()
 ,NULL
 ,NULL
 ,NULL
 ,NULL
 ,4)
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

  RETURN @recordId
END;
GO

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
  AND v.FEditable = 1

  EXEC (@str);
END
GO

--- Truck ---
DELETE FROM truck_def_values WHERE column_name = 'Invoice_Total' AND section_id=4
INSERT dbo.truck_def_values (
  label
 ,default_value
 ,description
 ,section_id
 ,column_name
 ,editable
 ,mandatory
 ,has_default_value
 ,paired_field_table
 ,paired_field_column
 ,handbook_name
 ,display_on_ui
 ,manual
 ,single_filing_order
 ,created_date
 ,created_user)
VALUES (
  'Invoice Total'
 ,NULL
 ,'Invoice Total'
 ,4
 ,'Invoice_Total'
 ,0
 ,0
 ,0
 ,NULL
 ,NULL
 ,NULL
 ,50
 ,0
 ,0
 ,DEFAULT
 ,SUSER_NAME());
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

  -- add invoice line
  EXEC dbo.truck_add_invoice_line_record @filingHeaderId
                                        ,@recordId
                                        ,@filingUser

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

--- Truck apply values to result table---
ALTER PROCEDURE dbo.truck_filing_param (@Filing_Headers_id INT)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = ''

  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = ' +
    CASE
      WHEN v.[value] IS NULL THEN 'NULL '
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
  LEFT JOIN dbo.truck_def_values_manual v
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(v.table_name)
  WHERE i.table_schema = 'dbo'
  AND v.filing_header_id = @Filing_Headers_id
  AND v.editable = 1

  EXEC (@str);
END
GO

--- Pipeline ---
DELETE FROM Pipeline_DEFValues WHERE ColName = 'Invoice_Total' AND section_id=4
INSERT dbo.Pipeline_DEFValues (
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
  5
 ,'Invoice Total'
 ,'Invoice Total'
 ,NULL
 ,'Invoice_Total'
 ,0
 ,0
 ,0
 ,0
 ,DEFAULT
 ,DEFAULT
 ,0
 ,NULL
 ,NULL
 ,NULL
 ,4);
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

  -- add invoice line
  EXEC dbo.pipeline_add_invoice_line_record @filingHeaderId
                                           ,@recordId
                                           ,@filingUser

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

ALTER PROCEDURE dbo.pipeline_filing_param (@Filing_Headers_id INT)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = ''

  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = try_cast(''' + ISNULL(v.value, '') + ''' as ' +
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
    + ') ' + 'where id = ' + CAST(v.record_id AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM information_schema.columns i
  LEFT JOIN dbo.Pipeline_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(v.table_name)
  WHERE i.table_schema = 'dbo'
  AND v.Filing_Headers_FK = @Filing_Headers_id
  AND v.FEditable = 1

  EXEC (@str);
END
GO