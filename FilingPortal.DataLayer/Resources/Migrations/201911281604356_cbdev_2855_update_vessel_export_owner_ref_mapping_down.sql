-- add declaration record --
ALTER PROCEDURE dbo.sp_exp_vessel_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'exp_vessel_declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM exp_vessel_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.exp_vessel_declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.exp_vessel_declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,main_supplier
       ,importer
       ,container
       ,transaction_related
       ,routed_tran
       ,tariff_type
       ,sold_en_route
       ,vessel
       ,port_of_loading
       ,dep
       ,discharge
       ,export
       ,exp
       ,etd
       ,export_date
       ,description
       ,owner_ref
       ,transport_ref
       ,country_of_dest
       ,destination
       ,origin
       ,state_of_origin
       ,inbond_type
       ,export_adjustment_value
       ,original_itn
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,usppi.ClientCode
       ,importer.ClientCode
       ,inbnd.container
       ,rule_usppi_consignee.transaction_related
       ,inbnd.routed_transaction
       ,inbnd.tariff_type
       ,inbnd.sold_en_route
       ,vessel.name
       ,inbnd.load_port
       ,inbnd.export_date
       ,inbnd.discharge_port
       ,inbnd.load_port
       ,inbnd.export_date
       ,inbnd.export_date
       ,inbnd.export_date
       ,inbnd.description
       ,inbnd.reference_number
       ,inbnd.transport_ref
       ,country.code
       ,foreign_port.unloco
       ,domestic_port.unloco
       ,domestic_port.state
       ,inbnd.in_bond
       ,inbnd.export_adjustment_value
       ,inbnd.original_itn
       ,GETDATE()
       ,@filingUser
      FROM dbo.exp_vessel_filing_detail AS detail
      INNER JOIN dbo.exp_vessel_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.Clients AS usppi
        ON inbnd.usppi_id = usppi.id
      LEFT JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN dbo.handbook_vessel AS vessel
        ON vessel.id = inbnd.vessel_id
      LEFT JOIN dbo.Countries AS country
        ON inbnd.country_of_destination_id = country.id
      LEFT JOIN dbo.exp_vessel_rule_usppi_consignee AS rule_usppi_consignee
        ON rule_usppi_consignee.usppi_id = inbnd.usppi_id
          AND rule_usppi_consignee.consignee_id = inbnd.importer_id
      LEFT JOIN dbo.CW_Domestic_Ports AS domestic_port
        ON inbnd.load_port = domestic_port.port_code
      LEFT JOIN dbo.CW_Foreign_Ports AS foreign_port
        ON inbnd.discharge_port = foreign_port.port_code
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO