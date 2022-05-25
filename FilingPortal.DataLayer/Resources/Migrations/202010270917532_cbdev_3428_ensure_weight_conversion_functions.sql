--
-- Create function [dbo].[fn_extract_api_from_customs_attribute]
--
GO
CREATE OR ALTER FUNCTION dbo.fn_extract_api_from_customs_attribute (@custAttr VARCHAR(50))
RETURNS DECIMAL(16, 2)
AS
BEGIN
  IF (@custAttr IS NULL) RETURN NULL
  
  DECLARE @chrIndex INT = CHARINDEX('=', @custAttr)

  IF (@chrIndex = 0) RETURN NULL

  DECLARE @strApi VARCHAR(50) = SUBSTRING(@custAttr, @chrIndex + 1, LEN(@custAttr))

  DECLARE @apiPath DECIMAL(16, 2) = CAST(@strApi AS DECIMAL(16, 2))

  RETURN @apiPath
END

GO

--
-- Create function [dbo].[fn_convert_weight]
--
GO
CREATE OR ALTER FUNCTION dbo.fn_convert_weight (@value DECIMAL(18, 9), @inbndUnit VARCHAR(2), @returnUnit VARCHAR(2))
RETURNS DECIMAL(18,9) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @inbndRate DECIMAL(18, 9) = NULL;

  SET @inbndRate = dbo.fn_app_weight_to_ton(@value, @inbndUnit)

  DECLARE @returnRate DECIMAL(18, 9) = dbo.fn_app_weight_to_ton(1, @returnUnit);

  
  RETURN @inbndRate / @returnRate
END
GO