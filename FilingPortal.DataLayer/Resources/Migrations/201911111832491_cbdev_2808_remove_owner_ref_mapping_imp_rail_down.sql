﻿ALTER PROCEDURE [dbo].[sp_imp_rail_add_declaration] (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_rail_declaration'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @masterBill VARCHAR(128)

  SET @operationId = COALESCE(@operationId, NEWID());

  SELECT TOP 1
    @masterBill = p.bill_of_lading
  FROM dbo.imp_rail_filing_detail d
  INNER JOIN dbo.imp_rail_inbound p
    ON p.id = d.inbound_id
  WHERE d.filing_header_id = @filingHeaderId

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM imp_rail_form_section_configuration ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_declaration pdt
      WHERE pdt.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_rail_declaration (
        parent_record_id
       ,filing_header_id
       ,carrier_scac
       ,country_of_export
       ,description
       ,destination
       ,destination_state
       ,discharge
       ,entry_port
       ,firms_code
       ,importer
       ,issuer
       ,main_supplier
       ,origin
       ,master_bill
       ,owner_ref
       ,operation_id)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,p.issuer_code AS Carrier_SCAC
       ,rp.export AS Country_of_Export
       ,p.description1 AS Description
       ,rp.destination
       ,rn.destination_state
       ,p.port_of_unlading AS Discharge
       ,p.port_of_unlading AS Entry_Port
       ,rp.firms_code
       ,rn.importer
       ,p.issuer_code AS Issuer
       ,rn.main_supplier
       ,rp.origin
       ,@masterBill
       ,p.reference_number1
       ,@operationId
      FROM dbo.imp_rail_filing_detail d
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = d.inbound_id
      LEFT JOIN dbo.imp_rail_rule_port rp
        ON p.port_of_unlading = rp.port
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn
        ON RTRIM(LTRIM(p.importer)) = RTRIM(LTRIM(rn.importer_name))
          AND (RTRIM(LTRIM(p.supplier)) = RTRIM(LTRIM(rn.supplier_name))
            OR (p.supplier IS NULL
              AND rn.supplier_name IS NULL))
      WHERE d.filing_header_id = @filingHeaderId
  END
END;
GO