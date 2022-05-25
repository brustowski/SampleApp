ALTER VIEW dbo.v_exp_truck_inbound_grid
AS
SELECT
  inbnd.id
 ,inbnd.exporter
 ,inbnd.importer
 ,inbnd.tariff_type
 ,inbnd.tariff
 ,inbnd.routed_tran
 ,inbnd.sold_en_route
 ,inbnd.master_bill
 ,inbnd.origin
 ,inbnd.export
 ,inbnd.export_date
 ,inbnd.eccn
 ,inbnd.goods_description
 ,inbnd.customs_qty
 ,inbnd.price
 ,inbnd.gross_weight
 ,inbnd.gross_weight_uom
 ,inbnd.hazardous
 ,inbnd.origin_indicator
 ,inbnd.goods_origin
 ,inbnd.deleted
 ,inbnd.created_date
 ,inbnd.modified_date AS modified_date
 ,inbnd.modified_user AS modified_user
 ,inbnd.is_update
 ,inbnd.is_auto
 ,inbnd.is_auto_processed
 ,inbnd.validation_passed
 ,inbnd.validation_result
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,filing_header.created_date AS entry_created_date
 ,filing_header.last_modified_date AS entry_modified_date
 ,COALESCE(filing_header.update_status, 0) AS update_status
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title 
 ,CAST(1 AS BIT) AS has_all_required_rules
FROM dbo.exp_truck_inbound AS inbnd

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.entry_status
   ,etfh.mapping_status
   ,etfh.update_status
   ,etfh.filing_status
   ,etfh.is_auto_filed
   ,etfh.created_date
   ,etfh.last_modified_date
  FROM dbo.exp_truck_filing_header AS etfh
  JOIN dbo.exp_truck_filing_detail AS etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'export'

WHERE inbnd.deleted = 0
GO

CREATE OR ALTER PROCEDURE dbo.sp_exp_truck_validate (@ids VARCHAR(MAX))
AS
BEGIN
  DECLARE @tbl TABLE (
    id INT NOT NULL
  )

  IF (@ids IS NULL)
    INSERT INTO @tbl (id)
      SELECT
        eti.id
      FROM exp_truck_inbound eti
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
     ,rule_consignee.found AS consignee_found
     ,rule_exporter_consignee.found AS exporter_consignee_found
     ,(SELECT
          [name] AS message
        FROM (VALUES (
        CASE rule_exporter_consignee.found
          WHEN 0 THEN rule_exporter_consignee.message
          ELSE NULL
        END
        ), (
        CASE rule_consignee.found
          WHEN 0 THEN rule_consignee.message
          ELSE NULL
        END)) X ([name])
        FOR JSON PATH)
      AS validation_json_result
    FROM @tbl t
    INNER JOIN exp_truck_inbound inbnd
      ON t.id = inbnd.id
    OUTER APPLY (SELECT
        IIF(EXISTS (SELECT
            1
          FROM exp_truck_rule_consignee AS rule_consignee
          WHERE rule_consignee.consignee_code = inbnd.importer)
        , 1, 0) AS found
       ,'Consignee rule is missing for Importer = "' + inbnd.importer + '"' AS message) AS rule_consignee
    OUTER APPLY (SELECT
        IIF(EXISTS (SELECT
            1
          FROM exp_truck_rule_exporter_consignee AS rule_exporter_consignee
          WHERE rule_exporter_consignee.consignee_code = inbnd.importer
          AND rule_exporter_consignee.exporter = inbnd.exporter)
        , 1, 0) AS found
       ,'Exporter-Consignee rule is missing for Exporter = "' + inbnd.exporter + '" and Importer = "' + inbnd.importer + '"' AS message) AS rule_exporter_consignee)

  UPDATE inbnd
  SET validation_passed = t.consignee_found & t.exporter_consignee_found
     ,validation_result = validation_json_result
  FROM cte t
  INNER JOIN exp_truck_inbound inbnd
    ON t.id = inbnd.id;
END
GO

ALTER PROCEDURE dbo.sp_cw_exp_truck_update_entry_status
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @filing_numbers VARCHAR(MAX);

  SELECT
    @filing_numbers = COALESCE(@filing_numbers + ', ', '') + '''' + filing_header.filing_number + ''''
  FROM dbo.exp_truck_filing_header AS filing_header  
  WHERE filing_header.filing_number IS NOT NULL
  AND COALESCE(filing_header.last_modified_date, filing_header.created_date) > DATEADD(MINUTE, -90, GETDATE())
  
  IF @filing_numbers IS NOT NULL
  BEGIN
    DECLARE @query VARCHAR(MAX);
    SET @query = 'SELECT entry_header.ch_status AS code, jdec.je_declarationreference AS filing_number
  FROM OdysseyCBRNYC.dbo.jobdeclaration AS jdec
  JOIN OdysseyCBRNYC.dbo.CusEntryHeader AS entry_header 
    ON entry_header.CH_JE = jdec.JE_PK
      AND entry_header.CH_Messagetype IN (''ENS'', ''ITN'')      
      AND entry_header.ch_status <> ''''
  WHERE jdec.je_declarationreference IN (' + @filing_numbers + ')';

    DECLARE @entry_statuses TABLE (
      code VARCHAR(3)
     ,filing_number VARCHAR(35)
    );
    INSERT INTO @entry_statuses
    EXECUTE (@query) AT CargoWiseServer;

    UPDATE header
    SET header.entry_status = entry_status.code
    FROM dbo.exp_truck_filing_header AS header
    JOIN @entry_statuses AS entry_status
      ON header.filing_number = entry_status.filing_number;
  END
END
GO