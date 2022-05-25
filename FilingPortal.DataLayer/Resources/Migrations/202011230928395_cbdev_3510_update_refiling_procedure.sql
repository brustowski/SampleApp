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
   ,tariff_type VARCHAR(128) NULL -- decalration, invoice line
   ,tariff VARCHAR(35) NULL -- invoice line
   ,export VARCHAR(128) NULL -- decalration
   ,invoice_qty_unit VARCHAR(10) -- invoice line 
   ,origin VARCHAR(128) -- declaration
   ,goods_description VARCHAR(512) -- invoice line
   ,routed_tran VARCHAR(10) --declaration
   ,sold_en_route VARCHAR(10) --declaration
   ,eccn VARCHAR(128) --declaration
   ,goods_origin VARCHAR(10) --inv line
   ,origin_indicator VARCHAR(128) --inv header
   ,gross_weight_unit VARCHAR(128) --inv line
   ,hazardous VARCHAR(10) --declaration
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
     ,phone
     ,tariff_type
     ,tariff
     ,invoice_qty_unit
     ,export
     ,origin
     ,goods_description
     ,routed_tran
     ,sold_en_route
     ,eccn
     ,goods_origin
     ,origin_indicator
     ,gross_weight_unit
     ,hazardous)
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
     ,inbnd.tariff_type
     ,inbnd.tariff
     ,dbo.fn_app_unit_by_tariff(inbnd.tariff, inbnd.tariff_type)
     ,inbnd.export
     ,inbnd.origin
     ,inbnd.goods_description
     ,inbnd.routed_tran
     ,inbnd.sold_en_route
     ,inbnd.eccn
     ,inbnd.goods_origin
     ,inbnd.origin_indicator
     ,inbnd.gross_weight_uom
     ,inbnd.hazardous
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
     ,tariff_type = src.tariff_type
     ,export = src.export
     ,port_of_loading = src.origin
     ,origin = domestic_port.unloco
     ,sold_en_route = src.sold_en_route
     ,routed_tran = src.routed_tran
     ,hazardous = src.hazardous
     ,eccn = src.eccn
     ,created_user = @filingUser
     ,created_date = @date

  FROM dbo.exp_truck_declaration AS declaration
  JOIN @update AS src
    ON declaration.filing_header_id = src.filing_header_id
  LEFT JOIN dbo.CW_Domestic_Ports AS domestic_port
    ON (LTRIM(src.origin) = LTRIM(domestic_port.port_code))
    AND domestic_port.country = 'US';

  -- update invoice header record
  UPDATE invoice_header
  SET invoice_total_amount = src.price
     ,ultimate_consignee = src.importer
     ,ultimate_consignee_type = src.ultimate_consignee_type
     ,origin_indicator = src.origin_indicator
     ,created_user = @filingUser
     ,created_date = @date
  FROM dbo.exp_truck_invoice_header AS invoice_header
  JOIN @update AS src
    ON invoice_header.filing_header_id = src.filing_header_id;

  -- update invoice line record
  UPDATE invoice_line
  SET price = src.price
     ,customs_qty = src.customs_qty
     ,gross_weight = src.gross_weight
     ,gross_weight_unit = src.gross_weight_unit
     ,goods_origin = src.goods_origin
     ,tariff_type = src.tariff_type
     ,tariff = src.tariff
     ,invoice_qty_unit = src.invoice_qty_unit
     ,goods_description = src.goods_description
     ,created_user = @filingUser
     ,created_date = @date
  FROM dbo.exp_truck_invoice_line AS invoice_line
  JOIN @update AS src
    ON invoice_line.filing_header_id = src.filing_header_id;
END;
GO