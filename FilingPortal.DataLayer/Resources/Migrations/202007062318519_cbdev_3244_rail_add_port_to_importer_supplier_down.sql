DROP INDEX IF EXISTS Idx__importer_name__supplier_name ON dbo.imp_rail_rule_importer_supplier
GO

CREATE UNIQUE INDEX Idx__importer_name__supplier_name
ON imp_rail_rule_importer_supplier (importer_name, supplier_name)
ON [PRIMARY]
GO

ALTER VIEW dbo.v_imp_rail_inbound_grid
AS
SELECT
  inbnd.id AS BD_Parsed_Id
 ,inbnd.broker_download_id AS BD_Parsed_EDIMessage_Id
 ,filing_header.id AS Filing_Headers_id
 ,inbnd.importer AS BD_Parsed_Importer
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
 ,Rule_ImporterSupplier_Importer.value AS Rule_ImporterSupplier_Importer
 ,Rule_ImporterSupplier_Main_Supplier.value AS Rule_ImporterSupplier_Main_Supplier
 ,Rule_Desc1_Desc2_Tariff.value AS Rule_Desc1_Desc2_Tariff
 ,rail_port.[port] AS Rule_Port_Port
 ,filing_header.filing_number AS Filing_Headers_FilingNumber
 ,filing_header.job_hyperlink AS Filing_Headers_JobLink
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS Filing_Headers_MappingStatus
 ,mapping_status.[name] AS Filing_Headers_MappingStatus_Title
 ,filing_status.id AS Filing_Headers_FilingStatus
 ,filing_status.[name] AS Filing_Headers_FilingStatus_Title
 ,COALESCE(rule_product_exact.goods_description, rule_product.goods_description) AS [description]
 ,(CAST(IIF(Rule_ImporterSupplier_Importer.value IS NOT NULL, 1, 0) & IIF(Rule_ImporterSupplier_Main_Supplier.value IS NOT NULL, 1, 0) & IIF(Rule_Desc1_Desc2_Tariff.value IS NOT NULL, 1, 0) AS BIT)) AS has_all_required_rules

FROM dbo.imp_rail_inbound AS inbnd
LEFT JOIN dbo.imp_rail_rule_port AS rail_port
  ON inbnd.port_of_unlading = rail_port.port
LEFT JOIN dbo.imp_rail_rule_importer_supplier AS importer_supplier
  ON inbnd.importer = importer_supplier.importer_name
    AND importer_supplier.product_description IS NULL
    AND (inbnd.supplier = importer_supplier.supplier_name
      OR (inbnd.supplier IS NULL
        AND importer_supplier.supplier_name IS NULL))
LEFT JOIN dbo.imp_rail_rule_importer_supplier AS importer_supplier_exact
  ON inbnd.importer = importer_supplier_exact.importer_name
    AND inbnd.description1 = importer_supplier_exact.product_description
    AND (inbnd.supplier = importer_supplier_exact.supplier_name
      OR (inbnd.supplier IS NULL
        AND importer_supplier_exact.supplier_name IS NULL))

LEFT JOIN dbo.imp_rail_rule_product AS rule_product_exact
  ON inbnd.description1 = rule_product_exact.description1
    AND inbnd.port_of_unlading = rule_product_exact.port
    AND inbnd.importer = rule_product_exact.importer
    AND inbnd.supplier = rule_product_exact.supplier

LEFT JOIN dbo.imp_rail_rule_product AS rule_product
  ON inbnd.description1 = rule_product.description1
    AND inbnd.importer = rule_product.importer
    AND inbnd.supplier = rule_product.supplier

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
    COALESCE(importer_supplier_exact.importer, importer_supplier.importer) AS value) AS Rule_ImporterSupplier_Importer
CROSS APPLY (SELECT
    COALESCE(importer_supplier_exact.main_supplier, importer_supplier.main_supplier) AS value) AS Rule_ImporterSupplier_Main_Supplier
