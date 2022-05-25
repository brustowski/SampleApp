ALTER PROCEDURE dbo.sp_imp_rail_parse_broker_download
AS
BEGIN
  DECLARE @ParsedRecordsTable TABLE (
    id INT NOT NULL
   ,consignee NVARCHAR(200) NULL
   ,supplier NVARCHAR(200) NULL
   ,equipment_initial VARCHAR(4) NULL
   ,equipment_number NVARCHAR(10) NULL
   ,issuer_code VARCHAR(5) NULL
   ,bill_of_lading NVARCHAR(20) NULL
   ,port_of_unlading VARCHAR(4) NULL
   ,description1 NVARCHAR(500) NULL
   ,manifest_units VARCHAR(3) NULL
   ,weight NVARCHAR(10) NULL
   ,weight_unit VARCHAR(2) NULL
   ,reference_number1 NVARCHAR(50) NULL
   ,importer NVARCHAR(200) NULL
   ,destination VARCHAR(2) NULL
   ,modified_date DATETIME
   ,created_date DATETIME
  )
  INSERT INTO @ParsedRecordsTable (id
  , consignee
  , supplier
  , equipment_initial
  , equipment_number
  , issuer_code
  , bill_of_lading
  , port_of_unlading
  , description1
  , manifest_units
  , weight
  , weight_unit
  , reference_number1
  , importer
  , destination
  , modified_date
  , created_date)
    SELECT
      broker_download.id
     ,SUBSTRING(broker_download.em_message_text, AMS0N.Index1 + 5, 35) AS '0N1_Name'
     ,SUBSTRING(broker_download.em_message_text, AMS0N.Index2 + 5, 35) AS '0N2_Name'
     ,SUBSTRING(broker_download.em_message_text, AMS1C.id + 3, 4) AS 'EquipmentInitial'
     ,dbo.fn_fixLeadingZeros(SUBSTRING(broker_download.em_message_text, AMS1C.id + 7, 10), 6) AS 'EquipmentNumber'
     ,SUBSTRING(broker_download.em_message_text, AMS1J.id + 2, 4) AS 'IssuerCode_1J'
     ,SUBSTRING(broker_download.em_message_text, AMS1B.id + 2, 12) AS 'BillofLading'
     ,SUBSTRING(broker_download.em_message_text, AMS1P.id + 2, 4) AS 'PortofUnlading'
     ,RTRIM(COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index1 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index2 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index3 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index4 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index5 + 12, 45), '')) AS 'Description1'
     ,SUBSTRING(broker_download.em_message_text, AMS1B.id + 29, 5) AS 'ManifestUnits'
     ,CAST(SUBSTRING(broker_download.em_message_text, AMS1B.id + 34, 10) AS INT) AS 'Weight_1B'
     ,SUBSTRING(broker_download.em_message_text, AMS1B.id + 44, 2) AS 'WeightUnit_1B'
     ,SUBSTRING(broker_download.em_message_text, AMS4B.Index1 + 5, 30) AS 'ReferenceNumber1'
     ,SUBSTRING(broker_download.em_message_text, AMS0NIM.id + 5, 35) AS '0NIM_Name'
     ,SUBSTRING(broker_download.em_message_text, AMS0NC1.id + 181, 2) AS '0NC1DEST'
     ,broker_download.cw_last_modified_date
     ,broker_download.em_system_create_time_utc

    FROM dbo.imp_rail_broker_download AS broker_download
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%1P[0-9][0-9][0-9][0-9]%') AMS1P
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%1J[A-Z,0-9][A-Z,0-9][A-Z,0-9][A-Z,0-9]%') AMS1J
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%1B[0-9][0-9][0-9][0-9]%') AMS1B
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '% 1C[A-Z,0-9][A-Z,0-9]%') AMS1C
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%0D[A-Z,0-9][A-Z,0-9]%') AMS0D
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%0NIM%') AMS0NIM
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%0NC1%') AMS0NC1
    OUTER APPLY (SELECT
        [1] AS Index1
       ,[2] AS Index2
       ,[3] AS Index3
       ,[4] AS Index4
       ,[5] AS Index5
       ,[6] AS Index6
      FROM (SELECT
          id
         ,ROW_NUMBER() OVER (ORDER BY id ASC) AS RowNumber
        FROM dbo.utfn_findPOS(broker_download.em_message_text, '%0N[A-Z,0-9][A-Z,0-9]%') f) src
      PIVOT
      (
      SUM(id)
      FOR RowNumber IN ([1], [2], [3], [4], [5], [6])
      ) piv) AS AMS0N
    OUTER APPLY (SELECT
        [1] AS Index1
       ,[2] AS Index2
      FROM (SELECT
          id
         ,ROW_NUMBER() OVER (ORDER BY id ASC) AS RowNumber
        FROM dbo.utfn_findPOS(broker_download.em_message_text, '%4B[A-Z,0-9][A-Z,0-9]%') f) src
      PIVOT
      (
      SUM(id)
      FOR RowNumber IN ([1], [2])
      ) piv) AS AMS4B
    OUTER APPLY (SELECT
        [1] AS Index1
       ,[2] AS Index2
       ,[3] AS Index3
       ,[4] AS Index4
       ,[5] AS Index5
      FROM (SELECT
          id
         ,ROW_NUMBER() OVER (ORDER BY id ASC) AS RowNumber
        FROM dbo.utfn_findPOS(broker_download.em_message_text, '%1D[A-Z,0-9][A-Z,0-9]%') f) src
      PIVOT
      (
      SUM(id)
      FOR RowNumber IN ([1], [2], [3], [4], [5])
      ) piv) AS AMS1D

    WHERE NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_inbound AS inbnd
      WHERE inbnd.broker_download_id = broker_download.id)
    GROUP BY SUBSTRING(broker_download.em_message_text, AMS1P.id + 2, 4)
            ,SUBSTRING(broker_download.em_message_text, AMS1J.id + 2, 4)
            ,SUBSTRING(broker_download.em_message_text, AMS1B.id + 2, 12)
            ,SUBSTRING(broker_download.em_message_text, AMS1B.id + 29, 5)
            ,SUBSTRING(broker_download.em_message_text, AMS1B.id + 34, 10)
            ,SUBSTRING(broker_download.em_message_text, AMS1B.id + 44, 2)
            ,SUBSTRING(broker_download.em_message_text, AMS4B.Index1 + 5, 30)
            ,SUBSTRING(broker_download.em_message_text, AMS0N.Index1 + 5, 35)
            ,SUBSTRING(broker_download.em_message_text, AMS0N.Index2 + 5, 35)
            ,SUBSTRING(broker_download.em_message_text, AMS1C.id + 3, 4)
            ,SUBSTRING(broker_download.em_message_text, AMS1C.id + 7, 10)
            ,RTRIM(COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index1 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index2 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index3 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index4 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index5 + 12, 45), ''))
            ,SUBSTRING(broker_download.em_message_text, AMS1D.Index2 + 12, 45)
            ,SUBSTRING(broker_download.em_message_text, AMS0NIM.id + 5, 35)
            ,SUBSTRING(broker_download.em_message_text, AMS0NC1.id + 181, 2)
            ,broker_download.id
            ,broker_download.cw_last_modified_date
            ,broker_download.em_system_create_time_utc
    ORDER BY broker_download.cw_last_modified_date;

  DECLARE @id INT;
  DECLARE @consignee NVARCHAR(200);
  DECLARE @supplier NVARCHAR(200);
  DECLARE @equipment_initial VARCHAR(4);
  DECLARE @equipment_number NVARCHAR(10);
  DECLARE @issuer_code VARCHAR(5);
  DECLARE @bill_of_lading NVARCHAR(20);
  DECLARE @port_of_unlading VARCHAR(4);
  DECLARE @description1 NVARCHAR(500);
  DECLARE @manifest_units VARCHAR(3);
  DECLARE @weight NVARCHAR(10);
  DECLARE @weight_unit VARCHAR(2);
  DECLARE @reference_number1 NVARCHAR(50);
  DECLARE @importer NVARCHAR(200);
  DECLARE @destination VARCHAR(2);
  DECLARE @modified_date DATETIME;
  DECLARE @created_date DATETIME;

  DECLARE @found_id INT;
  DECLARE @mapping_status TINYINT;

  DECLARE c CURSOR FOR SELECT
    pt.id
   ,pt.consignee
   ,pt.supplier
   ,pt.equipment_initial
   ,pt.equipment_number
   ,pt.issuer_code
   ,pt.bill_of_lading
   ,pt.port_of_unlading
   ,pt.description1
   ,pt.manifest_units
   ,pt.weight
   ,pt.weight_unit
   ,pt.reference_number1
   ,pt.importer
   ,pt.destination
   ,pt.modified_date
   ,pt.created_date
  FROM @ParsedRecordsTable AS pt;
  OPEN c;
  FETCH NEXT FROM c INTO @id, @consignee, @supplier, @equipment_initial, @equipment_number, @issuer_code, @bill_of_lading, @port_of_unlading, @description1, @manifest_units, @weight, @weight_unit, @reference_number1, @importer, @destination, @modified_date, @created_date;
  WHILE @@fetch_status = 0
  BEGIN

  SET @found_id = NULL;


  -- let's find parsed value equal to new received data
  SELECT
    @found_id = rbp.id
  FROM dbo.imp_rail_inbound AS rbp
  WHERE rbp.equipment_initial = @equipment_initial
  AND rbp.equipment_number = @equipment_number
  AND rbp.bill_of_lading = @bill_of_lading
  AND rbp.port_of_unlading = @port_of_unlading
  AND rbp.reference_number1 = @reference_number1
  ORDER BY rbp.created_date DESC
  OFFSET 0 ROW FETCH NEXT 1 ROW ONLY;

  SET @mapping_status = NULL;

  -- found? let's see if status is not open
  IF (@found_id IS NOT NULL)
    SELECT
      @mapping_status = header.mapping_status
    FROM dbo.imp_rail_inbound AS rbp
    JOIN dbo.imp_rail_filing_detail AS detail
      ON rbp.id = detail.inbound_id
    JOIN dbo.imp_rail_filing_header AS header
      ON detail.filing_header_id = header.id
        AND header.mapping_status > 0
    WHERE rbp.id = @found_id;

  -- Record not found at all or found only mapped records
  IF (@found_id IS NULL
    OR @mapping_status IS NOT NULL)
  BEGIN
    INSERT INTO dbo.imp_rail_inbound (broker_download_id
    , consignee
    , supplier
    , equipment_initial
    , equipment_number
    , issuer_code
    , bill_of_lading
    , port_of_unlading
    , description1
    , manifest_units
    , weight
    , weight_unit
    , reference_number1
    , importer
    , destination
    , deleted
    , duplicate_of
    , cw_created_date_utc)
      VALUES (@id, @consignee, @supplier, @equipment_initial, @equipment_number, @issuer_code, @bill_of_lading, @port_of_unlading, @description1, @manifest_units, @weight, @weight_unit, @reference_number1, @importer, @destination, 0, @found_id -- null is not found or ID of original record
      , @created_date);
  END
  ELSE
  BEGIN
    -- record found, but wasn't processed. We need to update
    UPDATE dbo.imp_rail_inbound
    SET broker_download_id = @id
       ,importer = @importer
       ,supplier = @supplier
       ,equipment_initial = @equipment_initial
       ,equipment_number = @equipment_number
       ,issuer_code = @issuer_code
       ,bill_of_lading = @bill_of_lading
       ,port_of_unlading = @port_of_unlading
       ,description1 = @description1
       ,manifest_units = @manifest_units
       ,weight = @weight
       ,weight_unit = @weight_unit
       ,reference_number1 = @reference_number1
       ,consignee = @consignee
       ,destination = @destination
    WHERE id = @found_id;
  END

  FETCH NEXT FROM c INTO @id, @consignee, @supplier, @equipment_initial, @equipment_number, @issuer_code, @bill_of_lading, @port_of_unlading, @description1, @manifest_units, @weight, @weight_unit, @reference_number1, @importer, @destination, @modified_date, @created_date;
  END;
  CLOSE c;
  DEALLOCATE c;

  DELETE broker_download
    FROM dbo.imp_rail_broker_download AS broker_download
  WHERE NOT EXISTS (SELECT
        *
      FROM dbo.imp_rail_inbound inbnd
      WHERE inbnd.broker_download_id = broker_download.id);
