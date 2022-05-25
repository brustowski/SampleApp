CREATE OR ALTER PROCEDURE dbo.sp_validate_daily_audit
AS
BEGIN
  DECLARE @validationResult VARCHAR(MAX) = NULL;
  DECLARE @tbl TABLE (
   fields VARCHAR(500)
   ,message VARCHAR(500)
  )

  DECLARE @recordId INT
         ,@jobHeaderStatus VARCHAR(3)
         ,@importDate SMALLDATETIME
         ,@exportDate SMALLDATETIME
         ,@releaseDate SMALLDATETIME
         ,@psd SMALLDATETIME
         ,@paymentDue SMALLDATETIME
         ,@endStatusDescription VARCHAR(200)
         ,@supplierMid VARCHAR(24)
         ,@manufacturerMid VARCHAR(24)

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    audit.id
   ,audit.job_header_status
   ,audit.import_date
   ,audit.export_date
   ,audit.release_date
   ,audit.psd
   ,audit.payment_due_date
   ,audit.ens_status_description
   ,audit.supplier_mid
   ,audit.manufacturer_mid
  FROM dbo.imp_rail_audit_daily audit

  OPEN cur

  FETCH NEXT FROM cur INTO @recordId, @jobHeaderStatus, @importDate, @exportDate, @releaseDate, @psd, @paymentDue, @endStatusDescription, @supplierMid, @manufacturerMid

  WHILE @@FETCH_STATUS = 0
  BEGIN

  DELETE FROM @tbl

  ----------------------------------------------------------
  -- Validation section
  ----------------------------------------------------------
  IF NOT (@jobHeaderStatus = 'INV'
    OR @jobHeaderStatus = 'JRB')
    INSERT INTO @tbl (fields, message)
      VALUES ('job_header_status', 'Job Header Status must be INV or JRB');

  IF (@importDate != @exportDate)
    INSERT INTO @tbl (fields, message)
      VALUES ('import_date, export_date', 'Import Date should be equal to Export date');

  IF (@importDate != @releaseDate)
    INSERT INTO @tbl (fields, message)
      VALUES ('import_date, release_date', 'Import Date should be equal to Release Date');

  IF (@psd > @paymentDue)
    INSERT INTO @tbl (fields, message)
      VALUES ('psd, payment_due_date', 'PSD should be less or equal to Payment Due');

  IF (@endStatusDescription != 'Clear Entry Summary Replace')
    INSERT INTO @tbl (fields, message)
      VALUES ('ens_status_description', 'ENS status description should be ''Clear Entry Summary Replace''');
  ----------------------------------------------------------

  SET @validationResult = (SELECT
      *
    FROM @tbl
    FOR JSON PATH, INCLUDE_NULL_VALUES);

  UPDATE imp_rail_audit_daily
  SET validation_result = @validationResult
  WHERE id = @recordId

  FETCH NEXT FROM cur INTO @recordId, @jobHeaderStatus, @importDate, @exportDate, @releaseDate, @psd, @paymentDue, @endStatusDescription, @supplierMid, @manufacturerMid

  END

  CLOSE cur
  DEALLOCATE cur


END
GO