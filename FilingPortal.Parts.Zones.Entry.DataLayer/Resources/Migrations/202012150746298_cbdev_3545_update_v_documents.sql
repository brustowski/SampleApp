DECLARE @command VARCHAR(MAX);

SELECT
  @command = OBJECT_DEFINITION(OBJECT_ID('dbo.v_documents'));

IF @command NOT LIKE '% zones_entry.document %'
BEGIN
  SET @command = REPLACE(@command, ';', '');
  SET @command = REPLACE(@command, 'create', 'ALTER') + char(10) + 'UNION ALL
SELECT
  zones_entry_header.id AS filing_header_id
 ,zones_entry_doc.id AS doc_id
 ,zones_entry_doc.file_name AS filename
 ,zones_entry_doc.extension file_extension
 ,zones_entry_doc.content AS file_Content
 ,zones_entry_doc.description AS file_desc
 ,zones_entry_doc.document_type AS document_type
 ,''Entry06'' AS transport_shipment_type
FROM zones_entry.document AS zones_entry_doc
JOIN zones_entry.filing_header AS zones_entry_header
  ON zones_entry_doc.filing_header_id = zones_entry_header.id';

  EXEC (@command)
END
GO