END
GO

ALTER VIEW dbo.v_imp_rail_inbound_grid
AS
SELECT
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
  FROM imp_rail_rule_importer_supplier irris
  WHERE irris.importer_name = ImporterConsignee.result
  AND irris.supplier_name = inbnd.supplier
  AND (irris.product_description = inbnd.description1
  OR irris.product_description IS NULL)
  AND (irris.port = inbnd.port_of_unlading
  OR irris.port IS NULL)
  ORDER BY irris.product_description DESC, irris.port DESC) AS Rule_ImporterSupplier_Importer
OUTER APPLY (SELECT TOP (1)
    irrp.tariff
   ,irrp.goods_description
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
  ORDER BY irrp.importer, irrp.supplier, irrp.port DESC, irrp.destination DESC) AS Rule_Desc1_Desc2_Tariff

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
        ORDER BY irris.product_description DESC, irris.port DESC) AS Rule_ImporterSupplier
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
        ORDER BY irrp.importer, irrp.supplier, irrp.port DESC, irrp.destination DESC) AS Rule_Desc1

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
         ,irris.destination_state
         ,irris.dft
         ,irris.freight
         ,irris.freight_type
        FROM imp_rail_rule_importer_supplier irris
        WHERE irris.importer_name = ImporterConsignee.result
        AND irris.supplier_name = inbnd.supplier
        AND (irris.product_description = inbnd.description1
        OR irris.product_description IS NULL)
        AND (irris.port = inbnd.port_of_unlading
        OR irris.port IS NULL)
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
        ORDER BY irris.product_description DESC, irris.port DESC) AS Rule_ImporterSupplier

      WHERE d.filing_header_id = @filingHeaderId
  END
END;
GO