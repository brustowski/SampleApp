--- restore Rail Filing Header table ---
-- restore functions
IF OBJECT_ID(N'dbo.rail_gross_weight_summary', 'FN') IS NOT NULL
  DROP FUNCTION dbo.rail_gross_weight_summary
GO
CREATE FUNCTION dbo.rail_gross_weight_summary (@filingHeaderId INT)
RETURNS DECIMAL(18, 9) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result DECIMAL(18, 9) = NULL;

  SELECT
    @result = SUM(ril.Gr_Weight)
  FROM dbo.Rail_InvoiceLines ril
  WHERE ril.Filing_Headers_FK = @filingHeaderId

  RETURN @result
END
GO

IF OBJECT_ID(N'dbo.rail_gross_weight_summary_unit', 'FN') IS NOT NULL
  DROP FUNCTION dbo.rail_gross_weight_summary_unit
GO
CREATE FUNCTION dbo.rail_gross_weight_summary_unit (@filingHeaderId INT)
RETURNS VARCHAR(2) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result VARCHAR(2) = NULL;

  SELECT TOP (1)
    @result = ril.Gr_Weight_Unit
  FROM dbo.Rail_InvoiceLines ril
  WHERE ril.Filing_Headers_FK = @filingHeaderId

  RETURN @result
END
GO

-- restore Filing Header summary columns
ALTER TABLE dbo.Rail_Filing_Headers
ADD GrossWeightSummaryUnit AS (dbo.rail_gross_weight_summary_unit(id))
GO

ALTER TABLE dbo.Rail_Filing_Headers
ADD GrossWeightSummary AS (dbo.rail_gross_weight_summary(id))
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
 ,c.Bill_Num AS Bill_of_lading
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