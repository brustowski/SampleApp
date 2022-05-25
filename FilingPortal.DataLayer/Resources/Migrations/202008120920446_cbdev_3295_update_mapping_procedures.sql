ALTER VIEW dbo.v_imp_rail_inbound_grid 
AS SELECT
  inbnd.id AS BD_Parsed_Id
 ,inbnd.broker_download_id AS BD_Parsed_EDIMessage_Id
 ,filing_header.id AS Filing_Headers_id
 ,inbnd.importer AS BD_Parsed_Importer
 ,inbnd.consignee AS BD_Parser_Consignee
 ,ImporterConsignee.result AS DB_Parsed_ImporterConsignee
 ,inbnd.supplier AS BD_Parsed_Supplier
 ,inbnd.port_of_unlading AS BD_Parsed_PortOfUnlading
 ,inbnd.description1 AS BD_Parsed_Description1
 ,inbnd.bill_of_lading AS BD_Parsed_BillofLading
 ,inbnd.issuer_code AS BD_Parsed_Issuer_Code
 ,CONCAT(inbnd.equipment_initial, inbnd.equipment_number) AS BD_Parsed_Container_Number
 ,inbnd.reference_number1 AS BD_Parsed_ReferenceNumber1
 ,inbnd.created_date AS BD_Parsed_CreatedDate
 ,inbnd.deleted AS BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(inbnd.duplicate_of, 0)) AS BD_Parsed_Is_Duplicated
 ,Rule_ImporterSupplier_Importer.importer AS Rule_ImporterSupplier_Importer
 ,Rule_ImporterSupplier_Importer.main_supplier AS Rule_ImporterSupplier_Main_Supplier
 ,Rule_Desc1_Desc2_Tariff.tariff AS Rule_Desc1_Desc2_Tariff
 ,rail_port.[port] AS Rule_Port_Port
 ,filing_header.filing_number AS Filing_Headers_FilingNumber
 ,filing_header.job_hyperlink AS Filing_Headers_JobLink
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS Filing_Headers_MappingStatus
 ,mapping_status.[name] AS Filing_Headers_MappingStatus_Title
 ,filing_status.id AS Filing_Headers_FilingStatus
 ,filing_status.[name] AS Filing_Headers_FilingStatus_Title
 ,inbnd.destination
 ,Rule_Desc1_Desc2_Tariff.goods_description AS [description]
 ,(CAST(IIF(Rule_ImporterSupplier_Importer.importer IS NOT NULL, 1, 0) & IIF(Rule_ImporterSupplier_Importer.main_supplier IS NOT NULL, 1, 0) & IIF(Rule_Desc1_Desc2_Tariff.tariff IS NOT NULL, 1, 0) AS BIT)) AS has_all_required_rules

FROM dbo.imp_rail_inbound AS inbnd
LEFT JOIN dbo.imp_rail_rule_port AS rail_port
  ON inbnd.port_of_unlading = rail_port.port
OUTER APPLY (SELECT
    irfh.id
   ,irfh.filing_number
   ,irfh.job_hyperlink
   ,irfh.entry_status
   ,irfh.mapping_status
   ,irfh.filing_status
  FROM dbo.imp_rail_filing_header irfh
  JOIN dbo.imp_rail_filing_detail irfd
    ON irfh.id = irfd.filing_header_id
  WHERE irfd.inbound_id = inbnd.id
  AND irfh.mapping_status > 0) AS filing_header
CROSS APPLY (SELECT
    COALESCE(inbnd.importer, inbnd.consignee) AS result) AS ImporterConsignee
OUTER APPLY (SELECT TOP (1)
    irris.importer
   ,irris.main_supplier
  FROM dbo.imp_rail_rule_importer_supplier AS irris
  WHERE irris.importer_name = ImporterConsignee.result
  AND irris.supplier_name = inbnd.supplier
  AND (irris.product_description = inbnd.description1
  OR irris.product_description IS NULL
  )
  AND (irris.port = inbnd.port_of_unlading
  OR irris.port IS NULL)
  AND (irris.destination = inbnd.destination
  OR irris.destination IS NULL)
  ORDER BY irris.product_description DESC, irris.port DESC, irris.destination DESC) AS Rule_ImporterSupplier_Importer
OUTER APPLY (SELECT TOP (1)
    irrp.tariff
   ,irrp.goods_description
  FROM dbo.imp_rail_rule_product AS irrp
  WHERE irrp.description1 = inbnd.description1
  AND (irrp.importer = ImporterConsignee.result
  OR irrp.importer IS NULL)
  AND (irrp.supplier = inbnd.supplier
  OR irrp.supplier IS NULL)
  AND (irrp.port = inbnd.port_of_unlading
  OR irrp.port IS NULL)
  AND (irrp.destination = inbnd.destination
  OR irrp.destination IS NULL)
  ORDER BY irrp.importer DESC, irrp.supplier DESC, irrp.port DESC, irrp.destination DESC) AS Rule_Desc1_Desc2_Tariff

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'import'
GO

-- add rail declaration record --
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
        ORDER BY irris.product_description DESC, irris.port DESC, irris.destination DESC) AS Rule_ImporterSupplier
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
        ORDER BY irrp.importer DESC, irrp.supplier DESC, irrp.port DESC, irrp.destination DESC) AS Rule_Desc1
      WHERE d.filing_header_id = @filingHeaderId
  END
