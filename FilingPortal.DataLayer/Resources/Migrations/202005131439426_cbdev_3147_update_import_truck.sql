UPDATE inbnd
SET inbnd.importer_code = COALESCE(rule_importer.cw_ior, '')
  , inbnd.supplier_code = COALESCE(rule_importer.cw_supplier, '')
FROM dbo.imp_truck_inbound AS inbnd
LEFT JOIN dbo.imp_truck_rule_importer AS rule_importer
  ON rule_importer.ior = inbnd.importer;
GO

DECLARE @command VARCHAR(MAX);
DECLARE @tableName VARCHAR(128) = 'dbo.imp_truck_inbound';
DECLARE @columnName VARCHAR(128) = 'importer_code';
SELECT
  @command = 'ALTER TABLE ' + @tableName + ' DROP CONSTRAINT ' + d.name + ';' + CHAR(10)
FROM sys.columns AS c
JOIN sys.default_constraints AS d
  ON d.parent_object_id = c.object_id
    AND d.parent_column_id = c.column_id
WHERE c.name = @columnName
AND c.object_id = OBJECT_ID(@tableName, 'U');

EXEC (@command);

SET @columnName = 'supplier_code';
SELECT
  @command = 'ALTER TABLE ' + @tableName + ' DROP CONSTRAINT ' + d.name + ';' + CHAR(10)
FROM sys.columns AS c
JOIN sys.default_constraints AS d
  ON d.parent_object_id = c.object_id
    AND d.parent_column_id = c.column_id
WHERE c.name = @columnName
AND c.object_id = OBJECT_ID(@tableName, 'U');

EXEC (@command);
GO

ALTER VIEW dbo.v_imp_truck_inbound_grid
AS SELECT
  inbnd.id
 ,inbnd.importer_code AS importer
 ,inbnd.supplier_code AS supplier
 ,inbnd.paps
 ,inbnd.created_date
 ,inbnd.[deleted]
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,IIF(rule_importer.ior IS NULL, 0, 1) AS has_importer_rule
 ,IIF(rule_port.entry_port IS NULL, 0, 1) AS has_port_rule
FROM dbo.imp_truck_inbound inbnd

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.entry_status
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.imp_truck_filing_header etfh
  JOIN dbo.imp_truck_filing_detail etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN dbo.imp_truck_rule_importer AS rule_importer
  ON inbnd.importer_code = rule_importer.cw_ior
    AND inbnd.supplier_code = rule_importer.cw_supplier
LEFT JOIN dbo.imp_truck_rule_port rule_port
  ON rule_port.entry_port = rule_importer.entry_port
    AND rule_port.arrival_port = rule_importer.arrival_port
LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'import'

WHERE inbnd.deleted = 0
GO

-- add truck import declaration record --
ALTER PROCEDURE dbo.sp_imp_truck_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_truck_declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM dbo.imp_truck_form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_declaration AS declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.imp_truck_declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,main_supplier
       ,importer
       ,issuer
       ,master_bill
       ,carrier_scac
       ,discharge
       ,entry_port
       ,destination_state
       ,[description]
       ,firms_code)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_importer.cw_supplier
       ,rule_importer.cw_ior
       ,SUBSTRING(inbnd.PAPs, 1, 4)
       ,SUBSTRING(inbnd.PAPs, 5, LEN(inbnd.PAPs))
       ,SUBSTRING(inbnd.PAPs, 1, 4)
       ,rule_importer.arrival_port
       ,rule_importer.entry_port
       ,rule_importer.destination_state
       ,rule_importer.goods_description
       ,rule_port.firms_code
      FROM dbo.imp_truck_filing_detail AS detail
      INNER JOIN dbo.imp_truck_inbound AS inbnd
        ON detail.inbound_id = inbnd.id
      LEFT JOIN dbo.imp_truck_rule_importer AS rule_importer
        ON inbnd.importer_code = rule_importer.cw_ior
        AND inbnd.supplier_code = rule_importer.cw_supplier
      LEFT JOIN dbo.imp_truck_rule_port AS rule_port
        ON rule_importer.arrival_port = rule_port.arrival_port
          AND rule_importer.entry_port = rule_port.entry_port
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

