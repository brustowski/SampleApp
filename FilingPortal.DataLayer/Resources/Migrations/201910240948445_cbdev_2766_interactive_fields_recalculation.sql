--
-- Create column [invoice_total] on table [dbo].[imp_rail_invoice_header]
--
ALTER TABLE [dbo].[imp_rail_invoice_header]
  ADD [invoice_total_n] [numeric](18, 6) NULL
GO

--
-- Create default constraint
--
ALTER TABLE [dbo].[imp_rail_invoice_header]
	ADD CONSTRAINT DF_invoice_total_default
	DEFAULT (0) FOR [invoice_total_n]
GO

--
-- Update data in a new column
--
UPDATE imp_rail_invoice_header
  SET [invoice_total_n] = [invoice_total]
GO
--
-- Drop column [invoice_total] from table [dbo].[imp_rail_invoice_header]
--
ALTER TABLE [dbo].[imp_rail_invoice_header]
  DROP COLUMN IF EXISTS [invoice_total]
GO

--
-- Drop fn_imp_rail_invoice_total as it is not used anymore
--
DROP FUNCTION IF EXISTS dbo.fn_imp_rail_invoice_total
GO

--
-- Rename [invoice_total_n] => [invoice_total]
--
EXEC sys.sp_rename @objname = N'imp_rail_invoice_header.invoice_total_n'
                  ,@newname = 'invoice_total'
                  ,@objtype = 'COLUMN'


--
-- Refresh view [dbo].[v_imp_rail_report]
--
EXEC sp_refreshview '[dbo].[v_imp_rail_report]'
GO

--
-- Create type [dbo].[field_update_list]
--
CREATE TYPE [dbo].[field_update_list] AS TABLE (
  [id] [int] NOT NULL,
  [record_id] [int] NOT NULL,
  [parent_record_id] [int] NOT NULL,
  [value] [varchar](512) NULL
)
GO

--
-- Alter procedure [dbo].[sp_imp_rail_recalculate]
--
GO
-- recalculate rail fileds
ALTER PROCEDURE [dbo].[sp_imp_rail_recalculate] (@json VARCHAR(MAX))
AS

  SET NOCOUNT ON;

  DECLARE @InboundList field_update_list;
  INSERT INTO @InboundList (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,record_id
     ,parent_record_id
     ,value
    FROM OPENJSON(@json)
    WITH (id INT
    ,record_id INT
    ,parent_record_id INT
    ,value VARCHAR(512));

  DECLARE @config TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,value VARCHAR(512)
   ,column_name VARCHAR(100)
   ,table_name VARCHAR(100)
  )
  INSERT INTO @config (id, record_id, parent_record_id, value, column_name, table_name)
    SELECT
      inbound.id
     ,inbound.record_id
     ,inbound.parent_record_id
     ,inbound.value
     ,irfc.column_name
     ,irfsc.table_name
    FROM @InboundList inbound
    LEFT JOIN imp_rail_form_configuration irfc
      ON inbound.Id = irfc.id
    INNER JOIN imp_rail_form_section_configuration irfsc
      ON irfc.section_id = irfsc.id



  DECLARE @tbl field_update_list;

  WITH invoice_total_results
  AS
  (SELECT
      config1.parent_record_id
     ,SUM(CAST(NULLIF(config1.value, '') AS NUMERIC(18, 6)) * CAST(NULLIF(config2.value, '') AS NUMERIC(18, 6))) AS value
    FROM @config config1
    JOIN @config config2
      ON config1.record_id = config2.record_id
    WHERE config1.table_name = 'imp_rail_invoice_line'
    AND config1.column_name = 'customs_qty'
    AND config2.column_name = 'price_unit'
    GROUP BY config1.parent_record_id)

  INSERT INTO @tbl (id, record_id, parent_record_id, value)
    SELECT
      id
     ,record_id
     ,[@config].parent_record_id
     ,invoice_total_results.value
    FROM @config
    INNER JOIN invoice_total_results
      ON [@config].record_id = invoice_total_results.parent_record_id
    WHERE column_name = 'invoice_total'

  SELECT
    *
  FROM @tbl
  FOR JSON AUTO;

  RETURN 0;
GO