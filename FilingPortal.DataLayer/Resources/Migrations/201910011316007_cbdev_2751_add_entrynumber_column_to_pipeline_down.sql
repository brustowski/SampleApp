-- Add Pipeline Declaration Tab record --
ALTER PROCEDURE dbo.pipeline_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @ImporterCode VARCHAR(128);
  DECLARE @IDs TABLE (
    ID INT
  );

  --Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Pipeline_DeclarationTab pdt
      WHERE pdt.Filing_Headers_FK = @parentId)
  BEGIN

    INSERT INTO Pipeline_DeclarationTab (Filing_Headers_FK
    , PI_FK
    , Main_Supplier
    , Importer
    , Issuer
    , Batch_Ticket
    , Pipeline
    , Carrier_SCAC
    , Discharge
    , Entry_Port
    , Dep
    , Arr
    , Arr_2
    , Origin
    , Destination
    , Destination_State
    , ETA
    , Export_Date
    , Description
    , Owner_Ref
    , FIRMs_Code
    , Master_Bill
    , Importer_of_record)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,details.Pipeline_Inbounds_FK
       ,ruleImporters.Supplier
       ,@ImporterCode
       ,ruleFacility.Issuer
       ,REPLACE(inbound.TicketNumber, '-', '')
       ,ruleFacility.Pipeline
       ,ruleFacility.Issuer
       ,ruleFacility.port
       ,ruleFacility.port
       ,inbound.ExportDate
       ,inbound.ImportDate
       ,inbound.ImportDate
       ,ruleFacility.Origin
       ,ruleFacility.Destination
       ,ruleFacility.Destination_State
       ,inbound.ImportDate
       ,inbound.ImportDate
       ,CONCAT(ruleFacility.pipeline, ' P/L', ': ', inbound.Batch)
       ,inbound.TicketNumber
       ,ruleFacility.FIRMs_Code
       ,REPLACE(inbound.TicketNumber, '-', '')
       ,@ImporterCode
      FROM Pipeline_Filing_Details details
      INNER JOIN Pipeline_Inbound inbound
        ON details.Pipeline_Inbounds_FK = inbound.id
      LEFT JOIN Pipeline_Rule_Facility ruleFacility
        ON inbound.facility = ruleFacility.facility
      LEFT JOIN Pipeline_Rule_Importer ruleImporters
        ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
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
GO

ALTER VIEW dbo.Pipeline_Inbound_Grid
AS
SELECT
  pi.Id
 ,pi.Importer
 ,pi.Batch
 ,pi.TicketNumber
 ,pi.Facility
 ,pi.SiteName
 ,pi.Quantity
 ,pi.API
 ,pi.ExportDate
 ,pi.ImportDate
 ,pi.CreatedDate
 ,fh.id AS Filing_Headers_Id
 ,ISNULL(fh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ms.name Filing_Headers_MappingStatus_Title
 ,ISNULL(fh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,fs.name Filing_Headers_FilingStatus_Title
 ,fh.FilingNumber AS Filing_Headers_FilingNumber
 ,fh.job_link
 ,pi.FDeleted
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Pipeline_Rule_Importer ruleImporter
        WHERE RTRIM(LTRIM(pi.Importer)) = RTRIM(LTRIM(ruleImporter.importer))) THEN 1
    ELSE 0
  END AS has_importer_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Pipeline_Rule_BatchCode ruleBatch
        WHERE dbo.extractBatchCode(pi.Batch) = ruleBatch.batch_code) THEN 1
    ELSE 0
  END AS has_batch_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Pipeline_Rule_Facility ruleFacility
        WHERE pi.Facility = ruleFacility.facility) THEN 1
    ELSE 0
  END AS has_facility_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Pipeline_Rule_Price rulePrice
        INNER JOIN Clients clients
          ON rulePrice.importer_id = clients.id
        WHERE RTRIM(LTRIM(pi.Importer)) = RTRIM(LTRIM(clients.ClientCode))
        AND clients.id = rulePrice.importer_id) THEN 1
    ELSE 0
  END AS has_price_rule

FROM dbo.Pipeline_Inbound pi
LEFT OUTER JOIN dbo.Pipeline_Filing_Details fd
  ON pi.Id = fd.Pipeline_Inbounds_FK
LEFT OUTER JOIN dbo.Pipeline_Filing_Headers fh
  ON fd.Filing_Headers_FK = fh.id
    AND fh.MappingStatus <> 0
LEFT JOIN MappingStatus ms
  ON ISNULL(fh.MappingStatus, 0) = ms.id
LEFT JOIN FilingStatus fs
  ON ISNULL(fh.FilingStatus, 0) = fs.id
WHERE NOT EXISTS (SELECT
    1
  FROM dbo.Pipeline_Filing_Headers fh
  INNER JOIN dbo.Pipeline_Filing_Details fd
    ON fh.id = fd.Filing_Headers_FK
  WHERE fh.MappingStatus > 0
  AND pi.Id = fd.Pipeline_Inbounds_FK)
AND pi.FDeleted = 0

UNION

SELECT
  pi.Id
 ,pi.Importer
 ,pi.Batch
 ,pi.TicketNumber
 ,pi.Facility
 ,pi.SiteName
 ,pi.Quantity
 ,pi.API
 ,pi.ExportDate
 ,pi.ImportDate
 ,pi.CreatedDate
 ,fh.id AS Filing_Headers_Id
 ,ISNULL(fh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ms.name Filing_Headers_MappingStatus_Title
 ,ISNULL(fh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,fs.name Filing_Headers_FilingStatus_Title
 ,fh.FilingNumber AS Filing_Headers_FilingNumber
 ,fh.job_link
 ,pi.FDeleted
 ,1 AS has_importer_rule
 ,1 AS has_batch_rule
 ,1 AS has_facility_rule
 ,1 AS has_price_rule
FROM dbo.Pipeline_Filing_Headers fh
INNER JOIN dbo.Pipeline_Filing_Details fd
  ON fh.id = fd.Filing_Headers_FK
INNER JOIN dbo.Pipeline_Inbound pi
  ON fd.Pipeline_Inbounds_FK = pi.Id
LEFT JOIN MappingStatus ms
  ON ISNULL(fh.MappingStatus, 0) = ms.id
LEFT JOIN FilingStatus fs
  ON ISNULL(fh.FilingStatus, 0) = fs.id
WHERE fh.MappingStatus > 0
AND pi.FDeleted = 0
GO