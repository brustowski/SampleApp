IF NOT EXISTS (SELECT
      1
    FROM sys.columns
    WHERE Name = N'status'
    AND Object_ID = OBJECT_ID(N'isf.documents'))
BEGIN
  ALTER TABLE isf.documents
  ADD status VARCHAR(128) NULL
END
GO

IF NOT EXISTS (SELECT
      1
    FROM sys.columns
    WHERE Name = N'error_description'
    AND Object_ID = OBJECT_ID(N'isf.filing_header'))
BEGIN
  ALTER TABLE isf.filing_header
  ADD error_description VARCHAR(MAX) NULL
END
GO

IF NOT EXISTS (SELECT
      1
    FROM sys.columns
    WHERE Name = N'response_xml'
    AND Object_ID = OBJECT_ID(N'isf.filing_header'))
BEGIN
  ALTER TABLE isf.filing_header
  ADD response_xml VARCHAR(MAX) NULL
END
GO

IF NOT EXISTS (SELECT
      1
    FROM sys.columns
    WHERE Name = N'request_xml'
    AND Object_ID = OBJECT_ID(N'isf.filing_header'))
BEGIN
  ALTER TABLE isf.filing_header
  ADD request_xml VARCHAR(MAX) NULL
END
GO

DECLARE @command VARCHAR(MAX);

SELECT
  @command = OBJECT_DEFINITION(OBJECT_ID('dbo.v_documents'));

IF @command NOT LIKE '% isf.documents %'
BEGIN
  SET @command = REPLACE(@command, ';', '');
  SET @command = REPLACE(@command, 'create', 'ALTER') + char(10) + 'UNION ALL
SELECT
  isf_header.id AS filing_header_id
 ,isf_doc.id AS doc_id
 ,isf_doc.file_name AS filename
 ,isf_doc.extension file_extension
 ,isf_doc.content AS file_Content
 ,isf_doc.description AS file_desc
 ,isf_doc.document_type AS document_type
 ,''ISF'' AS transport_shipment_type
FROM isf.documents AS isf_doc
JOIN isf.filing_header AS isf_header
  ON isf_doc.filing_header_id = isf_header.id';

  EXEC (@command)
END
GO