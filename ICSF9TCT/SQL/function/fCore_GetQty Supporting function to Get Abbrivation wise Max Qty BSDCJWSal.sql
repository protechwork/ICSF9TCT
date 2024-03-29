
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** 
    Object      :-  UserDefinedFunction [dbo].[fCore_GetQty]   
	Created By  :-  Shaikh Azhar 
	Script Date :-  23-02-2023 16:13:00 
	Purpose     :-  Supporting function to Get Abbrivation wise Max Qty , Used in DC Job Work Sales
                    After Save Function ICSF9TCT/Scripts/JSBS.js - BSDCJWSal   "DCJWSal Controller Method CheckAnxJWSale"
******/
alter function [dbo].[fCore_GetQty] 
                        (
                            @iProduct int,
							@iVendor int,
							@iBranch int,
                            @sAbbr varchar(10)
                        )
                        returns decimal (18,2)
                        begin
                            declare @Qty decimal (18,2)=0
							declare @vType int=0
							Select @vType=iVoucherType from cCore_Vouchers_0 where sAbbr=@sAbbr
                            SELECT  TOP (1) @Qty=abs(sum(TCI.fQuantity))
							FROM  dbo.tCore_Header_0 AS CH INNER JOIN
							dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN
							dbo.tCore_Indta_0 AS TCI ON CD.iBodyId = TCI.iBodyId
							WHERE     (CH.iVoucherType = @vType) AND (TCI.iProduct = @iProduct) AND (CD.iBookNo=@iVendor) 
							and (CD.iFaTag=@iBranch)
                            return abs(isnull(@Qty,0))
                        end