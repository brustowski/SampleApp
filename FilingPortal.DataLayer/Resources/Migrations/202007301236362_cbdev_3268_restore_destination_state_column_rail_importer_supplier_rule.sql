ALTER PROCEDURE dbo.sp_imp_rail_add_declaration (@filingHeaderId INT,
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
       ,operation_id)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,p.issuer_code AS Carrier_SCAC
       ,rp.export AS Country_of_Export
       ,Rule_Desc1.goods_description
       ,rp.destination
       ,Rule_ImporterSupplier.destination_state
       ,p.port_of_unlading AS Discharge
       ,p.port_of_unlading AS Entry_Port
       ,rp.firms_code
       ,Rule_ImporterSupplier.importer
       ,p.issuer_code AS Issuer
       ,Rule_ImporterSupplier.main_supplier
       ,rp.origin
       ,@masterBill
       ,@operationId
      FROM dbo.imp_rail_filing_detail d
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = d.inbound_id
      LEFT JOIN dbo.imp_rail_rule_port rp
        ON p.port_of_unlading = rp.port
      CROSS APPLY (SELECT
          COALESCE(p.importer, p.consignee) AS result) AS ImporterConsignee
      OUTER APPLY (SELECT TOP (1)
          irris.importer
         ,irris.main_supplier
         ,irris.destination_state
        FROM imp_rail_rule_importer_supplier irris
        WHERE irris.importer_name = ImporterConsignee.result
        AND irris.supplier_name = p.supplier
        AND (irris.product_description = p.description1
        OR irris.product_description IS NULL)
        AND (irris.port = p.port_of_unlading
        OR irris.port IS NULL)
        AND (irris.destination = p.destination
        OR irris.destination IS NULL)
        ORDER BY irris.product_description DESC, irris.port DESC) AS Rule_ImporterSupplier
      OUTER APPLY (SELECT TOP (1)
          irrp.goods_description
        FROM imp_rail_rule_product irrp
        WHERE irrp.description1 = p.description1
        AND (irrp.importer = ImporterConsignee.result
        OR irrp.importer IS NULL)
        AND (irrp.supplier = p.supplier
        OR irrp.supplier IS NULL)
        AND (irrp.port = p.port_of_unlading
        OR irrp.port IS NULL)
        AND (irrp.destination = p.destination
        OR irrp.destination IS NULL)
        ORDER BY irrp.importer, irrp.supplier, irrp.port DESC, irrp.destination DESC) AS Rule_Desc1
      WHERE d.filing_header_id = @filingHeaderId
  END
END;
GO

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
       ,Rule_Desc1.attribute_1
       ,Rule_Desc1.attribute_2
       ,Rule_ImporterSupplier.consignee
       ,Rule_ImporterSupplier.destination_state
       ,rule_port.export
       ,Rule_Desc1.goods_description
       ,Rule_ImporterSupplier.manufacturer
       ,Rule_ImporterSupplier.country_of_origin
       ,Rule_ImporterSupplier.country_of_origin
       ,Rule_Desc1.prod_id_1
       ,Rule_Desc1.tariff
       ,Rule_ImporterSupplier.relationship
       ,Rule_Desc1.template_hts_quantity
       ,Rule_ImporterSupplier.dft
       ,Rule_Desc1.invoice_uom
       ,Rule_ImporterSupplier.value
       ,Rule_Desc1.template_invoice_quantity
       ,Rule_Desc1.invoice_uom
       ,IIF(Rule_ImporterSupplier.freight_type = 0, Rule_ImporterSupplier.freight, Rule_ImporterSupplier.freight * Rule_Desc1.template_invoice_quantity)
       ,Rule_ImporterSupplier.value * Rule_Desc1.template_invoice_quantity
       ,Rule_Desc1.goods_description
       ,@operationId
      FROM dbo.imp_rail_filing_detail AS detail
      INNER JOIN dbo.imp_rail_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_rail_rule_port AS rule_port
        ON inbnd.port_of_unlading = rule_port.port
      CROSS APPLY (SELECT
          COALESCE(inbnd.importer, inbnd.consignee) AS result) AS ImporterConsignee
      OUTER APPLY (SELECT TOP (1)
          irris.consignee
         ,irris.manufacturer
         ,irris.manufacturer_address
         ,irris.country_of_origin
         ,irris.relationship
         ,irris.value
         ,irris.dft
         ,irris.freight
         ,irris.freight_type
         ,irris.destination_state
        FROM imp_rail_rule_importer_supplier irris
        WHERE irris.importer_name = ImporterConsignee.result
        AND irris.supplier_name = inbnd.supplier
        AND (irris.product_description = inbnd.description1
        OR irris.product_description IS NULL)
        AND (irris.port = inbnd.port_of_unlading
        OR irris.port IS NULL)
        AND (irris.destination = inbnd.destination
        OR irris.destination IS NULL)
        ORDER BY irris.product_description DESC, irris.port DESC) AS Rule_ImporterSupplier
      OUTER APPLY (SELECT TOP (1)
          irrp.template_invoice_quantity
         ,irrp.attribute_1
         ,irrp.attribute_2
         ,irrp.goods_description
         ,prod_id_1
         ,irrp.tariff
         ,irrp.template_hts_quantity
         ,invoice_uom
        FROM imp_rail_rule_product irrp
        WHERE irrp.description1 = inbnd.description1
        AND (irrp.importer = ImporterConsignee.result
        OR irrp.importer IS NULL)
        AND (irrp.supplier = inbnd.supplier
        OR irrp.supplier IS NULL)
        AND (irrp.port = inbnd.port_of_unlading
        OR irrp.port IS NULL)
        AND (irrp.destination = inbnd.destination
        OR irrp.destination IS NULL)
        ORDER BY irrp.importer, irrp.supplier, irrp.port DESC, irrp.destination DESC) AS Rule_Desc1

      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO
