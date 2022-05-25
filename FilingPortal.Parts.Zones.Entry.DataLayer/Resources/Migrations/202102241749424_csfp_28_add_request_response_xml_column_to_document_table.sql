IF NOT EXISTS (SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[zones_entry].[document]') 
         AND name = 'request_xml')
BEGIN
    ALTER TABLE [zones_entry].[document] add request_xml xml null 
	END 
GO

IF NOT EXISTS (SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[zones_entry].[document]') 
         AND name = 'response_xml')
BEGIN
    ALTER TABLE [zones_entry].[document] add response_xml xml null 
	END 
GO