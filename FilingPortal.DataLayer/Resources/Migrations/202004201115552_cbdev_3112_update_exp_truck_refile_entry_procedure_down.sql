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
   ,export_date DATETIME NOT NULL
   ,customs_qty DECIMAL(18, 6) NOT NULL
   ,price DECIMAL(18, 6) NOT NULL
   ,gross_weight DECIMAL(18, 6) NOT NULL
  )

  INSERT INTO @update (
      id
     ,filing_header_id
     ,export_date
     ,customs_qty
     ,price
     ,gross_weight)
    SELECT
      inbnd.id
     ,filing_header.id
     ,inbnd.export_date
     ,inbnd.customs_qty
     ,inbnd.price
     ,inbnd.gross_weight
    FROM dbo.exp_truck_filing_header AS filing_header
    JOIN dbo.exp_truck_filing_detail AS filing_detail
      ON filing_header.id = filing_detail.filing_header_id
    JOIN dbo.exp_truck_inbound AS inbnd
      ON filing_detail.inbound_id = inbnd.id
    WHERE filing_header.id = @filingHeaderId;

  -- update declaration record
  UPDATE declaration
  SET declaration.dep = src.export_date
     ,declaration.exp_date = src.export_date
     ,created_user = @filingUser
     ,created_date = @date
  FROM dbo.exp_truck_declaration AS declaration
  JOIN @update AS src
    ON declaration.filing_header_id = src.filing_header_id;

  -- update invoice header record
  UPDATE invoice_header
  SET invoice_header.invoice_total_amount = src.price
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