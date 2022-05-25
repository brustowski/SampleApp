-- add declaration tab record --
ALTER PROCEDURE dbo.rail_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_DeclarationTab pdt
      WHERE pdt.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Rail_DeclarationTab (Carrier_SCAC
    , Country_of_Export
    , Description
    , Destination
    , Destination_State
    , Discharge
    , Entry_Port
    , FIRMs_Code
    , Importer
    , Issuer
    , Main_Supplier
    , Origin
    , Master_Bill
    , FILING_HEADERS_FK)
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
       ,p.BillofLading
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
END;
GO