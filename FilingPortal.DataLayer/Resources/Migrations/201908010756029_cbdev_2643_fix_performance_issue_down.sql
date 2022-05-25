-- Add Truck Export Declaration Tab record --
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

-- Add Truck Export Invoice Header record --
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

-- Add Truck Export Invoice Line record --
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

-- Add Truck Export def values manual records --
ALTER PROCEDURE dbo.truck_export_add_def_values_manual (@tableName VARCHAR(128)
, @filingHeaderId INT
, @parentId INT
, @recordId INT)
AS
BEGIN
  DECLARE @str_val NVARCHAR(500)
  DECLARE @parmDefinition NVARCHAR(500)
  DECLARE @defValueOut VARCHAR(128)
  DECLARE @dataType VARCHAR(128)
  DECLARE @defValue VARCHAR(500)
  DECLARE @id INT;
  DECLARE @columnName VARCHAR(128)

  DECLARE c CURSOR FOR SELECT DISTINCT
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
   ,v.default_value
   ,v.id
   ,v.column_name
  FROM dbo.truck_export_def_values v
  INNER JOIN dbo.truck_export_sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.manual > 0
  OR v.has_default_value > 0
  OR v.display_on_ui > 0)
  ORDER BY id

  OPEN c
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  WHILE @@fetch_status = 0
  BEGIN
  SET @defValueOut = NULL;
  SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(''' + @defValue + ''' as ' + @dataType + ') as varchar(500))';
  SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@parmDefinition
                       ,@defValueOut = @defValueOut OUTPUT;
  IF @defValueOut IS NULL
  BEGIN
    SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(' + @defValue + ' as ' + @dataType + ') as varchar(500))';
    SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';

    DECLARE @result FLOAT
  BEGIN TRY
    EXECUTE sp_executesql @str_val
                         ,@parmDefinition
                         ,@defValueOut = @defValueOut OUTPUT;
  END TRY
  BEGIN CATCH
    SET @defValueOut = NULL;
  END CATCH;
  END;

  IF @dataType LIKE 'date%'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS DATETIME), 'MM/dd/yyyy');
  END;
  IF @dataType LIKE 'numeric'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS FLOAT), '0.######');
  END;

  IF NOT EXISTS (SELECT
        *
      FROM dbo.truck_export_def_values_manual tedvm
      WHERE tedvm.record_id = @recordId
      AND tedvm.table_name = @tableName
      AND tedvm.column_name = @columnName)
  BEGIN
    INSERT INTO dbo.truck_export_def_values_manual (
        filing_header_id
       ,parent_record_id
       ,section_name
       ,section_title
       ,record_id
       ,column_name
       ,table_name
       ,modification_date
       ,value
       ,editable
       ,display_on_ui
       ,has_default_value
       ,mandatory
       ,manual
       ,description
       ,label
       ,handbook_name
       ,paired_field_table
       ,paired_field_column)
      SELECT
        @filingHeaderId
       ,@parentId
       ,tes.name
       ,tes.title
       ,@recordId
       ,tedv.column_name
       ,tes.table_name
       ,GETDATE()
       ,@defValueOut
       ,tedv.editable
       ,tedv.display_on_ui
       ,tedv.has_default_value
       ,tedv.mandatory
       ,tedv.manual
       ,tedv.description
       ,tedv.label
       ,handbook_name
       ,paired_field_table
       ,paired_field_column
      FROM dbo.truck_export_def_values tedv
      INNER JOIN dbo.truck_export_sections tes
        ON tedv.section_id = tes.id
      WHERE tedv.id = @id
  END
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

-- Apply Truck Export def values for specified table --
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

-- Add Pipeline Container record --
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
         ,rulefacility.Issuer AS Bill_Issuer_SCAC
        FROM Pipeline_Filing_Details details
        INNER JOIN Pipeline_Inbound inbound
          ON details.Pipeline_Inbounds_FK = inbound.id
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
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

-- Add Pipeline Declaration Tab record --
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
         ,ruleFacility.Issuer AS Issuer
         ,REPLACE(inbound.TicketNumber, '-', '') AS Batch_Ticket
         ,ruleFacility.Pipeline AS Pipeline
         ,ruleFacility.Issuer AS Carrier_SCAC
         ,ruleFacility.port AS Discharge
         ,ruleFacility.port AS Entry_Port
         ,inbound.ExportDate AS Dep
         ,inbound.ImportDate AS Arr
         ,inbound.ImportDate AS Arr_2
         ,ruleFacility.Origin AS Origin
         ,ruleFacility.Destination AS Destination
         ,ruleFacility.Destination_State AS Destination_State
         ,inbound.ImportDate AS ETA
         ,inbound.ImportDate AS Export_Date
         ,CONCAT(ruleFacility.pipeline, ' P/L', ': ', inbound.Batch) AS Description
         ,inbound.TicketNumber AS Owner_Ref
         ,ruleFacility.FIRMs_Code AS FIRMs_Code
         ,REPLACE(inbound.TicketNumber, '-', '') AS Master_Bill
        FROM Pipeline_Filing_Details details
        INNER JOIN Pipeline_Inbound inbound
          ON details.Pipeline_Inbounds_FK = inbound.id
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))

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

