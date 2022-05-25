-- add rail invoice line record --
ALTER PROCEDURE dbo.sp_imp_rail_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_rail_invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM dbo.imp_rail_form_section_configuration AS ps
  WHERE ps.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_invoice_line AS pil
      WHERE pil.parent_record_id = @parentId)
  BEGIN
    INSERT INTO dbo.imp_rail_invoice_line (
        parent_record_id
       ,filing_header_id
       ,attribute_1
       ,attribute_2
       ,consignee
       ,dest_state
       ,export
       ,goods_description
       ,manufacturer
       ,org
       ,origin
       ,prod_id_1
       ,tariff
       ,transaction_related
       ,customs_qty
       ,spi
       ,uq
       ,price_unit
       ,invoice_qty
       ,invoice_qty_unit
       ,amount
       ,line_price
       ,description
       ,operation_id)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,COALESCE(rule_product_exact.attribute_1, rule_product.attribute_1) AS attribute_1
       ,COALESCE(rule_product_exact.attribute_2, rule_product.attribute_2) AS attribute_2
       ,COALESCE(rule_supplier_exact.consignee, rule_supplier.consignee) AS consignee
       ,COALESCE(rule_supplier_exact.destination_state, rule_supplier.destination_state) AS destination_state
       ,rule_port.export
       ,COALESCE(rule_product_exact.goods_description, rule_product.goods_description) AS goods_description
       ,COALESCE(rule_supplier_exact.manufacturer, rule_supplier.manufacturer) AS manufacturer
       ,COALESCE(rule_supplier_exact.country_of_origin, rule_supplier.country_of_origin) AS org
       ,COALESCE(rule_supplier_exact.country_of_origin, rule_supplier.country_of_origin) AS origin
       ,COALESCE(rule_product_exact.prod_id_1, rule_product.prod_id_1) AS prod_id_1
       ,COALESCE(rule_product_exact.tariff, rule_product.tariff) AS tariff
       ,COALESCE(rule_supplier_exact.relationship, rule_supplier.relationship) AS transaction_related
       ,COALESCE(rule_product_exact.template_hts_quantity, rule_product.template_hts_quantity) AS customs_qty
       ,COALESCE(rule_supplier_exact.dft, rule_supplier.dft) AS spi
       ,COALESCE(rule_product_exact.invoice_uom, rule_product.invoice_uom) AS uq
       ,COALESCE(rule_supplier_exact.[value], rule_supplier.[value]) AS price_unit
       ,COALESCE(rule_product_exact.template_invoice_quantity, rule_product.template_invoice_quantity) AS invoice_qty
       ,COALESCE(rule_product_exact.invoice_uom, rule_product.invoice_uom) AS invoice_qty_unit
       ,COALESCE(rule_supplier_exact.freight, rule_supplier.freight) * COALESCE(rule_product_exact.template_hts_quantity, rule_product.template_hts_quantity) AS amount
       ,COALESCE(rule_supplier_exact.[value], rule_supplier.[value]) * COALESCE(rule_product_exact.template_invoice_quantity, rule_product.template_invoice_quantity) AS line_price
       ,COALESCE(rule_product_exact.goods_description, rule_product.goods_description) AS description
       ,@operationId
      FROM dbo.imp_rail_filing_detail AS detail
      INNER JOIN dbo.imp_rail_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_rail_rule_port AS rule_port
        ON inbnd.port_of_unlading = rule_port.port
      LEFT JOIN dbo.imp_rail_rule_importer_supplier AS rule_supplier
        ON inbnd.importer = rule_supplier.importer_name
          AND rule_supplier.product_description IS NULL
          AND (inbnd.supplier = rule_supplier.supplier_name
            OR (inbnd.supplier IS NULL
              AND rule_supplier.supplier_name IS NULL))
      LEFT JOIN dbo.imp_rail_rule_importer_supplier AS rule_supplier_exact
        ON inbnd.importer = rule_supplier_exact.importer_name
          AND (inbnd.description1 = rule_supplier_exact.product_description)
          AND (inbnd.supplier = rule_supplier_exact.supplier_name
            OR (inbnd.supplier IS NULL
              AND rule_supplier_exact.supplier_name IS NULL))
      LEFT JOIN dbo.imp_rail_rule_product AS rule_product_exact
        ON rule_product_exact.description1 = inbnd.description1
          AND inbnd.port_of_unlading = rule_product_exact.port
          AND (
            (rule_supplier_exact.importer = rule_product_exact.importer
              AND rule_supplier_exact.main_supplier = rule_product_exact.supplier)
            OR (rule_supplier.importer = rule_product_exact.importer
              AND rule_supplier.main_supplier = rule_product_exact.supplier)
          )
      LEFT JOIN dbo.imp_rail_rule_product AS rule_product
        ON rule_product.description1 = inbnd.description1
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO
