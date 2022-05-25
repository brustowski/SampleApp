ALTER VIEW dbo.v_imp_truck_inbound_grid 
AS SELECT
  inbnd.id
 ,inbnd.Importer AS base_importer
 ,inbnd.paps
 ,inbnd.created_date
 ,inbnd.[deleted]
 ,rule_importer.cw_ior AS importer
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

LEFT JOIN dbo.imp_truck_rule_importer rule_importer
  ON inbnd.importer = rule_importer.ior
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
  FROM imp_truck_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_declaration declaration
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
       ,SUBSTRING(inbound.PAPs, 1, 4)
       ,SUBSTRING(inbound.PAPs, 5, LEN(inbound.PAPs))
       ,SUBSTRING(inbound.PAPs, 1, 4)
       ,rule_importer.arrival_port
       ,rule_importer.Entry_Port
       ,rule_importer.Destination_State
       ,rule_importer.Goods_Description
       ,rule_port.FIRMs_Code
      FROM dbo.imp_truck_filing_detail detail
      INNER JOIN dbo.imp_truck_inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN dbo.imp_truck_rule_importer rule_importer
        ON inbound.importer = rule_importer.ior
      LEFT JOIN dbo.imp_truck_rule_port rule_port
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
  FROM imp_truck_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_invoice_header invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_truck_invoice_header (
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
       ,rule_importer.cw_ior
       ,rule_importer.transactions_related
       ,rule_importer.cw_supplier
       ,rule_importer.cw_supplier
       ,rule_importer.cw_ior
       ,rule_importer.cw_ior
       ,rule_importer.cw_ior
       ,rule_importer.ce
       ,rule_importer.co
      FROM dbo.imp_truck_filing_detail detail
      INNER JOIN dbo.imp_truck_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_truck_rule_importer rule_importer
        ON inbnd.importer = rule_importer.ior
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
  FROM imp_truck_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_invoice_line invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO imp_truck_invoice_line (
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
       ,rule_importer.Tariff
       ,rule_importer.custom_quantity
       ,rule_importer.Goods_Description
       ,rule_importer.SPI
       ,rule_importer.co
       ,rule_importer.ce
       ,rule_importer.gross_weight
       ,rule_importer.gross_weight_uq
       ,rule_importer.custom_uq
       ,rule_importer.Line_Price
       ,rule_importer.product_id
       ,rule_importer.custom_attrib1
       ,rule_importer.custom_attrib2
       ,rule_importer.Invoice_Qty
       ,rule_importer.invoice_uq
       ,rule_importer.charges
       ,rule_importer.invoice_qty * rule_importer.line_price
      FROM dbo.imp_truck_filing_detail detail
      INNER JOIN dbo.imp_truck_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_truck_rule_importer rule_importer
        ON inbnd.importer = rule_importer.ior
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
    @allowMultiple = ps.is_array
  FROM imp_truck_form_section_configuration ps
  WHERE ps.table_name = @tableName;

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_misc misc
      WHERE misc.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_truck_misc (
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
       ,rule_importer.Recon_Issue
       ,rule_importer.nafta_recon
      FROM dbo.imp_truck_filing_detail detail
      INNER JOIN dbo.imp_truck_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_truck_rule_importer rule_importer
        ON inbnd.Importer = rule_importer.ior
      LEFT JOIN app_users_data user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO