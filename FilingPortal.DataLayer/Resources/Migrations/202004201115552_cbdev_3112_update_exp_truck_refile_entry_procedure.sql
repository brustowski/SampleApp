-- refile truck export entry --
ALTER PROCEDURE dbo.sp_exp_truck_refile_entry (@filingHeaderId INT,
@filingUser VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @date DATE = GETDATE();

  DECLARE @update TABLE (
    id INT PRIMARY KEY
   ,filing_header_id INT NOT NULL
   ,export_date DATETIME NOT NULL -- declaration
   ,customs_qty DECIMAL(18, 6) NOT NULL -- invoice line
   ,price DECIMAL(18, 6) NOT NULL -- invoice header, invoice line
   ,gross_weight DECIMAL(18, 6) NOT NULL -- invoice line
   ,importer VARCHAR(128) NOT NULL -- declaration, invoice header
   ,destination VARCHAR(5) NULL -- declaration
   ,country VARCHAR(2) NULL -- declaration
   ,ultimate_consignee_type VARCHAR(1) NULL -- invoice header
   ,tran_related VARCHAR(1) NULL -- invoice header
   ,address VARCHAR(128) NULL -- invoice header
   ,contact VARCHAR(128) NULL -- invoice header
   ,phone VARCHAR(128) NULL -- invoice header
  )

  INSERT INTO @update (
      id
     ,filing_header_id
     ,export_date
     ,customs_qty
     ,price
     ,gross_weight
     ,importer
     ,destination
     ,country
     ,ultimate_consignee_type
     ,tran_related
     ,address
     ,contact
     ,phone)
    SELECT
      inbnd.id
     ,filing_header.id
     ,inbnd.export_date
     ,inbnd.customs_qty
     ,inbnd.price
     ,inbnd.gross_weight
     ,inbnd.importer
     ,rule_consignee.destination
     ,rule_consignee.country
     ,rule_consignee.ultimate_consignee_type
     ,rule_exporter.tran_related
     ,rule_exporter.address
     ,rule_exporter.contact
     ,rule_exporter.phone
    FROM dbo.exp_truck_filing_header AS filing_header
    JOIN dbo.exp_truck_filing_detail AS filing_detail
      ON filing_header.id = filing_detail.filing_header_id
    JOIN dbo.exp_truck_inbound AS inbnd
      ON filing_detail.inbound_id = inbnd.id
    LEFT JOIN dbo.exp_truck_rule_consignee AS rule_consignee
      ON inbnd.importer = rule_consignee.consignee_code
    LEFT JOIN dbo.exp_truck_rule_exporter_consignee AS rule_exporter
      ON inbnd.importer = rule_exporter.consignee_code
        AND inbnd.exporter = rule_exporter.exporter
    WHERE filing_header.id = @filingHeaderId;

  -- update declaration record
  UPDATE declaration
  SET declaration.dep = src.export_date
     ,declaration.exp_date = src.export_date
     ,importer = src.importer
     ,destination = src.destination
     ,country_of_dest = src.destination
     ,tran_related = src.tran_related
     ,created_user = @filingUser
     ,created_date = @date
  FROM dbo.exp_truck_declaration AS declaration
  JOIN @update AS src
    ON declaration.filing_header_id = src.filing_header_id;

  -- update invoice header record
  UPDATE invoice_header
  SET invoice_header.invoice_total_amount = src.price
     ,invoice_header.usppi_address = src.address
     ,invoice_header.usppi_contact = src.contact
     ,invoice_header.usppi_phone = src.phone
     ,invoice_header.ultimate_consignee_type = src.ultimate_consignee_type
     ,created_user = @filingUser
     ,created_date = @date
  FROM dbo.exp_truck_invoice_header AS invoice_header
  JOIN @update AS src
    ON invoice_header.filing_header_id = src.filing_header_id;

  -- update invoice line record
  UPDATE invoice_line
  SET invoice_line.price = src.price
     ,invoice_line.customs_qty = src.customs_qty
     ,invoice_line.gross_weight = src.gross_weight
     ,created_user = @filingUser
     ,created_date = @date
  FROM dbo.exp_truck_invoice_line AS invoice_line
  JOIN @update AS src
    ON invoice_line.filing_header_id = src.filing_header_id;
END;
GO