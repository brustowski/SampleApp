--- Rail ---
DROP INDEX IF EXISTS Idx_RailDeclarationTab_filingHeadersFK ON dbo.Rail_DeclarationTab
GO
CREATE INDEX Idx_RailDeclarationTab_filingHeadersFK
ON dbo.Rail_DeclarationTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_RailContainersTab_filingHeadersFK ON dbo.Rail_ContainersTab
GO
CREATE INDEX Idx_RailContainersTab_filingHeadersFK
ON dbo.Rail_ContainersTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_RailInvoiceHeaders_filingHeadersFK ON dbo.Rail_InvoiceHeaders
GO
CREATE INDEX Idx_RailInvoiceHeaders_filingHeadersFK
ON dbo.Rail_InvoiceHeaders (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_RailInvoiceLines_invoiceHeadersFK ON dbo.Rail_InvoiceLines
GO
CREATE INDEX Idx_RailInvoiceLines_invoiceHeadersFK
ON dbo.Rail_InvoiceLines (InvoiceHeaders_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_RailMISC_filingHeadersFK ON dbo.Rail_MISC
GO
CREATE INDEX Idx_RailMISC_filingHeadersFK
ON dbo.Rail_MISC (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_RailDEFValuesManual_recordId_tableName_columnName ON dbo.Rail_DEFValues_Manual
GO
CREATE INDEX Idx_RailDEFValuesManual_recordId_tableName_columnName
ON dbo.Rail_DEFValues_Manual (record_id, table_name, column_name)
ON [PRIMARY]
GO

-- add rail containers tab record --
ALTER PROCEDURE dbo.rail_add_container_record (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add container data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Rail_ContainersTab pct
      WHERE pct.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO Rail_ContainersTab (
        BDP_FK
       ,Bill_Issuer_SCAC
       ,Bill_Num
       ,Bill_Number
       ,Container_Number
       ,Gross_Weight
       ,Gross_Weight_Unit
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        details.BDP_FK
       ,p.IssuerCode
       ,p.BillofLading
       ,CONCAT('MB:', p.BillofLading)
       ,CONCAT(EquipmentInitial, EquipmentNumber)
       ,p.Weight
       ,p.WeightUnit
       ,details.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details details
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = details.BDP_FK
      WHERE details.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
  SET NOCOUNT OFF
END;
GO
-- add rail declaration tab record --
ALTER PROCEDURE dbo.rail_add_declaration_record (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );
  DECLARE @masterBill VARCHAR(128)

  SELECT TOP 1
    @masterBill = p.BillofLading
  FROM dbo.Rail_Filing_Details d
  INNER JOIN dbo.Rail_BD_Parsed p
    ON p.BDP_PK = d.BDP_FK
  WHERE d.Filing_Headers_FK = @filingHeaderId

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Rail_DeclarationTab pdt
      WHERE pdt.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO Rail_DeclarationTab (
        Carrier_SCAC
       ,Country_of_Export
       ,Description
       ,Destination
       ,Destination_State
       ,Discharge
       ,Entry_Port
       ,FIRMs_Code
       ,Importer
       ,Issuer
       ,Main_Supplier
       ,Origin
       ,Master_Bill
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        p.IssuerCode AS Carrier_SCAC
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
       ,rp.Origin AS Origin
       ,@masterBill
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
      WHERE d.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
  SET NOCOUNT OFF
END;
GO
-- add rail invoice header record --
ALTER PROCEDURE dbo.rail_add_invoice_header_record (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_InvoiceHeaders'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Rail_InvoiceHeaders pih
      WHERE pih.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO Rail_InvoiceHeaders (
        Consignee
       ,Export
       ,Importer
       ,Manufacturer
       ,Origin
       ,Seller
       ,Ship_to_party
       ,Sold_to_party
       ,Supplier
       ,Transaction_Related
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
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
      WHERE d.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.rail_add_invoice_line_record @filingHeaderId
                                         ,@recordId
                                         ,@filingUser

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
  SET NOCOUNT OFF;
END;
GO
-- add rail invoice line record --
ALTER PROCEDURE dbo.rail_add_invoice_line_record (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Rail_InvoiceLines pil
      WHERE pil.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO Rail_InvoiceLines (
        InvoiceHeaders_FK
       ,Attribute_1
       ,Attribute_2
       ,Consignee
       ,Dest_State
       ,Export
       ,Goods_Description
       ,Manufacturer
       ,ORG
       ,Origin
       ,Prod_ID_1
       ,Tariff
       ,Transaction_Related
       ,Customs_QTY
       ,SPI
       ,UQ
       ,PriceUnit
       ,Invoice_Qty
       ,Invoice_Qty_Unit
       ,Amount
       ,Line_Price
       ,Description
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId AS InvoiceHeaders_FK
       ,rd.Attribute_1 AS Attribute_1
       ,rd.Attribute_2 AS Attribute_2
       ,rn.Consignee AS Consignee
       ,rn.Destination_State AS Dest_State
       ,rp.Export AS Export
       ,rd.Goods_Description AS Goods_Description
       ,rn.Manufacturer AS Manufacturer
       ,rn.CountryofOrigin AS ORG
       ,rn.CountryofOrigin AS Origin
       ,rd.Prod_ID_1 AS Prod_ID_1
       ,rd.Tariff AS Tariff
       ,rn.Relationship AS Transaction_Related
       ,rd.Template_HTS_Quantity AS Customs_QTY
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
      WHERE d.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
  SET NOCOUNT OFF
END;
GO
-- add rail misc record --
ALTER PROCEDURE dbo.rail_add_misc_record (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );


  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Rail_MISC pm
      WHERE pm.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO Rail_MISC (
        Recon_Issue
       ,FTA_Recon
       ,Payment_Type
       ,Broker_to_Pay
       ,Submitter
       ,Filing_Headers_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        ISNULL(rn.Value_Recon, 'N/A') AS Recon_Issue
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
      WHERE d.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    PRINT @recordId

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
  SET NOCOUNT OFF
END;
GO
-- Add Rail Import def values manual records --
ALTER PROCEDURE dbo.rail_add_def_values_manual (@tableName VARCHAR(128)
,
@filingHeaderId INT
,
@parentId INT
,
@recordId INT)
AS
BEGIN
  DECLARE @str_val NVARCHAR(500)
  DECLARE @parmDefinition NVARCHAR(500)
  DECLARE @defValueOut VARCHAR(128)
  DECLARE @dataType VARCHAR(128)
  DECLARE @defValue VARCHAR(500)
  DECLARE @id INT;
  DECLARE @columnName VARCHAR(128)

  DECLARE c CURSOR FOR SELECT DISTINCT
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
   ,v.DefValue
   ,v.id
   ,v.ColName
  FROM dbo.Rail_DEFValues v
  INNER JOIN dbo.rail_sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.FManual > 0
  OR v.FHasDefaultVal > 0
  OR v.Display_on_UI > 0
  OR v.SingleFilingOrder > 0)
  ORDER BY id

  OPEN c
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  WHILE @@fetch_status = 0
  BEGIN
  SET @defValueOut = NULL;
  SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(''' + @defValue + ''' as ' + @dataType + ') as varchar(500))';
  SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@parmDefinition
                       ,@defValueOut = @defValueOut OUTPUT;
  IF @defValueOut IS NULL
  BEGIN
    SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(' + @defValue + ' as ' + @dataType + ') as varchar(500))';
    SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';

    DECLARE @result FLOAT
  BEGIN TRY
    EXECUTE sp_executesql @str_val
                         ,@parmDefinition
                         ,@defValueOut = @defValueOut OUTPUT;
  END TRY
  BEGIN CATCH
    SET @defValueOut = NULL;
  END CATCH;
  END;

  IF @dataType LIKE 'date%'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS DATETIME), 'MM/dd/yyyy');
  END;
  IF @dataType LIKE 'numeric'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS FLOAT), '0.######');
  END;

  INSERT INTO dbo.Rail_DEFValues_Manual (
      Filing_Headers_FK
     ,parent_record_id
     ,section_name
     ,section_title
     ,record_id
     ,column_name
     ,table_name
     ,ModifiedDate
     ,value
     ,FEditable
     ,Display_on_UI
     ,FHasDefaultVal
     ,FMandatory
     ,FManual
     ,description
     ,ValueLabel
     ,handbook_name
     ,paired_field_table
     ,paired_field_column)
    SELECT
      @filingHeaderId
     ,@parentId
     ,s.name
     ,s.title
     ,@recordId
     ,dv.ColName
     ,s.table_name
     ,GETDATE()
     ,@defValueOut
     ,dv.FEditable
     ,dv.Display_on_UI
     ,dv.FHasDefaultVal
     ,dv.FMandatory
     ,dv.FManual
     ,dv.ValueDesc
     ,dv.ValueLabel
     ,dv.handbook_name
     ,dv.paired_field_table
     ,dv.paired_field_column
    FROM dbo.Rail_DEFValues dv
    INNER JOIN dbo.rail_sections s
      ON dv.section_id = s.id
    WHERE dv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

--- Truck import ---
DROP INDEX IF EXISTS Idx_TruckDeclarationTab_filingHeadersFK ON dbo.Truck_DeclarationTab
GO
CREATE INDEX Idx_TruckDeclarationTab_filingHeadersFK
ON dbo.Truck_DeclarationTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_TruckContainersTab_filingHeadersFK ON dbo.Truck_ContainersTab
GO
CREATE INDEX Idx_TruckContainersTab_filingHeadersFK
ON dbo.Truck_ContainersTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_TruckInvoiceHeaders_filingHeadersFK ON dbo.Truck_InvoiceHeaders
GO
CREATE INDEX Idx_TruckInvoiceHeaders_filingHeadersFK
ON dbo.Truck_InvoiceHeaders (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_TruckInvoiceLines_invoiceHeadersFK ON dbo.Truck_InvoiceLines
GO
CREATE INDEX Idx_TruckInvoiceLines_invoiceHeadersFK
ON dbo.Truck_InvoiceLines (InvoiceHeaders_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_TruckMISC_filingHeadersFK ON dbo.Truck_MISC
GO
CREATE INDEX Idx_TruckMISC_filingHeadersFK
ON dbo.Truck_MISC (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_TruckDefValuesManual_recordId_tableName_columnName ON dbo.truck_def_values_manual
GO
CREATE INDEX Idx_TruckDefValuesManual_recordId_tableName_columnName
ON dbo.truck_def_values_manual (record_id, table_name, column_name)
ON [PRIMARY]
GO

-- add truck declaration tab record --
ALTER PROCEDURE dbo.truck_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add declarationTab data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Truck_DeclarationTab declarationTab
      WHERE declarationTab.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO dbo.Truck_DeclarationTab (
        Filing_Headers_FK
       ,TI_FK
       ,Main_Supplier
       ,Importer
       ,Issuer
       ,Master_Bill
       ,Carrier_SCAC
       ,Discharge
       ,Entry_Port
       ,Destination_State
       ,Description
       ,FIRMs_Code)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,details.BDP_FK AS TI_FK
       ,ruleImporters.cw_supplier
       ,ruleImporters.cw_ior
       ,SUBSTRING(inbound.PAPs, 1, 4)
       ,SUBSTRING(inbound.PAPs, 5, LEN(inbound.PAPs))
       ,SUBSTRING(inbound.PAPs, 1, 4)
       ,ruleImporters.arrival_port
       ,ruleImporters.Entry_Port
       ,ruleImporters.Destination_State
       ,ruleImporters.Goods_Description
       ,rulePorts.FIRMs_Code
      FROM Truck_Filing_Details details
      INNER JOIN Truck_Inbound inbound
        ON details.BDP_FK = inbound.id
      LEFT JOIN Truck_Rule_Importers ruleImporters
        ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
      LEFT JOIN Truck_Rule_Ports rulePorts
        ON (RTRIM(LTRIM(ruleImporters.arrival_port)) = RTRIM(LTRIM(rulePorts.arrival_port))
            AND RTRIM(LTRIM(ruleImporters.Entry_Port)) = RTRIM(LTRIM(rulePorts.Entry_Port)))
      WHERE details.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.truck_add_def_values_manual @tableName
                                        ,@filingHeaderId
                                        ,@parentId
                                        ,@recordId

    -- apply default values
    EXEC dbo.truck_apply_def_values @tableName
                                   ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END
GO
-- add truck containers tab record --
ALTER PROCEDURE dbo.truck_add_container_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add сontainersTab data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Truck_ContainersTab сontainersTab
      WHERE сontainersTab.Filing_Headers_FK = @parentId)

  BEGIN
    INSERT INTO dbo.Truck_ContainersTab (
        Filing_Headers_FK
       ,TI_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,details.BDP_FK
      FROM Truck_Filing_Details details
      INNER JOIN Truck_Inbound inbound
        ON details.BDP_FK = inbound.id
      WHERE details.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.truck_add_def_values_manual @tableName
                                        ,@filingHeaderId
                                        ,@parentId
                                        ,@recordId

    -- apply default values
    EXEC dbo.truck_apply_def_values @tableName
                                   ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END
GO
-- add truck invoice header record --
ALTER PROCEDURE dbo.truck_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_InvoiceHeaders'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add invoiceHeaders data and apply rules
  -- invLines.Invoice_Qty * invLines.PriceUnit AS Invoice_Total -- replace with computed field
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Truck_InvoiceHeaders invoiceHeaders
      WHERE invoiceHeaders.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO dbo.Truck_InvoiceHeaders (
        Filing_Headers_FK
       ,Invoice_No
       ,Supplier
       ,Consignee
       ,Transaction_Related
       ,Manufacturer
       ,Seller
       ,Importer
       ,Sold_to_party
       ,Ship_to_party
       ,Export
       ,Origin)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,inbound.PAPs
       ,ruleImporters.cw_supplier
       ,ruleImporters.cw_ior
       ,ruleImporters.transactions_related
       ,ruleImporters.cw_supplier
       ,ruleImporters.cw_supplier
       ,ruleImporters.cw_ior
       ,ruleImporters.cw_ior
       ,ruleImporters.cw_ior
       ,ruleimporters.ce
       ,ruleImporters.co
      FROM dbo.Truck_Filing_Details details
      INNER JOIN dbo.Truck_Inbound inbound
        ON inbound.id = details.BDP_FK
      LEFT JOIN Truck_Rule_Importers ruleImporters
        ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
      WHERE details.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.truck_add_invoice_line_record @filingHeaderId
                                          ,@recordId
                                          ,@filingUser

    -- fill the def value manual table
    EXEC dbo.truck_add_def_values_manual @tableName
                                        ,@filingHeaderId
                                        ,@parentId
                                        ,@recordId

    -- apply default values
    EXEC dbo.truck_apply_def_values @tableName
                                   ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END
GO
-- add truck invoice line record --
ALTER PROCEDURE dbo.truck_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add Invoice Lines data and apply rules  
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Truck_InvoiceLines invoiceLines
      WHERE invoiceLines.Filing_Headers_FK = @parentId)

  BEGIN
    INSERT INTO dbo.Truck_InvoiceLines (
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,TI_FK
       ,Invoice_No
       ,Transaction_Related
       ,Tariff
       ,Customs_QTY
       ,Goods_Description
       ,SPI
       ,ORG
       ,Export
       ,Gr_Weight
       ,Gr_Weight_Unit
       ,UQ
       ,PriceUnit
       ,Prod_ID_1
       ,Attribute_1
       ,Attribute_2
       ,Invoice_Qty
       ,Invoice_Qty_Unit
       ,Amount
       ,Line_Price)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,@parentId
       ,details.BDP_FK
       ,inbound.PAPs
       ,ruleImporters.transactions_related
       ,ruleImporters.Tariff
       ,ruleImporters.custom_quantity
       ,ruleImporters.Goods_Description
       ,ruleImporters.SPI
       ,ruleImporters.co
       ,ruleimporters.ce
       ,ruleImporters.gross_weight
       ,ruleImporters.gross_weight_uq
       ,ruleImporters.custom_uq
       ,ruleImporters.Line_Price
       ,ruleImporters.product_id
       ,ruleImporters.custom_attrib1
       ,ruleImporters.custom_attrib2
       ,ruleImporters.Invoice_Qty
       ,ruleImporters.invoice_uq
       ,ruleImporters.charges
       ,ruleImporters.invoice_qty * ruleImporters.line_price
      FROM dbo.Truck_Filing_Details details
      INNER JOIN dbo.Truck_Inbound inbound
        ON inbound.id = details.BDP_FK
      LEFT JOIN Truck_Rule_Importers ruleImporters
        ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
      WHERE details.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.truck_add_def_values_manual @tableName
                                        ,@filingHeaderId
                                        ,@parentId
                                        ,@recordId

    -- apply default values
    EXEC dbo.truck_apply_def_values @tableName
                                   ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
-- add truck misc record --
ALTER PROCEDURE dbo.truck_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Truck_MISC misc
      WHERE misc.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO dbo.Truck_MISC (
        Filing_Headers_FK
       ,TI_FK
       ,Branch
       ,Broker
       ,Preparer_Dist_Port
       ,Recon_Issue
       ,FTA_Recon)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,details.BDP_FK
       ,userData.Branch
       ,userData.Broker
       ,userData.Location
       ,ruleImporters.Recon_Issue
       ,ruleImporters.nafta_recon
      FROM dbo.Truck_Filing_Details details
      INNER JOIN dbo.Truck_Inbound inbound
        ON inbound.id = details.BDP_FK
      LEFT JOIN Truck_Rule_Importers ruleImporters
        ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
      LEFT JOIN app_users_data userData
        ON userData.UserAccount = @FilingUser
      WHERE details.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN


    -- fill the def value manual table
    EXEC dbo.truck_add_def_values_manual @tableName
                                        ,@filingHeaderId
                                        ,@parentId
                                        ,@recordId

    -- apply default values
    EXEC dbo.truck_apply_def_values @tableName
                                   ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
-- Add Truck Import def values manual records --
ALTER PROCEDURE dbo.truck_add_def_values_manual (@tableName VARCHAR(128)
, @filingHeaderId INT
, @parentId INT
, @recordId INT)
AS
BEGIN
  DECLARE @str_val NVARCHAR(500)
  DECLARE @parmDefinition NVARCHAR(500)
  DECLARE @defValueOut VARCHAR(128)
  DECLARE @dataType VARCHAR(128)
  DECLARE @defValue VARCHAR(500)
  DECLARE @id INT;
  DECLARE @columnName VARCHAR(128)

  DECLARE c CURSOR FOR SELECT DISTINCT
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
   ,v.default_value
   ,v.id
   ,v.column_name
  FROM dbo.truck_def_values v
  INNER JOIN dbo.truck_sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.manual > 0
  OR v.has_default_value > 0
  OR v.display_on_ui > 0
  OR v.single_filing_order > 0)
  ORDER BY id

  OPEN c
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  WHILE @@fetch_status = 0
  BEGIN
  SET @defValueOut = NULL;
  SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(''' + @defValue + ''' as ' + @dataType + ') as varchar(500))';
  SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@parmDefinition
                       ,@defValueOut = @defValueOut OUTPUT;
  IF @defValueOut IS NULL
  BEGIN
    SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(' + @defValue + ' as ' + @dataType + ') as varchar(500))';
    SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';

    DECLARE @result FLOAT
  BEGIN TRY
    EXECUTE sp_executesql @str_val
                         ,@parmDefinition
                         ,@defValueOut = @defValueOut OUTPUT;
  END TRY
  BEGIN CATCH
    SET @defValueOut = NULL;
  END CATCH;
  END;

  IF @dataType LIKE 'date%'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS DATETIME), 'MM/dd/yyyy');
  END;
  IF @dataType LIKE 'numeric'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS FLOAT), '0.######');
  END;

  INSERT INTO dbo.truck_def_values_manual (
      filing_header_id
     ,parent_record_id
     ,section_name
     ,section_title
     ,record_id
     ,column_name
     ,table_name
     ,modification_date
     ,value
     ,editable
     ,display_on_ui
     ,has_default_value
     ,mandatory
     ,manual
     ,description
     ,label
     ,handbook_name
     ,paired_field_table
     ,paired_field_column)
    SELECT
      @filingHeaderId
     ,@parentId
     ,ts.name
     ,ts.title
     ,@recordId
     ,dv.column_name
     ,ts.table_name
     ,GETDATE()
     ,@defValueOut
     ,dv.editable
     ,dv.display_on_ui
     ,dv.has_default_value
     ,dv.mandatory
     ,dv.manual
     ,dv.description
     ,dv.label
     ,handbook_name
     ,paired_field_table
     ,paired_field_column
    FROM dbo.truck_def_values dv
    INNER JOIN dbo.truck_sections ts
      ON dv.section_id = ts.id
    WHERE dv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

