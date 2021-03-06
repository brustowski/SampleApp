਍ഀഀ
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
		,manufacturer_address਍ऀऀⰀ最爀漀猀猀开眀攀椀最栀琀ഀഀ
		,gross_weight_unit਍ऀऀⰀ挀栀愀爀最攀猀ഀഀ
		,loading_port਍ऀऀⰀ椀渀瘀漀椀挀攀开焀琀礀开甀焀⤀ഀഀ
      SELECT਍ऀऀ 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
		,@parentId਍ऀऀⰀ䀀漀瀀攀爀愀琀椀漀渀䤀搀ഀഀ
        ,GETDATE()਍       Ⰰ䀀昀椀氀椀渀最唀猀攀爀ഀഀ
	   ,parsed_line.item_no਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀稀漀渀攀开猀琀愀琀甀猀ഀഀ
	   ,parsed_line.co਍ऀ   Ⰰ瀀愀爀猀攀搀开搀愀琀愀⸀挀攀ഀഀ
	   ,parsed_line.hts਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀搀攀猀挀爀椀瀀琀椀漀渀ഀഀ
	   ,parsed_line.qty1਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀焀琀礀㄀ഀഀ
	   ,parsed_line.value਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀洀椀搀ഀഀ
	   ,parsed_line.gross_wgt਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀最爀漀猀猀开氀戀猀ഀഀ
	   ,parsed_line.charges਍ऀ   Ⰰ瀀愀爀猀攀搀开搀愀琀愀⸀昀漀爀攀椀最渀开瀀漀爀琀ഀഀ
	   ,dbo.fn_app_unit_by_tariff(parsed_line.hts, 'HTS')਍ऀ 䘀刀伀䴀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀昀椀氀椀渀最开搀攀琀愀椀氀 搀攀琀愀椀氀ഀഀ
      JOIN zones_ftz214.inbound inbnd਍        伀一 椀渀戀渀搀⸀椀搀 㴀 搀攀琀愀椀氀⸀椀渀戀漀甀渀搀开椀搀ഀഀ
	  LEFT JOIN zones_ftz214.inbound_parsed_data parsed_data਍ऀऀ伀一 椀渀戀渀搀⸀椀搀 㴀 瀀愀爀猀攀搀开搀愀琀愀⸀椀搀ഀഀ
਍ऀ  䰀䔀䘀吀 䨀伀䤀一 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀戀漀甀渀搀开瀀愀爀猀攀搀开氀椀渀攀 瀀愀爀猀攀搀开氀椀渀攀ഀഀ
		ON inbnd.id = parsed_line.id਍ഀഀ
      LEFT JOIN dbo.Clients AS clients਍ऀऀ伀一 椀渀戀渀搀⸀愀瀀瀀氀椀挀愀渀琀开椀搀 㴀 挀氀椀攀渀琀猀⸀椀搀ഀഀ
	WHERE detail.filing_header_id = @filingHeaderId਍  䔀一䐀㬀ഀഀ
END;਍ഀഀ
GO਍ⴀⴀ 愀搀搀 椀渀瘀漀椀挀攀 氀椀渀攀 爀攀挀漀爀搀 ⴀⴀഀഀ
ALTER   PROCEDURE [zones_ftz214].[sp_add_invoice_line] (@filingHeaderId INT,਍䀀瀀愀爀攀渀琀䤀搀 䤀一吀Ⰰഀഀ
@filingUser NVARCHAR(255) = NULL,਍䀀漀瀀攀爀愀琀椀漀渀䤀搀 唀一䤀儀唀䔀䤀䐀䔀一吀䤀䘀䤀䔀刀 㴀 一唀䰀䰀 伀唀吀倀唀吀⤀ഀഀ
AS਍䈀䔀䜀䤀一ഀഀ
  SET NOCOUNT ON;਍ഀഀ
  DECLARE @tableName VARCHAR(128) = 'zones_ftz214.invoice_line';਍  䐀䔀䌀䰀䄀刀䔀 䀀愀氀氀漀眀䴀甀氀琀椀瀀氀攀 䈀䤀吀 㴀 　㬀ഀഀ
਍  匀䔀吀 䀀漀瀀攀爀愀琀椀漀渀䤀搀 㴀 䌀伀䄀䰀䔀匀䌀䔀⠀䀀漀瀀攀爀愀琀椀漀渀䤀搀Ⰰ 一䔀圀䤀䐀⠀⤀⤀㬀ഀഀ
਍  ⴀⴀ 最攀琀 猀攀挀琀椀漀渀 瀀爀漀瀀攀爀琀礀 椀猀开愀爀爀愀礀ഀഀ
  SELECT਍    䀀愀氀氀漀眀䴀甀氀琀椀瀀氀攀 㴀 猀攀挀琀椀漀渀⸀椀猀开愀爀爀愀礀ഀഀ
  FROM zones_ftz214.form_section_configuration section਍  圀䠀䔀刀䔀 猀攀挀琀椀漀渀⸀琀愀戀氀攀开渀愀洀攀 㴀 䀀琀愀戀氀攀一愀洀攀ഀഀ
਍  ⴀⴀ 愀搀搀 椀渀瘀漀椀挀攀 氀椀渀攀 搀愀琀愀 愀渀搀 愀瀀瀀氀礀 爀甀氀攀猀ഀഀ
  IF @allowMultiple = 1਍    伀刀 一伀吀 䔀堀䤀匀吀匀 ⠀匀䔀䰀䔀䌀吀ഀഀ
        1਍      䘀刀伀䴀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀瘀漀椀挀攀开氀椀渀攀 椀渀瘀漀椀挀攀开氀椀渀攀ഀഀ
      WHERE invoice_line.parent_record_id = @parentId)਍  䈀䔀䜀䤀一ഀഀ
    INSERT INTO zones_ftz214.invoice_line (਍ऀऀ 昀椀氀椀渀最开栀攀愀搀攀爀开椀搀ഀഀ
		,parent_record_id਍ऀऀⰀ漀瀀攀爀愀琀椀漀渀开椀搀ഀഀ
		,created_date਍ऀऀⰀ挀爀攀愀琀攀搀开甀猀攀爀ഀഀ
		,invoice_no਍ऀऀⰀ稀漀渀攀开猀琀愀琀甀猀ഀഀ
		,origin਍ऀऀⰀ攀砀瀀漀爀琀ഀഀ
		,tariff਍ऀऀⰀ最漀漀搀猀开搀攀猀挀爀椀瀀琀椀漀渀ഀഀ
		,customs_qty਍ऀऀⰀ椀渀瘀漀椀挀攀开焀琀礀ഀഀ
		,line_price਍ऀऀⰀ洀愀渀甀昀愀挀琀甀爀攀爀开愀搀搀爀攀猀猀ഀഀ
		,gross_weight਍ऀऀⰀ最爀漀猀猀开眀攀椀最栀琀开甀渀椀琀ഀഀ
		,charges਍ऀऀⰀ氀漀愀搀椀渀最开瀀漀爀琀ഀഀ
		,invoice_qty_uq਍ऀऀⰀ挀漀渀猀椀最渀攀攀⤀ഀഀ
      SELECT਍ऀऀ 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
		,@parentId਍ऀऀⰀ䀀漀瀀攀爀愀琀椀漀渀䤀搀ഀഀ
        ,GETDATE()਍       Ⰰ䀀昀椀氀椀渀最唀猀攀爀ഀഀ
	   ,parsed_line.item_no਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀稀漀渀攀开猀琀愀琀甀猀ഀഀ
	   ,parsed_line.co਍ऀ   Ⰰ瀀愀爀猀攀搀开搀愀琀愀⸀挀攀ഀഀ
	   ,parsed_line.hts਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀搀攀猀挀爀椀瀀琀椀漀渀 ⴀⴀ 最漀漀搀猀开搀攀猀挀爀椀瀀琀椀漀渀ഀഀ
	   ,parsed_line.qty1਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀焀琀礀㄀ഀഀ
	   ,parsed_line.value਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀洀椀搀ഀഀ
	   ,parsed_line.gross_wgt਍ऀ   Ⰰ瀀愀爀猀攀搀开氀椀渀攀⸀最爀漀猀猀开氀戀猀ഀഀ
	   ,parsed_line.charges਍ऀ   Ⰰ瀀愀爀猀攀搀开搀愀琀愀⸀昀漀爀攀椀最渀开瀀漀爀琀ഀഀ
	   ,dbo.fn_app_unit_by_tariff(parsed_line.hts, 'HTS')਍ऀ   Ⰰ挀氀椀攀渀琀猀⸀䌀氀椀攀渀琀䌀漀搀攀ഀഀ
	 FROM zones_ftz214.filing_detail detail਍      䨀伀䤀一 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀戀漀甀渀搀 椀渀戀渀搀ഀഀ
        ON inbnd.id = detail.inbound_id਍ऀ  䰀䔀䘀吀 䨀伀䤀一 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀戀漀甀渀搀开瀀愀爀猀攀搀开搀愀琀愀 瀀愀爀猀攀搀开搀愀琀愀ഀഀ
		ON inbnd.id = parsed_data.id਍ഀഀ
	  LEFT JOIN zones_ftz214.inbound_parsed_line parsed_line਍ऀऀ伀一 椀渀戀渀搀⸀椀搀 㴀 瀀愀爀猀攀搀开氀椀渀攀⸀椀搀ഀഀ
਍      䰀䔀䘀吀 䨀伀䤀一 搀戀漀⸀䌀氀椀攀渀琀猀 䄀匀 挀氀椀攀渀琀猀ഀഀ
		ON inbnd.applicant_id = clients.id਍ऀ圀䠀䔀刀䔀 搀攀琀愀椀氀⸀昀椀氀椀渀最开栀攀愀搀攀爀开椀搀 㴀 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
  END;਍䔀一䐀㬀ഀഀ
