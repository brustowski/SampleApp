--- drop Filing Header summary columns ---
IF EXISTS (SELECT
      1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Rail_Filing_Headers'
    AND COLUMN_NAME = 'GrossWeightSummaryUnit'
    AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE dbo.Rail_Filing_Headers
  DROP COLUMN GrossWeightSummaryUnit
END
GO

IF EXISTS (SELECT
      1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Rail_Filing_Headers'
    AND COLUMN_NAME = 'GrossWeightSummary'
    AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE dbo.Rail_Filing_Headers
  DROP COLUMN GrossWeightSummary
END
GO

--- drop functions ---
IF OBJECT_ID(N'dbo.rail_gross_weight_summary', 'FN') IS NOT NULL
  DROP FUNCTION dbo.rail_gross_weight_summary
GO

IF OBJECT_ID(N'dbo.rail_gross_weight_summary_unit', 'FN') IS NOT NULL
  DROP FUNCTION dbo.rail_gross_weight_summary_unit
GO

--- Rail Filing Data View ---
ALTER VIEW dbo.v_Rail_Filing_Data
AS
SELECT
  f.BDP_FK AS id
 ,h.id AS Filing_header_id
 ,p.BDP_EM AS Manifest_id
 ,d.Importer AS Importer
 ,d.Entry_Port AS Port_code
 ,d.Master_Bill AS Bill_of_lading
 ,c.Container_Number AS Container_number
 ,p.ReferenceNumber1 AS Train_number
 ,ISNULL(c.Gross_Weight, 0) AS Gross_weight
 ,ISNULL(c.Gross_Weight_Unit, 0) AS Gross_weight_unit

FROM dbo.Rail_Filing_Headers h
INNER JOIN dbo.Rail_Filing_Details f
  ON h.id = f.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed p
  ON f.BDP_FK = p.BDP_PK
LEFT OUTER JOIN dbo.Rail_DeclarationTab d
  ON d.Filing_Headers_FK = h.id
LEFT OUTER JOIN dbo.Rail_ContainersTab c
  ON c.Filing_Headers_FK = h.id
    AND f.BDP_FK = c.BDP_FK
GO