END;
GO

-- add rail invoice header record --
ALTER PROCEDURE dbo.sp_imp_rail_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_rail_invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM dbo.imp_rail_form_section_configuration AS ps
  WHERE ps.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_invoice_header AS pih
      WHERE pih.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.imp_rail_invoice_header (parent_record_id
    , filing_header_id
    , consignee
    , export
    , importer
    , manufacturer
    , manufacturer_address
    , origin
    , seller
    , ship_to_party
    , sold_to_party
    , supplier
    , supplier_address
    , transaction_related
    , invoice_total
    , operation_id)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,Rule_ImporterSupplier.consignee
       ,rule_port.export
       ,Rule_ImporterSupplier.importer
       ,Rule_ImporterSupplier.manufacturer
       ,Rule_ImporterSupplier.manufacturer_address
       ,Rule_ImporterSupplier.country_of_origin
       ,Rule_ImporterSupplier.seller
       ,Rule_ImporterSupplier.ship_to_party
       ,Rule_ImporterSupplier.sold_to_party
       ,Rule_ImporterSupplier.main_supplier
       ,Rule_ImporterSupplier.main_supplier_address
       ,Rule_ImporterSupplier.relationship
       ,Rule_ImporterSupplier.[value] * Rule_Desc1.template_invoice_quantity
       ,@operationId
      FROM dbo.imp_rail_filing_detail AS detail
      JOIN dbo.imp_rail_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_rail_rule_port AS rule_port
        ON inbnd.port_of_unlading = rule_port.port
      CROSS APPLY (SELECT
          COALESCE(inbnd.importer, inbnd.consignee) AS result) AS ImporterConsignee
      OUTER APPLY (SELECT TOP (1)
          irris.consignee
         ,irris.importer
         ,irris.manufacturer
         ,irris.manufacturer_address
         ,irris.country_of_origin
         ,irris.seller
         ,irris.ship_to_party
         ,irris.sold_to_party
         ,irris.main_supplier
         ,irris.main_supplier_address
         ,irris.relationship
         ,irris.value
        FROM imp_rail_rule_importer_supplier irris
        WHERE irris.importer_name = ImporterConsignee.result
        AND irris.supplier_name = inbnd.supplier
        AND (irris.product_description = inbnd.description1
        OR irris.product_description IS NULL)
        AND (irris.port = inbnd.port_of_unlading
        OR irris.port IS NULL)
        AND (irris.destination = inbnd.destination
        OR irris.destination IS NULL)
        ORDER BY irris.product_description DESC, irris.port DESC, irris.destination DESC) AS Rule_ImporterSupplier
      OUTER APPLY (SELECT TOP (1)
          irrp.template_invoice_quantity
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
        ORDER BY irrp.importer DESC, irrp.supplier DESC, irrp.port DESC, irrp.destination DESC) AS Rule_Desc1

      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.sp_imp_rail_add_invoice_line @filingHeaderId
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
        ORDER BY irris.product_description DESC, irris.port DESC, irris.destination DESC) AS Rule_ImporterSupplier
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
        ORDER BY irrp.importer DESC, irrp.supplier DESC, irrp.port DESC, irrp.destination DESC) AS Rule_Desc1

      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

-- add rail misc record --
ALTER PROCEDURE dbo.sp_imp_rail_add_misc (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_rail_misc';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM imp_rail_form_section_configuration ps
  WHERE ps.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_misc pm
      WHERE pm.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_rail_misc (parent_record_id
    , filing_header_id
    , recon_issue
    , fta_recon
    , payment_type
    , broker_to_pay
    , submitter
    , branch
    , broker
    , preparer_dist_port
    , operation_id)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,ISNULL(Rule_ImporterSupplier.value_recon, 'N/A')
       ,Rule_ImporterSupplier.fta_recon
       ,Rule_ImporterSupplier.payment_type
       ,Rule_ImporterSupplier.broker_to_pay
       ,Rule_ImporterSupplier.importer
       ,user_data.Branch
       ,user_data.Broker
       ,user_data.Location
       ,@operationId
      FROM dbo.imp_rail_filing_detail d
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = d.inbound_id
      LEFT JOIN app_users_data user_data
        ON user_data.UserAccount = @filingUser
      CROSS APPLY (SELECT
          COALESCE(p.importer, p.consignee) AS result) AS ImporterConsignee
      OUTER APPLY (SELECT TOP (1)
          irris.value_recon
         ,irris.fta_recon
         ,irris.payment_type
         ,irris.broker_to_pay
         ,irris.importer
        FROM imp_rail_rule_importer_supplier irris
        WHERE irris.importer_name = ImporterConsignee.result
        AND irris.supplier_name = p.supplier
        AND (irris.product_description = p.description1
        OR irris.product_description IS NULL)
        AND (irris.port = p.port_of_unlading
        OR irris.port IS NULL)
        AND (irris.destination = p.destination
        OR irris.destination IS NULL)
        ORDER BY irris.product_description DESC, irris.port DESC, irris.destination DESC) AS Rule_ImporterSupplier

      WHERE d.filing_header_id = @filingHeaderId
  END
END;
GO

