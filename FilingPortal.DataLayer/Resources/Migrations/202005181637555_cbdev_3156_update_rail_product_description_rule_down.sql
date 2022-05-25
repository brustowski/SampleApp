ALTER VIEW dbo.v_imp_rail_inbound_grid 
AS SELECT
  inbound.id AS BD_Parsed_Id
 ,inbound.broker_download_id AS BD_Parsed_EDIMessage_Id
 ,filing_header.id AS Filing_Headers_id
 ,inbound.importer AS BD_Parsed_Importer
 ,inbound.supplier AS BD_Parsed_Supplier
 ,inbound.port_of_unlading AS BD_Parsed_PortOfUnlading
 ,inbound.description1 AS BD_Parsed_Description1
 ,inbound.bill_of_lading AS BD_Parsed_BillofLading
 ,inbound.issuer_code AS BD_Parsed_Issuer_Code
 ,CONCAT(inbound.equipment_initial, inbound.equipment_number) AS BD_Parsed_Container_Number
 ,inbound.reference_number1 AS BD_Parsed_ReferenceNumber1
 ,inbound.created_date AS BD_Parsed_CreatedDate
 ,inbound.deleted AS BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(inbound.duplicate_of, 0)) AS BD_Parsed_Is_Duplicated
 ,COALESCE(importer_supplier_exact.importer, importer_supplier.importer) AS Rule_ImporterSupplier_Importer
 ,COALESCE(importer_supplier_exact.main_supplier, importer_supplier.main_supplier) AS Rule_ImporterSupplier_Main_Supplier
 ,rail_description.tariff AS Rule_Desc1_Desc2_Tariff
 ,rail_port.[port] AS Rule_Port_Port
 ,filing_header.filing_number AS Filing_Headers_FilingNumber
 ,filing_header.job_hyperlink AS Filing_Headers_JobLink
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS Filing_Headers_MappingStatus
 ,mapping_status.[name] AS Filing_Headers_MappingStatus_Title
 ,filing_status.id AS Filing_Headers_FilingStatus
 ,filing_status.[name] AS Filing_Headers_FilingStatus_Title
 ,rail_description.[description]

FROM dbo.imp_rail_inbound inbound
LEFT JOIN dbo.imp_rail_rule_port rail_port
  ON inbound.port_of_unlading = rail_port.Port
LEFT JOIN dbo.imp_rail_rule_importer_supplier importer_supplier
  ON inbound.importer = importer_supplier.importer_name
    AND importer_supplier.product_description IS NULL
    AND (inbound.supplier = importer_supplier.supplier_name
      OR (inbound.supplier IS NULL
        AND importer_supplier.supplier_name IS NULL))
LEFT JOIN dbo.imp_rail_rule_importer_supplier importer_supplier_exact
  ON inbound.importer = importer_supplier_exact.importer_name
    AND inbound.description1 = importer_supplier_exact.product_description
    AND (inbound.supplier = importer_supplier_exact.supplier_name
      OR (inbound.supplier IS NULL
        AND importer_supplier_exact.supplier_name IS NULL))
LEFT JOIN dbo.imp_rail_rule_product rail_description
  ON rail_description.description1 = inbound.description1

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
  WHERE irfd.inbound_id = inbound.id
  AND irfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
  AND entry_status.status_type = 'import'
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
  FROM imp_rail_form_section_configuration ps
  WHERE ps.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_invoice_header pih
      WHERE pih.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_rail_invoice_header (
        parent_record_id
       ,filing_header_id
       ,consignee
       ,export
       ,importer
       ,manufacturer
       ,manufacturer_address
       ,origin
       ,seller
       ,ship_to_party
       ,sold_to_party
       ,supplier
       ,supplier_address
       ,transaction_related
       ,invoice_total
       ,operation_id)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,COALESCE(rn_exact.consignee, rn.consignee)
       ,rp.export
       ,COALESCE(rn_exact.importer, rn.importer)
       ,COALESCE(rn_exact.manufacturer, rn.manufacturer)
       ,COALESCE(rn_exact.manufacturer_address, rn.manufacturer_address)
       ,COALESCE(rn_exact.country_of_origin, rn.country_of_origin) AS origin
       ,COALESCE(rn_exact.seller, rn.seller)
       ,COALESCE(rn_exact.ship_to_party, rn.ship_to_party)
       ,COALESCE(rn_exact.sold_to_party, rn.sold_to_party)
       ,COALESCE(rn_exact.main_supplier, rn.main_supplier) AS supplier
       ,COALESCE(rn_exact.main_supplier_address, rn.main_supplier_address) AS supplier_address
       ,COALESCE(rn_exact.relationship, rn.relationship) AS transaction_related
       ,COALESCE(rn_exact.[value], rn.[value]) * rd.template_invoice_quantity
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
      LEFT JOIN dbo.imp_rail_rule_product rd
        ON RTRIM(LTRIM(rd.description1)) = RTRIM(LTRIM(p.description1))
      WHERE d.filing_header_id = @filingHeaderId

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
  FROM imp_rail_form_section_configuration ps
  WHERE ps.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_invoice_line pil
      WHERE pil.parent_record_id = @parentId)
  BEGIN
    INSERT INTO imp_rail_invoice_line (parent_record_id
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
       ,rd.attribute_1
       ,rd.attribute_2
       ,COALESCE(rn_exact.consignee, rn.consignee)
       ,COALESCE(rn_exact.destination_state, rn.destination_state)
       ,rp.export
       ,rd.goods_description
       ,COALESCE(rn_exact.manufacturer, rn.manufacturer)
       ,COALESCE(rn_exact.country_of_origin, rn.country_of_origin) AS org
       ,COALESCE(rn_exact.country_of_origin, rn.country_of_origin) AS origin
       ,rd.prod_id_1
       ,rd.tariff
       ,COALESCE(rn_exact.relationship, rn.relationship) AS transaction_related
       ,rd.template_hts_quantity AS customs_qty
       ,COALESCE(rn_exact.dft, rn.dft) AS spi
       ,rd.invoice_uom AS uq
       ,COALESCE(rn_exact.[value], rn.[value]) AS price_unit
       ,rd.template_invoice_quantity AS invoice_qty
       ,rd.invoice_uom AS invoice_qty_unit
       ,COALESCE(rn_exact.freight, rn.freight) AS amount
       ,COALESCE(rn_exact.[value], rn.[value]) * rd.template_invoice_quantity AS line_price
       ,rd.description AS description
       ,@operationId
      FROM dbo.imp_rail_filing_detail d
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = d.inbound_id
      LEFT JOIN dbo.imp_rail_rule_port rp
        ON p.port_of_unlading = rp.port
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn
        ON p.importer = rn.importer_name
		AND rn.product_description IS NULL
          AND (p.supplier = rn.supplier_name
            OR (p.supplier IS NULL
              AND rn.supplier_name IS NULL))
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn_exact
        ON p.importer = rn_exact.importer_name
          AND (p.description1 = rn_exact.product_description)
          AND (p.supplier = rn_exact.supplier_name
            OR (p.supplier IS NULL
              AND rn_exact.supplier_name IS NULL))
      LEFT JOIN dbo.imp_rail_rule_product rd
        ON RTRIM(LTRIM(rd.description1)) = RTRIM(LTRIM(p.description1))
      WHERE d.filing_header_id = @filingHeaderId
  END
END;
GO