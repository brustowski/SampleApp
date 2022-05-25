DECLARE @command VARCHAR(MAX);

SELECT
  @command = OBJECT_DEFINITION(OBJECT_ID('dbo.v_documents'));

IF @command NOT LIKE '% canada_imp_truck.documents %'
BEGIN
  SET @command = REPLACE(@command, ';', '');
  SET @command = REPLACE(@command, 'create', 'ALTER') + char(10) + 'UNION ALL
SELECT
  canada_imp_truck_header.id AS filing_header_id
 ,canada_imp_truck_doc.id AS doc_id
 ,canada_imp_truck_doc.file_name AS filename
 ,canada_imp_truck_doc.extension file_extension
 ,canada_imp_truck_doc.content AS file_Content
 ,canada_imp_truck_doc.description AS file_desc
 ,canada_imp_truck_doc.document_type AS document_type
 ,''CA_Truck_Imp'' AS transport_shipment_type
FROM canada_imp_truck.documents AS canada_imp_truck_doc
JOIN canada_imp_truck.filing_header AS canada_imp_truck_header
  ON canada_imp_truck_doc.filing_header_id = canada_imp_truck_header.id';

  EXEC (@command)
END
GO