-- create views 
ALTER VIEW dbo.v_exp_vessel_inbound_grid
AS
SELECT DISTINCT
  inbnd.id
 ,usppi.ClientCode AS usppi
 ,usppi_address.code AS [address]
 ,inbnd.contact
 ,inbnd.phone
 ,importer.ClientCode AS importer
 ,vessel.[name] AS vessel
 ,inbnd.export_date
 ,inbnd.load_port
 ,inbnd.discharge_port
 ,country.code AS country_of_destination
 ,inbnd.tariff_type
 ,inbnd.tariff
 ,inbnd.goods_description
 ,inbnd.origin_indicator
 ,inbnd.quantity
 ,inbnd.[weight]
 ,inbnd.[value]
 ,inbnd.transport_ref
 ,inbnd.container
 ,inbnd.in_bond
 ,inbnd.sold_en_route
 ,inbnd.export_adjustment_value
 ,inbnd.original_itn
 ,inbnd.routed_transaction
 ,inbnd.reference_number
 ,inbnd.[description]
 ,inbnd.created_date
 ,inbnd.deleted
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN rule_usppi_consignee.id IS NULL THEN 0
    ELSE 1
  END AS has_usppi_consignee_rule
FROM dbo.exp_vessel_inbound inbnd
JOIN dbo.Clients AS importer
  ON inbnd.importer_id = importer.id
JOIN dbo.Clients AS usppi
  ON inbnd.usppi_id = usppi.id
LEFT JOIN dbo.Client_Addresses AS usppi_address
  ON inbnd.address_id = usppi_address.id
LEFT JOIN dbo.handbook_vessel AS vessel
  ON inbnd.vessel_id = vessel.id
LEFT JOIN dbo.Countries AS country
  ON inbnd.country_of_destination_id = country.id
LEFT JOIN dbo.exp_vessel_rule_usppi_consignee AS rule_usppi_consignee
  ON rule_usppi_consignee.usppi_id = inbnd.usppi_id
    AND rule_usppi_consignee.consignee_id = inbnd.importer_id
OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.exp_vessel_filing_header AS etfh
  JOIN dbo.exp_vessel_filing_detail AS etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN dbo.MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN dbo.FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
WHERE inbnd.deleted = 0
GO

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
       ,COALESCE(inbnd.reference_number, '') + ' - ' + COALESCE(inbnd.contact, '')
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

-- add invoice header record --
ALTER PROCEDURE dbo.sp_exp_vessel_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'exp_vessel_invoice_header'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

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
      FROM dbo.exp_vessel_invoice_header AS header
      WHERE header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.exp_vessel_invoice_header (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,usppi
       ,usppi_address
       ,usppi_contact
       ,usppi_phone
       ,ultimate_consignee
       ,ultimate_consignee_address
       ,ultimate_consignee_type
       ,origin_indicator
       ,export_date
       ,created_date
       ,created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,usppi.ClientCode
       ,usppi_address.code
       ,inbnd.contact
       ,inbnd.phone
       ,consignee.ClientCode
       ,consignee_address.code
       ,usppi_consignee_rule.ultimate_consignee_type
       ,inbnd.origin_indicator
       ,inbnd.export_date
       ,GETDATE()
       ,@filingUser
      FROM dbo.exp_vessel_filing_detail AS detail
      INNER JOIN dbo.exp_vessel_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.Clients AS usppi
        ON inbnd.usppi_id = usppi.id
      LEFT JOIN dbo.Client_Addresses AS usppi_address
        ON inbnd.address_id = usppi_address.id
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.importer_id = consignee.id
      LEFT JOIN dbo.exp_vessel_rule_usppi_consignee AS usppi_consignee_rule
        ON usppi_consignee_rule.usppi_id = inbnd.usppi_id
          AND usppi_consignee_rule.consignee_id = inbnd.importer_id
      LEFT JOIN dbo.Client_Addresses AS consignee_address
        ON consignee_address.id = usppi_consignee_rule.consignee_address_id
      WHERE detail.filing_header_id = @filingHeaderId;

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.sp_exp_vessel_add_invoice_line @filingHeaderId
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