CROSS APPLY (SELECT
    COALESCE(rule_product_exact.tariff, rule_product.tariff) AS value) AS Rule_Desc1_Desc2_Tariff

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'import'
GO

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
    INSERT INTO imp_rail_declaration (parent_record_id
    , filing_header_id
    , carrier_scac
    , country_of_export
    , description
    , destination
    , destination_state
    , discharge
    , entry_port
    , firms_code
    , importer
    , issuer
    , main_supplier
    , origin
    , master_bill
    , operation_id)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,p.issuer_code AS Carrier_SCAC
       ,rp.export AS Country_of_Export
       ,p.description1 AS Description
       ,rp.destination
       ,COALESCE(rn_exact.destination_state, rn.destination_state)
       ,p.port_of_unlading AS Discharge
       ,p.port_of_unlading AS Entry_Port
       ,rp.firms_code
       ,COALESCE(rn_exact.importer, rn.importer)
       ,p.issuer_code AS Issuer
       ,COALESCE(rn_exact.main_supplier, rn.main_supplier)
       ,rp.origin
       ,@masterBill
       ,@operationId
      FROM dbo.imp_rail_filing_detail d
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = d.inbound_id
      LEFT JOIN dbo.imp_rail_rule_port rp
        ON p.port_of_unlading = rp.port
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn
        ON RTRIM(LTRIM(p.importer)) = RTRIM(LTRIM(rn.importer_name))
          AND rn.product_description IS NULL
          AND (RTRIM(LTRIM(p.supplier)) = RTRIM(LTRIM(rn.supplier_name))
            OR (p.supplier IS NULL
              AND rn.supplier_name IS NULL))
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn_exact
        ON RTRIM(LTRIM(p.importer)) = RTRIM(LTRIM(rn_exact.importer_name))
          AND (p.description1 = rn_exact.product_description)
          AND (RTRIM(LTRIM(p.supplier)) = RTRIM(LTRIM(rn_exact.supplier_name))
            OR (p.supplier IS NULL
              AND rn_exact.supplier_name IS NULL))
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
       ,COALESCE(rule_supplier_exact.consignee, rule_supplier.consignee)
       ,rule_port.export
       ,COALESCE(rule_supplier_exact.importer, rule_supplier.importer)
       ,COALESCE(rule_supplier_exact.manufacturer, rule_supplier.manufacturer)
       ,COALESCE(rule_supplier_exact.manufacturer_address, rule_supplier.manufacturer_address)
       ,COALESCE(rule_supplier_exact.country_of_origin, rule_supplier.country_of_origin) AS origin
       ,COALESCE(rule_supplier_exact.seller, rule_supplier.seller)
       ,COALESCE(rule_supplier_exact.ship_to_party, rule_supplier.ship_to_party)
       ,COALESCE(rule_supplier_exact.sold_to_party, rule_supplier.sold_to_party)
       ,COALESCE(rule_supplier_exact.main_supplier, rule_supplier.main_supplier) AS supplier
       ,COALESCE(rule_supplier_exact.main_supplier_address, rule_supplier.main_supplier_address) AS supplier_address
       ,COALESCE(rule_supplier_exact.relationship, rule_supplier.relationship) AS transaction_related
       ,COALESCE(rule_supplier_exact.[value], rule_supplier.[value]) * COALESCE(rule_product_exact.template_invoice_quantity, rule_product.template_invoice_quantity)
       ,@operationId
      FROM dbo.imp_rail_filing_detail AS detail
      JOIN dbo.imp_rail_inbound AS inbnd
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
          AND inbnd.description1 = rule_supplier_exact.product_description
          AND (inbnd.supplier = rule_supplier_exact.supplier_name
            OR (inbnd.supplier IS NULL
              AND rule_supplier_exact.supplier_name IS NULL))
      LEFT JOIN dbo.imp_rail_rule_product AS rule_product_exact
        ON inbnd.description1 = rule_product_exact.description1
          AND inbnd.port_of_unlading = rule_product_exact.port
          AND inbnd.importer = rule_product_exact.importer
          AND inbnd.supplier = rule_product_exact.supplier

      LEFT JOIN dbo.imp_rail_rule_product AS rule_product
        ON inbnd.description1 = rule_product.description1
          AND inbnd.importer = rule_product.importer
          AND inbnd.supplier = rule_product.supplier
		  AND rule_product_exact.description1 IS NULL

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
    INSERT INTO dbo.imp_rail_invoice_line (parent_record_id
    , filing_header_id
    , attribute_1
    , attribute_2
    , consignee
    , dest_state
    , export
    , goods_description
    , manufacturer
    , org
    , origin
    , prod_id_1
    , tariff
    , transaction_related
    , customs_qty
    , spi
    , uq
    , price_unit
    , invoice_qty
    , invoice_qty_unit
    , amount
    , line_price
    , description
    , operation_id)
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
       ,COALESCE(rule_supplier_exact.freight, rule_supplier.freight) AS amount
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
        ON inbnd.description1 = rule_product_exact.description1
          AND inbnd.port_of_unlading = rule_product_exact.port
          AND inbnd.importer = rule_product_exact.importer
          AND inbnd.supplier = rule_product_exact.supplier

      LEFT JOIN dbo.imp_rail_rule_product AS rule_product
        ON inbnd.description1 = rule_product.description1
          AND inbnd.importer = rule_product.importer
          AND inbnd.supplier = rule_product.supplier
		  AND rule_product_exact.description1 IS NULL

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
       ,ISNULL(COALESCE(rn_exact.value_recon, rn.value_recon), 'N/A') AS recon_issue
       ,COALESCE(rn_exact.fta_recon, rn.fta_recon)
       ,COALESCE(rn_exact.payment_type, rn.payment_type)
       ,COALESCE(rn_exact.broker_to_pay, rn.broker_to_pay)
       ,COALESCE(rn_exact.importer, rn.importer) AS submitter
       ,user_data.Branch
       ,user_data.Broker
       ,user_data.Location
       ,@operationId
      FROM dbo.imp_rail_filing_detail d
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = d.inbound_id
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn
        ON RTRIM(LTRIM(p.importer)) = RTRIM(LTRIM(rn.importer_name))
		AND rn.product_description IS NULL
          AND (RTRIM(LTRIM(p.supplier)) = RTRIM(LTRIM(rn.supplier_name))
            OR (p.supplier IS NULL
              AND rn.supplier_name IS NULL))
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn_exact
        ON RTRIM(LTRIM(p.importer)) = RTRIM(LTRIM(rn_exact.importer_name))
          AND (p.description1 = rn_exact.product_description)
          AND (RTRIM(LTRIM(p.supplier)) = RTRIM(LTRIM(rn_exact.supplier_name))
            OR (p.supplier IS NULL
              AND rn_exact.supplier_name IS NULL))
      LEFT JOIN app_users_data user_data
        ON user_data.UserAccount = @filingUser
      WHERE d.filing_header_id = @filingHeaderId
  END
END;
GO