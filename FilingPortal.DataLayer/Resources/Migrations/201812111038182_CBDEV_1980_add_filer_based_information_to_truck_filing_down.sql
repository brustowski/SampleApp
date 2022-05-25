-- Filing procedures

ALTER PROCEDURE dbo.truck_filing(
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