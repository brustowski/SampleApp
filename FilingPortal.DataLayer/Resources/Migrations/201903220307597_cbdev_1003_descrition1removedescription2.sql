SET ARITHABORT OFF
go
ALTER VIEW [dbo].[Rail_Inbound_Grid]
AS
SELECT DISTINCT
  bd_parsed.BDP_PK BD_Parsed_Id
 ,bd_parsed.BDP_EM BD_Parsed_EDIMessage_Id
 ,filing_headers.id Filing_Headers_id
 ,bd_parsed.Importer BD_Parsed_Importer
 ,bd_parsed.Supplier BD_Parsed_Supplier
 ,bd_parsed.PortOfUnlading BD_Parsed_PortOfUnlading
 ,bd_parsed.Description1 BD_Parsed_Description1
 ,bd_parsed.BillofLading BD_Parsed_BillofLading
 ,bd_parsed.IssuerCode BD_Parsed_Issuer_Code
 ,CONCAT(bd_parsed.EquipmentInitial, bd_parsed.EquipmentNumber) AS BD_Parsed_Container_Number
 ,bd_parsed.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,bd_parsed.CreatedDate BD_Parsed_CreatedDate
 ,bd_parsed.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(bd_parsed.DuplicateOf, 0)) BD_Parsed_Is_Duplicated
 ,importer_supplier.Importer Rule_ImporterSupplier_Importer
 ,importer_supplier.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,rail_description.Tariff Rule_Desc1_Desc2_Tariff
 ,rail_port.Port Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.name Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.name Filing_Headers_FilingStatus_Title
 ,rail_description.Description
 ,filing_headers.ErrorDescription

FROM dbo.Rail_BD_Parsed bd_parsed
LEFT JOIN dbo.Rail_Rule_Port rail_port
  ON bd_parsed.PortOfUnlading = rail_port.Port
LEFT JOIN dbo.Rail_Rule_ImporterSupplier importer_supplier
  ON bd_parsed.Importer = importer_supplier.Importer_Name
    AND (bd_parsed.Supplier = importer_supplier.Supplier_Name
      OR (bd_parsed.Supplier IS NULL
        AND importer_supplier.Supplier_Name IS NULL))
LEFT JOIN dbo.Rail_Rule_Desc1_Desc2 rail_description
  ON rail_description.Description1 = bd_parsed.Description1 
LEFT JOIN dbo.Rail_Filing_Details filing_details
  ON filing_details.BDP_FK = bd_parsed.BDP_PK
LEFT JOIN dbo.Rail_Filing_Headers filing_headers
  ON filing_headers.id = filing_details.Filing_Headers_FK
    AND filing_headers.MappingStatus <> 0
LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_headers.MappingStatus, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_headers.FilingStatus, 0) = filing_status.id
WHERE NOT EXISTS (SELECT
    *
  FROM dbo.Rail_Filing_Headers filing_headers
  INNER JOIN dbo.Rail_Filing_Details filing_details
    ON filing_headers.id = filing_details.Filing_Headers_FK
  WHERE filing_headers.MappingStatus > 0
  AND bd_parsed.BDP_PK = filing_details.BDP_FK)

UNION ALL

SELECT
  filing_details.BDP_FK BD_Parsed_Id
 ,bd_parsed.BDP_EM BD_Parsed_EDIMessage_Id
 ,filing_headers.id Filing_Headers_id
 ,bd_parsed.Importer BD_Parsed_Importer
 ,bd_parsed.Supplier BD_Parsed_Supplier
 ,declaration_tab.Entry_Port BD_Parsed_PortOfUnlading
 ,declaration_tab.Description BD_Parsed_Description1 
 ,declaration_tab.Master_Bill BD_Parsed_BillofLading
 ,bd_parsed.IssuerCode BD_Parsed_Issuer_Code
 ,containers_tab.Container_Number AS BD_Parsed_Container_Number
 ,bd_parsed.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,bd_parsed.CreatedDate BD_Parsed_CreatedDate
 ,bd_parsed.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(bd_parsed.DuplicateOf, 0)) BD_Parsed_Is_Duplicated
 ,declaration_tab.Importer Rule_ImporterSupplier_Importer
 ,declaration_tab.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,invoice_lines.Tariff Rule_Desc1_Desc2_Tariff
 ,declaration_tab.Entry_Port Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.name Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.name Filing_Headers_FilingStatus_Title
 ,invoice_lines.Description
 ,filing_headers.ErrorDescription