--- Truck Export ---
DROP INDEX IF EXISTS Idx_TruckExportDeclarationTab_filingHeadersId ON dbo.truck_export_declarations
GO
CREATE INDEX Idx_TruckExportDeclarationTab_filingHeadersId
ON dbo.truck_export_declarations (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_TruckExportInvoiceHeaders_filingHeadersId ON dbo.truck_export_invoice_headers
GO
CREATE INDEX Idx_TruckExportInvoiceHeaders_filingHeadersId
ON dbo.truck_export_invoice_headers (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_TruckExportInvoiceLines_invoiceHeadersId ON dbo.truck_export_invoice_lines
GO
CREATE INDEX Idx_TruckExportInvoiceLines_invoiceHeadersId
ON dbo.truck_export_invoice_lines (invoice_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

-- Add Truck Export def values manual records --
ALTER PROCEDURE dbo.truck_export_add_def_values_manual (@tableName VARCHAR(128)
, @filingHeaderId INT
, @parentId INT
, @recordId INT)
AS
BEGIN
  DECLARE @str_val NVARCHAR(500)
  DECLARE @parmDefinition NVARCHAR(500)
  DECLARE @defValueOut VARCHAR(128)
  DECLARE @dataType VARCHAR(128)
  DECLARE @defValue VARCHAR(500)
  DECLARE @id INT;
  DECLARE @columnName VARCHAR(128)

  DECLARE c CURSOR FOR SELECT DISTINCT
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
   ,v.default_value
   ,v.id
   ,v.column_name
  FROM dbo.truck_export_def_values v
  INNER JOIN dbo.truck_export_sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.manual > 0
  OR v.has_default_value > 0
  OR v.display_on_ui > 0
  OR v.single_filing_order > 0)
  ORDER BY id

  OPEN c
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  WHILE @@fetch_status = 0
  BEGIN
  SET @defValueOut = NULL;
  SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(''' + @defValue + ''' as ' + @dataType + ') as varchar(500))';
  SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@parmDefinition
                       ,@defValueOut = @defValueOut OUTPUT;
  IF @defValueOut IS NULL
  BEGIN
    SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(' + @defValue + ' as ' + @dataType + ') as varchar(500))';
    SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';

    DECLARE @result FLOAT
  BEGIN TRY
    EXECUTE sp_executesql @str_val
                         ,@parmDefinition
                         ,@defValueOut = @defValueOut OUTPUT;
  END TRY
  BEGIN CATCH
    SET @defValueOut = NULL;
  END CATCH;
  END;

  IF @dataType LIKE 'date%'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS DATETIME), 'MM/dd/yyyy');
  END;
  IF @dataType LIKE 'numeric'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS FLOAT), '0.######');
  END;

  INSERT INTO dbo.truck_export_def_values_manual (
      filing_header_id
     ,parent_record_id
     ,section_name
     ,section_title
     ,record_id
     ,column_name
     ,table_name
     ,modification_date
     ,value
     ,editable
     ,display_on_ui
     ,has_default_value
     ,mandatory
     ,manual
     ,description
     ,label
     ,handbook_name
     ,paired_field_table
     ,paired_field_column)
    SELECT
      @filingHeaderId
     ,@parentId
     ,tes.name
     ,tes.title
     ,@recordId
     ,tedv.column_name
     ,tes.table_name
     ,GETDATE()
     ,@defValueOut
     ,tedv.editable
     ,tedv.display_on_ui
     ,tedv.has_default_value
     ,tedv.mandatory
     ,tedv.manual
     ,tedv.description
     ,tedv.label
     ,handbook_name
     ,paired_field_table
     ,paired_field_column
    FROM dbo.truck_export_def_values tedv
    INNER JOIN dbo.truck_export_sections tes
      ON tedv.section_id = tes.id
    WHERE tedv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

