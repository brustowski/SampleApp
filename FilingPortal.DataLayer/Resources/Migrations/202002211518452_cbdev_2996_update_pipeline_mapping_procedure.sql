-- add pipeline import declaration record --
ALTER PROCEDURE dbo.sp_imp_pipeline_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_pipeline_declaration'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @importerCode VARCHAR(128);

  SET @operationId = COALESCE(@operationId, NEWID());

  --Get importer code for the inbound importer
  SELECT
    @importerCode = dbo.fn_imp_pipeline_importer_code(@filingHeaderId);

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_pipeline_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_pipeline_declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.imp_pipeline_declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,main_supplier
       ,importer
       ,issuer
       ,batch_ticket
       ,pipeline
       ,carrier_scac
       ,discharge
       ,entry_port
       ,dep
       ,arr
       ,arr2
       ,origin
       ,destination
       ,destination_state
       ,eta
       ,export_date
       ,[description]
       ,owner_ref
       ,firms_code
       ,master_bill
       ,importer_of_record
       ,entry_number
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_importer.supplier
       ,@importerCode
       ,rule_facility.issuer
       ,REPLACE(inbnd.ticket_number, '-', '')
       ,rule_facility.pipeline
       ,rule_facility.issuer
       ,rule_facility.[port]
       ,rule_facility.[port]
       ,inbnd.export_date
       ,inbnd.import_date
       ,inbnd.import_date
       ,rule_facility.origin
       ,rule_facility.destination
       ,rule_facility.destination_state
       ,inbnd.import_date
       ,inbnd.export_date
       ,CONCAT(rule_facility.pipeline, ' P/L', ': ', inbnd.batch)
       ,inbnd.ticket_number
       ,rule_facility.firms_code
       ,REPLACE(inbnd.ticket_number, '-', '')
       ,@ImporterCode
       ,inbnd.entry_number
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_pipeline_filing_detail detail
      JOIN dbo.imp_pipeline_inbound inbnd
        ON detail.inbound_id = inbnd.id
      LEFT JOIN dbo.imp_pipeline_rule_facility rule_facility
        ON inbnd.facility = rule_facility.facility
      LEFT JOIN dbo.imp_pipeline_rule_importer rule_importer
        ON @importerCode = rule_importer.importer
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO