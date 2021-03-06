਍ⴀⴀ 愀搀搀 椀渀瘀漀椀挀攀 氀椀渀攀 爀攀挀漀爀搀 ⴀⴀഀഀ
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
		,line_price਍ऀऀⰀ洀愀渀甀昀愀挀琀甀爀攀爀ഀഀ
		,gross_weight਍ऀऀⰀ最爀漀猀猀开眀攀椀最栀琀开甀渀椀琀ഀഀ
		,charges਍ऀऀⰀ氀漀愀搀椀渀最开瀀漀爀琀⤀ഀഀ
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
	 FROM zones_ftz214.filing_detail detail਍      䨀伀䤀一 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀戀漀甀渀搀 椀渀戀渀搀ഀഀ
        ON inbnd.id = detail.inbound_id਍ऀ  䰀䔀䘀吀 䨀伀䤀一 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀戀漀甀渀搀开瀀愀爀猀攀搀开搀愀琀愀 瀀愀爀猀攀搀开搀愀琀愀ഀഀ
		ON inbnd.id = parsed_data.id਍ഀഀ
	  LEFT JOIN zones_ftz214.inbound_parsed_line parsed_line਍ऀऀ伀一 椀渀戀渀搀⸀椀搀 㴀 瀀愀爀猀攀搀开氀椀渀攀⸀椀搀ഀഀ
਍      䰀䔀䘀吀 䨀伀䤀一 搀戀漀⸀䌀氀椀攀渀琀猀 䄀匀 挀氀椀攀渀琀猀ഀഀ
		ON inbnd.applicant_id = clients.id਍ऀ圀䠀䔀刀䔀 搀攀琀愀椀氀⸀昀椀氀椀渀最开栀攀愀搀攀爀开椀搀 㴀 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
  END;਍䔀一䐀㬀ഀഀ
