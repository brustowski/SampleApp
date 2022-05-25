--- Remove Vessel Import Report View ---
IF OBJECT_ID('dbo.Vessel_Import_Report', 'V') IS NOT NULL
  DROP VIEW dbo.Vessel_Import_Report
GO

--- Remove Vessel Import Declaration table ---
IF EXISTS (SELECT
      *
    FROM sys.triggers
    WHERE object_id = OBJECT_ID(N'dbo.vessel_import_declarations_befor_delete'))
  DROP TRIGGER dbo.vessel_import_declarations_befor_delete
GO

IF OBJECT_ID('dbo.Vessel_Import_Declarations', 'U') IS NOT NULL
  DROP TABLE dbo.Vessel_Import_Declarations
GO

--- Remove Vessel Import Packings table ---
IF EXISTS (SELECT
      *
    FROM sys.triggers
    WHERE object_id = OBJECT_ID(N'dbo.vessel_import_packings_befor_delete'))
  DROP TRIGGER dbo.vessel_import_packings_befor_delete
GO

IF OBJECT_ID('dbo.Vessel_Import_Packings', 'U') IS NOT NULL
  DROP TABLE dbo.Vessel_Import_Packings
GO

--- Remove Vessel Import Miscs table ---
IF EXISTS (SELECT
      *
    FROM sys.triggers
    WHERE object_id = OBJECT_ID(N'dbo.vessel_import_miscs_befor_delete'))
  DROP TRIGGER dbo.vessel_import_miscs_befor_delete
GO

IF OBJECT_ID('dbo.Vessel_Import_Miscs', 'U') IS NOT NULL
  DROP TABLE dbo.Vessel_Import_Miscs
GO

--- Remove Vessel Import Invoice Lines table ---
IF EXISTS (SELECT
      *
    FROM sys.triggers
    WHERE object_id = OBJECT_ID(N'dbo.vessel_import_invoice_lines_befor_delete'))
  DROP TRIGGER dbo.vessel_import_invoice_lines_befor_delete
GO

IF OBJECT_ID('dbo.Vessel_Import_Invoice_Lines', 'U') IS NOT NULL
  DROP TABLE dbo.Vessel_Import_Invoice_Lines
GO

IF OBJECT_ID('dbo.vessel_import_invoice_line_number', 'FN') IS NOT NULL
  DROP FUNCTION dbo.vessel_import_invoice_line_number
GO

--- Remove Vessel Import Invoice Headers table ---
IF EXISTS (SELECT
      *
    FROM sys.triggers
    WHERE object_id = OBJECT_ID(N'dbo.vessel_import_invoice_headers_befor_delete'))
  DROP TRIGGER dbo.vessel_import_invoice_headers_befor_delete
GO

IF OBJECT_ID('dbo.Vessel_Import_Invoice_Headers', 'U') IS NOT NULL
  DROP TABLE dbo.Vessel_Import_Invoice_Headers
GO