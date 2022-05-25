ALTER VIEW us_exp_rail.v_inbound_grid
AS
SELECT
  inbnd.id
 ,inbnd.exporter
 ,inbnd.importer
 ,inbnd.master_bill
 ,(STUFF(CAST((SELECT
      [text()] = ', ' + ic.container_number
    FROM us_exp_rail.inbound_containers ic
    WHERE ic.inbound_record_id = inbnd.id
    FOR XML PATH (''), TYPE)
  AS VARCHAR(250)), 1, 2, ''))
  AS containers
 ,inbnd.load_port
 ,inbnd.export_port
 ,inbnd.carrier
 ,inbnd.tariff_type
 ,inbnd.tariff
 ,inbnd.goods_description
 ,ISNULL(container.customs_qty, 0) AS customs_qty
 ,ISNULL(container.price, 0) AS price
 ,ISNULL(container.gross_weight, 0) AS gross_weight
 ,inbnd.gross_weight_uom
 ,filing_header.id AS filing_header_id
 ,inbnd.created_user
 ,inbnd.created_date
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,ISNULL(filing_header.confirmed, 0) AS confirmed
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,has_consignee_rule.value AS has_consignee_rule
 ,has_exporter_consignee_rule.value AS has_exporter_consignee_rule
 ,CAST(has_consignee_rule.value & has_exporter_consignee_rule.value AS BIT) AS has_all_required_rules
 ,inbnd.deleted
FROM us_exp_rail.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
   ,fh.mapping_status
   ,fh.filing_status
   ,fh.confirmed
  FROM us_exp_rail.filing_header AS fh
  JOIN us_exp_rail.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'export'
LEFT JOIN (SELECT
    ic.inbound_record_id
   ,SUM(ic.price) AS price
   ,SUM(ic.customs_qty) AS customs_qty
   ,SUM(ic.gross_weight) AS gross_weight
  FROM us_exp_rail.inbound_containers ic
  GROUP BY ic.inbound_record_id) AS container
  ON container.inbound_record_id = inbnd.id
CROSS APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM us_exp_rail.rule_consignee rule_consignee
      WHERE RTRIM(LTRIM(rule_consignee.consignee_code)) = RTRIM(LTRIM(inbnd.importer)))
    , 1, 0) AS value) AS has_consignee_rule

CROSS APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM us_exp_rail.rule_exporter_consignee rule_exporter_consignee
      WHERE RTRIM(LTRIM(rule_exporter_consignee.consignee_code)) = RTRIM(LTRIM(inbnd.importer))
      AND RTRIM(LTRIM(rule_exporter_consignee.exporter)) = RTRIM(LTRIM(inbnd.exporter)))
    , 1, 0) AS value) AS has_exporter_consignee_rule

WHERE inbnd.deleted = 0
GO

-- add rail invoice header record --
CREATE OR ALTER PROCEDURE us_exp_rail.sp_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM us_exp_rail.invoice_header invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO invoice_header (filing_header_id
    , parent_record_id
    , operation_id
    , usppi_address
    , usppi_contact
    , usppi_phone
    , ultimate_consignee_type
    , usppi
    , invoice_total_amount
    , ultimate_consignee)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_exporter.address
       ,rule_exporter.contact
       ,rule_exporter.phone
       ,rule_consignee.ultimate_consignee_type
       ,inbound.exporter
       ,container.price
       ,inbound.importer
      FROM filing_detail detail
      JOIN inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN rule_consignee rule_consignee
        ON inbound.importer = rule_consignee.consignee_code
      LEFT JOIN rule_exporter_consignee rule_exporter
        ON inbound.importer = rule_exporter.consignee_code
          AND inbound.exporter = rule_exporter.exporter
      LEFT JOIN (SELECT
          ic.inbound_record_id
         ,SUM(ic.price) AS price
        FROM us_exp_rail.inbound_containers ic
        GROUP BY ic.inbound_record_id) AS container
        ON container.inbound_record_id = inbound.id
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
    EXEC us_exp_rail.sp_add_invoice_line @filingHeaderId
                                        ,@recordId
                                        ,@filingUser
                                        ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END;
GO

--
-- add rail export declaration record
--
CREATE OR ALTER PROCEDURE us_exp_rail.sp_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'us_exp_rail.declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM us_exp_rail.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM us_exp_rail.declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO us_exp_rail.declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,destination
       ,country_of_dest
       ,tran_related
       ,importer
       ,tariff_type
       ,master_bill
       ,main_supplier
       ,carrier
       ,port_of_loading
       ,export
       ,description
       ,origin)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_consignee.destination
       ,rule_consignee.country
       ,trule_exporter.tran_related
       ,inbnd.importer
       ,inbnd.tariff_type
       ,inbnd.master_bill
       ,inbnd.exporter
       ,inbnd.carrier
       ,inbnd.load_port
       ,inbnd.export_port
       ,inbnd.goods_description
       ,domestic_port.unloco
      FROM us_exp_rail.filing_detail AS detail
      JOIN us_exp_rail.inbound AS inbnd
        ON detail.inbound_id = inbnd.id
      LEFT JOIN us_exp_rail.rule_consignee AS rule_consignee
        ON inbnd.importer = rule_consignee.consignee_code
      LEFT JOIN us_exp_rail.rule_exporter_consignee AS trule_exporter
        ON inbnd.importer = trule_exporter.consignee_code
          AND inbnd.exporter = trule_exporter.exporter
      LEFT JOIN dbo.CW_Domestic_Ports AS domestic_port
        ON domestic_port.port_code = inbnd.load_port
      WHERE detail.filing_header_id = @filingHeaderId;
  END;
END;
GO