FROM dbo.Rail_Filing_Headers filing_headers
INNER JOIN dbo.Rail_Filing_Details filing_details
  ON filing_headers.id = filing_details.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed bd_parsed
  ON filing_details.BDP_FK = bd_parsed.BDP_PK
LEFT JOIN dbo.MappingStatus mapping_status
  ON filing_headers.MappingStatus = mapping_status.id
LEFT JOIN dbo.FilingStatus filing_status
  ON filing_headers.FilingStatus = filing_status.id
LEFT JOIN dbo.Rail_DeclarationTab declaration_tab
  ON declaration_tab.Filing_Headers_FK = filing_headers.id
    AND filing_details.BDP_FK = declaration_tab.BDP_FK
LEFT JOIN dbo.Rail_ContainersTab containers_tab
  ON containers_tab.Filing_Headers_FK = filing_headers.id
    AND filing_details.BDP_FK = containers_tab.BDP_FK
LEFT JOIN dbo.Rail_InvoiceLines invoice_lines
  ON invoice_lines.Filing_Headers_FK = filing_headers.id
    AND filing_details.BDP_FK = invoice_lines.BDP_FK
WHERE filing_headers.MappingStatus > 0
Go

ALTER PROCEDURE [dbo].[rail_filing] (@Filing_Headers_id INT)
AS
BEGIN
  SET DATEFORMAT mdy;
  DECLARE @str VARCHAR(MAX) = '';
  -- RAIL_DECLARATIONTAB---------------------------------------
  INSERT INTO RAIL_DECLARATIONTAB (BDP_FK, Carrier_SCAC, Country_of_Export, Description, Destination, Destination_State, Discharge, Entry_Port, FIRMs_Code, Importer, Issuer, Main_Supplier, Master_Bill, Origin, FILING_HEADERS_FK)
    SELECT
      d.BDP_FK
     ,p.IssuerCode AS Carrier_SCAC
     ,rp.Export AS Country_of_Export
     ,Description1 AS Description
     ,rp.Destination AS Destination
     ,rn.Destination_State AS Destination_State
     ,p.PortOfUnlading AS Discharge
     ,p.PortOfUnlading AS Entry_Port
     ,rp.FIRMs_Code AS FIRMs_Code
     ,rn.Importer AS Importer
     ,p.IssuerCode AS Issuer
     ,rn.Main_Supplier AS Main_Supplier
     ,p.BillofLading AS Master_Bill
     ,rp.Origin AS Origin
     ,d.FILING_HEADERS_FK
    FROM dbo.Rail_Filing_Details d
    INNER JOIN dbo.Rail_BD_Parsed p
      ON p.BDP_PK = d.BDP_FK
    LEFT JOIN dbo.Rail_Rule_Port rp
      ON p.PortOfUnlading = RP.Port
    LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
      ON RTRIM(LTRIM(p.Importer)) = RTRIM(LTRIM(rn.Importer_Name))
        AND (RTRIM(LTRIM(p.Supplier)) = RTRIM(LTRIM(rn.Supplier_Name))
          OR (p.Supplier IS NULL
            AND rn.Supplier_Name IS NULL))
    WHERE NOT EXISTS (SELECT
        *
      FROM RAIL_DECLARATIONTAB r
      WHERE r.Filing_Headers_FK = d.Filing_Headers_FK
      AND d.BDP_FK = r.BDP_FK)
    AND d.Filing_Headers_FK = @Filing_Headers_id
  -- PRINT 'RAIL_DECLARATIONTAB'
  -- RAIL_CONTAINERSTAB---------------------------------------
  INSERT INTO RAIL_CONTAINERSTAB (BDP_FK, Bill_Issuer_SCAC, Bill_Num, Bill_Number, Container_Number, FILING_HEADERS_FK)
    SELECT
      d.BDP_FK
     ,p.IssuerCode AS Bill_Issuer_SCAC
     ,p.BillofLading AS Bill_Num
     ,CONCAT('MB:', p.BillofLading) AS Bill_Number
     ,CONCAT(EquipmentInitial, EquipmentNumber) AS Container_Number
     ,d.FILING_HEADERS_FK
    FROM dbo.Rail_Filing_Details d
    INNER JOIN dbo.Rail_BD_Parsed p
      ON p.BDP_PK = d.BDP_FK
    WHERE NOT EXISTS (SELECT
        *
      FROM RAIL_CONTAINERSTAB r
      WHERE r.Filing_Headers_FK = d.Filing_Headers_FK
      AND d.BDP_FK = r.BDP_FK)
    AND d.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'RAIL_CONTAINERSTAB'
  -- RAIL_INVOICELINES---------------------------------------
  INSERT INTO RAIL_INVOICELINES (BDP_FK, Attribute_1, Attribute_2, Gr_Weight, Gr_Weight_Unit, Consignee, Dest_State, Export, Goods_Description, InvoiceHeaders_FK, Manufacturer, ORG, Origin,
  Prod_ID_1, Tariff, Transaction_Related, Customs_QTY, SPI, UQ, PriceUnit, Invoice_Qty, Invoice_Qty_Unit, Amount, Line_Price, Description, FILING_HEADERS_FK)
    SELECT
      d.BDP_FK
     ,rd.Attribute_1 AS Attribute_1
     ,rd.Attribute_2 AS Attribute_2
     ,p.Weight AS Gr_Weight
     ,p.WeightUnit AS Gr_Weight_Unit
     ,rn.Consignee AS Consignee
     ,rn.Destination_State AS Dest_State
     ,rp.Export AS Export
     ,rd.Goods_Description AS Goods_Description
     ,ISNULL(ih.max_id, 0) + DENSE_RANK() OVER (ORDER BY p.Importer, p.Supplier, p.ReferenceNumber1, PortofUnlading) AS InvoiceHeaders_FK
     ,rn.Manufacturer AS Manufacturer
     ,rn.CountryofOrigin AS ORG
     ,rn.CountryofOrigin AS Origin
     ,rd.Prod_ID_1 AS Prod_ID_1
     ,rd.Tariff AS Tariff
     ,rn.Relationship AS Transaction_Related
     ,rd.Template_Invoice_Quantity AS Customs_QTY
     ,rn.DFT AS SPI
     ,rd.Invoice_UOM AS UQ
     ,rn.Value AS PriceUnit
     ,rd.Template_Invoice_Quantity AS Invoice_Qty
     ,rd.Invoice_UOM AS Invoice_Qty_Unit
     ,rn.Freight AS Amount
     ,rn.Value * rd.Template_Invoice_Quantity AS Line_Price
     ,rd.Description AS Description
     ,d.FILING_HEADERS_FK
    FROM dbo.Rail_Filing_Details d
         INNER JOIN dbo.Rail_BD_Parsed p
           ON p.BDP_PK = d.BDP_FK
         LEFT JOIN dbo.Rail_Rule_Port rp
           ON p.PortOfUnlading = RP.Port
         LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
           ON p.Importer = rn.Importer_Name
             AND (p.Supplier = rn.Supplier_Name
               OR (p.Supplier IS NULL
                 AND rn.Supplier_Name IS NULL))
         LEFT JOIN dbo.Rail_Rule_Desc1_Desc2 rd
           ON RTRIM(LTRIM(rd.Description1)) = RTRIM(LTRIM(p.Description1))
            
        ,(SELECT
             MAX(id) max_id
           FROM Rail_InvoiceHeaders) ih
    WHERE NOT EXISTS (SELECT
        *
      FROM RAIL_INVOICELINES r
      WHERE r.Filing_Headers_FK = d.Filing_Headers_FK
      AND d.BDP_FK = r.BDP_FK)
    AND d.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'RAIL_INVOICELINES'
  -- RAIL_INVOICEHEADERS---------------------------------------
  INSERT INTO RAIL_INVOICEHEADERS (Consignee, Export, Importer, Manufacturer, Origin, Seller, Ship_to_party, Sold_to_party, Supplier,
  Transaction_Related, Invoice_Total, FILING_HEADERS_FK, id)
    SELECT DISTINCT
      rn.Consignee AS Consignee
     ,rp.Export AS Export
     ,rn.Importer AS Importer
     ,rn.Manufacturer AS Manufacturer
     ,rn.CountryofOrigin AS Origin
     ,rn.Seller AS Seller
     ,rn.Ship_to_party AS Ship_to_party
     ,rn.Sold_to_party AS Sold_to_party
     ,rn.Main_Supplier AS Supplier
     ,rn.Relationship AS Transaction_Related
     ,ri.PriceUnit * ri.Customs_QTY AS Invoice_Total
     ,d.FILING_HEADERS_FK
     ,ri.InvoiceHeaders_FK AS id
    FROM dbo.Rail_Filing_Details d
    INNER JOIN dbo.Rail_BD_Parsed p
      ON p.BDP_PK = d.BDP_FK
    LEFT JOIN dbo.Rail_Rule_Port rp
      ON p.PortOfUnlading = RP.Port
    LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
      ON p.Importer = rn.Importer_Name
        AND (p.Supplier = rn.Supplier_Name
          OR (p.Supplier IS NULL
            AND rn.Supplier_Name IS NULL))
    INNER JOIN RAIL_INVOICELINES ri
      ON ri.Filing_Headers_FK = d.Filing_Headers_FK
        AND d.BDP_FK = ri.BDP_FK
    WHERE NOT EXISTS (SELECT
        *
      FROM RAIL_INVOICEHEADERS r
      WHERE r.Filing_Headers_FK = d.Filing_Headers_FK
      AND ri.InvoiceHeaders_FK = r.id)
    AND d.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'RAIL_INVOICEHEADERS'
  -- Rail_MISC---------------------------------------
  INSERT INTO Rail_MISC (BDP_FK, Recon_Issue, FTA_Recon, Payment_Type, Broker_to_Pay, Submitter,
  Filing_Headers_FK)
    SELECT
      d.BDP_FK
     ,ISNULL(rn.Value_Recon, 'N/A') AS Recon_Issue
     ,rn.FTA_Recon AS FTA_Recon
     ,rn.Payment_Type AS Payment_Type
     ,rn.Broker_to_Pay AS Broker_to_Pay
     ,rn.Importer AS Submitter
     ,d.FILING_HEADERS_FK
    FROM dbo.Rail_Filing_Details d
    INNER JOIN dbo.Rail_BD_Parsed p
      ON p.BDP_PK = d.BDP_FK
    LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
      ON RTRIM(LTRIM(p.Importer)) = RTRIM(LTRIM(rn.Importer_Name))
        AND (RTRIM(LTRIM(p.Supplier)) = RTRIM(LTRIM(rn.Supplier_Name))
          OR (p.Supplier IS NULL
            AND rn.Supplier_Name IS NULL))
    WHERE NOT EXISTS (SELECT
        *
      FROM Rail_MISC r
      WHERE r.Filing_Headers_FK = d.Filing_Headers_FK
      AND d.BDP_FK = r.BDP_FK)
    AND d.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'Rail_MISC'

  DECLARE @str_val NVARCHAR(500);
  DECLARE @ParmDefinition NVARCHAR(500);
  DECLARE @DefvalOUT VARCHAR(128);

  DECLARE @datatype VARCHAR(128)
         ,@DefValue VARCHAR(500)
         ,@id INT;
  DECLARE c CURSOR FOR SELECT DISTINCT
    data_type +
    CASE
      WHEN data_type IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN data_type IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN data_type IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
   ,DefValue
   ,v.id
  FROM information_schema.columns i
  LEFT JOIN dbo.Rail_DEFValues v
    ON UPPER(i.column_name) = UPPER(v.ColName)
    AND UPPER(i.table_name) = UPPER(v.TableName)
  WHERE i.table_schema = 'dbo'
  AND (FManual > 0
  OR FHasDefaultVal > 0
  OR Display_on_UI > 0)
  AND NOT EXISTS (SELECT
      *
    FROM Rail_DEFValues_Manual r
    WHERE r.Filing_Headers_FK = @Filing_Headers_id)
  ORDER BY id
  OPEN c
  FETCH NEXT FROM c INTO @datatype, @DefValue, @id
  WHILE @@fetch_status = 0
  BEGIN
  SET @DefvalOUT = NULL;
  SET @str_val = N'SELECT @valOUT = try_cast(try_cast(''' + @DefValue + ''' as ' + @datatype + ') as  varchar(500))';
  SET @ParmDefinition = N'@valOUT varchar(500) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@ParmDefinition
                       ,@valOUT = @DefvalOUT OUTPUT;
  IF @DefvalOUT IS NULL
  BEGIN
    SET @str_val = N'SELECT @valOUT = try_cast(try_cast(' + @DefValue + ' as ' + @datatype + ') as  varchar(500))';
    SET @ParmDefinition = N'@valOUT varchar(500) OUTPUT';

    DECLARE @result FLOAT
  BEGIN TRY
    EXECUTE sp_executesql @str_val
                         ,@ParmDefinition
                         ,@valOUT = @DefvalOUT OUTPUT;
  END TRY
  BEGIN CATCH
    SET @DefvalOUT = NULL;
  END CATCH;
  END;
  --PRINT concat(@id,' - ',@DefvalOUT, ' ', @datatype);

  IF @datatype LIKE 'date%'
  BEGIN
    SET @DefvalOUT = FORMAT(TRY_CAST(@DefvalOUT AS DATETIME), 'MM/dd/yyyy');
  END;
  IF @datatype LIKE 'numeric'
  BEGIN
    SET @DefvalOUT = FORMAT(TRY_CAST(@DefvalOUT AS FLOAT), '0.######');
  END;
  INSERT INTO Rail_DEFValues_Manual (Filing_Headers_FK, Display_on_UI, ValueLabel, TableName, ColName, FManual, FHasDefaultVal
  , FEditable, UI_Section, FMandatory, DefValue)
    SELECT
      @Filing_Headers_id
     ,Display_on_UI
     ,ValueLabel
     ,TableName
     ,ColName
     ,FManual
     ,FHasDefaultVal
     ,FEditable
     ,UI_Section
     ,FMandatory
     ,@DefvalOUT
    FROM Rail_DEFValues
    WHERE id = @id

  FETCH NEXT FROM c INTO @datatype, @DefValue, @id
  END
  CLOSE c
  DEALLOCATE c

  SET @str = ''
  SELECT
    @str = @str + 'update  ' + TableName + ' set ' + ColName + '= try_cast(''' + DefValue + ''' as ' +
    data_type +
    CASE
      WHEN data_type IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN data_type IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN data_type IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
    + ') ' +
    CASE
      WHEN UPPER(TableName) = 'RAIL_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END
    + '; '
    + CHAR(10)
  FROM information_schema.columns i
  LEFT JOIN dbo.Rail_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.ColName)
      AND UPPER(i.table_name) = UPPER(v.TableName)
  WHERE i.table_schema = 'dbo'
  AND Filing_Headers_FK = @Filing_Headers_id
  AND DefValue IS NOT NULL
  AND FHasDefaultVal > 0
  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables  
  AND EXISTS (SELECT
      *
    FROM information_schema.columns c
    WHERE (UPPER(i.table_name) = UPPER(c.table_name)
    AND UPPER(c.column_name) = 'FILING_HEADERS_FK')
    OR UPPER(c.table_name) = 'RAIL_FILING_HEADERS')
  ORDER BY CASE
    WHEN UPPER(i.table_name) = 'RAIL_FILING_HEADERS' THEN 0
    ELSE 1
  END
  --PRINT @str;
  EXEC (@str);

  DECLARE @List VARCHAR(MAX);
  DECLARE @List2 VARCHAR(MAX);
  DECLARE @sql VARCHAR(MAX);
  DECLARE @sql2 VARCHAR(1000) = '';
  DECLARE @s VARCHAR(MAX);

  DECLARE c CURSOR FOR SELECT DISTINCT
    TableName
  FROM Rail_DEFValues_Manual V
  WHERE Filing_Headers_FK = @Filing_Headers_id
  OPEN c
  FETCH NEXT FROM c INTO @s
  WHILE @@fetch_status = 0
  BEGIN

  SET @List = STUFF((SELECT DISTINCT
      ',' + 'isnull(cast(' +
      CASE
        WHEN t.name LIKE 'date%' THEN 'format(' + QUOTENAME(c.NAME) + ', ''MM/dd/yyyy'')'
        WHEN t.name LIKE 'numeric' THEN 'format(' + QUOTENAME(c.NAME) + ', ''0.######'')'
        ELSE QUOTENAME(c.NAME)
      END
      + ' as VARCHAR(MAX)),'''') AS ' + QUOTENAME(c.NAME)
    FROM sys.columns c
    INNER JOIN sys.types t
      ON c.user_type_id = t.user_type_id
    WHERE objecT_id = OBJECT_ID(@s)
    AND UPPER(c.NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  SET @List2 = STUFF((SELECT DISTINCT
      ',' + QUOTENAME(NAME)
    FROM sys.columns
    WHERE objecT_id = OBJECT_ID(@s)
    AND UPPER(NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables 
  SET @sql2 = '';
  IF EXISTS (SELECT
        *
      FROM sys.columns
      WHERE (objecT_id = OBJECT_ID(@s)
      AND UPPER(NAME) = 'FILING_HEADERS_FK')
      OR objecT_id = OBJECT_ID('Rail_Filing_Headers'))
    SET @sql2 =
    CASE
      WHEN UPPER(@s) = 'RAIL_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END;

  SET @sql = 'SELECT  ColName,value                  
               FROM  (SELECT top 1 ' + @List + '  FROM  ' + @s + ' ' + @sql2 + ')  p
               UNPIVOT (value  FOR  ColName  IN  (' + @List2 + '))  as  unpvt'
  --PRINT @sql
  SET @sql = '
MERGE Rail_DEFValues_Manual AS t 
USING (' + @sql + ' ) AS s 
ON (upper(t.ColName) = s.ColName and upper(t.TableName) = ''' + @s + ''' AND t.Filing_Headers_FK = ' + CAST(@Filing_Headers_id AS VARCHAR(32)) + ')
WHEN MATCHED THEN 
    UPDATE SET DefValue = case when s.value='''' then null else s.value end  ;';
  --PRINT @sql
  EXEC (@sql)

  FETCH NEXT FROM c INTO @s
  END
  CLOSE c
  DEALLOCATE c
END;
GO