-- add truck import invoice header record --
ALTER PROCEDURE dbo.sp_imp_truck_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_truck_invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM dbo.imp_truck_form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_invoice_header AS invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.imp_truck_invoice_header (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,invoice_no
       ,supplier
       ,consignee
       ,transaction_related
       ,manufacturer
       ,seller
       ,importer
       ,sold_to_party
       ,ship_to_party
       ,export
       ,origin)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbnd.PAPs
       ,rule_importer.cw_supplier
       ,rule_importer.consignee_code
       ,rule_importer.transactions_related
       ,rule_importer.cw_supplier
       ,rule_importer.cw_supplier
       ,rule_importer.cw_ior
       ,rule_importer.cw_ior
       ,rule_importer.cw_ior
       ,rule_importer.ce
       ,rule_importer.co
      FROM dbo.imp_truck_filing_detail AS detail
      JOIN dbo.imp_truck_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_truck_rule_importer AS rule_importer
        ON inbnd.importer_code = rule_importer.cw_ior
        AND inbnd.supplier_code = rule_importer.cw_supplier
      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.sp_imp_truck_add_invoice_line @filingHeaderId
                                          ,@recordId
                                          ,@filingUser
                                          ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END;
END;
GO

-- add truck import invoice line record --
ALTER PROCEDURE dbo.sp_imp_truck_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_truck_invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM dbo.imp_truck_form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_invoice_line AS invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO dbo.imp_truck_invoice_line (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,invoice_no
       ,transaction_related
       ,tariff
       ,customs_qty
       ,goods_description
       ,spi
       ,org
       ,export
       ,gr_weight
       ,gr_weight_unit
       ,uq
       ,price_unit
       ,prod_id1
       ,attribute1
       ,attribute2
       ,invoice_qty
       ,invoice_qty_unit
       ,amount
       ,line_price)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbnd.PAPs
       ,rule_importer.transactions_related
       ,rule_importer.tariff
       ,rule_importer.custom_quantity
       ,rule_importer.goods_description
       ,rule_importer.spi
       ,rule_importer.co
       ,rule_importer.ce
       ,rule_importer.gross_weight
       ,rule_importer.gross_weight_uq
       ,rule_importer.custom_uq
       ,rule_importer.line_price
       ,rule_importer.product_id
       ,rule_importer.custom_attrib1
       ,rule_importer.custom_attrib2
       ,rule_importer.invoice_qty
       ,rule_importer.invoice_uq
       ,rule_importer.charges
       ,rule_importer.invoice_qty * rule_importer.line_price
      FROM dbo.imp_truck_filing_detail AS detail
      INNER JOIN dbo.imp_truck_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_truck_rule_importer AS rule_importer
        ON inbnd.importer_code = rule_importer.cw_ior
          AND inbnd.supplier_code = rule_importer.cw_supplier
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO

-- add truck import misc record --
ALTER PROCEDURE dbo.sp_imp_truck_add_misc (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_truck_misc';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM dbo.imp_truck_form_section_configuration AS section
  WHERE section.table_name = @tableName;

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_misc AS misc
      WHERE misc.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.imp_truck_misc (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,branch
       ,[broker]
       ,preparer_dist_port
       ,recon_issue
       ,fta_recon)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,user_data.Branch
       ,user_data.Broker
       ,user_data.Location
       ,rule_importer.recon_issue
       ,rule_importer.nafta_recon
      FROM dbo.imp_truck_filing_detail AS detail
      JOIN dbo.imp_truck_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_truck_rule_importer AS rule_importer
        ON inbnd.importer_code = rule_importer.cw_ior
          AND inbnd.supplier_code = rule_importer.cw_supplier
      LEFT JOIN app_users_data AS user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO