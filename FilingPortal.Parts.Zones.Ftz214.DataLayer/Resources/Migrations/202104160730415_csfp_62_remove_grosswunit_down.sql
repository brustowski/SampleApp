-- add invoice line record --਍䄀䰀吀䔀刀   倀刀伀䌀䔀䐀唀刀䔀 嬀稀漀渀攀猀开昀琀稀㈀㄀㐀崀⸀嬀猀瀀开愀搀搀开椀渀瘀漀椀挀攀开氀椀渀攀崀 ⠀䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀 䤀一吀Ⰰഀഀ
@parentId INT,਍䀀昀椀氀椀渀最唀猀攀爀 一嘀䄀刀䌀䠀䄀刀⠀㈀㔀㔀⤀ 㴀 一唀䰀䰀Ⰰഀഀ
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)਍䄀匀ഀഀ
BEGIN਍  匀䔀吀 一伀䌀伀唀一吀 伀一㬀ഀഀ
਍  䐀䔀䌀䰀䄀刀䔀 䀀琀愀戀氀攀一愀洀攀 嘀䄀刀䌀䠀䄀刀⠀㄀㈀㠀⤀ 㴀 ✀稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀瘀漀椀挀攀开氀椀渀攀✀㬀ഀഀ
  DECLARE @allowMultiple BIT = 0;਍ഀഀ
  SET @operationId = COALESCE(@operationId, NEWID());਍ഀഀ
  -- get section property is_array਍  匀䔀䰀䔀䌀吀ഀഀ
    @allowMultiple = section.is_array਍  䘀刀伀䴀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀昀漀爀洀开猀攀挀琀椀漀渀开挀漀渀昀椀最甀爀愀琀椀漀渀 猀攀挀琀椀漀渀ഀഀ
  WHERE section.table_name = @tableName਍ഀഀ
  -- add invoice line data and apply rules਍  䤀䘀 䀀愀氀氀漀眀䴀甀氀琀椀瀀氀攀 㴀 ㄀ഀഀ
    OR NOT EXISTS (SELECT਍        ㄀ഀഀ
      FROM zones_ftz214.invoice_line invoice_line਍      圀䠀䔀刀䔀 椀渀瘀漀椀挀攀开氀椀渀攀⸀瀀愀爀攀渀琀开爀攀挀漀爀搀开椀搀 㴀 䀀瀀愀爀攀渀琀䤀搀⤀ഀഀ
  BEGIN਍    䤀一匀䔀刀吀 䤀一吀伀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀瘀漀椀挀攀开氀椀渀攀 ⠀ഀഀ
		 filing_header_id਍ऀऀⰀ瀀愀爀攀渀琀开爀攀挀漀爀搀开椀搀ഀഀ
		,operation_id਍ऀऀⰀ挀爀攀愀琀攀搀开搀愀琀攀ഀഀ
		,created_user਍ऀऀⰀ椀渀瘀漀椀挀攀开渀漀ഀഀ
		,zone_status਍ऀऀⰀ漀爀椀最椀渀ഀഀ
		,export਍ऀऀⰀ琀愀爀椀昀昀ഀഀ
		,goods_description਍ऀऀⰀ挀甀猀琀漀洀猀开焀琀礀ഀഀ
		,invoice_qty਍ऀऀⰀ氀椀渀攀开瀀爀椀挀攀ഀഀ
		,manufacturer਍ऀऀⰀ最爀漀猀猀开眀攀椀最栀琀ഀഀ
		,gross_weight_unit਍ऀऀⰀ挀栀愀爀最攀猀ഀഀ
		,loading_port)਍      匀䔀䰀䔀䌀吀ഀഀ
		 @filingHeaderId਍ऀऀⰀ䀀瀀愀爀攀渀琀䤀搀ഀഀ
		,@operationId਍        Ⰰ䜀䔀吀䐀䄀吀䔀⠀⤀ഀഀ
       ,@filingUser਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀椀琀攀洀开渀漀ഀഀ
	   ,parsed_line.zone_status਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀挀漀ഀഀ
	   ,parsed_data.ce਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀栀琀猀ഀഀ
	   ,parsed_line.description਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀焀琀礀㄀ഀഀ
	   ,parsed_line.qty1਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀瘀愀氀甀攀ഀഀ
	   ,parsed_line.mid਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀最爀漀猀猀开眀最琀ഀഀ
	   ,parsed_line.gross_lbs਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀挀栀愀爀最攀猀ഀഀ
	   ,parsed_data.foreign_port਍ऀ 䘀刀伀䴀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀昀椀氀椀渀最开搀攀琀愀椀氀 搀攀琀愀椀氀ഀഀ
      JOIN zones_ftz214.inbound inbnd਍        伀一 椀渀戀渀搀⸀椀搀 㴀 搀攀琀愀椀氀⸀椀渀戀漀甀渀搀开椀搀ഀഀ
	  LEFT JOIN zones_ftz214.inbound_parsed_data parsed_data਍ऀऀ伀一 椀渀戀渀搀⸀椀搀 㴀 瀀愀爀猀攀搀开搀愀琀愀⸀椀搀ഀഀ
਍ऀ  䰀䔀䘀吀 䨀伀䤀一 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀戀漀甀渀搀开瀀愀爀猀攀搀开氀椀渀攀 瀀愀爀猀攀搀开氀椀渀攀ഀഀ
		ON inbnd.id = parsed_line.id਍ഀഀ
      LEFT JOIN dbo.Clients AS clients਍ऀऀ伀一 椀渀戀渀搀⸀愀瀀瀀氀椀挀愀渀琀开椀搀 㴀 挀氀椀攀渀琀猀⸀椀搀ഀഀ
	WHERE detail.filing_header_id = @filingHeaderId਍  䔀一䐀㬀ഀഀ
END;਍