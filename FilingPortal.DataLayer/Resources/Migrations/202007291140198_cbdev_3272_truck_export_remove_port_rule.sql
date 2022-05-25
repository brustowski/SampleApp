-- add truck export declaration record --
ALTER PROCEDURE dbo.sp_exp_truck_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'exp_truck_declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM exp_truck_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.exp_truck_declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO exp_truck_declaration (filing_header_id
    , parent_record_id
    , operation_id
    , destination
    , country_of_dest
    , tran_related
    , routed_tran
    , eccn
    , export
    , importer
    , tariff_type
    , sold_en_route
    , master_bill
    , owner_ref
    , port_of_loading
    , transport_ref
    , main_supplier
    , dep
    , exp_date
    , hazardous
    , origin)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_consignee.destination
       ,rule_consignee.country
       ,trule_exporter.tran_related
       ,inbound.routed_tran
       ,inbound.eccn
       ,inbound.export
       ,inbound.importer
       ,inbound.tariff_type
       ,inbound.sold_en_route
       ,inbound.master_bill
       ,inbound.master_bill
       ,inbound.origin
       ,inbound.master_bill
       ,inbound.exporter
       ,inbound.export_date
       ,inbound.export_date
       ,inbound.hazardous
       ,cdp.unloco
      FROM exp_truck_filing_detail detail
      JOIN exp_truck_inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN exp_truck_rule_consignee rule_consignee
        ON inbound.importer = rule_consignee.consignee_code
      LEFT JOIN exp_truck_rule_exporter_consignee trule_exporter
        ON inbound.importer = trule_exporter.consignee_code
          AND inbound.exporter = trule_exporter.exporter
      LEFT JOIN CW_Domestic_Ports cdp
        ON (LTRIM(inbound.origin) = LTRIM(cdp.port_code))
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cw_get_unloco_dictionary
AS
BEGIN

  DECLARE @myStatement VARCHAR(MAX)
  SET @myStatement = '
SELECT RL_Code AS [UNLOCO Code], RL_Portname AS PortName, RL_NameWithDiacriticals AS Name_Diacriticals ,RL_RN_NKCountryCode AS Country
FROM  RefUNLOCO UN
WHERE UN.RL_IsActive = 1'

  TRUNCATE TABLE cw_unloco_dictionary

  INSERT INTO cw_unloco_dictionary (unloco, port_name, port_diacriticals, country_code)
  EXECUTE (@myStatement) AT CargoWiseServer

END
GO