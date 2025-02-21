CREATE PROCEDURE [dbo].[spCheckCurrency]
	
AS
BEGIN
WITH traininghistory as (
	SELECT h.Employee_Number, h.TrainDocID, h.TrainDocRev, Max(h.TrainDate) TrainDate
	FROM 
	[EmployeeTraining].[dbo].tblTrainingHistory h
	INNER JOIN [EmployeeTraining].[dbo].tblDoc d
		on h.TrainDocID = d.DocID		
		and h.TrainDocRev = d.DocCurrentRev
		and d.DocStatus = 'Active'
	left join [EmployeeTraining].[dbo].[TrainingDocumentTypes] t
		on t.TypeCode = Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1)
	WHERE 1=1
	and (h.TrainingLevel = 'P' or isnull(h.ActiveTrainingApproval, '') = 'A')
	and DateDiff(d, trainDate, GetDate()) <= isnull(d.CurrencyDays, t.DefaultCurrencyDays)
	group by h.Employee_Number, h.TrainDocID, h.TrainDocRev								
), 
jpt as (
	select max(b.createDate) createDate, b.doc_ID, b.employee_number, d.DocCurrentRev
		from  [EmployeeTraining].[dbo].[xxatr_jobprocedure_tracking] b

		inner join [EmployeeTraining].[dbo].[tblDoc] d
			on d.DocID = Upper(b.doc_ID)
			and b.[status] = 'Verified'

		left join [EmployeeTraining].[dbo].[TrainingDocumentTypes] t
			on t.TypeCode = Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1) 
		
		where 1=1
		and DATEDIFF(D,b.createDate, GETDATE()) <= isnull(d.CurrencyDays, t.DefaultCurrencyDays)
		group by b.doc_ID, b.employee_number, d.DocCurrentRev	
				
),
oltp as (
	SELECT 
		Employee.EmployeeName EmployeeName,
		Employee.FullName FullName,
		TrainingRequirementBase.TrainingRequirementName Document,
		TrainingRequirement.TrainingRequirementRevision rev,
		Max(TxnDate) txndate
   
		  FROM
			  [USMER-DMESDB001].[OLTP].[Camstar_Sch].TaskItem 

			  INNER JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].ExecuteTaskHistory 
			  ON (ExecuteTaskHistory.TaskId=TaskItem.TaskItemId)

			   INNER JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].HistoryMainline 
			   ON (HistoryMainline.HistoryMainlineId=ExecuteTaskHistory.HistoryMainlineId)

			   INNER JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].HistoryCrossRef 
			   ON (HistoryMainline.HistoryId=HistoryCrossRef.HistoryId
			   and HistoryMainline.TxnId between HistoryCrossRef.StartTxnId and HistoryCrossRef.EndTxnId)

			   INNER JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].Employee
			   on Employee.EmployeeId = HistoryMainline.EmployeeId   
  
			  inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRecord 
				ON (Employee.EmployeeId=TrainingRecord.EmployeeId)
  
			  inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirement 
				ON (TrainingRecord.TrainingRequirementId=TrainingRequirement.TrainingRequirementId)
  
			  inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingReqGroupEntries 
				ON (TrainingReqGroupEntries.EntriesId=TrainingRequirement.TrainingRequirementId
				)
  
			  inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirementGroup 
				ON (TrainingRequirementGroup.TrainingRequirementGroupId=TrainingReqGroupEntries.TrainingRequirementGroupId
				and TrainingRequirementGroup.TrainingRequirementGroupId = ExecuteTaskHistory.TrainingReqGroupId)
  
			  inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirementBase
				ON (TrainingRequirementBase.TrainingRequirementBaseId=TrainingRequirement.TrainingRequirementBaseId)

			inner join [EmployeeTraining].[dbo].[tblDoc] d
				on d.DocCurrentRev = TrainingRequirement.TrainingRequirementRevision
				and d.DocID = TrainingRequirementBase.TrainingRequirementName

			left join [EmployeeTraining].[dbo].[TrainingDocumentTypes] t
				on t.TypeCode = Substring(TrainingRequirementBase.TrainingRequirementName, 1,PATINDEX('%[^A-z]%', TrainingRequirementBase.TrainingRequirementName)-1)

			where DATEDIFF(d, TxnDate, GETDATE()) <= isnull(d.CurrencyDays, t.DefaultCurrencyDays)
			and ExecuteTaskHistory.Pass = 1
			group by Employee.EmployeeName,
					Employee.FullName,
					TrainingRequirementBase.TrainingRequirementName,
					TrainingRequirement.TrainingRequirementRevision
)
select distinct
ctr.Employee_number EmployeeNumber
	  ,k.username UNumber
	  ,k.[FIRST_NAME]
	  ,k.[LAST_NAME]
      ,ctr.CurrentTrainingDocID DocumentNumber
      ,d.DocCurrentRev DocumentRevision
      ,Getdate() [LastModifiedDate]
	  ,'Disallowed' [TrainingStatus]
	  ,0 [ID]
	  ,cast(0 as bit) [sync]
	  ,0 [SyncAttempt]
