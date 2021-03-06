਍ഀഀ
ALTER   PROCEDURE [zones_ftz214].[sp_add_misc] (@filingHeaderId INT,਍䀀瀀愀爀攀渀琀䤀搀 䤀一吀Ⰰഀഀ
@filingUser NVARCHAR(255) = NULL,਍䀀漀瀀攀爀愀琀椀漀渀䤀搀 唀一䤀儀唀䔀䤀䐀䔀一吀䤀䘀䤀䔀刀 㴀 一唀䰀䰀 伀唀吀倀唀吀⤀ഀഀ
AS਍䈀䔀䜀䤀一ഀഀ
  SET NOCOUNT ON;਍ഀഀ
  DECLARE @tableName VARCHAR(128) = 'misc';਍  䐀䔀䌀䰀䄀刀䔀 䀀愀氀氀漀眀䴀甀氀琀椀瀀氀攀 䈀䤀吀 㴀 　㬀ഀഀ
਍  匀䔀吀 䀀漀瀀攀爀愀琀椀漀渀䤀搀 㴀 䌀伀䄀䰀䔀匀䌀䔀⠀䀀漀瀀攀爀愀琀椀漀渀䤀搀Ⰰ 一䔀圀䤀䐀⠀⤀⤀㬀ഀഀ
਍  ⴀⴀ 最攀琀 猀攀挀琀椀漀渀 瀀爀漀瀀攀爀琀礀 椀猀开愀爀爀愀礀ഀഀ
  SELECT਍    䀀愀氀氀漀眀䴀甀氀琀椀瀀氀攀 㴀 瀀猀⸀椀猀开愀爀爀愀礀ഀഀ
  FROM zones_ftz214.form_section_configuration ps਍  圀䠀䔀刀䔀 瀀猀⸀琀愀戀氀攀开渀愀洀攀 㴀 䀀琀愀戀氀攀一愀洀攀㬀ഀഀ
਍  ⴀⴀ 愀搀搀 洀椀猀挀 搀愀琀愀 愀渀搀 愀瀀瀀氀礀 爀甀氀攀猀ഀഀ
  IF @allowMultiple = 1਍    伀刀 一伀吀 䔀堀䤀匀吀匀 ⠀匀䔀䰀䔀䌀吀ഀഀ
        1਍      䘀刀伀䴀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀洀椀猀挀 洀椀猀挀ഀഀ
      WHERE misc.filing_header_id = @filingHeaderId)਍  䈀䔀䜀䤀一ഀഀ
    INSERT INTO zones_ftz214.misc (filing_header_id਍ऀऀⰀ瀀愀爀攀渀琀开爀攀挀漀爀搀开椀搀ഀഀ
		,operation_id਍ऀऀⰀ戀爀愀渀挀栀ഀഀ
		,[broker]਍ऀऀⰀ挀爀攀愀琀攀搀开搀愀琀攀ഀഀ
		,created_user਍ऀऀⰀ嬀猀攀爀瘀椀挀攀崀ഀഀ
		,submitter)਍      匀䔀䰀䔀䌀吀 䐀䤀匀吀䤀一䌀吀ഀഀ
        @filingHeaderId਍       Ⰰ䀀瀀愀爀攀渀琀䤀搀ഀഀ
       ,@operationId਍       Ⰰ甀猀攀爀开搀愀琀愀⸀䈀爀愀渀挀栀ഀഀ
       ,user_data.[Broker]਍       Ⰰ䜀䔀吀䐀䄀吀䔀⠀⤀ഀഀ
       ,@filingUser਍ऀ   Ⰰ琀爀愀渀猀瀀漀爀琀䴀漀搀攀⸀猀攀爀瘀椀挀攀开挀漀搀攀ഀഀ
	   ,Clients.ClientCode਍      䘀刀伀䴀 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀昀椀氀椀渀最开搀攀琀愀椀氀 搀攀琀愀椀氀ഀഀ
      JOIN zones_ftz214.inbound inbnd਍        伀一 椀渀戀渀搀⸀椀搀 㴀 搀攀琀愀椀氀⸀椀渀戀漀甀渀搀开椀搀ഀഀ
      LEFT JOIN zones_ftz214.inbound_parsed_data parsed_data਍ऀऀ伀一 椀渀戀渀搀⸀椀搀 㴀 瀀愀爀猀攀搀开搀愀琀愀⸀椀搀ഀഀ
      LEFT JOIN dbo.app_users_data user_data਍        伀一 甀猀攀爀开搀愀琀愀⸀唀猀攀爀䄀挀挀漀甀渀琀 㴀 䀀昀椀氀椀渀最唀猀攀爀ഀഀ
	  JOIN handbook_transport_mode AS transportMode਍ऀऀ伀一 琀爀愀渀猀瀀漀爀琀䴀漀搀攀⸀挀漀搀攀开渀甀洀戀攀爀 㴀 瀀愀爀猀攀搀开搀愀琀愀⸀洀漀琀ഀഀ
				LEFT JOIN dbo.Clients AS clients਍ऀऀ伀一 椀渀戀渀搀⸀愀瀀瀀氀椀挀愀渀琀开椀搀 㴀 挀氀椀攀渀琀猀⸀椀搀ഀഀ
      WHERE detail.filing_header_id = @filingHeaderId਍  䔀一䐀㬀ഀഀ
END;਍