SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	 Shaikh Azhar 
-- Created date: 12-03-2023 
-- Description:	Update Trigger IDate when Qty > 0 
-- =============================================
Alter TRIGGER trgBatchAftInst on tCore_Batch_0
FOR INSERT
AS declare @BodyId int
   select @BodyId=iBodyID from inserted i;
BEGIN
	declare @Qty decimal(18,2)	
	SET NOCOUNT ON;
	Select @Qty = fQuantity from tCore_Indta_0 where iBodyId=@BodyId
    if (@Qty > 0)
	begin 
		update  tCore_Batch_0 set IMFDate =(Select IDate from dbo.tCore_Header_0 as tch inner join 
		dbo.tCore_Data_0 as tcd on tch.iHeaderId = tcd.iHeaderId
		where iBodyId=@BodyId) where  iBodyId=@BodyId
	end  

END
GO
