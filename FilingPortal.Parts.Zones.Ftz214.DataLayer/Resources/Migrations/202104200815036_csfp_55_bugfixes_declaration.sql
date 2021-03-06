਍ഀഀ
਍ⴀⴀ 愀搀搀 搀攀挀氀愀爀愀琀椀漀渀 爀攀挀漀爀搀 ⴀⴀഀഀ
ALTER   PROCEDURE [zones_ftz214].[sp_add_declaration] (਍ऀ䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀 䤀一吀Ⰰഀഀ
	@parentId INT,਍ऀ䀀昀椀氀椀渀最唀猀攀爀 一嘀䄀刀䌀䠀䄀刀⠀㈀㔀㔀⤀ 㴀 一唀䰀䰀Ⰰഀഀ
	@operationId UNIQUEIDENTIFIER = NULL OUTPUT)਍䄀匀ഀഀ
BEGIN਍ऀ匀䔀吀 一伀䌀伀唀一吀 伀一㬀ഀഀ
਍ऀ䐀䔀䌀䰀䄀刀䔀 䀀琀愀戀氀攀一愀洀攀 嘀䄀刀䌀䠀䄀刀⠀㄀㈀㠀⤀ 㴀 ✀搀攀挀氀愀爀愀琀椀漀渀✀ഀഀ
	DECLARE @allowMultiple BIT = 0;਍ഀഀ
	SET @operationId = COALESCE(@operationId, NEWID());਍ഀഀ
	-- get section property is_array਍ऀ匀䔀䰀䔀䌀吀 䀀愀氀氀漀眀䴀甀氀琀椀瀀氀攀 㴀 猀攀挀琀椀漀渀⸀椀猀开愀爀爀愀礀ഀഀ
	FROM zones_ftz214.form_section_configuration section਍ऀ圀䠀䔀刀䔀 猀攀挀琀椀漀渀⸀琀愀戀氀攀开渀愀洀攀 㴀 䀀琀愀戀氀攀一愀洀攀ഀഀ
