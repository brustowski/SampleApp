ALTER PROCEDURE dbo.sp_validate_daily_audit
AS
BEGIN
  DECLARE @validationResult VARCHAR(MAX) = NULL;
  DECLARE @tbl TABLE (
    fields VARCHAR(500)
   ,message VARCHAR(500)
   ,overrideId VARCHAR(128)
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
         ,@containersCount INT
         ,@grossWeightUnit VARCHAR(2)
         ,@customsQtyUnit VARCHAR(3)
         ,@grossWeight DECIMAL(18, 6)
         ,@customsQty DECIMAL(18, 6)

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
   ,audit.containers_count
   ,audit.gross_weight_uq
   ,audit.customs_qty_unit
   ,audit.gross_weight
   ,audit.customs_qty
  FROM dbo.imp_rail_audit_daily audit

  OPEN cur

  FETCH NEXT FROM cur INTO @recordId, @jobHeaderStatus, @importDate, @exportDate, @releaseDate, @psd, @paymentDue, @endStatusDescription, @supplierMid, @manufacturerMid, @containersCount, @grossWeightUnit, @customsQtyUnit, @grossWeight, @customsQty

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

  IF (@containersCount BETWEEN 1 AND 12
    AND NOT @grossWeightUnit = 'KG')
  BEGIN
    DECLARE @postfix VARCHAR(1);
    SET @postfix = 's';
    IF (@containersCount = 1)
      SET @postfix = '';

    INSERT INTO @tbl (fields, message, overrideId)
      VALUES ('containers_count, gross_weight_uq', CONCAT('For ', @containersCount, ' container', @postfix, ' Gross Weight Uq should be KG'), 'GrossWeightValidation');
  END

  IF (@containersCount > 12
    AND NOT @grossWeightUnit = 'T')
    INSERT INTO @tbl (fields, message, overrideId)
      VALUES ('containers_count, gross_weight_uq', CONCAT('For ', @containersCount, ' containers Gross Weight Uq should be T'), 'GrossWeightValidation');


  IF (@grossWeightUnit = @customsQtyUnit
    AND NOT (@grossWeight = @customsQty))
  BEGIN
    INSERT INTO @tbl (fields, message)
      VALUES ('gross_weight, customs_qty', CONCAT('Customs Qty should be equal to Gross Weight (', @grossWeight, ' ', @grossWeightUnit, ')'));
  END

  ----------------------------------------------------------

  SET @validationResult = (SELECT
      fields
     ,message
     ,overrideId
    FROM @tbl
    FOR JSON PATH, INCLUDE_NULL_VALUES);

  UPDATE imp_rail_audit_daily
  SET validation_result = @validationResult
  WHERE id = @recordId

  FETCH NEXT FROM cur INTO @recordId, @jobHeaderStatus, @importDate, @exportDate, @releaseDate, @psd, @paymentDue, @endStatusDescription, @supplierMid, @manufacturerMid, @containersCount, @grossWeightUnit, @customsQtyUnit, @grossWeight, @customsQty

  END

  CLOSE cur
  DEALLOCATE cur


END
GO


DROP FUNCTION IF EXISTS dbo.fn_convert_weight
GO

DROP FUNCTION IF EXISTS dbo.fn_extract_api_from_customs_attribute
GO