SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create VIEW [dbo].[vIcs_PndngLst]
AS
SELECT TOP (100) PERCENT tCore_Data_0.iBodyId, iRefId,bBase,tCore_Header_0.iDate, sAbbr+':'+sVoucherNo[DocNo],mCore_Product.sName,
                        MIN(tCore_Links_0.fValue)[fQuantity],
                        CASE WHEN bBase = 0 THEN 0 ELSE MIN(tCore_Links_0.fValue)-SUM(BaseTable.Used) END[Balance],
                        CASE 
                            WHEN MIN(BaseTable.iUserId) > 0 AND bBase = 1 AND tCore_Links_0.bClosed = 1 THEN 'Manual Closed' 
	                        WHEN SUM(BaseTable.Used) = 0 AND MIN(ISNULL(BaseTable.iUserId,0)) = 0 THEN 'Pending' 
	                        WHEN MIN(tCore_Links_0.fValue)-SUM(ISNULL(BaseTable.Used,0)) <> 0 AND bBase = 1 THEN 'Partial Consumed'
	                        WHEN bBase = 1 THEN 'Closed'
	                        Else ''
                        END[Status],dbo.cCore_Vouchers_0.iVoucherType, dbo.tCore_Header_0.sVoucherNo, dbo.cCore_Vouchers_0.sName AS VoucherName, vrCore_Department.iMasterId AS Branch, BookNo.iMasterId AS Account,tCore_Data_0.iInvTag,
                         dbo.cCore_Vouchers_0.sAbbr
                        --MIN(CASE WHEN BaseTable.iUserId is null THEN 'Auto' ELSE 'Manual' END)[Status]
                        FROM tCore_Links_0 
                        JOIN tCore_Data_0 ON tCore_Links_0.iTransactionId = tCore_Data_0.iTransactionId
                        LEFT JOIN tCore_Indta_0 ON tCore_Indta_0.iBodyId = tCore_Data_0.iBodyId
                        JOIN tCore_Header_0 ON tCore_Data_0.iHeaderId = tCore_Header_0.iHeaderId
                        JOIN cCore_Vouchers_0 WITH (READUNCOMMITTED) ON cCore_Vouchers_0.iVoucherType = tCore_Header_0.iVoucherType
                        LEFT JOIN mCore_Product ON tCore_Indta_0.iProduct = mCore_Product.iMasterId 
                        LEFT JOIN 
                        (
                            SELECT iTransactionId, iUserId, sum(Balance)[Used] FROM
                            (
	                            SELECT DISTINCT a.iTransactionId, ISNULL(b.fValue,0)[Balance],a.iUserId FROM
	                            (
		                            SELECT iRefId, tCore_Links_0.iTransactionId, fValue, tCore_Links_0.iUserId FROM tCore_Links_0 
                                    JOIN tCore_Data_0 ON tCore_Links_0.iTransactionId = tCore_Data_0.iTransactionId
                                    JOIN tCore_Header_0 ON tCore_Data_0.iHeaderId = tCore_Header_0.iHeaderId
                                    WHERE  bBase = 1 AND tCore_Header_0.bSuspended = 0 
	                            ) a 
	                            LEFT JOIN
                                (
                                    SELECT iRefId, sum(fValue)[fValue] FROM tCore_Links_0 
                                    JOIN tCore_Data_0 ON tCore_Links_0.iTransactionId = tCore_Data_0.iTransactionId
                                    JOIN tCore_Header_0 ON tCore_Data_0.iHeaderId = tCore_Header_0.iHeaderId
                                    WHERE bBase = 0 AND tCore_Header_0.bSuspended = 0 AND tCore_Data_0.bSuspendLinkSaved = 0
                                    Group by iRefId
                                ) b 
                                ON a.iRefId = b.iRefId
                            )B group by iTransactionId, iUserId
                        )BaseTable ON BaseTable.iTransactionId = tCore_Data_0.iTransactionId LEFT JOIN 
(
    SELECT 0[iTreeId],mCore_Department.iMasterId,mCore_Department.sName FROM mCore_Department   WHERE mCore_Department.iStatus < 5
)vrCore_Department ON vrCore_Department.iMasterId = tCore_Data_0.iFaTag  LEFT JOIN (SELECT 0[iTreeId],mCore_Account.iMasterId,mCore_Account.sName FROM mCore_Account   WHERE mCore_Account.iStatus < 5) BookNo ON BookNo.iMasterId = tCore_Data_0.iBookNo LEFT JOIN (SELECT 0[iTreeId],mCore_Account.iMasterId,mCore_Account.sName FROM mCore_Account   WHERE mCore_Account.iStatus < 5) Code ON Code.iMasterId = tCore_Data_0.iCode WHERE tCore_Header_0.bSuspended = 0 
AND tCore_Data_0.iAuthStatus < 2 
--AND (( vrCore_Department.iMasterId = N'2' AND Code.iMasterId = N'43'  OR  vrCore_Department.iMasterId = N'2' AND BookNo.iMasterId = N'43' )) 
GROUP BY tCore_Data_0.iBodyId, dbo.cCore_Vouchers_0.iVoucherType,dbo.cCore_Vouchers_0.sName,tCore_Header_0.iDate, sAbbr, vrCore_Department.iMasterId,BookNo.iMasterId,sVoucherNo,mCore_Product.sName, tCore_Links_0.iRefId,tCore_Links_0.bBase,tCore_Links_0.bClosed,tCore_Data_0.iInvTag ORDER BY iRefId,bBase Desc

GO