--- Pipeline ---
DROP INDEX IF EXISTS Idx_PipelineDeclarationTab_filingHeadersFK ON dbo.Pipeline_DeclarationTab
GO
CREATE INDEX Idx_PipelineDeclarationTab_filingHeadersFK
ON dbo.Pipeline_DeclarationTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_PipelineContainersTab_filingHeadersFK ON dbo.Pipeline_ContainersTab
GO
CREATE INDEX Idx_PipelineContainersTab_filingHeadersFK
ON dbo.Pipeline_ContainersTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_PipelineInvoiceHeaders_filingHeadersFK ON dbo.Pipeline_InvoiceHeaders
GO
CREATE INDEX Idx_PipelineInvoiceHeaders_filingHeadersFK
ON dbo.Pipeline_InvoiceHeaders (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_PipelineInvoiceLines_invoiceHeadersFK ON dbo.Pipeline_InvoiceLines
GO
CREATE INDEX Idx_PipelineInvoiceLines_invoiceHeadersFK
ON dbo.Pipeline_InvoiceLines (InvoiceHeaders_FK)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_PipelineMISC_filingHeadersFK ON dbo.Pipeline_MISC
GO
CREATE INDEX Idx_PipelineMISC_filingHeadersFK
ON dbo.Pipeline_MISC (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

-- Add Pipeline Import def values manual records --
ALTER PROCEDURE dbo.pipeline_add_def_values_manual (@tableName VARCHAR(128)
, @filingHeaderId INT
, @parentId INT
, @recordId INT)
AS
BEGIN
  DECLARE @str_val NVARCHAR(500)
  DECLARE @parmDefinition NVARCHAR(500)
  DECLARE @defValueOut VARCHAR(128)
  DECLARE @dataType VARCHAR(128)
  DECLARE @defValue VARCHAR(500)
  DECLARE @id INT;
  DECLARE @columnName VARCHAR(128)

  DECLARE c CURSOR FOR SELECT DISTINCT
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
   ,v.DefValue
   ,v.id
   ,v.ColName
  FROM dbo.Pipeline_DEFValues v
  INNER JOIN dbo.pipeline_sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.FManual > 0
  OR v.FHasDefaultVal > 0
  OR v.Display_on_UI > 0
  OR v.SingleFilingOrder > 0)
  ORDER BY id

  OPEN c
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  WHILE @@fetch_status = 0
  BEGIN
  SET @defValueOut = NULL;
  SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(''' + @defValue + ''' as ' + @dataType + ') as varchar(500))';
  SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@parmDefinition
                       ,@defValueOut = @defValueOut OUTPUT;
  IF @defValueOut IS NULL
  BEGIN
    SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(' + @defValue + ' as ' + @dataType + ') as varchar(500))';
    SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';

    DECLARE @result FLOAT
  BEGIN TRY
    EXECUTE sp_executesql @str_val
                         ,@parmDefinition
                         ,@defValueOut = @defValueOut OUTPUT;
  END TRY
  BEGIN CATCH
    SET @defValueOut = NULL;
  END CATCH;
  END;

  IF @dataType LIKE 'date%'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS DATETIME), 'MM/dd/yyyy');
  END;
  IF @dataType LIKE 'numeric'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS FLOAT), '0.######');
  END;

  INSERT INTO dbo.Pipeline_DEFValues_Manual (
      Filing_Headers_FK
     ,parent_record_id
     ,section_name
     ,section_title
     ,record_id
     ,column_name
     ,table_name
     ,ModifiedDate
     ,value
     ,FEditable
     ,Display_on_UI
     ,FHasDefaultVal
     ,FMandatory
     ,FManual
     ,description
     ,ValueLabel
     ,handbook_name
     ,paired_field_table
     ,paired_field_column)
    SELECT
      @filingHeaderId
     ,@parentId
     ,s.name
     ,s.title
     ,@recordId
     ,dv.ColName
     ,s.table_name
     ,GETDATE()
     ,@defValueOut
     ,dv.FEditable
     ,dv.Display_on_UI
     ,dv.FHasDefaultVal
     ,dv.FMandatory
     ,dv.FManual
     ,dv.ValueDesc
     ,dv.ValueLabel
     ,dv.handbook_name
     ,dv.paired_field_table
     ,dv.paired_field_column
    FROM dbo.Pipeline_DEFValues dv
    INNER JOIN dbo.pipeline_sections s
      ON dv.section_id = s.id
    WHERE dv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

