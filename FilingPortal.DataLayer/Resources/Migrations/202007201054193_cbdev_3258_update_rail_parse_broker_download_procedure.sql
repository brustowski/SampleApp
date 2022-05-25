ALTER PROCEDURE dbo.sp_imp_rail_parse_broker_download
AS
BEGIN
  DECLARE @ParsedRecordsTable TABLE (
    id INT NOT NULL
   ,importer NVARCHAR(200) NULL
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
   ,modified_date DATETIME
   ,created_date DATETIME
  )
  INSERT INTO @ParsedRecordsTable (
      id
     ,importer
     ,supplier
     ,equipment_initial
     ,equipment_number
     ,issuer_code
     ,bill_of_lading
     ,port_of_unlading
     ,description1
     ,manifest_units
     ,weight
     ,weight_unit
     ,reference_number1
     ,modified_date
     ,created_date)
    SELECT
      broker_download.id
     ,SUBSTRING(broker_download.em_message_text, AMS0NCN.ID + 5, 35) AS 'ConsigneeImporter'
     ,SUBSTRING(broker_download.em_message_text, AMS0NSH.ID + 5, 35) AS 'ShipperSupplier'
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
     ,broker_download.cw_last_modified_date
     ,broker_download.em_system_create_time_utc
    FROM dbo.imp_rail_broker_download AS broker_download
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%1P[0-9][0-9][0-9][0-9]%') AMS1P
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%1J[A-Z,0-9][A-Z,0-9][A-Z,0-9][A-Z,0-9]%') AMS1J
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%1B[0-9][0-9][0-9][0-9]%') AMS1B
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '% 1C[A-Z,0-9][A-Z,0-9]%') AMS1C
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%0D[A-Z,0-9][A-Z,0-9]%') AMS0D
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%0NSH%') AS AMS0NSH
    OUTER APPLY dbo.utfn_findPOS(broker_download.em_message_text, '%0NCN%') AS AMS0NCN
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
            ,SUBSTRING(broker_download.em_message_text, AMS0NCN.ID + 5, 35)
            ,SUBSTRING(broker_download.em_message_text, AMS0NSH.ID + 5, 35)
            ,SUBSTRING(broker_download.em_message_text, AMS1C.id + 3, 4)
            ,SUBSTRING(broker_download.em_message_text, AMS1C.id + 7, 10)
            ,RTRIM(COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index1 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index2 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index3 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index4 + 12, 45), '') + COALESCE(SUBSTRING(broker_download.em_message_text, AMS1D.Index5 + 12, 45), ''))
            ,SUBSTRING(broker_download.em_message_text, AMS1D.Index2 + 12, 45)
            ,broker_download.id
            ,broker_download.cw_last_modified_date
            ,broker_download.em_system_create_time_utc
    ORDER BY broker_download.cw_last_modified_date;

  DECLARE @id INT;
  DECLARE @importer NVARCHAR(200);
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
  DECLARE @modified_date DATETIME;
  DECLARE @created_date DATETIME;

  DECLARE @found_id INT;
  DECLARE @mapping_status TINYINT;

  DECLARE c CURSOR FOR SELECT
    pt.id
   ,pt.importer
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
   ,pt.modified_date
   ,pt.created_date
  FROM @ParsedRecordsTable AS pt;
  OPEN c;
  FETCH NEXT FROM c INTO @id, @importer, @supplier, @equipment_initial, @equipment_number, @issuer_code, @bill_of_lading, @port_of_unlading, @description1, @manifest_units, @weight, @weight_unit, @reference_number1, @modified_date, @created_date;
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
    INSERT INTO dbo.imp_rail_inbound (
        broker_download_id
       ,importer
       ,supplier
       ,equipment_initial
       ,equipment_number
       ,issuer_code
       ,bill_of_lading
       ,port_of_unlading
       ,description1
       ,manifest_units
       ,weight
       ,weight_unit
       ,reference_number1
       ,deleted
       ,duplicate_of
       ,cw_created_date_utc)
    VALUES (
      @id
     ,@importer
     ,@supplier
     ,@equipment_initial
     ,@equipment_number
     ,@issuer_code
     ,@bill_of_lading
     ,@port_of_unlading
     ,@description1
     ,@manifest_units
     ,@weight
     ,@weight_unit
     ,@reference_number1
     ,0
     ,@found_id -- null is not found or ID of original record
     ,@created_date);
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
    WHERE id = @found_id;
  END

  FETCH NEXT FROM c INTO @id, @importer, @supplier, @equipment_initial, @equipment_number, @issuer_code, @bill_of_lading, @port_of_unlading, @description1, @manifest_units, @weight, @weight_unit, @reference_number1, @modified_date, @created_date;
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

