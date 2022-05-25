DECLARE @command VARCHAR(MAX);

SELECT
  @command = OBJECT_DEFINITION(OBJECT_ID('dbo.v_documents'));

IF @command NOT LIKE '% inbond.documents %'
BEGIN
  SET @command = REPLACE(@command, ';', '');
  SET @command = REPLACE(@command, 'create', 'ALTER') + char(10) + 'UNION ALL
SELECT
  inbond_header.id AS filing_header_id
 ,inbond_doc.id AS doc_id
 ,inbond_doc.file_name AS filename
 ,inbond_doc.extension file_extension
 ,inbond_doc.content AS file_Content
 ,inbond_doc.description AS file_desc
 ,inbond_doc.document_type AS document_type
 ,''InBond'' AS transport_shipment_type
FROM inbond.documents AS inbond_doc
JOIN inbond.filing_header AS inbond_header
  ON inbond_doc.filing_header_id = inbond_header.id';

  EXEC (@command)
END
GO