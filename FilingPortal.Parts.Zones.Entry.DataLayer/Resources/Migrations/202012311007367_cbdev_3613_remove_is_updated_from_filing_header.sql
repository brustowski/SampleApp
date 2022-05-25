--
-- Drop column [mapping_status] from table [zones_entry].[filing_header]
--
ALTER TABLE zones_entry.filing_header
  DROP COLUMN IF EXISTS mapping_status
GO

--
-- Drop column [filing_status] from table [zones_entry].[filing_header]
--
ALTER TABLE zones_entry.filing_header
  DROP COLUMN IF EXISTS filing_status
GO