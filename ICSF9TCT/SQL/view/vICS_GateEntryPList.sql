create view vICS_GateEntryPList 
as
Select Account,Branch,iInvTag,sabbr,svoucherno,Docno,status  from vIcs_PndngLst where status in ('Pending','Partial Consumed') and (fquantity-abs(balance) != 0) 
