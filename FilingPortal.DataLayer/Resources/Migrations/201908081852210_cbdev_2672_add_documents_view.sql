--create documents view 
IF OBJECT_ID(N'dbo.v_Documents', 'V') IS NOT NULL
  DROP VIEW dbo.v_Documents
GO
CREATE VIEW [dbo].[v_Documents]
 AS


	select 
		rail_header.id AS  filing_header_id
		,rail_doc.id as doc_id 
		,rail_doc.filename AS filename
		,rail_doc.file_extension AS file_extension
		,rail_doc.file_content AS file_Content
		,rail_doc.file_desc AS file_desc 
		,rail_doc.DocumentType as document_type
		,'Rail_Imp' as transport_shipment_type
    from Rail_Documents rail_doc
		inner join Rail_Filing_Headers rail_header
		on rail_doc.Filing_Headers_FK=rail_header.id
	union all				
	select 
		truck_header.id AS  filing_header_id
		,truck_doc.id as doc_id
		,truck_doc.filename AS filename
		,truck_doc.file_extension AS file_extension
		,truck_doc.file_content AS file_Content
		,truck_doc.file_desc AS file_desc
		,truck_doc.document_type as document_type
		,'Truck_Imp' as transport_shipment_type 
	from Truck_Documents truck_doc
		inner join truck_Filing_Headers truck_header
	on truck_doc.Filing_Headers_FK=truck_header.id

	union all
	select 
		pipeline_header.id AS  filing_header_id
		,pipeline_doc.id as doc_id
		,pipeline_doc.filename AS filename
		,pipeline_doc.file_extension AS file_extension
		,pipeline_doc.file_content AS file_Content
		,pipeline_doc.file_desc AS file_desc
		,pipeline_doc.DocumentType as document_type
		,'Pipeline_Imp' as transport_shipment_type 
	from pipeline_Documents pipeline_doc
		inner join pipeline_Filing_Headers pipeline_header
		on pipeline_doc.Filing_Headers_FK=pipeline_header.id
		union all
	select 
		vessel_header.id AS  filing_header_id
		,vessel_doc.id as doc_id
		,vessel_doc.file_name AS filename
		,vessel_doc.extension AS file_extension
		,vessel_doc.content AS file_Content
		,vessel_doc.description AS file_desc
		,vessel_doc.document_type as document_type
		,'Vessel_Imp' as transport_shipment_type 
	from [dbo].[Vessel_Import_Documents] vessel_doc
		inner join [dbo].[Vessel_Import_Filing_Headers] vessel_header
		on vessel_doc.[filing_header_id]=vessel_header.id
	union all
	select 
		truck_export_header.id AS  filing_header_id	
		,truck_export_doc.id as doc_id
		,truck_export_doc.file_name AS filename
		,truck_export_doc.extension file_extension
		,truck_export_doc.content AS file_Content
		,truck_export_doc.description AS file_desc
		,truck_export_doc.document_type as document_type
		,'Truck_Export' as transport_shipment_type 
	from truck_export_Documents truck_export_doc
		inner join truck_export_Filing_Headers truck_export_header
		on truck_export_doc.filing_header_id=truck_export_header.id
		union all
	select 
		vessel_export_header.id AS  filing_header_id	
		,vessel_export_doc.id as doc_id
		,vessel_export_doc.file_name AS filename
		,vessel_export_doc.extension file_extension
		,vessel_export_doc.content AS file_Content
		,vessel_export_doc.description AS file_desc
		,vessel_export_doc.document_type as document_type
		,'Vessel_Export' as transport_shipment_type 
	from [dbo].[Vessel_Export_Documents] vessel_export_doc
		inner join dbo.vessel_export_Filing_Headers vessel_export_header
		on vessel_export_doc.filing_header_id=vessel_export_header.id

GO
