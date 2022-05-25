-- gets invoice total
CREATE OR ALTER FUNCTION dbo.fn_imp_rail_invoice_total (@invoiceHeaderId INT)
RETURNS NUMERIC(18, 6) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result NUMERIC(18, 6) = NULL;

  SELECT
    @result = SUM(invoice_line.customs_qty * invoice_line.price_unit)
  FROM dbo.imp_rail_invoice_line invoice_line
  WHERE invoice_line.parent_record_id = @invoiceHeaderId
  GROUP BY invoice_line.parent_record_id

  RETURN @result
END;
GO

--
-- Drop default constraint on table [dbo].[imp_rail_invoice_header]
--
ALTER TABLE dbo.imp_rail_invoice_header
  DROP CONSTRAINT IF EXISTS DF_invoice_total_default
GO

--
-- Drop column [invoice_total] from table [dbo].[imp_rail_invoice_header]
--
ALTER TABLE [dbo].[imp_rail_invoice_header]
  DROP COLUMN IF EXISTS [invoice_total]
GO

--
-- Create column [invoice_total] on table [dbo].[imp_rail_invoice_header]
--
ALTER TABLE [dbo].[imp_rail_invoice_header]
  ADD [invoice_total] AS ([dbo].[fn_imp_rail_invoice_total]([id]))
GO

--
-- Refresh view [dbo].[v_imp_rail_report]
--
EXEC sp_refreshview '[dbo].[v_imp_rail_report]'
GO

--
-- Drop type [dbo].[field_update_list]
--
DROP TYPE IF EXISTS [dbo].[field_update_list]
GO

--
-- Alter procedure [dbo].[sp_imp_rail_recalculate]
--
GO
-- recalculate rail fileds
ALTER PROCEDURE [dbo].[sp_imp_rail_recalculate] (@json VARCHAR(MAX))
AS
RETURN
GO