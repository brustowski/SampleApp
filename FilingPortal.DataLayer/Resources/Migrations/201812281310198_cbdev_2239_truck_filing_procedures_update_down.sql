CREATE TABLE dbo.Truck_FilingData (
  id INT IDENTITY
 ,TI_FK INT NOT NULL
 ,FILING_HEADERS_FK INT NOT NULL
 ,CreatedDate DATETIME NULL
 ,CreatedUser VARCHAR(128) NULL
 ,Supplier NVARCHAR(128) NULL
 ,Importer NVARCHAR(128) NULL
 ,Shipment_Type NVARCHAR(128) NULL
 ,Transport NVARCHAR(128) NULL
 ,Entry_Type NVARCHAR(128) NULL
 ,RLF NVARCHAR(128) NULL
 ,Enable_Entry_Sum DECIMAL NULL
 ,Type NVARCHAR(128) NULL
 ,Certify_Cargo_Release NVARCHAR(128) NULL
 ,Service NVARCHAR(128) NULL
 ,Issuer NVARCHAR(128) NULL
 ,Master_Bill NVARCHAR(128) NULL
 ,SCAC NVARCHAR(128) NULL
 ,Discharge NVARCHAR(128) NULL
 ,Entry_Port NVARCHAR(128) NULL
 ,Dep DATETIME NULL
 ,Arr DATETIME NULL
 ,Arr_2 DATETIME NULL
 ,HMF NVARCHAR(128) NULL
 ,Origin NVARCHAR(128) NULL
 ,Destination NVARCHAR(128) NULL
 ,State NVARCHAR(128) NULL
 ,Country_of_Export NVARCHAR(128) NULL
 ,ETA DATETIME NULL
 ,Date DATETIME NULL
 ,Description NVARCHAR(MAX) NULL
 ,Owner_Ref NVARCHAR(128) NULL
 ,INCO NVARCHAR(128) NULL
 ,Total_Weight DECIMAL NULL
 ,Total_Volume DECIMAL NULL
 ,No_Packages NVARCHAR(128) NULL
 ,Code NVARCHAR(128) NULL
 ,Centralized_Exam_Site NVARCHAR(128) NULL
 ,Purchased NVARCHAR(128) NULL
 ,Manual_Entry NVARCHAR(128) NULL
 ,Importer_of_record NVARCHAR(128) NULL
 ,Split_Shipment_Release NVARCHAR(128) NULL
 ,Custom_Tab_Check_Local_Client NVARCHAR(128) NULL
 ,Bill_Type NVARCHAR(128) NULL
 ,Bill_Num NVARCHAR(128) NULL
 ,Manifest_QTY DECIMAL NULL
 ,Packing_UQ NVARCHAR(128) NULL
 ,Bill_Issuer_SCAC NVARCHAR(128) NULL
 ,Invoice_No NVARCHAR(128) NULL
 ,Address NVARCHAR(128) NULL
 ,Invoice_Total DECIMAL NULL
 ,Curr NVARCHAR(128) NULL
 ,Payment_Date DATETIME NULL
 ,Consignee NVARCHAR(128) NULL
 ,Inv_Date DATETIME NULL
 ,Agreed_Place NVARCHAR(128) NULL
 ,Inv_Gross_Weight DECIMAL NULL
 ,Net_Weight DECIMAL NULL
 ,Manufacturer NVARCHAR(128) NULL
 ,Seller NVARCHAR(128) NULL
 ,Sold_to_party NVARCHAR(128) NULL
 ,Ship_to_party NVARCHAR(128) NULL
 ,Broker_PGA_Contact_Name NVARCHAR(128) NULL
 ,Broker_PGA_Contact_Phone NVARCHAR(128) NULL
 ,Broker_PGA_Contact_Email NVARCHAR(128) NULL
 ,LNO NVARCHAR(128) NULL
 ,Tariff NVARCHAR(128) NULL
 ,Customs_QTY DECIMAL NULL
 ,Line_Price DECIMAL NULL
 ,Goods_Description NVARCHAR(MAX) NULL
 ,ORG NVARCHAR(128) NULL
 ,SPI NVARCHAR(128) NULL
 ,Gr_Weight DECIMAL NULL
 ,UQ NVARCHAR(128) NULL
 ,Price_Unit DECIMAL NULL
 ,Prod_ID_1 NVARCHAR(128) NULL
 ,Attribute_1 NVARCHAR(128) NULL
 ,Attribute_2_manual NVARCHAR(128) NULL
 ,Attribute_3 NVARCHAR(128) NULL
 ,Export NVARCHAR(128) NULL
 ,Invoice_Qty DECIMAL NULL
 ,Invoice_Qty_Unit NVARCHAR(128) NULL
 ,Inv_Price DECIMAL NULL
 ,Gross_Weight DECIMAL NULL
 ,FIRMs_Code NVARCHAR(128) NULL
 ,Amount DECIMAL NULL
 ,CIF_Component NVARCHAR(128) NULL
 ,EPA_TSCA_Toxic_Substance_Control_Act_Indicator NVARCHAR(128) NULL
 ,TSCA_Indicator NVARCHAR(128) NULL
 ,Certifying_Individual NVARCHAR(128) NULL
 ,Branch NVARCHAR(128) NULL
 ,Broker NVARCHAR(128) NULL
 ,Merge_By NVARCHAR(128) NULL
 ,Tax_Deferrable_Ind NVARCHAR(128) NULL
 ,Preparer_Dist_Port NVARCHAR(128) NULL
 ,Recon_Issue NVARCHAR(128) NULL
 ,FTA_Recon NVARCHAR(128) NULL
 ,Bond_Type NVARCHAR(128) NULL
 ,Payment_Type NVARCHAR(128) NULL
 ,Broker_to_Pay NVARCHAR(128) NULL
 ,Prelim_Statement_Date DATETIME NULL
 ,Submitter NVARCHAR(128) NULL
 ,MID NVARCHAR(128) NULL
 ,Manufacturer_Code NVARCHAR(128) NULL
 ,Surety_Code NVARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Truck_FilingData] PRIMARY KEY CLUSTERED (id)
)
GO

