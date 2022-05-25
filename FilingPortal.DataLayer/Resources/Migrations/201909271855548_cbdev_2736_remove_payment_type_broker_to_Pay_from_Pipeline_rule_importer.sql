-- Filing procedures

-- Add Pipeline MISC record --
ALTER PROCEDURE [dbo].[pipeline_add_misc_record] (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @ImporterCode VARCHAR(128)
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Pipeline_MISC pm
      WHERE pm.Filing_Headers_FK = @parentId)
  BEGIN

    INSERT INTO Pipeline_MISC (
        Filing_Headers_FK
       ,PI_FK
       ,Branch
       ,Broker
       ,Preparer_Dist_Port
       ,recon_issue
       ,fta_recon
      )
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,details.Pipeline_Inbounds_FK
       ,userData.Branch
       ,userData.Broker
       ,userData.Location
       ,ruleImporters.recon_issue
       ,ruleImporters.fta_recon
        FROM dbo.Pipeline_Filing_Details details
      INNER JOIN dbo.Pipeline_Inbound inbound
        ON inbound.id = details.Pipeline_Inbounds_FK
      LEFT JOIN Pipeline_Rule_Importer ruleImporters
        ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
      LEFT JOIN app_users_data userData
        ON userData.UserAccount = @filingUser
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
    EXEC dbo.pipeline_add_def_values_manual @tableName
                                           ,@filingHeaderId
                                           ,@parentId
                                           ,@recordId

    -- apply default values
    EXEC dbo.pipeline_apply_def_values @tableName
                                      ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END;