--- Vessel Import ---
DROP INDEX IF EXISTS Idx_VesselImportDeclarations_filingHeadersId ON dbo.Vessel_Import_Declarations
GO
CREATE INDEX Idx_VesselImportDeclarations_filingHeadersId
ON dbo.Vessel_Import_Declarations (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_VesselImportPackings_filingHeadersId ON dbo.Vessel_Import_Packings
GO
CREATE INDEX Idx_VesselImportPackings_filingHeadersId
ON dbo.Vessel_Import_Packings (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_VesselImportInvoiceHeaders_filingHeadersId ON dbo.Vessel_Import_Invoice_Headers
GO
CREATE INDEX Idx_VesselImportInvoiceHeaders_filingHeadersId
ON dbo.Vessel_Import_Invoice_Headers (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_VesselImportInvoiceLines_invoiceHeadersId ON dbo.Vessel_Import_Invoice_Lines
GO
CREATE INDEX Idx_VesselImportInvoiceLines_invoiceHeadersId
ON dbo.Vessel_Import_Invoice_Lines (invoice_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_VesselImportMISC_filingHeadersId ON dbo.Vessel_Import_Miscs
GO
CREATE INDEX Idx_VesselImportMISC_filingHeadersId
ON dbo.Vessel_Import_Miscs (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_VesselImportDefValuesManual_recordId_tableName_columnName ON dbo.Vessel_Import_Def_Values_Manual
GO
CREATE INDEX Idx_VesselImportDefValuesManual_recordId_tableName_columnName
ON dbo.Vessel_Import_Def_Values_Manual (record_id, table_name, column_name)
ON [PRIMARY]
GO

--- add vessel import declarations record ---
ALTER PROCEDURE dbo.vessel_import_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Declarations vid
      WHERE vid.filing_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Import_Declarations (
        filing_header_id
       ,main_supplier
       ,importer
       ,container
       ,ent_type
       ,eta
       ,arr
       ,vessel
       ,port_of_discharge
       ,entry_port
       ,dest_state
       ,firms_code
       ,description
       ,hmf
       ,owner_ref
       ,destination)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,supplier.ClientCode
       ,importer.ClientCode
       ,vi.container
       ,vi.entry_type
       ,vi.eta
       ,vi.eta
       ,vessel.name
       ,vrp.entry_port
       ,vrp.entry_port
       ,states.StateCode
       ,firms.firms_code
       ,vpd.name
       ,vrp.hmf
       ,vi.owner_ref
       ,vrp.destination_code
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Vessels vessel
        ON vessel.id = vi.vessel_id
      LEFT JOIN dbo.Vessel_Rule_Port vrp
        ON vi.firms_code_id = vrp.firms_code_id
      LEFT JOIN dbo.Vessel_ProductDescriptions vpd
        ON vi.product_description_id = vpd.id
      LEFT JOIN dbo.app_users_data aud
        ON vi.user_id = aud.UserAccount
      LEFT JOIN cw_firms_codes firms
        ON firms.id = vi.firms_code_id
      LEFT JOIN US_States states
        ON firms.state_id = states.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO

--- add vessel import invoice header record ---
ALTER PROCEDURE dbo.vessel_import_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Invoice_Headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Invoice_Headers viih
      WHERE viih.filing_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Import_Invoice_Headers (
        filing_header_id
       ,supplier
       ,seller
       ,manufacturer
       ,importer
       ,sold_to_party
       ,ship_to_party
       ,country_of_origin
       ,consignee
      --    , transaction_related
      )
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,country.code
       ,importer.ClientCode
      --       ,IIF(vi.importer_id = vi.supplier_id, 'Y', 'N')

      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Countries country
        ON vi.country_of_origin_id = country.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.vessel_import_add_invoice_line_record @filingHeaderId
                                                  ,@recordId
                                                  ,@filingUser

    UPDATE dbo.Vessel_Import_Invoice_Headers
    SET line_total = line.total
       ,invoice_total = line.total
    FROM Vessel_Import_Invoice_Headers viih
    LEFT JOIN (SELECT
        viil.invoice_header_id
       ,SUM(viil.price) AS total
      FROM Vessel_Import_Invoice_Lines viil
      GROUP BY viil.invoice_header_id) AS line
      ON viih.id = line.invoice_header_id
    WHERE viih.id = @recordId


    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO

--- add vessel import invoice line record ---
ALTER PROCEDURE dbo.vessel_import_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Invoice_Lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Invoice_Lines viil
      WHERE viil.invoice_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Import_Invoice_Lines (
        invoice_header_id
       ,classification
       ,tariff
       ,goods_description
       ,destination_state
       ,consignee
       ,seller
       ,attribute1
       ,attribute2
       ,attribute3
       ,epa_tsca_toxic_substance_control_act_indicator
       ,tsca_indicator
       ,certifying_individual
       ,hmf
       ,product
       ,customs_qty_unit
       ,manufacturer
       ,sold_to_party
       ,customs_qty
       ,invoice_qty
       ,unit_price
       ,country_of_origin
       ,price
       ,invoice_quantity_unit)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,tariff.USC_Tariff
       ,tariff.USC_Tariff
       ,COALESCE(NULLIF(rule_product.goods_description, ''), vpd.name)
       ,state.StateCode
       ,importer.ClientCode
       ,supplier.ClientCode
       ,COALESCE(NULLIF(rule_product.customs_attribute1, ''), vpd.name)
       ,rule_product.customs_attribute2
       ,vi.owner_ref
       ,rule_product.tsca_requirement
       ,IIF(rule_product.tsca_requirement = 'D', '+', NULL)
       ,IIF(rule_product.tsca_requirement = 'D', 'CB', NULL)
       ,rule_port.hmf
       ,vpd.name
       ,tariff.Unit
       ,supplier.ClientCode
       ,importer.ClientCode
       ,vi.customs_qty
       ,vi.customs_qty
       ,vi.unit_price
       ,country.code
       ,vi.unit_price * vi.customs_qty
       ,COALESCE(NULLIF(rule_product.invoice_uq, ''), tariff.Unit)
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Vessel_ProductDescriptions vpd
        ON vi.product_description_id = vpd.id
      LEFT JOIN dbo.Tariff tariff
        ON vi.classification_id = tariff.id
      LEFT JOIN dbo.Vessel_Rule_Product rule_product
        ON tariff.USC_Tariff = rule_product.tariff
      LEFT JOIN dbo.Vessel_Rule_Port rule_port
        ON vi.firms_code_id = rule_port.firms_code_id
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Countries country
        ON vi.country_of_origin_id = country.id
      LEFT JOIN cw_firms_codes firms
        ON vi.firms_code_id = firms.id
      LEFT JOIN US_States state
        ON firms.state_id = state.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO

--- Vessel import add misc record ---
ALTER PROCEDURE dbo.vessel_import_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Miscs'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Miscs vim
      WHERE vim.filing_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Import_Miscs (
        filing_header_id
       ,branch
       ,broker
       ,preparer_dist_port)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,userData.Branch
       ,userData.Broker
       ,userData.Location
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN app_users_data userData
        ON userData.UserAccount = vi.user_id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO

--- add vessel import packing record ---
ALTER PROCEDURE dbo.vessel_import_add_packing_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Packings'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Packings vip
      WHERE vip.filing_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Import_Packings (
        filing_header_id)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO

-- Add Vessel import def value manual values records --
ALTER PROCEDURE dbo.vessel_import_add_def_values_manual (@tableName VARCHAR(128)
, @filingHeaderId INT
, @parentId INT
, @recordId INT)
AS
BEGIN
  DECLARE @str_val NVARCHAR(500)
  DECLARE @parmDefinition NVARCHAR(500)
  DECLARE @defValueOut VARCHAR(128)
  DECLARE @dataType VARCHAR(128)
  DECLARE @defValue VARCHAR(500)
  DECLARE @id INT;
  DECLARE @columnName VARCHAR(128)

  DECLARE c CURSOR FOR SELECT DISTINCT
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
   ,v.default_value
   ,v.id
   ,v.column_name
  FROM dbo.Vessel_Import_Def_Values v
  INNER JOIN dbo.Vessel_Import_Sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.manual > 0
  OR v.has_default_value > 0
  OR v.display_on_ui > 0
  OR v.single_filing_order > 0)
  ORDER BY id

  OPEN c
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  WHILE @@fetch_status = 0
  BEGIN
  SET @defValueOut = NULL;
  SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(''' + @defValue + ''' as ' + @dataType + ') as varchar(500))';
  SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@parmDefinition
                       ,@defValueOut = @defValueOut OUTPUT;
  IF @defValueOut IS NULL
  BEGIN
    SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(' + @defValue + ' as ' + @dataType + ') as varchar(500))';
    SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';

    DECLARE @result FLOAT
  BEGIN TRY
    EXECUTE sp_executesql @str_val
                         ,@parmDefinition
                         ,@defValueOut = @defValueOut OUTPUT;
  END TRY
  BEGIN CATCH
    SET @defValueOut = NULL;
  END CATCH;
  END;

  IF @dataType LIKE 'date%'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS DATETIME), 'MM/dd/yyyy');
  END;
  IF @dataType LIKE 'numeric'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS FLOAT), '0.######');
  END;

  INSERT INTO dbo.Vessel_Import_Def_Values_Manual (
      filing_header_id
     ,parent_record_id
     ,section_name
     ,section_title
     ,record_id
     ,column_name
     ,table_name
     ,modification_date
     ,value
     ,editable
     ,display_on_ui
     ,has_default_value
     ,mandatory
     ,manual
     ,description
     ,label
     ,handbook_name
     ,paired_field_table
     ,paired_field_column)
    SELECT
      @filingHeaderId
     ,@parentId
     ,s.name
     ,s.title
     ,@recordId
     ,dv.column_name
     ,s.table_name
     ,GETDATE()
     ,@defValueOut
     ,dv.editable
     ,dv.display_on_ui
     ,dv.has_default_value
     ,dv.mandatory
     ,dv.manual
     ,dv.description
     ,dv.label
     ,dv.handbook_name
     ,dv.paired_field_table
     ,dv.paired_field_column
    FROM dbo.Vessel_Import_Def_Values dv
    INNER JOIN dbo.Vessel_Import_Sections s
      ON dv.section_id = s.id
    WHERE dv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

