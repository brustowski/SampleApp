-- drop sequence 
IF EXISTS (SELECT
      *
    FROM sys.sequences AS seq
    JOIN sys.schemas AS schm
      ON seq.schema_id = schm.schema_id
        AND schm.name = N'inbond'
    WHERE seq.name = N'seq_master_bill_number'
    AND seq.type = 'SO')
  DROP SEQUENCE inbond.seq_master_bill_number;
GO

-- drop timer job 
DECLARE @dbn VARCHAR(30)
SET @dbn = DB_NAME()
-- job name
DECLARE @jobName VARCHAR(30) = 'j_csfp_inbond_reset_bill_num';

EXEC ('
USE msdb

DECLARE @job_Id BINARY(16) = null

SELECT @job_Id = job_id FROM msdb.dbo.sysjobs where name like ''' + @jobName + ''';
if @job_id is not null
EXEC msdb.dbo.sp_delete_job @job_id=@job_Id, @delete_unused_schedule=1
')
GO

-- recreate fucntion 
IF OBJECT_ID('inbond.fn_inbond_number', 'FN') IS NOT NULL
BEGIN
  DROP FUNCTION inbond.fn_inbond_number;
END
GO
CREATE FUNCTION inbond.fn_inbond_number ()
RETURNS VARCHAR(10)
AS
BEGIN
  DECLARE @current VARCHAR(10);
  DECLARE @currentNum INT;
  DECLARE @next VARCHAR(10);
  DECLARE @nextNum INT = 1;

  SELECT
    @current = movement_header.in_bond_number
  FROM inbond.filing_header AS filing_header
  JOIN inbond.movement_header AS movement_header
    ON filing_header.id = movement_header.filing_header_id
  WHERE filing_header.mapping_status IS NOT NULL
  AND filing_header.mapping_status <> 0
  ORDER BY movement_header.created_date DESC
  OFFSET 0 ROWS FETCH NEXT 1 ROW ONLY;

  IF @current IS NOT NULL
  BEGIN
    DECLARE @part VARCHAR(2) = RIGHT(@current, 2);
    SET @currentNum = CAST(@part AS INT);
    SET @nextNum = @currentNum + 1;
    IF @nextNum > 99
      RETURN NULL;
  END;

  SET @next = FORMAT(GETDATE(), N'MMddyyyy') + FORMAT(@nextNum, N'D2');
  RETURN @next;
END
GO

-- add bill record --
ALTER PROCEDURE inbond.sp_add_bill (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'inbond.bill'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @masterBill VARCHAR(10);
  SET @masterBill = inbond.fn_inbond_number();
  IF @masterBill IS NULL
    THROW 60000, 'Unable to generate new Master Bill number because sequence is exceeded', 1;

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
      FROM inbond.bill AS bill
      WHERE bill.filing_header_id = @filingHeaderId)
  BEGIN

    DECLARE @addrId TABLE (
      id INT NULL
    );

    DECLARE @clientAddressId UNIQUEIDENTIFIER = (SELECT TOP(1)
        rule_entry.consignee_address_id
      FROM inbond.filing_detail AS detail
      INNER JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN inbond.rule_entry AS rule_entry
        ON rule_entry.firms_code_id = inbnd.firms_code_id
          AND rule_entry.importer_id = inbnd.importer_id
          AND rule_entry.carrier = inbnd.carrier
          AND rule_entry.consignee_id = inbnd.consignee_id
          AND rule_entry.us_port_of_destination = inbnd.port_of_destination
      WHERE detail.Filing_Headers_FK = @filingHeaderId);

    IF @clientAddressId IS NOT NULL
    BEGIN

      INSERT INTO dbo.app_addresses (
          cw_address_id
         ,is_overriden)
      OUTPUT INSERTED.id INTO @addrId
      VALUES (
        @clientAddressId
       ,0);
    END

    INSERT INTO inbond.bill (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,manifest_qty
       ,manifest_qty_unit
       ,weight
       ,shipper
       ,consignee
       ,consignee_address_id
       ,master_bill)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,inbnd.manifest_qty
       ,inbnd.manifest_qty_unit
       ,inbnd.weight
       ,shipper.ClientCode
       ,consignee.ClientCode
       ,(SELECT TOP (1)
            id
          FROM @addrId)
       ,@masterBill
      FROM inbond.filing_detail AS detail
      INNER JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN inbond.rule_entry AS rule_entry
        ON rule_entry.firms_code_id = inbnd.firms_code_id
          AND rule_entry.importer_id = inbnd.importer_id
          AND rule_entry.carrier = inbnd.carrier
          AND rule_entry.consignee_id = inbnd.consignee_id
          AND rule_entry.us_port_of_destination = inbnd.port_of_destination
      LEFT JOIN dbo.Clients AS shipper
        ON shipper.id = rule_entry.shipper_id

      WHERE detail.Filing_Headers_FK = @filingHeaderId;
  END
END;
GO