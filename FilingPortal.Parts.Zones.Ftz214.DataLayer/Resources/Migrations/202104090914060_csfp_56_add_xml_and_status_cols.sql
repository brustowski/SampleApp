IF NOT EXISTS (
  SELECT
    *
  FROM
    INFORMATION_SCHEMA.COLUMNS
  WHERE
   TABLE_SCHEMA = 'zones_ftz214' AND TABLE_NAME = 'filing_header' AND COLUMN_NAME = 'request_xml')
BEGIN
ALTER TABLE [zones_ftz214].[filing_header]
            ADD [request_xml] [xml] NULL
END
IF NOT EXISTS (
  SELECT
    *
  FROM
    INFORMATION_SCHEMA.COLUMNS
  WHERE
   TABLE_SCHEMA = 'zones_ftz214' AND TABLE_NAME = 'filing_header' AND COLUMN_NAME = 'response_xml')
BEGIN
ALTER TABLE [zones_ftz214].[filing_header]
                  ADD [response_xml] [varchar](max) NULL
END
IF NOT EXISTS (
  SELECT
    *
  FROM
    INFORMATION_SCHEMA.COLUMNS
  WHERE
   TABLE_SCHEMA = 'zones_ftz214' AND TABLE_NAME = 'filing_header' AND COLUMN_NAME = 'error_description')
BEGIN
ALTER TABLE [zones_ftz214].[filing_header]
                  ADD [error_description] [varchar](max) NULL
END