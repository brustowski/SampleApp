

-- add declaration record --
ALTER   PROCEDURE [zones_ftz214].[sp_add_declaration] (
	@filingHeaderId INT,
	@parentId INT,
	@filingUser NVARCHAR(255) = NULL,
	@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @tableName VARCHAR(128) = 'declaration'
	DECLARE @allowMultiple BIT = 0;

	SET @operationId = COALESCE(@operationId, NEWID());

	-- get section property is_array
	SELECT @allowMultiple = section.is_array
	FROM zones_ftz214.form_section_configuration section
	WHERE section.table_name = @tableName

	-- add declaration data
	IF @allowMultiple = 1 OR NOT EXISTS (
		SELECT 1
		FROM zones_ftz214.declaration declaration
		WHERE declaration.filing_header_id = @filingHeaderId
	)
	BEGIN
		INSERT INTO zones_ftz214.declaration (
			 filing_header_id
			,parent_record_id
			,operation_id
			,importer
			,transport
			,container
			,admission_type
			,direct_delivery
			,issuer
			,ocean_bill
			,vessel
			,voyage
			,loading_port
			,discharge_port
			,ftz_port
			,dep
			,arr
			,arr2
			,hmf
			,first_arr_date
			,description
			,firms_code
			,zone_id
			,year
			,applicant
			,ftz_operator
			,created_date
			,created_user
		)
		SELECT
			 @filingHeaderId
			,@parentId
			,@operationId
			,clients.ClientCode
			,transportMode.code
			,transportMode.container_code
			,inbnd.admission_type
			,direct_delivery
			,parsed_data.imp_carrier_code
			,parsed_data.master
			,parsed_data.imp_vessel
			,parsed_data.flt_voy_trip --voyage
			,parsed_data.foreign_port
			,parsed_data.unlading_port
			,parsed_data.zone_port
			,parsed_data.export_date
			,parsed_data.import_date
			,parsed_data.import_date
			,parsed_line.hmf
			,parsed_data.est_arr_date
			,parsed_line.description
			,parsed_data.ptt_firms
			,inbnd.zone_id
			,parsed_data.admission_year
			,parsed_data.applicant_irs_no
			,parsed_data.submitter_irs_no
			,GETDATE()
			,@filingUser
		FROM zones_ftz214.filing_detail AS detail

		JOIN zones_ftz214.inbound AS inbnd
		ON inbnd.id = detail.inbound_id
		
		LEFT JOIN zones_ftz214.inbound_parsed_data parsed_data
		ON inbnd.id = parsed_data.id

		LEFT JOIN zones_ftz214.inbound_parsed_line parsed_line
		ON inbnd.id = parsed_line.id

		LEFT JOIN dbo.Clients AS clients
		ON inbnd.applicant_id = clients.id

		JOIN handbook_transport_mode AS transportMode
		ON transportMode.code_number = parsed_data.mot


		WHERE detail.filing_header_id = @filingHeaderId
	END
END;
GO



