CREATE FUNCTION dbo.fn_tmpFixLeadingZeros (@data AS VARCHAR(MAX), @length AS INT)
RETURNS VARCHAR(MAX)
BEGIN 
  DECLARE @result VARCHAR(MAX)
  DECLARE @lenDiff INT
  SET @result = RTRIM(LTRIM(@data))
  SET @result = SUBSTRING(@result, PATINDEX('%[^0]%',@result), LEN(@result))
  SET @lenDiff =  @length -DATALENGTH(@result)
  IF @lenDiff>0 BEGIN  
	  SET @result = REPLICATE('0', @lenDiff) + @result
  END
  RETURN @result
END
GO

UPDATE dbo.Rail_BD_Parsed
  SET EquipmentNumber = dbo.fn_tmpFixLeadingZeros(EquipmentNumber,6)
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.fn_tmpFixLeadingZeros') AND type IN ('IF', 'FN', 'TF'))
  DROP FUNCTION dbo.fn_tmpFixLeadingZeros
GO