--- Vessel Export --- 
DROP INDEX IF EXISTS Idx_VesselExportDeclarations_filingHeadersId ON dbo.Vessel_Export_Declarations
GO
CREATE INDEX Idx_VesselExportDeclarations_filingHeadersId
ON dbo.Vessel_Export_Declarations (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_VesselExportInvoiceHeaders_filingHeadersId ON dbo.Vessel_Export_Invoice_Headers
GO
CREATE INDEX Idx_VesselExportInvoiceHeaders_filingHeadersId
ON dbo.Vessel_Export_Invoice_Headers (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

DROP INDEX IF EXISTS Idx_VesselExportInvoiceLines_invoiceHeadersId ON dbo.Vessel_Export_Invoice_Lines
GO
CREATE INDEX Idx_VesselExportInvoiceLines_invoiceHeadersId
ON dbo.Vessel_Export_Invoice_Lines (invoice_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

--- add vessel export declarations record ---
ALTER PROCEDURE dbo.vessel_export_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Export_Declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM dbo.Vessel_Export_Sections section
  WHERE section.table_name = @tableName

  -- add data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Export_Declarations ved
      WHERE ved.filing_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Export_Declarations (
        filing_header_id
       ,main_supplier
       ,importer
       ,container
       ,transaction_related
       ,routed_tran
       ,tariff_type
       ,sold_en_route
       ,vessel
       ,port_of_loading
       ,dep
       ,discharge
       ,export
       ,exp
       ,etd
       ,export_date
       ,description
       ,owner_ref
       ,transport_ref
       ,country_of_dest
       ,destination
       ,origin
       ,state_of_origin
       ,inbond_type
       ,export_adjustment_value
       ,original_itn)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,usppi.ClientCode
       ,importer.ClientCode
       ,intake.container
       ,usppi_consignee.transaction_related
       ,intake.routed_transaction
       ,intake.tariff_type
       ,intake.sold_en_route
       ,vessel.name
       ,intake.load_port
       ,intake.export_date
       ,intake.discharge_port
       ,intake.load_port
       ,intake.export_date
       ,intake.export_date
       ,intake.export_date
       ,intake.description
       ,intake.transport_ref + ' - ' + intake.scheduler
       ,intake.transport_ref
       ,country.code
       ,foreign_port.unloco
       ,domestic_port.unloco
       ,domestic_port.state
       ,intake.in_bond
       ,intake.export_adjustment_value
       ,intake.original_itn

      FROM dbo.Vessel_Export_Filing_Details detail
      INNER JOIN dbo.Vessel_Exports intake
        ON intake.id = detail.vessel_export_id
      LEFT JOIN dbo.Clients usppi
        ON intake.usppi_id = usppi.id
      LEFT JOIN dbo.Clients importer
        ON intake.importer_id = importer.id
      LEFT JOIN dbo.Vessels vessel
        ON vessel.id = intake.vessel_id
      LEFT JOIN dbo.Countries country
        ON intake.country_of_destination_id = country.id
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI_Consignee usppi_consignee
        ON usppi_consignee.usppi_id = intake.usppi_id
          AND usppi_consignee.consignee_id = intake.importer_id
      LEFT JOIN CW_Domestic_Ports domestic_port
        ON intake.load_port = domestic_port.port_code
      LEFT JOIN CW_Foreign_Ports foreign_port
        ON intake.discharge_port = foreign_port.port_code
      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_export_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_export_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Export_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
-- add vessel export invoice header record --
ALTER PROCEDURE dbo.vessel_export_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Export_Invoice_Headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ves.is_array
  FROM dbo.Vessel_Export_Sections ves
  WHERE ves.table_name = @tableName

  -- add data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Export_Invoice_Headers veih
      WHERE veih.filing_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Export_Invoice_Headers (
        filing_header_id
       ,usppi
       ,usppi_address
       ,usppi_contact
       ,usppi_phone
       ,ultimate_consignee
       ,ultimate_consignee_address
       ,ultimate_consignee_type
       ,origin_indicator
       ,export_date)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,usppi.ClientCode
       ,usppi_address.code
       ,usppi_rule.contact
       ,usppi_rule.phone
       ,consignee.ClientCode
       ,consignee_address.code
       ,usppi_consignee_rule.ultimate_consignee_type
       ,intake.origin_indicator
       ,intake.export_date
      FROM dbo.Vessel_Export_Filing_Details detail
      INNER JOIN dbo.Vessel_Exports intake
        ON intake.id = detail.vessel_export_id
      LEFT JOIN dbo.Clients usppi
        ON intake.usppi_id = usppi.id
      LEFT JOIN dbo.Clients consignee
        ON intake.importer_id = consignee.id
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI usppi_rule
        ON usppi_rule.usppi_id = intake.usppi_id
      LEFT JOIN dbo.Client_Addresses usppi_address
        ON usppi_address.id = usppi_rule.address_id
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI_Consignee usppi_consignee_rule
        ON usppi_consignee_rule.usppi_id = intake.usppi_id
          AND usppi_consignee_rule.consignee_id = intake.importer_id
      LEFT JOIN dbo.Client_Addresses consignee_address
        ON consignee_address.id = usppi_consignee_rule.consignee_address_id
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
    EXEC dbo.vessel_export_add_invoice_line_record @filingHeaderId
                                                  ,@recordId
                                                  ,@filingUser

    UPDATE dbo.Vessel_Export_Invoice_Headers
    SET invoice_total_amount = line.total
    FROM Vessel_Export_Invoice_Headers viih
    LEFT JOIN (SELECT
        viil.invoice_header_id
       ,SUM(viil.price) AS total
      FROM Vessel_Export_Invoice_Lines viil
      GROUP BY viil.invoice_header_id) AS line
      ON viih.id = line.invoice_header_id
    WHERE viih.id = @recordId

    -- fill the def value manual table
    EXEC dbo.vessel_export_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_export_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Export_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
-- add vessel export invoice line record --
ALTER PROCEDURE dbo.vessel_export_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Export_Invoice_Lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ves.is_array
  FROM dbo.Vessel_Export_Sections ves
  WHERE ves.table_name = @tableName

  -- add data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Export_Invoice_Lines veil
      WHERE veil.invoice_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Export_Invoice_Lines (
        invoice_header_id
       ,tariff_type
       ,tariff
       ,customs_qty
       ,customs_qty_unit
       ,price
       ,gross_weight
       ,goods_description
       ,invoice_qty_unit
       ,goods_origin)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,intake.tariff_type
       ,intake.tariff
       ,intake.quantity
       ,dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)
       ,intake.[value]
       ,COALESCE(intake.weight, dbo.weightToTon(intake.quantity, dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)))
       ,intake.goods_description
       ,dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)
       ,IIF(intake.origin_indicator = 'D', 'US', NULL)
      FROM dbo.Vessel_export_Filing_Details detail
      INNER JOIN dbo.Vessel_Exports intake
        ON intake.id = detail.vessel_export_id

      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_export_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_export_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Export_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
