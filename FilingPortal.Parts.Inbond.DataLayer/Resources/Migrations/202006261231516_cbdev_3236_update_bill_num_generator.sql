-- create sequence
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
CREATE SEQUENCE inbond.seq_master_bill_number
AS INT
START WITH 1
INCREMENT BY 1
MINVALUE 1
MAXVALUE 99
NO CACHE;
GO

-- create job
DECLARE @dbn VARCHAR(30)
SET @dbn = DB_NAME()
-- job name
DECLARE @jobName VARCHAR(30) = 'j_csfp_inbond_reset_bill_num';
-- job description
DECLARE @jobDescription VARCHAR(100) = 'Reset In-Bond bill number sequence generator';
-- command to execute
DECLARE @command VARCHAR(100) = 'ALTER SEQUENCE inbond.seq_master_bill_number RESTART WITH 1;';
-- generate unique guid5
DECLARE @scheduleUid UNIQUEIDENTIFIER = '1B1F514F-FF03-48C8-891E-ED4FCB919DDD';

EXEC ('
USE msdb

DECLARE @job_Id BINARY(16) = null

SELECT @job_Id = job_id FROM msdb.dbo.sysjobs where name like ''' + @jobName + ''';
if @job_id is not null
EXEC msdb.dbo.sp_delete_job @job_id=@job_Id, @delete_unused_schedule=1

BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=''[Uncategorized (Local)]'' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=''JOB'', @type=''LOCAL'', @name=''[Uncategorized (Local)]''
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=''' + @jobName + ''',
		@enabled=1,
		@notify_level_eventlog=0,
		@notify_level_email=0,
		@notify_level_netsend=0,
		@notify_level_page=0,
		@delete_level=0,
		@description=''' + @jobDescription + ''',
		@category_name=''[Uncategorized (Local)]'',
		@job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [execute]    Script Date: 10.12.2018 18:39:22 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=''execute'',
		@step_id=1,
		@cmdexec_success_code=0,
		@on_success_action=1,
		@on_success_step_id=0,
		@on_fail_action=2,
		@on_fail_step_id=0,
		@retry_attempts=0,
		@retry_interval=0,
		@os_run_priority=0, @subsystem=''TSQL'',
		@command=''' + @command + ''',
		@database_name=''' + @dbn + ''',
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=''' + @jobName + ''',
		@enabled=1,
		@freq_type=4,
		@freq_interval=1,
		@freq_subday_type=1,
		@freq_subday_interval=0,
		@freq_relative_interval=0,
		@freq_recurrence_factor=0,
		@active_start_date=20181210,
		@active_end_date=99991231,
		@active_start_time=0,
		@active_end_time=235959,
		@schedule_uid=''' + @scheduleUid + '''
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = ''(local)''
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
')
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
  BEGIN TRY
    SET @masterBill = FORMAT(GETDATE(), N'MMddyyyy') + FORMAT(NEXT VALUE FOR inbond.seq_master_bill_number, N'D2');
  END TRY
  BEGIN CATCH
    THROW 60000, 'Unable to generate new Master Bill number because sequence is exceeded', 1;
  END CATCH;
  
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

-- drop function
IF OBJECT_ID('inbond.fn_inbond_number', 'FN') IS NOT NULL
BEGIN
  DROP FUNCTION inbond.fn_inbond_number;
END
GO