UPDATE doc
  SET file_desc = dt.Description
FROM dbo.imp_rail_document AS doc
  JOIN dbo.DocumentTypes AS dt
  ON doc.document_type = dt.TypeName
WHERE doc.file_desc IS NULL;
GO
UPDATE doc
SET file_description = dt.Description
FROM dbo.imp_pipeline_document AS doc
JOIN dbo.DocumentTypes AS dt
  ON doc.document_type = dt.TypeName
WHERE doc.file_description IS NULL;
GO