-- Add Vessel export def value manual values records --
ALTER PROCEDURE dbo.vessel_export_add_def_values_manual (@tableName VARCHAR(128)
, @filingHeaderId INT
, @parentId INT
, @recordId INT)
AS
BEGIN
  DECLARE @str_val NVARCHAR(500)
  DECLARE @parmDefinition NVARCHAR(500)
  DECLARE @defValueOut VARCHAR(128)
  DECLARE @dataType VARCHAR(128)
  DECLARE @defValue VARCHAR(500)
  DECLARE @id INT;
  DECLARE @columnName VARCHAR(128)

  DECLARE c CURSOR FOR SELECT DISTINCT
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
   ,v.default_value
   ,v.id
   ,v.column_name
  FROM dbo.Vessel_Export_Def_Values v
  INNER JOIN dbo.Vessel_Export_Sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.manual > 0
  OR v.has_default_value > 0
  OR v.display_on_ui > 0
  OR v.single_filing_order > 0)
  ORDER BY id

  OPEN c
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  WHILE @@fetch_status = 0
  BEGIN
  SET @defValueOut = NULL;
  SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(''' + @defValue + ''' as ' + @dataType + ') as varchar(500))';
  SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@parmDefinition
                       ,@defValueOut = @defValueOut OUTPUT;
  IF @defValueOut IS NULL
  BEGIN
    SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(' + @defValue + ' as ' + @dataType + ') as varchar(500))';
    SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';

    DECLARE @result FLOAT
  BEGIN TRY
    EXECUTE sp_executesql @str_val
                         ,@parmDefinition
                         ,@defValueOut = @defValueOut OUTPUT;
  END TRY
  BEGIN CATCH
    SET @defValueOut = NULL;
  END CATCH;
  END;

  IF @dataType LIKE 'date%'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS DATETIME), 'MM/dd/yyyy');
  END;
  IF @dataType LIKE 'numeric'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS FLOAT), '0.######');
  END;

  INSERT INTO dbo.Vessel_Export_Def_Values_Manual (
      filing_header_id
     ,parent_record_id
     ,section_name
     ,section_title
     ,record_id
     ,column_name
     ,table_name
     ,modification_date
     ,value
     ,editable
     ,display_on_ui
     ,has_default_value
     ,mandatory
     ,manual
     ,description
     ,label
     ,handbook_name
     ,paired_field_table
     ,paired_field_column)
    SELECT
      @filingHeaderId
     ,@parentId
     ,s.name
     ,s.title
     ,@recordId
     ,dv.column_name
     ,s.table_name
     ,GETDATE()
     ,@defValueOut
     ,dv.editable
     ,dv.display_on_ui
     ,dv.has_default_value
     ,dv.mandatory
     ,dv.manual
     ,dv.description
     ,dv.label
     ,dv.handbook_name
     ,dv.paired_field_table
     ,dv.paired_field_column
    FROM dbo.Vessel_Export_Def_Values dv
    INNER JOIN dbo.Vessel_Export_Sections s
      ON dv.section_id = s.id
    WHERE dv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO