਍ⴀⴀ 愀搀搀 琀爀甀挀欀 椀洀瀀漀爀琀 椀渀瘀漀椀挀攀 栀攀愀搀攀爀 爀攀挀漀爀搀 ⴀⴀഀഀ
ALTER   PROCEDURE [zones_ftz214].[sp_add_invoice_header] (@filingHeaderId INT,਍䀀瀀愀爀攀渀琀䤀搀 䤀一吀Ⰰഀഀ
@filingUser NVARCHAR(255) = NULL,਍䀀漀瀀攀爀愀琀椀漀渀䤀搀 唀一䤀儀唀䔀䤀䐀䔀一吀䤀䘀䤀䔀刀 㴀 一唀䰀䰀 伀唀吀倀唀吀⤀ഀഀ
AS਍䈀䔀䜀䤀一ഀഀ
  SET NOCOUNT ON;਍ഀഀ
  DECLARE @tableName VARCHAR(128) = 'invoice_header';਍  䐀䔀䌀䰀䄀刀䔀 䀀愀氀氀漀眀䴀甀氀琀椀瀀氀攀 䈀䤀吀 㴀 　㬀ഀഀ
  DECLARE @IDs TABLE (਍    䤀䐀 䤀一吀ഀഀ
  );਍ഀഀ
  SET @operationId = COALESCE(@operationId, NEWID());਍ഀഀ
  -- get section property is_array਍  匀䔀䰀䔀䌀吀ഀഀ
    @allowMultiple = section.is_array਍  䘀刀伀䴀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀昀漀爀洀开猀攀挀琀椀漀渀开挀漀渀昀椀最甀爀愀琀椀漀渀 猀攀挀琀椀漀渀ഀഀ
  WHERE section.table_name = @tableName਍ഀഀ
  -- add invoice header data and apply rules਍  䤀䘀 䀀愀氀氀漀眀䴀甀氀琀椀瀀氀攀 㴀 ㄀ഀഀ
    OR NOT EXISTS (SELECT਍        ㄀ഀഀ
      FROM zones_ftz214.invoice_header invoice_header਍      圀䠀䔀刀䔀 椀渀瘀漀椀挀攀开栀攀愀搀攀爀⸀昀椀氀椀渀最开栀攀愀搀攀爀开椀搀 㴀 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀⤀ഀഀ
  BEGIN਍    䤀一匀䔀刀吀 䤀一吀伀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀瘀漀椀挀攀开栀攀愀搀攀爀 ⠀ഀഀ
	 filing_header_id਍    Ⰰ瀀愀爀攀渀琀开爀攀挀漀爀搀开椀搀ഀഀ
    ,operation_id਍    Ⰰ挀爀攀愀琀攀搀开搀愀琀攀ഀഀ
    ,created_user਍ऀⰀ椀洀瀀漀爀琀攀爀ഀഀ
	,consignee਍ऀⰀ猀栀椀瀀开琀漀开瀀愀爀琀礀ഀഀ
	,invoice_no)਍    伀唀吀倀唀吀 䤀一匀䔀刀吀䔀䐀⸀䤀䐀 䤀一吀伀 䀀䤀䐀猀 ⠀䤀䐀⤀ഀഀ
      SELECT DISTINCT਍        䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
       ,@parentId਍       Ⰰ䀀漀瀀攀爀愀琀椀漀渀䤀搀ഀഀ
       ,GETDATE()਍       Ⰰ䀀昀椀氀椀渀最唀猀攀爀ഀഀ
	   ,Clients.ClientCode਍ऀ   Ⰰ䌀氀椀攀渀琀猀⸀䌀氀椀攀渀琀䌀漀搀攀ഀഀ
	   ,Clients.ClientCode਍ऀ   Ⰰ瀀愀爀猀攀搀开搀愀琀愀⸀氀椀渀攀开渀漀ഀഀ
      FROM zones_ftz214.filing_detail detail਍      䨀伀䤀一 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀戀漀甀渀搀 椀渀戀渀搀ഀഀ
        ON inbnd.id = detail.inbound_id਍ऀ  䰀䔀䘀吀 䨀伀䤀一 搀戀漀⸀䌀氀椀攀渀琀猀 䄀匀 挀氀椀攀渀琀猀ഀഀ
		ON inbnd.applicant_id = clients.id਍ऀ  䰀䔀䘀吀 䨀伀䤀一 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀戀漀甀渀搀开瀀愀爀猀攀搀开搀愀琀愀 瀀愀爀猀攀搀开搀愀琀愀ഀഀ
		ON inbnd.id = parsed_data.id਍      圀䠀䔀刀䔀 搀攀琀愀椀氀⸀昀椀氀椀渀最开栀攀愀搀攀爀开椀搀 㴀 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
਍    䐀䔀䌀䰀䄀刀䔀 䀀爀攀挀漀爀搀䤀搀 䤀一吀ഀഀ
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT਍      䤀䐀ഀഀ
    FROM @IDs਍ഀഀ
    OPEN cur਍ഀഀ
    FETCH NEXT FROM cur INTO @recordId਍    圀䠀䤀䰀䔀 䀀䀀䘀䔀吀䌀䠀开匀吀䄀吀唀匀 㴀 　ഀഀ
    BEGIN਍ഀഀ
    -- add invoice line਍    䔀堀䔀䌀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀猀瀀开愀搀搀开椀渀瘀漀椀挀攀开氀椀渀攀 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
                                        ,@recordId਍                                        Ⰰ䀀昀椀氀椀渀最唀猀攀爀ഀഀ
                                        ,@operationId਍ഀഀ
    FETCH NEXT FROM cur INTO @recordId਍ഀഀ
    END਍ഀഀ
    CLOSE cur਍    䐀䔀䄀䰀䰀伀䌀䄀吀䔀 挀甀爀ഀഀ
਍  䔀一䐀㬀ഀഀ
END;਍