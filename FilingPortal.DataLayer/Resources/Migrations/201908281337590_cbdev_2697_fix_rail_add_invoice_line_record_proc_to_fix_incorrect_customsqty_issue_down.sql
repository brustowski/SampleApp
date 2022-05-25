-- add invoice line record --
ALTER PROCEDURE [dbo].[rail_add_invoice_line_record] (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
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
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_InvoiceLines pil
      WHERE pil.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
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
END;