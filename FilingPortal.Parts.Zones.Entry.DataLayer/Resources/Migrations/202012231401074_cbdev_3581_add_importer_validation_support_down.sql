ALTER VIEW zones_entry.v_inbound_grid
AS
SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id
 ,importer.ClientCode AS importer
 ,inbnd.entry_port
 ,inbnd.arrival_date
 ,inbnd.owner_ref
 ,inbnd.firms_code
 ,inbnd.entry_no
 ,inbnd.vessel_name
 ,inbnd.xml_type
 ,inbnd.deleted AS is_deleted
 ,inbnd.created_user
 ,inbnd.created_date
 ,inbnd.modified_date AS modified_date
 ,inbnd.modified_user AS modified_user
 ,inbnd.is_update
 ,inbnd.is_auto
 ,inbnd.is_auto_processed
 ,inbnd.validation_passed
 ,inbnd.validation_result
 ,filing_header.filing_number
 ,filing_header.job_link
 ,filing_header.created_date AS entry_created_date
 ,filing_header.last_modified_date AS entry_modified_date
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,job_status.id AS job_status
 ,job_status.[name] AS job_status_title
 ,job_status.code AS job_status_code
FROM zones_entry.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
   ,fh.job_status
   ,fh.created_date
   ,fh.last_modified_date
  FROM zones_entry.filing_header AS fh
  JOIN zones_entry.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.job_status > 0) AS filing_header

LEFT JOIN common.job_statuses AS job_status
  ON ISNULL(filing_header.job_status, 0) = job_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'in-bond'
LEFT JOIN dbo.Clients AS importer
  ON inbnd.importer_id = importer.id

WHERE inbnd.deleted = 0
GO

ALTER PROCEDURE zones_entry.sp_inbound_validate (@ids VARCHAR(MAX))
AS
BEGIN
  DECLARE @tbl TABLE (
    id INT NOT NULL
  )

  IF (@ids IS NULL)
    INSERT INTO @tbl (id)
      SELECT
        inbnd.id
      FROM zones_entry.inbound inbnd
  ELSE
  BEGIN
    INSERT INTO @tbl (id)
      SELECT
        CAST(value AS INT)
      FROM STRING_SPLIT(@ids, ',');
  END;

  WITH cte
  AS
  (SELECT
      inbnd.id
     ,rule_importer.found AS importer_found
     ,(SELECT
          [name] AS message
        FROM (VALUES (
        CASE rule_importer.found
          WHEN 0 THEN rule_importer.message
          ELSE NULL
        END)) X ([name])
        FOR JSON PATH)
      AS validation_json_result
    FROM @tbl t
    INNER JOIN zones_entry.inbound inbnd
      ON t.id = inbnd.id
    LEFT JOIN dbo.Clients AS importer
      ON inbnd.importer_id = importer.id
    OUTER APPLY (SELECT
        IIF(EXISTS (SELECT
            1
          FROM zones_entry.rule_importer AS rule_importer
          WHERE rule_importer.importer_id = inbnd.importer_id)
        , 1, 0) AS found
       ,'Importer rule is missing for Importer = "' + importer.ClientCode + '"' AS message) AS rule_importer)

  UPDATE inbnd
  SET validation_passed = t.importer_found
     ,validation_result = validation_json_result
  FROM cte t
  INNER JOIN zones_entry.inbound inbnd
    ON t.id = inbnd.id;
END
GO