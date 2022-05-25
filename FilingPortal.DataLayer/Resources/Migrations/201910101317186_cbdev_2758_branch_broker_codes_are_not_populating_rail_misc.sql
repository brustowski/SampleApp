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
    INSERT INTO Rail_MISC (Recon_Issue
    , FTA_Recon
    , Payment_Type
    , Broker_to_Pay
    , Submitter
    , Branch
    , Broker
    , Preparer_Dist_Port
    , Filing_Headers_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        ISNULL(rn.Value_Recon, 'N/A') AS Recon_Issue
       ,rn.FTA_Recon AS FTA_Recon
       ,rn.Payment_Type AS Payment_Type
       ,rn.Broker_to_Pay AS Broker_to_Pay
       ,rn.Importer AS Submitter
       ,userData.Branch
       ,userData.Broker
       ,userData.Location
       ,d.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details d
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = d.BDP_FK
      LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
        ON RTRIM(LTRIM(p.Importer)) = RTRIM(LTRIM(rn.Importer_Name))
          AND (RTRIM(LTRIM(p.Supplier)) = RTRIM(LTRIM(rn.Supplier_Name))
            OR (p.Supplier IS NULL
              AND rn.Supplier_Name IS NULL))
      LEFT JOIN app_users_data userData
        ON userData.UserAccount = @filingUser
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