ALTER TABLE dbo.Truck_FilingData
ADD CONSTRAINT [FK_dbo.Truck_FilingData_dbo.Truck_Filing_Headers_FILING_HEADERS_FK] FOREIGN KEY (FILING_HEADERS_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Truck_FilingData
ADD CONSTRAINT [FK_dbo.Truck_FilingData_dbo.Truck_Inbound_TI_FK] FOREIGN KEY (TI_FK) REFERENCES dbo.Truck_Inbound (Id) ON DELETE CASCADE
GO


ALTER PROCEDURE dbo.truck_filing (@Filing_Headers_id INT = 117,
@FilingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;

  -- Truck_Filing Data---------------------------------------

  INSERT INTO Truck_FilingData (TI_FK,
  Filing_Headers_FK,
  Supplier,
  Importer,
  Issuer,
  Master_Bill,
  SCAC,
  Discharge,
  entry_port,
  State,
  Description,
  Code,
  Invoice_No,
  Consignee,
  Manufacturer,
  Seller,
  Sold_To_Party,
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
  recon_issue,
  FTA_Recon,
  Branch,
  Broker,
  Preparer_Dist_Port)
    SELECT
      tfd.BDP_FK AS TI_FK
     ,tfd.Filing_Headers_FK
     ,tri.cw_supplier AS Supplier
     ,tri.cw_ior AS Importer
     ,SUBSTRING(ti.PAPs, 1, 4) AS Issuer
     ,SUBSTRING(ti.PAPs, 5, LEN(ti.PAPs)) AS Master_Bill
     ,SUBSTRING(ti.PAPs, 1, 4) AS Scac
     ,tri.arrival_port AS Discharge
     ,tri.Entry_port AS Entry_port
     ,tri.destination_state AS State
     ,tri.Goods_description AS Description
     ,trp.firms_code AS Code
     ,ti.PAPs
     ,tri.cw_ior AS Consignee
     ,tri.cw_supplier AS Manufacturer
     ,tri.cw_supplier AS Seller
     ,tri.cw_ior AS Sold_to_party
     ,tri.cw_ior AS Ship_to_party
     ,tri.Tariff AS Tariff
     ,tri.custom_quantity AS Customs_qty
     ,tri.Goods_description AS Goods_description
     ,tri.SPI AS SPI
     ,tri.gross_weight AS Gr_weight
     ,tri.custom_uq AS UQ
     ,tri.Line_Price AS Price_unit
     ,tri.product_id AS Prod_id_1
     ,tri.custom_attrib1 AS Attribute_1
     ,tri.custom_attrib2 AS Attribute_2
     ,tri.Invoice_qty AS Invoice_qty
     ,tri.invoice_uq AS Invoice_qty_unit
     ,tri.charges AS Amount
     ,tri.Recon_issue AS Recon_issue
     ,tri.nafta_recon AS FTA_recon
     ,aud.Branch
     ,aud.Broker
     ,aud.Location
    FROM Truck_Filing_Details tfd
    INNER JOIN Truck_Inbound ti
      ON tfd.BDP_FK = ti.id
    LEFT JOIN Truck_Rule_Importers tri
      ON RTRIM(LTRIM(ti.Importer)) = RTRIM(LTRIM(tri.ior))
    LEFT JOIN Truck_Rule_Ports trp
      ON (RTRIM(LTRIM(tri.arrival_port)) = RTRIM(LTRIM(trp.arrival_port))
          AND RTRIM(LTRIM(tri.Entry_port)) = RTRIM(LTRIM(trp.Entry_port)))
    LEFT JOIN app_users_data aud
      ON aud.UserAccount = @FilingUser
    WHERE NOT EXISTS (SELECT
        *
      FROM Truck_FilingData r
      WHERE r.Filing_Headers_FK = tfd.Filing_Headers_FK
      AND tfd.BDP_FK = r.TI_FK)
    AND tfd.Filing_Headers_FK = @Filing_Headers_id

  DECLARE @str_val NVARCHAR(500)
         ,@ParmDefinition NVARCHAR(500)
         ,@DefvalOUT VARCHAR(128);

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
   ,td.id
  FROM INFORMATION_SCHEMA.columns i
  LEFT JOIN dbo.Truck_DEFValues td
    ON UPPER(i.column_name) = UPPER(td.ColName)
    AND UPPER(i.table_name) = 'TRUCK_FILINGDATA'
  WHERE i.table_schema = 'dbo'
  AND (FManual > 0
  OR FHasDefaultVal > 0
  OR Display_on_UI > 0)
  AND NOT EXISTS (SELECT
      *
    FROM Truck_DEFValues_Manual r
    WHERE r.Filing_Headers_FK = @Filing_Headers_id)
  ORDER BY id
  OPEN c
  FETCH NEXT FROM c INTO @datatype, @DefValue, @id
  WHILE @@fetch_status = 0
  BEGIN
  SET @DefvalOUT = NULL;
  SET @str_val = N'SELECT @valOUT = try_cast(try_cast(''' + @DefValue + ''' as ' + @datatype + ') as varchar(128))';
  SET @ParmDefinition = N'@valOUT varchar(128) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@ParmDefinition
                       ,@valOUT = @DefvalOUT OUTPUT;
  IF @DefvalOUT IS NULL
  BEGIN
    SET @str_val = N'SELECT @valOUT = try_cast(try_cast(' + @DefValue + ' as ' + @datatype + ') as varchar(128))';
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
  INSERT INTO Truck_DEFValues_Manual (Filing_Headers_FK,
  Display_on_UI,
  ValueLabel,
  ColName,
  FManual,
  FHasDefaultVal,
  FEditable,
  UI_Section,
  FMandatory,
  DefValue)
    SELECT
      @Filing_Headers_id
     ,Display_on_UI
     ,ValueLabel
     ,ColName
     ,FManual
     ,FHasDefaultVal
     ,FEditable
     ,UI_Section
     ,FMandatory
     ,@DefvalOUT
    FROM Truck_DEFValues
    WHERE id = @id
  FETCH NEXT FROM c INTO @datatype, @DefValue, @id
  END
  CLOSE c
  DEALLOCATE c

  DECLARE @str VARCHAR(MAX) = '';

  SELECT
    @str = @str + 'update Truck_FilingData set ' + ColName + '= try_cast(''' + DefValue + ''' as ' +
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
    + ') where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64)) + '; ' + CHAR(10)

  FROM INFORMATION_SCHEMA.columns i
  LEFT JOIN dbo.Truck_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.ColName)
      AND UPPER(i.table_name) = 'TRUCK_FILINGDATA'
  WHERE i.table_schema = 'dbo'
  AND Filing_Headers_FK = @Filing_Headers_id
  AND DefValue IS NOT NULL
  AND FHasDefaultVal > 0
  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables  
  AND EXISTS (SELECT
      *
    FROM INFORMATION_SCHEMA.columns c
    WHERE (UPPER(i.table_name) = UPPER(c.table_name)
    AND UPPER(c.column_name) = 'FILING_HEADERS_FK')
    OR UPPER(c.table_name) = 'TRUCK_FILING_HEADERS')
  ORDER BY CASE
    WHEN UPPER(i.table_name) = 'TRUCK_FILING_HEADERS' THEN 0
    ELSE 1
  END
  --PRINT @str;
  EXEC (@str);

  DECLARE @List VARCHAR(MAX);
  DECLARE @List2 VARCHAR(MAX);
  DECLARE @sql VARCHAR(MAX);
  DECLARE @sql2 VARCHAR(1000) = '';
  DECLARE @s VARCHAR(MAX) = 'Truck_FilingData';

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
    WHERE object_id = OBJECT_ID(@s)
    AND UPPER(c.NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  SET @List2 = STUFF((SELECT DISTINCT
      ',' + QUOTENAME(NAME)
    FROM sys.columns
    WHERE object_id = OBJECT_ID(@s)
    AND UPPER(NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables 
  SET @sql2 = '';
  IF EXISTS (SELECT
        *
      FROM sys.columns
      WHERE (object_id = OBJECT_ID(@s)
      AND UPPER(NAME) = 'FILING_HEADERS_FK')
      OR object_id = OBJECT_ID('Truck_Filing_Headers'))
    SET @sql2 =
    CASE
      WHEN UPPER(@s) = 'TRUCK_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END;

  SET @sql = 'SELECT  ColName,value                  
               FROM  (SELECT top 1 ' + @List + '  FROM  ' + @s + ' ' + @sql2 + ')  p
               UNPIVOT (value  FOR  ColName  IN  (' + @List2 + '))  as  unpvt'
  --PRINT @sql
  SET @sql = '
MERGE Truck_DEFValues_Manual AS t 
USING (' + @sql + ' ) AS s 
ON (upper(t.ColName) = s.ColName AND t.Filing_Headers_FK = ' + CAST(@Filing_Headers_id AS VARCHAR(32)) + ')
WHEN MATCHED THEN 
    UPDATE SET DefValue = case when s.value='''' then null else s.value end  ;';
  --PRINT @sql
  EXEC (@sql)

END
GO

ALTER PROCEDURE [dbo].[truck_filing_param](
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

ALTER PROCEDURE [dbo].[truck_filing_del](
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

ALTER VIEW dbo.Truck_Inbound_Grid
AS select 
  ti.Id AS ID,
  tfh.id AS Filing_Headers_Id,
  ti.Importer AS BaseImporter,
  tri.cw_ior AS Importer,
  ti.PAPs,
  ti.CreatedDate,
  isnull(tfh.MappingStatus, 0) AS Filing_Headers_MappingStatus,
  isnull(tfh.FilingStatus, 0) AS Filing_Headers_FilingStatus,
  tfh.ErrorDescription
from  
dbo.Truck_Inbound ti 
left join dbo.Truck_Rule_Importers tri on ti.Importer=tri.ior
left join dbo.Truck_Filing_Details tfd on tfd.BDP_FK = ti.Id
left join dbo.Truck_Filing_Headers tfh on tfh.id = tfd.Filing_Headers_FK and tfh.MappingStatus<>0
where not exists 
  (select * 
    from  
      dbo.Truck_Filing_Headers tfh 
      inner join dbo.Truck_Filing_Details tfd on tfh.id = tfd.Filing_Headers_FK 
    where tfh.MappingStatus > 0 and ti.Id = tfd.BDP_FK) and ti.FDeleted=0 

union

select 
  ti.Id AS ID,
  tfh.id AS Filing_Headers_Id,
  ti.Importer AS BaseImporter,
  fd.Importer as Importer,
  ti.PAPs,
  ti.CreatedDate,
  isnull(tfh.MappingStatus, 0) AS Filing_Headers_MappingStatus,
  isnull(tfh.FilingStatus, 0) AS Filing_Headers_FilingStatus,
  tfh.ErrorDescription

from
 dbo.Truck_Filing_Headers tfh
   inner join dbo.Truck_Filing_Details tfd on  tfh.id = tfd.Filing_Headers_FK
   inner join dbo.Truck_Inbound ti on tfd.BDP_FK = ti.Id
   left join dbo.Truck_FilingData fd on fd.Filing_Headers_FK = tfh.id and tfd.BDP_FK = fd.TI_FK
where tfh.MappingStatus>0 and ti.FDeleted=0;
GO