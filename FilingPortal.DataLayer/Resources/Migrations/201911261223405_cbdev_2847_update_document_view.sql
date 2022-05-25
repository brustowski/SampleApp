ALTER VIEW dbo.v_Documents
AS
SELECT
  rail_header.id AS filing_header_id
 ,rail_doc.id AS doc_id
 ,rail_doc.filename AS filename
 ,rail_doc.file_extension AS file_extension
 ,rail_doc.file_content AS file_Content
 ,rail_doc.file_desc AS file_desc
 ,rail_doc.document_type AS document_type
 ,'Rail_Imp' AS transport_shipment_type
FROM dbo.imp_rail_document rail_doc
JOIN dbo.imp_rail_filing_header rail_header
  ON rail_doc.filing_header_id = rail_header.id
UNION ALL
SELECT
  truck_header.id AS filing_header_id
 ,truck_doc.id AS doc_id
 ,truck_doc.[file_name] AS filename
 ,truck_doc.file_extension AS file_extension
 ,truck_doc.file_content AS file_Content
 ,truck_doc.file_description AS file_desc
 ,truck_doc.document_type AS document_type
 ,'Truck_Imp' AS transport_shipment_type
FROM dbo.imp_truck_document truck_doc
JOIN dbo.imp_truck_filing_header truck_header
  ON truck_doc.filing_header_id = truck_header.id
UNION ALL
SELECT
  pipeline_header.id AS filing_header_id
 ,pipeline_doc.id AS doc_id
 ,pipeline_doc.[file_name] AS filename
 ,pipeline_doc.file_extension AS file_extension
 ,pipeline_doc.file_content AS file_Content
 ,pipeline_doc.file_description AS file_desc
 ,pipeline_doc.document_type AS document_type
 ,'Pipeline_Imp' AS transport_shipment_type
FROM dbo.imp_pipeline_document pipeline_doc
JOIN dbo.imp_pipeline_filing_header pipeline_header
  ON pipeline_doc.filing_header_id = pipeline_header.id
UNION ALL
SELECT
  vessel_header.id AS filing_header_id
 ,vessel_doc.id AS doc_id
 ,vessel_doc.[file_name] AS filename
 ,vessel_doc.file_extension AS file_extension
 ,vessel_doc.file_content AS file_Content
 ,vessel_doc.file_description AS file_desc
 ,vessel_doc.document_type AS document_type
 ,'Vessel_Imp' AS transport_shipment_type
FROM dbo.imp_vessel_document vessel_doc
JOIN dbo.imp_vessel_filing_header AS vessel_header
  ON vessel_doc.filing_header_id = vessel_header.id
UNION ALL
SELECT
  truck_export_header.id AS filing_header_id
 ,truck_export_doc.id AS doc_id
 ,truck_export_doc.[file_name] AS [filename]
 ,truck_export_doc.file_extension file_extension
 ,truck_export_doc.file_content AS file_Content
 ,truck_export_doc.file_description AS file_desc
 ,truck_export_doc.document_type AS document_type
 ,'Truck_Export' AS transport_shipment_type
FROM dbo.exp_truck_document truck_export_doc
JOIN dbo.exp_truck_filing_header truck_export_header
  ON truck_export_doc.filing_header_id = truck_export_header.id
UNION ALL
SELECT
  vessel_export_header.id AS filing_header_id
 ,vessel_export_doc.id AS doc_id
 ,vessel_export_doc.file_name AS filename
 ,vessel_export_doc.file_extension file_extension
 ,vessel_export_doc.file_content AS file_Content
 ,vessel_export_doc.file_description AS file_desc
 ,vessel_export_doc.document_type AS document_type
 ,'Vessel_Export' AS transport_shipment_type
FROM dbo.exp_vessel_document AS vessel_export_doc
JOIN dbo.exp_vessel_filing_header AS vessel_export_header
  ON vessel_export_doc.filing_header_id = vessel_export_header.id
GO