IF OBJECT_ID(N'app_create_handbook', 'P') IS NOT NULL
  DROP PROCEDURE app_create_handbook
GO

CREATE PROCEDURE app_create_handbook (@name VARCHAR(40))
AS
BEGIN
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  SET NOCOUNT ON;
  DECLARE @sSQL NVARCHAR(500);

  SET @sSQL = N'CREATE TABLE ' + QUOTENAME('Handbook_' + @name) + ' (id INT PRIMARY KEY IDENTITY, value VARCHAR(255) NOT NULL)'

  BEGIN TRY
	EXEC sp_executesql @sSQL
  END TRY
  BEGIN CATCH
  END CATCH
END
GO

IF OBJECT_ID(N'app_get_handbook', 'P') IS NOT NULL
  DROP PROCEDURE app_get_handbook
GO

CREATE PROCEDURE app_get_handbook(@name VARCHAR(40))
AS 
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    DECLARE @sSQL nvarchar(500);

    SELECT @sSQL = N'SELECT value FROM' + QUOTENAME('Handbook_' + @name);

    EXEC sp_executesql @sSQL
END
GO

IF OBJECT_ID(N'v_Handbooks', 'V') IS NOT NULL
  DROP VIEW v_Handbooks
GO

CREATE VIEW v_Handbooks
AS
SELECT DISTINCT
  SUBSTRING(table_name, 10, LEN(i.TABLE_NAME) - 9) AS TableName
FROM information_schema.columns i
WHERE i.table_schema = 'dbo'
AND i.table_name LIKE 'Handbook_%'
GO

IF OBJECT_ID(N'Handbook_Currency', 'U') IS NULL
  EXECUTE dbo.app_create_handbook 'Currency'
GO
IF OBJECT_ID(N'Handbook_Units', 'U') IS NULL
  EXECUTE dbo.app_create_handbook 'Units'
GO
