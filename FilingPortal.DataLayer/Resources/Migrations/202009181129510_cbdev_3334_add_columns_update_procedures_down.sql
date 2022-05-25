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
   ,tariff_type VARCHAR(128) NULL -- decalration, invoice line
   ,tariff VARCHAR(35) NULL -- invoice line
   ,export VARCHAR(128) NULL -- decalration
   ,invoice_qty_unit VARCHAR(10) -- invoice line 
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
     ,export)
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
     ,tariff_type = src.tariff_type
     ,tariff = src.tariff
     ,invoice_qty_unit = src.invoice_qty_unit
     ,created_user = @filingUser
     ,created_date = @date
  FROM dbo.exp_truck_invoice_line AS invoice_line
  JOIN @update AS src
    ON invoice_line.filing_header_id = src.filing_header_id;
END;
GO

ALTER VIEW dbo.v_exp_truck_inbound_grid 
AS SELECT
  inbnd.id
 ,inbnd.exporter
 ,inbnd.importer
 ,inbnd.tariff_type
 ,inbnd.tariff
 ,inbnd.routed_tran
 ,inbnd.sold_en_route
 ,inbnd.master_bill
 ,inbnd.origin
 ,inbnd.export
 ,inbnd.export_date
 ,inbnd.eccn
 ,inbnd.goods_description
 ,inbnd.customs_qty
 ,inbnd.price
 ,inbnd.gross_weight
 ,inbnd.gross_weight_uom
 ,inbnd.hazardous
 ,inbnd.origin_indicator
 ,inbnd.goods_origin
 ,update_record.created_date AS uploaded_date
 ,update_record.created_user AS uploaded_by_user
 ,update_record.importer AS update_importer
 ,update_record.tariff_type AS update_tariff_type
 ,update_record.tariff AS update_tariff
 ,update_record.export AS update_export
 ,update_record.export_date AS update_export_date
 ,update_record.customs_qty AS update_customs_qty
 ,update_record.price AS update_price
 ,update_record.gross_weight AS update_gross_weight
 ,inbnd.deleted
 ,inbnd.created_date
 ,ISNULL(filing_header.last_modified_date, inbnd.created_date) AS last_modified_date
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CAST(ISNULL(filing_header.is_auto_filed, 0) AS BIT) AS is_auto_filed
 ,rule_consignee.found AS has_consignee_rule
 ,rule_exporter_consignee.found AS has_exporter_consignee_rule
 ,CAST(rule_consignee.found & rule_exporter_consignee.found AS BIT) AS has_all_required_rules
 ,update_rule_consignee.found AS has_update_consignee_rule
 ,update_rule_exporter_consignee.found AS has_update_exporter_consignee_rule
 ,CAST(update_rule_consignee.found & update_rule_exporter_consignee.found AS BIT) AS has_update_all_required_rules
FROM dbo.exp_truck_inbound AS inbnd

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.entry_status
   ,etfh.mapping_status
   ,etfh.filing_status
   ,etfh.is_auto_filed
   ,etfh.last_modified_date
  FROM dbo.exp_truck_filing_header AS etfh
  JOIN dbo.exp_truck_filing_detail AS etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'export'
LEFT JOIN dbo.exp_truck_update_record AS update_record
  ON inbnd.exporter = update_record.exporter
    AND inbnd.master_bill = update_record.master_bill
OUTER APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM exp_truck_rule_consignee AS rule_consignee
      WHERE rule_consignee.consignee_code = inbnd.importer)
    , 1, 0) AS found) AS rule_consignee
OUTER APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM exp_truck_rule_exporter_consignee AS rule_exporter_consignee
      WHERE rule_exporter_consignee.consignee_code = inbnd.importer
      AND rule_exporter_consignee.exporter = inbnd.exporter)
    , 1, 0) AS found) AS rule_exporter_consignee

OUTER APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM exp_truck_rule_consignee AS rule_consignee
      WHERE rule_consignee.consignee_code = update_record.importer)
    , 1, 0) AS found) AS update_rule_consignee
OUTER APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM exp_truck_rule_exporter_consignee AS rule_exporter_consignee
      WHERE rule_exporter_consignee.consignee_code = update_record.importer
      AND rule_exporter_consignee.exporter = update_record.exporter)
    , 1, 0) AS found) AS update_rule_exporter_consignee

WHERE inbnd.deleted = 0
GO
