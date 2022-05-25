IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rail_BD_Parsed_del]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[rail_BD_Parsed_del]
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rail_filing]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[rail_filing]
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rail_filing_del]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[rail_filing_del]
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[rail_filing_param]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[rail_filing_param]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[truck_filing]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[truck_filing]
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[truck_filing_del]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[truck_filing_del]
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[truck_filing_param]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[truck_filing_param]
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[truck_inbound_del]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[truck_inbound_del]
GO

/****** Object:  StoredProcedure [dbo].[rail_BD_Parsed_del]    Script Date: 24.12.2018 16:45:03 ******/
create PROCEDURE [dbo].[rail_BD_Parsed_del](
@BDP_PK int,
@FDeleted bit
)
AS 
BEGIN
update dbo.Rail_BD_Parsed set FDeleted= @FDeleted where BDP_PK=@BDP_PK
 and not exists(select * from dbo.Rail_Filing_Details d inner join dbo.Rail_Filing_Headers h on d.Filing_Headers_FK=h.id where d.BDP_FK = @BDP_PK and (isnull(MappingStatus,0)>0 or isnull(FilingStatus,0)>0) );
end;

GO

/****** Object:  StoredProcedure [dbo].[rail_filing]    Script Date: 24.12.2018 16:45:03 ******/
CREATE PROCEDURE [dbo].[rail_filing] (@Filing_Headers_id INT)
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
     ,p.Description2 AS Attribute_1
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
           ON rd.Description1 = p.Description1
             AND (p.Description2 = rd.Description2
               OR (p.Description2 IS NULL
                 AND rd.Description2 IS NULL))
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
         ,@DefValue VARCHAR(128)
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
  SET @str_val = N'SELECT @valOUT = try_cast(try_cast(''' + @DefValue + ''' as ' + @datatype + ') as  varchar(128))';
  SET @ParmDefinition = N'@valOUT varchar(128) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@ParmDefinition
                       ,@valOUT = @DefvalOUT OUTPUT;
  IF @DefvalOUT IS NULL
  BEGIN
    SET @str_val = N'SELECT @valOUT = try_cast(try_cast(' + @DefValue + ' as ' + @datatype + ') as  varchar(128))';
    SET @ParmDefinition = N'@valOUT varchar(128) OUTPUT';

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
    SET @DefvalOUT = FORMAT(TRY_CAST(@DefvalOUT AS FLOAT), '###.######');
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
        WHEN t.name LIKE 'numeric' THEN 'format(' + QUOTENAME(c.NAME) + ', ''###.######'')'
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
/****** Object:  StoredProcedure [dbo].[rail_filing_del]    Script Date: 24.12.2018 16:45:03 ******/
CREATE PROCEDURE [dbo].[rail_filing_del](
@Filing_Headers_id int
)
AS 
BEGIN

declare @str varchar(max) = '' ;

set @str = ''
select @str = @str + 'delete from ' +TableName+' where Filing_Headers_FK='+ cast(@Filing_Headers_id as varchar(64)) + '; '
+char(10)
from information_schema.columns i  inner join dbo.Rail_DEFValues_Manual v on upper(i.column_name)= upper(v.ColName) and upper(i.table_name) = upper(v.TableName)
where  i.table_schema = 'dbo' and Filing_Headers_FK=@Filing_Headers_id 
	and exists(select * from information_schema.columns c where upper(i.table_name) = upper(c.table_name) and upper(c.column_name) ='FILING_HEADERS_FK')
group by TableName 
--print @str;
exec(@str);
delete from [dbo].[Rail_Documents] where  Filing_Headers_FK=@Filing_Headers_id 
delete from [dbo].[Rail_DEFValues_Manual] where  Filing_Headers_FK=@Filing_Headers_id 
delete from [dbo].[Rail_Filing_Details] where  Filing_Headers_FK=@Filing_Headers_id 
delete from [dbo].[Rail_Filing_Headers] where  id=@Filing_Headers_id 
 
end;

GO
/****** Object:  StoredProcedure [dbo].[rail_filing_param]    Script Date: 24.12.2018 16:45:03 ******/
CREATE PROCEDURE [dbo].[rail_filing_param](
@Filing_Headers_id int = 117
)
AS 
BEGIN

declare @str varchar(max) = ''  ;
 
set @str = ''

select @str = @str + 'update  ' +TableName+' set '+ColName +'= '+ case when DefValue is null then 'NULL' else
'cast(''' + replace(DefValue, '''', '''''') +  ''' as '+  
data_type +
   CASE WHEN data_type IN ('char', 'varchar','nchar','nvarchar') THEN '('+
             CASE WHEN CHARACTER_MAXIMUM_LENGTH=-1 THEN 'MAX'
                  ELSE CONVERT(VARCHAR(4),
                               CASE WHEN data_type IN ('nchar','nvarchar')
                               THEN  CHARACTER_MAXIMUM_LENGTH/2 ELSE CHARACTER_MAXIMUM_LENGTH END )
                  END +')'
          WHEN data_type IN ('decimal','numeric')
                  THEN '('+ CONVERT(VARCHAR(4),NUMERIC_PRECISION)+','
                          + CONVERT(VARCHAR(4),NUMERIC_SCALE)+')'
                  ELSE '' END
+')' end
+
case when upper(TableName)='RAIL_FILING_HEADERS' 
	then ' where id='+ cast(@Filing_Headers_id as varchar(64))
	else ' where Filing_Headers_FK='+ cast(@Filing_Headers_id as varchar(64))
end 
+ '; '
+char(10)
from information_schema.columns i  left join dbo.Rail_DEFValues_Manual v on upper(i.column_name)= upper(v.ColName) and upper(i.table_name) = upper(v.TableName)
where  i.table_schema = 'dbo' and Filing_Headers_FK=@Filing_Headers_id
-- we don't update table without Filing_Headers_FK, but we use the first value from this table
	and exists(select * from information_schema.columns c where (upper(i.table_name) = upper(c.table_name) and upper(c.column_name) ='FILING_HEADERS_FK') or upper(c.table_name) ='RAIL_FILING_HEADERS')
--print @str;
exec(@str);
end;

GO
/****** Object:  StoredProcedure [dbo].[truck_filing]    Script Date: 24.12.2018 16:45:03 ******/
CREATE PROCEDURE [dbo].[truck_filing](
@Filing_Headers_id int = 117
)
AS 
BEGIN
SET DATEFORMAT mdy;

-- Truck_Filing Data---------------------------------------

INSERT INTO Truck_FilingData (
  TI_FK, 
  FILING_HEADERS_FK, 
  Supplier, 
  Importer, 
  Issuer, 
  Master_Bill, 
  SCAC, 
  Discharge, 
  Entry_Port, 
  State, 
  Description, 
  Code, 
  Invoice_No, 
  Consignee, 
  Manufacturer, 
  Seller, 
  Sold_to_party, 
  Ship_to_party, 
  Tariff, 
  Customs_QTY, 
  Goods_Description, 
  SPI, 
  Gr_Weight, 
  UQ, 
  Price_Unit, 
  Prod_ID_1, 
  Attribute_1, 
  Attribute_2_manual, 
  Invoice_Qty, 
  Invoice_Qty_Unit, 
  Amount, 
  Recon_Issue, 
  FTA_Recon)
  SELECT
  tfd.BDP_FK AS TI_FK,
  tfd.Filing_Headers_FK,
  tri.cw_supplier AS Supplier,
  tri.cw_ior AS Importer,
  SUBSTRING(ti.PAPs,1,4) AS Issuer,
  SUBSTRING(ti.PAPs, 5, LEN(ti.PAPs)) AS Master_Bill,
  SUBSTRING(ti.PAPs,1,4) AS Scac,
  tri.arrival_port AS Discharge,
  tri.entry_port AS Entry_port,
  tri.destination_state AS State,
  tri.goods_description AS Description,
  trp.firms_code AS Code,
  ti.PAPs,
  tri.cw_ior AS Consignee,
  tri.cw_supplier AS Manufacturer,
  tri.cw_supplier AS Seller,
  tri.cw_ior AS Sold_to_party,
  tri.cw_ior AS Ship_to_party,
  tri.tariff AS Tariff,
  tri.custom_quantity AS Customs_qty,
  tri.goods_description AS Goods_description,
  tri.spi AS SPI,
  tri.gross_weight AS Gr_weight,
  tri.custom_uq AS UQ,
  tri.line_price AS Price_unit,
  tri.product_id AS Prod_id_1,
  tri.custom_attrib1 AS Attribute_1,
  tri.custom_attrib2 AS Attribute_2,
  tri.invoice_qty AS Invoice_qty,
  tri.invoice_uq AS Invoice_qty_unit,
  tri.charges AS Amount,
  tri.recon_issue AS Recon_issue,
  tri.nafta_recon AS FTA_recon
  FROM Truck_Filing_Details tfd 
    INNER JOIN Truck_Inbound ti ON tfd.BDP_FK = ti.Id
    LEFT JOIN Truck_Rule_Importers tri ON rtrim(ltrim(ti.Importer))=rtrim(ltrim(tri.ior))
    LEFT JOIN Truck_Rule_Ports trp ON (rtrim(ltrim(tri.arrival_port))=rtrim(ltrim(trp.arrival_port)) AND rtrim(ltrim(tri.entry_port))=rtrim(ltrim(trp.entry_port)))
  WHERE NOT EXISTS(select * from Truck_FilingData r where r.Filing_Headers_FK = tfd.Filing_Headers_FK and tfd.BDP_FK=r.TI_FK ) and
  tfd.Filing_Headers_FK = @Filing_Headers_id
 
PRINT 'Truck_FilingData'
 
DECLARE @str_val nvarchar(500),
        @ParmDefinition nvarchar(500),
        @DefvalOUT varchar(128);

declare @datatype varchar(128), 
        @DefValue varchar(128), 
        @id int;

declare c cursor for  
select distinct 
   data_type +
   CASE WHEN data_type IN ('char', 'varchar','nchar','nvarchar') THEN '('+
             CASE WHEN CHARACTER_MAXIMUM_LENGTH=-1 THEN 'MAX'
                  ELSE CONVERT(VARCHAR(4),
                               CASE WHEN data_type IN ('nchar','nvarchar')
                               THEN  CHARACTER_MAXIMUM_LENGTH/2 ELSE CHARACTER_MAXIMUM_LENGTH END )
                  END +')'
          WHEN data_type IN ('decimal','numeric')
                  THEN '('+ CONVERT(VARCHAR(4),NUMERIC_PRECISION)+','
                          + CONVERT(VARCHAR(4),NUMERIC_SCALE)+')'
                  ELSE '' END
, DefValue, td.id
from information_schema.columns i  
left join dbo.Truck_DEFValues td on upper(i.column_name)= upper(td.ColName) and upper(i.table_name) = 'TRUCK_FILINGDATA'
where  i.table_schema = 'dbo' and (FManual>0 or FHasDefaultVal>0 or Display_on_UI > 0)
 and not exists(select * from Truck_DEFValues_Manual r where r.Filing_Headers_FK = @Filing_Headers_id ) 
 order by id
open c   
fetch next from c into @datatype, @DefValue, @id   
while @@fetch_status = 0   
begin  
set @DefvalOUT=null;
	SET @str_val = N'SELECT @valOUT = try_cast(try_cast('''+@DefValue+''' as '+@datatype+') as varchar(128))';  
	SET @ParmDefinition = N'@valOUT varchar(128) OUTPUT';  
	EXECUTE sp_executesql @str_val, @ParmDefinition, @valOUT=@DefvalOUT OUTPUT;
	if @DefvalOUT is null begin
		SET @str_val = N'SELECT @valOUT = try_cast(try_cast('+@DefValue+' as '+@datatype+') as varchar(128))';  
		SET @ParmDefinition = N'@valOUT varchar(128) OUTPUT'; 
	
		declare @result float
		BEGIN TRY
			EXECUTE sp_executesql @str_val, @ParmDefinition, @valOUT=@DefvalOUT OUTPUT;
		END TRY  
		BEGIN CATCH  
			set @DefvalOUT=null;
		END CATCH; 
	end;
	--PRINT concat(@id,' - ',@DefvalOUT, ' ', @datatype);

if @datatype like 'date%' begin
	set @DefvalOUT=format(try_cast(@DefvalOUT as datetime), 'MM/dd/yyyy');
end;
if @datatype like 'numeric' begin
	set @DefvalOUT=format(try_cast(@DefvalOUT as float),'###.######');
end;
insert into Truck_DEFValues_Manual (
  Filing_Headers_FK, 
  Display_on_UI, 
  ValueLabel, 
  ColName, 
  FManual, 
  FHasDefaultVal, 
  FEditable, 
  UI_Section, 
  FMandatory, 
  DefValue) 
select 
  @Filing_Headers_id,
  Display_on_UI, 
  ValueLabel, 
  ColName, 
  FManual, 
  FHasDefaultVal, 
  FEditable, 
  UI_Section, 
  FMandatory, 
  @DefvalOUT
from Truck_DEFValues where id =@id
   fetch next from c into @datatype, @DefValue, @id  
end   
close c   
deallocate c

declare @str varchar(max) = '';

select @str = @str + 'update Truck_FilingData set '+ColName +'= try_cast(''' +DefValue +  ''' as '+  
data_type +
   CASE WHEN data_type IN ('char', 'varchar','nchar','nvarchar') THEN '('+
             CASE WHEN CHARACTER_MAXIMUM_LENGTH=-1 THEN 'MAX'
                  ELSE CONVERT(VARCHAR(4),
                               CASE WHEN data_type IN ('nchar','nvarchar')
                               THEN  CHARACTER_MAXIMUM_LENGTH/2 ELSE CHARACTER_MAXIMUM_LENGTH END )
                  END +')'
          WHEN data_type IN ('decimal','numeric')
                  THEN '('+ CONVERT(VARCHAR(4),NUMERIC_PRECISION)+','
                          + CONVERT(VARCHAR(4),NUMERIC_SCALE)+')'
                  ELSE '' END
+') where Filing_Headers_FK='+ cast(@Filing_Headers_id as varchar(64)) + '; '+ char(10)

from information_schema.columns i 
  left join dbo.Truck_DEFValues_Manual v on upper(i.column_name)= upper(v.ColName) and upper(i.table_name) = 'TRUCK_FILINGDATA'
where 
  i.table_schema = 'dbo' 
  and Filing_Headers_FK=@Filing_Headers_id 
  and DefValue is not null 
  and FHasDefaultVal > 0
-- we don't update table without Filing_Headers_FK, but we use the first value from this tables  
	and exists(select * from information_schema.columns c where (upper(i.table_name) = upper(c.table_name) and upper(c.column_name) ='FILING_HEADERS_FK') or upper(c.table_name) ='TRUCK_FILING_HEADERS')
order by case when upper(i.table_name) ='TRUCK_FILING_HEADERS' then 0 else 1 end 
--PRINT @str;
exec(@str);

DECLARE @List VARCHAR(MAX) ;
DECLARE @List2 VARCHAR(MAX) ;
DECLARE @sql VARCHAR(MAX);
DECLARE @sql2 VARCHAR(1000)='';
DECLARE @s VARCHAR(MAX) = 'Truck_FilingData';

set @List = STUFF((SELECT DISTINCT  ',' + 'isnull(cast(' + 
case when t.name like 'date%' then 'format('+ QUOTENAME(c.NAME) +', ''MM/dd/yyyy'')'
when t.name like 'numeric' then 'format('+ QUOTENAME(c.NAME) +', ''###.######'')'
else QUOTENAME(c.NAME) end
+ ' as VARCHAR(MAX)),'''') AS ' + QUOTENAME(c.NAME) 
			FROM sys.columns c INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
			WHERE objecT_id = OBJECT_ID(@s)AND upper(c.NAME) not in ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER' )
			FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')

set @List2  = STUFF((SELECT DISTINCT ',' + QUOTENAME(NAME)
			FROM sys.columns	WHERE objecT_id = OBJECT_ID(@s)AND upper(NAME) not in ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER' )
			FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')

-- we don't update table without Filing_Headers_FK, but we use the first value from this tables 
set @sql2='';			
if exists(select * from sys.columns	WHERE (objecT_id = OBJECT_ID(@s)AND upper(NAME)='FILING_HEADERS_FK') or objecT_id = OBJECT_ID('Truck_Filing_Headers')) 
	set @sql2=case when upper(@s)='TRUCK_FILING_HEADERS' 
				then ' where id='+ cast(@Filing_Headers_id as varchar(64))
				else ' where Filing_Headers_FK='+ cast(@Filing_Headers_id as varchar(64))
			end ;
			
SET @sql = 'SELECT  ColName,value                  
               FROM  (SELECT top 1 ' + @List + '  FROM  '+@s+' '+@sql2+')  p
               UNPIVOT (value  FOR  ColName  IN  (' + @List2 + '))  as  unpvt'
--PRINT @sql
SET @sql = '
MERGE Truck_DEFValues_Manual AS t 
USING ('+@sql+' ) AS s 
ON (upper(t.ColName) = s.ColName AND t.Filing_Headers_FK = '+cast(@Filing_Headers_id as varchar(32))+')
WHEN MATCHED THEN 
    UPDATE SET DefValue = case when s.value='''' then null else s.value end  ;';
--PRINT @sql
EXEC (@sql)

end   

GO
/****** Object:  StoredProcedure [dbo].[truck_filing_del]    Script Date: 24.12.2018 16:45:03 ******/
CREATE PROCEDURE [dbo].[truck_filing_del](
@Filing_Headers_id int
)
AS 
BEGIN

declare @str varchar(max) = '' ;

DELETE FROM Truck_FilingData WHERE FILING_HEADERS_FK=@Filing_Headers_id;
delete from [dbo].[Truck_Documents] where Filing_Headers_FK=@Filing_Headers_id 
delete from [dbo].[Truck_DEFValues_Manual] where  Filing_Headers_FK=@Filing_Headers_id 
delete from [dbo].[Truck_Filing_Details] where  Filing_Headers_FK=@Filing_Headers_id 
delete from [dbo].[Truck_Filing_Headers] where  id=@Filing_Headers_id 
 
end;

GO
/****** Object:  StoredProcedure [dbo].[truck_filing_param]    Script Date: 24.12.2018 16:45:03 ******/
CREATE PROCEDURE [dbo].[truck_filing_param](
@Filing_Headers_id int = 117
)
AS 
BEGIN

declare @str varchar(max) = ''  ;
 
set @str = ''

select @str = @str + 'update Truck_FilingData set '+ ColName +'= '+ case when DefValue is null then 'NULL' else
'cast(''' + replace(DefValue, '''', '''''') +  ''' as '+  
data_type +
   CASE WHEN data_type IN ('char', 'varchar','nchar','nvarchar') THEN '('+
             CASE WHEN CHARACTER_MAXIMUM_LENGTH=-1 THEN 'MAX'
                  ELSE CONVERT(VARCHAR(4),
                               CASE WHEN data_type IN ('nchar','nvarchar')
                               THEN  CHARACTER_MAXIMUM_LENGTH/2 ELSE CHARACTER_MAXIMUM_LENGTH END )
                  END +')'
          WHEN data_type IN ('decimal','numeric')
                  THEN '('+ CONVERT(VARCHAR(4),NUMERIC_PRECISION)+','
                          + CONVERT(VARCHAR(4),NUMERIC_SCALE)+')'
                  ELSE '' END
+')' end
+ ' where Filing_Headers_FK='+ cast(@Filing_Headers_id as varchar(64)) + '; '
+char(10)
from information_schema.columns i  left join dbo.Truck_DEFValues_Manual v on upper(i.column_name)= upper(v.ColName) and upper(i.table_name) = 'TRUCK_FILINGDATA'
where  i.table_schema = 'dbo' and Filing_Headers_FK=@Filing_Headers_id
-- we don't update table without Filing_Headers_FK, but we use the first value from this table
	and exists(select * from information_schema.columns c where (upper(i.table_name) = upper(c.table_name) and upper(c.column_name) ='FILING_HEADERS_FK') or upper(c.table_name) ='TRUCK_FILING_HEADERS')
exec(@str);
end;

GO
/****** Object:  StoredProcedure [dbo].[truck_inbound_del]    Script Date: 24.12.2018 16:45:03 ******/
create PROCEDURE [dbo].[truck_inbound_del] (
@id int,
@FDeleted bit
)
AS 
BEGIN
update dbo.Truck_Inbound set FDeleted=@FDeleted where Id=@id
 and not exists(select * from dbo.Truck_Filing_Details d inner join dbo.Truck_Filing_Headers h on d.Filing_Headers_FK=h.id where d.BDP_FK = @id and (isnull(MappingStatus,0)>0 or isnull(FilingStatus,0)>0) );
end
;

GO
