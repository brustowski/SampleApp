﻿-- add main detail record --
ALTER PROCEDURE inbond.sp_add_main_detail (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'inbond.main_detail'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM inbond.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM inbond.main_detail AS main_detail
      WHERE main_detail.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO inbond.main_detail (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,importer
       ,firms_code
       ,transport_mode
       ,carrier_code
       ,conveyance
       ,importing_carrier_port_of_arrival
       ,voyage_trip_no
       ,branch
       ,authorized_agent
       ,entry_date
       ,eta
       ,port_of_presentation)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,importer.ClientCode
       ,firms_code.firms_code
       ,transport_mode.code
       ,inbnd.carrier
       ,inbnd.export_conveyance
       ,inbnd.port_of_arrival
       ,IIF(rule_entry.transport_mode = '10', NULL, 1)
       ,SUBSTRING(user_data.Branch, 1, 5)
       ,user_data.Broker
       ,inbnd.created_date
       ,GETDATE()
       ,rule_entry.port_of_presentation
      FROM inbond.filing_detail AS detail
      JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN dbo.cw_firms_codes AS firms_code
        ON inbnd.firms_code_id = firms_code.id
      LEFT JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN inbond.rule_entry AS rule_entry
        ON rule_entry.firms_code_id = inbnd.firms_code_id
          AND rule_entry.importer_id = inbnd.importer_id
          AND rule_entry.carrier = inbnd.carrier
          AND rule_entry.consignee_id = inbnd.consignee_id
          AND rule_entry.us_port_of_destination = inbnd.port_of_destination
      LEFT JOIN dbo.handbook_transport_mode AS transport_mode
        ON rule_entry.transport_mode = transport_mode.code_number
      LEFT JOIN dbo.app_users_data AS user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.Filing_Headers_FK = @filingHeaderId
  END
END;
GO

DECLARE @command VARCHAR(MAX);
DECLARE @tableName VARCHAR(128) = 'inbond.main_detail';
DECLARE @columnName VARCHAR(128) = 'voyage_trip_no';
SELECT @command = 'ALTER TABLE ' + @tableName + ' DROP CONSTRAINT ' + d.name + ';' + CHAR(10)
FROM sys.columns AS c
JOIN sys.default_constraints AS d ON d.parent_object_id = c.object_id AND d.parent_column_id = c.column_id
WHERE c.name = @columnName AND c.object_id = OBJECT_ID(@tableName, 'U');

EXEC (@command);
GO