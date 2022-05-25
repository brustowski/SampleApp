IF OBJECT_ID('dbo.handbook_units', 'U') IS NOT NULL
  DROP TABLE dbo.handbook_units;
GO

CREATE TABLE dbo.handbook_units (
  id INT IDENTITY
 ,value VARCHAR(3) NOT NULL
 ,display_value VARCHAR(3) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY];

INSERT INTO dbo.handbook_units (
    value
   ,display_value)
  SELECT DISTINCT
    lm.Value
   ,lm.DisplayValue
  FROM dbo.LookupMaster AS lm
  WHERE lm.Type IN ('Customs_UQ', 'Invoice_UQ', 'GrossWeight_UQ')
  AND lm.Value <> '';
GO

DELETE FROM dbo.LookupMaster
WHERE Type IN ('Customs_UQ', 'Invoice_UQ', 'GrossWeight_UQ');
GO