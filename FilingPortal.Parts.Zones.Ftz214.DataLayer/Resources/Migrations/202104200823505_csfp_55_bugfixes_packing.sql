਍ⴀⴀ 愀搀搀 瀀愀挀欀椀渀最 爀攀挀漀爀搀 ⴀⴀഀഀ
ALTER   PROCEDURE [zones_ftz214].[sp_add_packing] (@filingHeaderId INT,਍䀀瀀愀爀攀渀琀䤀搀 䤀一吀Ⰰഀഀ
@filingUser NVARCHAR(255) = NULL,਍䀀漀瀀攀爀愀琀椀漀渀䤀搀 唀一䤀儀唀䔀䤀䐀䔀一吀䤀䘀䤀䔀刀 㴀 一唀䰀䰀 伀唀吀倀唀吀⤀ഀഀ
AS਍䈀䔀䜀䤀一ഀഀ
  SET NOCOUNT ON;਍ഀഀ
  DECLARE @tableName VARCHAR(128) = 'packing';਍  䐀䔀䌀䰀䄀刀䔀 䀀愀氀氀漀眀䴀甀氀琀椀瀀氀攀 䈀䤀吀 㴀 　㬀ഀഀ
਍  匀䔀吀 䀀漀瀀攀爀愀琀椀漀渀䤀搀 㴀 䌀伀䄀䰀䔀匀䌀䔀⠀䀀漀瀀攀爀愀琀椀漀渀䤀搀Ⰰ 一䔀圀䤀䐀⠀⤀⤀㬀ഀഀ
਍  ⴀⴀ 最攀琀 猀攀挀琀椀漀渀 瀀爀漀瀀攀爀琀礀 椀猀开愀爀爀愀礀ഀഀ
  SELECT਍    䀀愀氀氀漀眀䴀甀氀琀椀瀀氀攀 㴀 猀攀挀琀椀漀渀⸀椀猀开愀爀爀愀礀ഀഀ
  FROM zones_ftz214.form_section_configuration section਍  圀䠀䔀刀䔀 猀攀挀琀椀漀渀⸀琀愀戀氀攀开渀愀洀攀 㴀 䀀琀愀戀氀攀一愀洀攀㬀ഀഀ
਍  ⴀⴀ 愀搀搀 瀀愀挀欀椀渀最 琀愀戀 搀愀琀愀 愀渀搀 愀瀀瀀氀礀 爀甀氀攀猀ഀഀ
  IF @allowMultiple = 1਍    伀刀 一伀吀 䔀堀䤀匀吀匀 ⠀匀䔀䰀䔀䌀吀ഀഀ
        1਍      䘀刀伀䴀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀瀀愀挀欀椀渀最 瀀愀挀欀椀渀最ഀഀ
      WHERE packing.filing_header_id = @parentId)਍  䈀䔀䜀䤀一ഀഀ
    INSERT INTO zones_ftz214.packing (਍ऀऀ昀椀氀椀渀最开栀攀愀搀攀爀开椀搀ഀഀ
		,parent_record_id਍ऀऀⰀ漀瀀攀爀愀琀椀漀渀开椀搀ഀഀ
		,manifest_qty਍ऀऀⰀ戀椀氀氀开渀甀洀戀攀爀ഀഀ
		,it_number਍ऀऀⰀ昀漀爀攀椀最渀开瀀漀爀琀ഀഀ
		,export_country਍ऀऀⰀ昀椀爀洀猀ഀഀ
		,created_date਍ऀऀⰀ挀爀攀愀琀攀搀开甀猀攀爀ഀഀ
		,bill_issuer_scac਍ऀऀⰀ洀愀渀椀昀攀猀琀开甀焀ഀഀ
		,bta_indicator)਍      匀䔀䰀䔀䌀吀 ഀഀ
		 @filingHeaderId਍ऀऀⰀ䀀瀀愀爀攀渀琀䤀搀ഀഀ
		,@operationId਍ऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀焀琀礀ഀഀ
		,parsed_data.master਍ऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀椀琀开渀漀ഀഀ
		,parsed_data.foreign_port਍ऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀挀攀ഀഀ
		,parsed_data.firms਍ऀऀⰀ䜀䔀吀䐀䄀吀䔀⠀⤀ഀഀ
		,@filingUser਍ऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀椀洀瀀开挀愀爀爀椀攀爀开挀漀搀攀ഀഀ
		,IIF(parsed_data.qtyuom = 'LBK', 'VL', parsed_data.qtyuom)਍ऀऀⰀ✀一✀ഀഀ
      FROM zones_ftz214.filing_detail detail਍      ഀഀ
	  JOIN zones_ftz214.inbound_parsed_data parsed_data਍      伀一 瀀愀爀猀攀搀开搀愀琀愀⸀椀搀 㴀 搀攀琀愀椀氀⸀椀渀戀漀甀渀搀开椀搀ഀഀ
      ਍ऀ  圀䠀䔀刀䔀 搀攀琀愀椀氀⸀昀椀氀椀渀最开栀攀愀搀攀爀开椀搀 㴀 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
  END;਍䔀一䐀㬀ഀഀ
