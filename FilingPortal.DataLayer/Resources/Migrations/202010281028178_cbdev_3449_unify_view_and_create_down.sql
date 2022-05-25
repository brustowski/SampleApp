-- soft delete inbound record
ALTER PROCEDURE dbo.sp_exp_truck_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mappingStatus INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mappingStatus = grid.mapping_status
  FROM v_exp_truck_inbound_grid grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE exp_truck_inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mappingStatus = 1
      OR @mappingStatus = 0
    BEGIN
      UPDATE exp_truck_inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.inbound_id
        FROM exp_truck_filing_detail detail
        WHERE detail.filing_header_id = @filingHeaderId)
    END
  END
END
GO