਍ऀⴀⴀ 愀搀搀 搀攀挀氀愀爀愀琀椀漀渀 搀愀琀愀ഀഀ
	IF @allowMultiple = 1 OR NOT EXISTS (਍ऀऀ匀䔀䰀䔀䌀吀 ㄀ഀഀ
		FROM zones_ftz214.declaration declaration਍ऀऀ圀䠀䔀刀䔀 搀攀挀氀愀爀愀琀椀漀渀⸀昀椀氀椀渀最开栀攀愀搀攀爀开椀搀 㴀 䀀昀椀氀椀渀最䠀攀愀搀攀爀䤀搀ഀഀ
	)਍ऀ䈀䔀䜀䤀一ഀഀ
		INSERT INTO zones_ftz214.declaration (਍ऀऀऀ 昀椀氀椀渀最开栀攀愀搀攀爀开椀搀ഀഀ
			,parent_record_id਍ऀऀऀⰀ漀瀀攀爀愀琀椀漀渀开椀搀ഀഀ
			,importer਍ऀऀऀⰀ琀爀愀渀猀瀀漀爀琀ഀഀ
			,container਍ऀऀऀⰀ愀搀洀椀猀猀椀漀渀开琀礀瀀攀ഀഀ
			,direct_delivery਍ऀऀऀⰀ椀猀猀甀攀爀ഀഀ
			,ocean_bill਍ऀऀऀⰀ瘀攀猀猀攀氀ഀഀ
			,voyage਍ऀऀऀⰀ氀漀愀搀椀渀最开瀀漀爀琀ഀഀ
			,discharge_port਍ऀऀऀⰀ昀琀稀开瀀漀爀琀ഀഀ
			,carrier_scac਍ऀऀऀⰀ搀攀瀀ഀഀ
			,arr਍ऀऀऀⰀ愀爀爀㈀ഀഀ
			,hmf਍ऀऀऀⰀ昀椀爀猀琀开愀爀爀开搀愀琀攀ഀഀ
			,description਍ऀऀऀⰀ昀椀爀洀猀开挀漀搀攀ഀഀ
			,zone_id਍ऀऀऀⰀ礀攀愀爀ഀഀ
			,applicant਍ऀऀऀⰀ昀琀稀开漀瀀攀爀愀琀漀爀ഀഀ
			,created_date਍ऀऀऀⰀ挀爀攀愀琀攀搀开甀猀攀爀ഀഀ
			,it_no਍ऀऀऀⰀ椀琀开搀愀琀攀ഀഀ
			,admission_no਍ऀऀऀⰀ甀氀琀椀洀愀琀攀开挀漀渀猀椀最渀攀攀ഀഀ
		)਍ऀऀ匀䔀䰀䔀䌀吀ഀഀ
			 @filingHeaderId਍ऀऀऀⰀ䀀瀀愀爀攀渀琀䤀搀ഀഀ
			,@operationId਍ऀऀऀⰀ挀氀椀攀渀琀猀⸀䌀氀椀攀渀琀䌀漀搀攀ഀഀ
			,transportMode.code਍ऀऀऀⰀ琀爀愀渀猀瀀漀爀琀䴀漀搀攀⸀挀漀渀琀愀椀渀攀爀开挀漀搀攀ഀഀ
			,inbnd.admission_type਍ऀऀऀⰀ搀椀爀攀挀琀开搀攀氀椀瘀攀爀礀ഀഀ
			,parsed_data.imp_carrier_code਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀洀愀猀琀攀爀ഀഀ
			,parsed_data.imp_vessel਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀昀氀琀开瘀漀礀开琀爀椀瀀 ⴀⴀ瘀漀礀愀最攀ഀഀ
			,parsed_data.foreign_port਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀甀渀氀愀搀椀渀最开瀀漀爀琀ഀഀ
			,parsed_data.zone_port਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀椀洀瀀开挀愀爀爀椀攀爀开挀漀搀攀 ⴀⴀ 挀愀爀爀椀攀爀开猀挀愀挀ഀഀ
			,parsed_data.export_date਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀椀洀瀀漀爀琀开搀愀琀攀ഀഀ
			,parsed_data.import_date ਍ऀऀऀⰀ瀀愀爀猀攀搀开氀椀渀攀⸀栀洀昀 ⴀⴀ 栀洀昀ഀഀ
			,parsed_data.est_arr_date਍ऀऀऀⰀ瀀愀爀猀攀搀开氀椀渀攀⸀搀攀猀挀爀椀瀀琀椀漀渀ഀഀ
			,parsed_data.ptt_firms਍ऀऀऀⰀ椀渀戀渀搀⸀稀漀渀攀开椀搀ഀഀ
			,parsed_data.admission_year਍ऀऀऀⰀ挀氀椀攀渀琀猀⸀䌀氀椀攀渀琀䌀漀搀攀ഀഀ
			,clients.ClientCode਍ऀऀऀⰀ䜀䔀吀䐀䄀吀䔀⠀⤀ഀഀ
			,@filingUser਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀椀琀开渀漀ഀഀ
			,parsed_data.it_date਍ऀऀऀⰀ瀀愀爀猀攀搀开搀愀琀愀⸀愀搀洀椀猀猀椀漀渀开渀漀ഀഀ
			,clients.ClientCode਍ഀഀ
		FROM zones_ftz214.filing_detail AS detail਍ഀഀ
		JOIN zones_ftz214.inbound AS inbnd਍ऀऀ伀一 椀渀戀渀搀⸀椀搀 㴀 搀攀琀愀椀氀⸀椀渀戀漀甀渀搀开椀搀ഀഀ
		਍ऀऀ䰀䔀䘀吀 䨀伀䤀一 稀漀渀攀猀开昀琀稀㈀㄀㐀⸀椀渀戀漀甀渀搀开瀀愀爀猀攀搀开搀愀琀愀 瀀愀爀猀攀搀开搀愀琀愀ഀഀ
		ON inbnd.id = parsed_data.id਍ഀഀ
		LEFT JOIN zones_ftz214.inbound_parsed_line parsed_line਍ऀऀ伀一 椀渀戀渀搀⸀椀搀 㴀 瀀愀爀猀攀搀开氀椀渀攀⸀椀搀ഀഀ
਍ऀऀ䰀䔀䘀吀 䨀伀䤀一 搀戀漀⸀䌀氀椀攀渀琀猀 䄀匀 挀氀椀攀渀琀猀ഀഀ
		ON inbnd.applicant_id = clients.id਍ഀഀ
		JOIN handbook_transport_mode AS transportMode਍ऀऀ伀一 琀爀愀渀猀瀀漀爀琀䴀漀搀攀⸀挀漀搀攀开渀甀洀戀攀爀 㴀 瀀愀爀猀攀搀开搀愀琀愀⸀洀漀琀ഀഀ
਍ഀഀ
		WHERE detail.filing_header_id = @filingHeaderId਍ऀ䔀一䐀ഀഀ
END;਍