from 
(select distinct isnull (Employee_number, mes.EmployeeNumber) Employee_number, isnull( CurrentTrainingDocID, DocumentNumber) CurrentTrainingDocID
	from [EmployeeTraining].[dbo].[tblCurrentTrainingRequirements] ctr
	full join MESFeedClient.dbo.tblMesLastTrainingActionTaken mes 
		on mes.EmployeeNumber = ctr.Employee_number
		and mes.DocumentNumber = ctr.CurrentTrainingDocID
	) as ctr

inner join [EmployeeTraining].[dbo].[tblDoc] d
	on d.DocID = ctr.CurrentTrainingDocID
	
inner join [KronosCustomInterfaces].[dbo].kEmployee k
	on ctr.Employee_Number = k.employee_id

left join traininghistory th
	on th.Employee_Number = ctr.Employee_number
	and th.TrainDocID = ctr.CurrentTrainingDocID
	and th.TrainDocRev = d.DocCurrentRev
left join jpt  j
	on j.doc_ID =  ctr.CurrentTrainingDocID
	and j.employee_number = ctr.Employee_number
	and j.DocCurrentRev = d.DocCurrentRev
left join oltp o
	on o.EmployeeName = k.username
	and o.Document = ctr.CurrentTrainingDocID
	and o.rev = d.DocCurrentRev

where not exists (select top 1 1
							  from [USMER-DMESDB001].[OLTP].[Camstar_Sch].[ModelingAuditTrailHeader] mh
								inner join [USMER-DMESDB001].[OLTP].[Camstar_Sch].[Employee] e
										on (mh.ParentInstanceId = e.EmployeeId)
								inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRecord tr
										ON (e.EmployeeId=tr.EmployeeId
										and mh.ObjectInstanceId = tr.TrainingRecordId)
								inner join [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRecordStatus ts
										on ts.TrainingRecordStatusId = tr.StatusId
								inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirement trq
										ON (tr.TrainingRequirementId=trq.TrainingRequirementId)
								 inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirementBase trqb
										ON (trqb.TrainingRequirementBaseId=trq.TrainingRequirementBaseId)
							  where mh.ObjectTypeId = 7238
							  and ts.TrainingRecordStatusName in  ('Trained', 'Disallowed', 'Training')
							  and e.EmployeeName = k.username
							  and trqb.TrainingRequirementName = ctr.CurrentTrainingDocID
							  and trq.TrainingRequirementRevision = d.DocCurrentRev
							  and DATEDIFF(d, mh.TxnDate, GETDATE()) <= isnull(d.CurrencyDays, 30)
							  order by mh.TxnDate desc)
and exists (select 1 from [USMER-DMESDB001].[OLTP].[Camstar_Sch].Employee oemp where oemp.EmployeeName = k.username)
and isnull(th.TrainDate,'') = '' 
and coalesce(j.createDate, o.txndate,'') = ''
and k.status = 'Active'
and d.DocStatus = 'Active'
and Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1) in ('MP','TP','QP','VA');
END