-- Add Pipeline MISC record --
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

-- Add Pipeline Invoice Line record --
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
         ,CONCAT(ruleFacility.pipeline, ' P/L') AS Attribute_3
         ,inbound.Quantity AS Invoice_Qty
         ,ruleImporters.origin AS ORG
         ,inbound.Quantity * ruleImporters.value AS Line_Price
         ,inbound.Quantity * ruleImporters.freight AS Amount
         ,ruleImporters.manufacturer AS Manufacturer
         ,ruleImporters.Consignee AS Consignee
         ,ruleImporters.Consignee AS Sold_To_Party
         ,ruleImporters.origin AS Origin
         ,ruleImporters.country_of_export AS Export
         ,ruleFacility.Destination_State AS Destination_State
        FROM dbo.Pipeline_Filing_Details details
        INNER JOIN dbo.Pipeline_Inbound inbound
          ON inbound.id = details.Pipeline_Inbounds_FK
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))

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

-- Add Pipeline Invoice Header record --
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
         ,ruleImporters.manufacturer AS Manufacturer
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

-- Add Pipeline def values manual records --
ALTER PROCEDURE dbo.pipeline_add_def_values_manual (@tableName VARCHAR(128)
, @filingHeaderId INT
, @parentId INT
, @recordId INT)
AS
BEGIN
  DECLARE @str_val NVARCHAR(500)
  DECLARE @parmDefinition NVARCHAR(500)
  DECLARE @defValueOut VARCHAR(128)
  DECLARE @dataType VARCHAR(128)
  DECLARE @defValue VARCHAR(500)
  DECLARE @id INT;
  DECLARE @columnName VARCHAR(128)

  DECLARE c CURSOR FOR SELECT DISTINCT
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
   ,v.DefValue
   ,v.id
   ,v.ColName
  FROM dbo.Pipeline_DEFValues v
  INNER JOIN dbo.pipeline_sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.FManual > 0
  OR v.FHasDefaultVal > 0
  OR v.Display_on_UI > 0)
  ORDER BY id

  OPEN c
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  WHILE @@fetch_status = 0
  BEGIN
  SET @defValueOut = NULL;
  SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(''' + @defValue + ''' as ' + @dataType + ') as varchar(500))';
  SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@parmDefinition
                       ,@defValueOut = @defValueOut OUTPUT;
  IF @defValueOut IS NULL
  BEGIN
    SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(' + @defValue + ' as ' + @dataType + ') as varchar(500))';
    SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';

    DECLARE @result FLOAT
  BEGIN TRY
    EXECUTE sp_executesql @str_val
                         ,@parmDefinition
                         ,@defValueOut = @defValueOut OUTPUT;
  END TRY
  BEGIN CATCH
    SET @defValueOut = NULL;
  END CATCH;
  END;

  IF @dataType LIKE 'date%'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS DATETIME), 'MM/dd/yyyy');
  END;
  IF @dataType LIKE 'numeric'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS FLOAT), '0.######');
  END;

  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_DEFValues_Manual dvm
      WHERE dvm.record_id = @recordId
      AND dvm.table_name = @tableName
      AND dvm.column_name = @columnName)
  BEGIN
    INSERT INTO dbo.Pipeline_DEFValues_Manual (
        Filing_Headers_FK
       ,parent_record_id
       ,section_name
       ,section_title
       ,record_id
       ,column_name
       ,table_name
       ,ModifiedDate
       ,value
       ,FEditable
       ,Display_on_UI
       ,FHasDefaultVal
       ,FMandatory
       ,FManual
       ,description
       ,ValueLabel
       ,handbook_name
       ,paired_field_table
       ,paired_field_column)
      SELECT
        @filingHeaderId
       ,@parentId
       ,s.name
       ,s.title
       ,@recordId
       ,dv.ColName
       ,s.table_name
       ,GETDATE()
       ,@defValueOut
       ,dv.FEditable
       ,dv.Display_on_UI
       ,dv.FHasDefaultVal
       ,dv.FMandatory
       ,dv.FManual
       ,dv.ValueDesc
       ,dv.ValueLabel
       ,dv.handbook_name
       ,dv.paired_field_table
       ,dv.paired_field_column
      FROM dbo.Pipeline_DEFValues dv
      INNER JOIN dbo.pipeline_sections s
        ON dv.section_id = s.id
      WHERE dv.id = @id
  END
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

-- Apply Pipeline def values for specified table --
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

-- Drop unnecessary indexes --
DROP INDEX Idx_recordId_tableName_columnName
ON dbo.Pipeline_DEFValues_Manual
GO