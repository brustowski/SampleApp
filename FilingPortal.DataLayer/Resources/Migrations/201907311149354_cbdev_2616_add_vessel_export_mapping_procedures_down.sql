--- Drop Vessel export report view ---
IF OBJECT_ID('dbo.Vessel_Export_Report', 'V') IS NOT NULL
BEGIN
  DROP VIEW dbo.Vessel_Export_Report
END
GO

--- Drop Vessel Export mapping procedures ---
IF OBJECT_ID('dbo.vessel_export_add_declaration_record', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_export_add_declaration_record
END

IF OBJECT_ID('dbo.vessel_export_add_invoice_header_record', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_export_add_invoice_header_record
END

IF OBJECT_ID('dbo.vessel_export_add_invoice_line_record', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_export_add_invoice_line_record
END

IF OBJECT_ID('dbo.vessel_export_add_def_values_manual', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_export_add_def_values_manual
END

IF OBJECT_ID('dbo.vessel_export_apply_def_values', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_export_apply_def_values
END

IF OBJECT_ID('dbo.vessel_export_delete_record', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_export_delete_record
END

IF OBJECT_ID('dbo.vessel_export_filing', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_export_filing
END

IF OBJECT_ID('dbo.vessel_export_filing_del', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_export_filing_del
END

IF OBJECT_ID('dbo.vessel_export_filing_param', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_export_filing_param
END

IF OBJECT_ID('dbo.vessel_export_del', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_export_del
END

--- Drop Vessel Export resulting tables ---
-- Vessel Export Declaration table --
IF EXISTS (SELECT
      *
    FROM sys.triggers
    WHERE object_id = OBJECT_ID(N'dbo.vessel_export_declarations_befor_delete'))
  DROP TRIGGER dbo.vessel_export_declarations_befor_delete
GO

IF OBJECT_ID('dbo.Vessel_Export_Declarations', 'U') IS NOT NULL
  DROP TABLE dbo.Vessel_Export_Declarations
GO

-- Vessel Export Invoice Lines table --
IF EXISTS (SELECT
      *
    FROM sys.triggers
    WHERE object_id = OBJECT_ID(N'dbo.vessel_export_invoice_lines_befor_delete'))
  DROP TRIGGER dbo.vessel_export_invoice_lines_befor_delete
GO

IF OBJECT_ID('dbo.Vessel_Export_Invoice_Lines', 'U') IS NOT NULL
  DROP TABLE dbo.Vessel_Export_Invoice_Lines
GO

IF OBJECT_ID('dbo.vessel_export_invoice_line_number', 'FN') IS NOT NULL
  DROP FUNCTION dbo.vessel_export_invoice_line_number
GO

-- Vessel Export Invoice Headers table --
IF EXISTS (SELECT
      *
    FROM sys.triggers
    WHERE object_id = OBJECT_ID(N'dbo.vessel_export_invoice_headers_befor_delete'))
  DROP TRIGGER dbo.vessel_export_invoice_headers_befor_delete
GO

IF OBJECT_ID('dbo.Vessel_Export_Invoice_Headers', 'U') IS NOT NULL
  DROP TABLE dbo.Vessel_Export_Invoice_Headers
GO

IF OBJECT_ID('dbo.vessel_export_invoice_header_number', 'FN') IS NOT NULL
  DROP FUNCTION dbo.vessel_export_invoice_header_number
GO

--- Clear Vessel export configuration ---
TRUNCATE TABLE dbo.Vessel_Export_Def_Values
GO
TRUNCATE TABLE dbo.Vessel_Export_Def_Values_Manual
GO
DELETE FROM dbo.Vessel_Export_Sections
GO

--- Restore functions ---
ALTER FUNCTION dbo.fn_getUnitByTariff (@tariff VARCHAR(35), @tarifftype VARCHAR(128))
RETURNS VARCHAR(128)

AS
BEGIN
  DECLARE @result VARCHAR(128) = NULL;


  IF @tarifftype = LOWER('SHB')
  BEGIN
    SET @result = (SELECT
        Unit
      FROM SchB_Tariff
      WHERE (UB_Tariff = REPLACE(@tariff, '.', '')))
  END
  ELSE
  BEGIN
    SET @result = (SELECT TOP (1)
        Unit
      FROM Tariff
      WHERE (USC_Tariff = REPLACE(@tariff, '.', '')
      AND [ToDateTime] >= GETDATE())
      ORDER BY FromDateTime DESC)
  END



  RETURN @result
END
GO