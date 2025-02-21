CREATE PROCEDURE [dbo].[spCheckLastActions]
	@attempts INT
AS
BEGIN

SELECT *

FROM
(
	SELECT DISTINCT
		sub.Employee_number EmployeeNumber
	  ,Coalesce(sub.[Username], sub.[EMAIL_ADDRESS]) EMAIL_ADDRESS
	  ,sub.[FIRST_NAME]
	  ,sub.[LAST_NAME]
      ,sub.[DocumentNumber]
      ,sub.[DocumentRevision]
      ,sub.[LastModifiedDate]
	  ,sub.[ID]
	  ,sub.[sync]
	  ,sub.[SyncAttempt]			
		
		,case
			when 
				case			
					--when requirement is passive, if there is not a passive and theres not an active record
					when isnull(sub.rtl,'') = 'P'  
							and pas_h.traininghistoryid is null 
							and act_h.traininghistoryid is null
							and ex_rs_h.TrainingHistoryID is null
							and p2_h.TrainingHistoryID is null
					then 'Required' 
					--when requirement is nonpassive, if activetrainingapproval is null then the record doesnt exist or its not approved
					when isnull(sub.rtl,'') = 'P2'
							and isnull(p2_h.TrainingHistoryID,'') = '' 
					then 'Required' 
					when isnull(sub.rtl,'') = 'PR' 
					then 'Expired'
					when isnull(sub.rtl,'')='I'
					then'Required'
					when isnull(sub.rtl,'') = 'A'
							and isnull(act_h.TrainingHistoryID,'') = '' 
					then 'Required'
					when isnull(sub.isSatisfied,'') = 0
					then 'Required'
					when sub.REHIRE_ASG_DATE > sub.RevUpdateDate and hh.TrainDate < sub.REHIRE_ASG_DATE
					then 'Required'
					else 'Current'
				end = 'Current'
			
			then    'Trained'
			else 
					case 
						when inTraining.isTraining = 1
						then 'Training'
						else 'Disallowed'
					end				
		end
		[TrainingStatus]

	FROM
		(
			select c.*,
				d.DocCurrentRev,
				d.DocDesc,
				d.DocPath,
				d.ExpirationMonths,
				d.DocID,
				d.DocStatus,
				d.RevUpdateDate,
				e.FULL_NAME ,
				e.DEPARTMENT_NUMBER ,
				e.DEPARTMENT_NAME ,
				e.TITLE Title,
				e.SUPER_EMAIL, 
					e.FIRST_NAME,
					e.LAST_NAME,
					e.EMAIL_ADDRESS,
					u.username, 
					m.DocumentNumber,
					m.DocumentRevision,
					m.LastModifiedDate,
					m.[ID],
					m.[sync],
					m.[SyncAttempt],
				[EmployeeTraining].[dbo].[fn_CalculateRequiredTrainingLevel](c.Employee_number, d.DocID, d.DocCurrentRev) rtl,
				[EmployeeTraining].[dbo].[fn_IsAllCORTSatisfied](c.Employee_number, d.DocID) isSatisfied,
				e.REHIRE_ASG_DATE
				from 
					[MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] m

				INNER JOIN 	[EmployeeTraining].[dbo].tblCurrentTrainingRequirements c
					on c.Employee_number = m.EmployeeNumber
					and c.CurrentTrainingDocID = m.DocumentNumber

				INNER JOIN [EmployeeTraining].[dbo].tblDoc d
					on c.CurrentTrainingDocID = d.DocID 

				INNER JOIN [EmployeeTraining].[dbo].vwCurrentEmployees2 e
					on c.Employee_number = e.EMPLOYEE_NUMBER 

				INNER JOIN KronosCustomInterfaces.dbo.kEmployee u
					on u.employee_id = e.EMPLOYEE_NUMBER

				where exists (select 1 from [USMER-DMESDB001].[OLTP].[Camstar_Sch].Employee oemp
					where oemp.EmployeeName = u.username) 
				and m.sync = 0
				and m.SyncAttempt < @attempts
				and Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1) in ('MP','TP','QP','VA')

		) sub


		LEFT JOIN [EmployeeTraining].[dbo].TrainingDocumentTypes t
			on substring(sub.CurrentTrainingDocID,1,datalength(sub.CurrentTrainingDocID)-6) = t.TypeCode

		LEFT JOIN [OracleDatabaseStubs].APPS.XXATR_EMPLOYEE_REQ_DOC_V obd 
			on obd.EMPLOYEE_NUMBER = sub.Employee_number 
			AND obd.DOCUMENT_NUMBER = sub.CurrentTrainingDocID
		
		LEFT JOIN [EmployeeTraining].[dbo].ChangeOrderRequiredTraining co
			on sub.DocID = co.DocumentId
			and sub.DocCurrentRev = co.Revision 
			and sub.Employee_number = co.EmployeeNumber
			and co.Active = 1
			and co.Committed = 1

		LEFT JOIN [EmployeeTraining].[dbo].tblTrainingHistory lh
		on lh.TrainingHistoryID =
		(
			SELECT TOP 1 h3.TrainingHistoryID
			FROM [EmployeeTraining].[dbo].tblTrainingHistory h3
			WHERE 
				h3.Employee_Number = sub.Employee_number
				and h3.TrainDocID = sub.CurrentTrainingDocID
			ORDER BY h3.TrainDocRev DESC
		)

		LEFT JOIN [EmployeeTraining].[dbo].tblTrainingHistory hh
		on hh.TrainingHistoryID =
		(
			select top 1 sub.TrainingHistoryID
			from
				(
				  SELECT h2.*
							FROM 
								[EmployeeTraining].[dbo].tblTrainingHistory h2
							INNER JOIN [EmployeeTraining].[dbo].tblDoc d2
							on h2.TrainDocID = d2.DocID
							and h2.TrainDocRev = d2.DocCurrentRev							
							WHERE 
								h2.Employee_Number =  sub.Employee_number
								and h2.TrainDocID = sub.CurrentTrainingDocID							
					) as sub
			ORDER BY TrainingHistoryID DESC
		)

		LEFT JOIN [EmployeeTraining].[dbo].tblTrainingHistory pas_h
		on pas_h.TrainingHistoryID =
		(
			select top 1 sub.TrainingHistoryID
			from
				(
				  SELECT h2.*
							FROM 
								[EmployeeTraining].[dbo].tblTrainingHistory h2
							INNER JOIN [EmployeeTraining].[dbo].tblDoc d2
							on h2.TrainDocID = d2.DocID
							and h2.TrainDocRev = d2.DocCurrentRev							
							WHERE 
								h2.Employee_Number =  sub.Employee_number
								and h2.TrainDocID = sub.CurrentTrainingDocID
								and h2.traininglevel = 'P'
					) as sub
		)

		LEFT JOIN [EmployeeTraining].[dbo].tblTrainingHistory act_h
		on act_h.TrainingHistoryID =
		(
			SELECT TOP 1
				TrainingHistoryID
			FROM [EmployeeTraining].[dbo].tblTrainingHistory h3
			INNER JOIN [EmployeeTraining].[dbo].tblDoc d
			on h3.TrainDocID = d.DocID
			and h3.TrainDocRev = d.DocCurrentRev
			WHERE
				h3.Employee_Number = sub.Employee_number
				and h3.TrainDocID = sub.CurrentTrainingDocID
				and h3.TrainingLevel = 'A'
				and h3.ActiveTrainingApproval = 'A'				
			ORDER BY
				TrainDate DESC
		)

		LEFT JOIN [EmployeeTraining].[dbo].tblTrainingHistory ex_rs_h
		on ex_rs_h.TrainingHistoryID =
		(
			SELECT TOP 1
				TrainingHistoryID
			FROM [EmployeeTraining].[dbo].tblTrainingHistory h3
			inner join [EmployeeTraining].[dbo].tblDoc d 
			on h3.TrainDocID = d.DocID
			and h3.TrainDocRev = d.DocCurrentRev
			WHERE
				h3.Employee_Number = sub.Employee_number
				and h3.TrainDocID = sub.CurrentTrainingDocID
				and h3.TrainingLevel ='PR'
				and h3.ActiveTrainingApproval = 'A'				
			ORDER BY
				TrainDate DESC
		)

		LEFT JOIN [EmployeeTraining].[dbo].tblTrainingHistory p2_h
		on p2_h.TrainingHistoryID =
		(
			SELECT TOP 1
				TrainingHistoryID
			FROM [EmployeeTraining].[dbo].tblTrainingHistory h3
			INNER JOIN [EmployeeTraining].[dbo].tblDoc d
			on h3.TrainDocID = d.DocID
			and h3.TrainDocRev = d.DocCurrentRev			
			WHERE
				h3.Employee_Number = sub.Employee_number
				and h3.TrainDocID = sub.CurrentTrainingDocID
				and h3.TrainingLevel = 'P2'
				and h3.ActiveTrainingApproval = 'A'				
			ORDER BY
				TrainDate DESC
		)


	--get last verified TV for employee/doc in mesaction (training)
	left join (
			select b.createDate, b.doc_ID, b.employee_number, b.isTraining, b.status, b.scanID, b.trackID
			from  [EmployeeTraining].[dbo].[xxatr_jobprocedure_tracking] b
			inner join (
				select j.employee_number, j.doc_ID
				, max(j.createDate) createdate
				from [MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] a
				inner join [EmployeeTraining].[dbo].[tblDoc] d
					on d.DocID = a.DocumentNumber
				inner join [EmployeeTraining].[dbo].[xxatr_jobprocedure_tracking] j
					on  j.doc_ID = a.DocumentNumber
					and j.employee_number = a.EmployeeNumber
					and j.status = 'Verified'
					--and j.isTraining = 1
					and DATEDIFF(d, j.createDate, getdate()) <= ISNULL(d.CurrencyDays, 30)
				group by j.employee_number, j.doc_ID) mm
				on mm.employee_number =  b.employee_number
				and mm.doc_ID = b.doc_ID
				and mm.createdate = b.createDate
	) inTraining
	on inTraining.employee_number = sub.Employee_number
	and inTraining.doc_ID = sub.CurrentTrainingDocID

	WHERE sub.DocStatus = 'Active'
) tr
WHERE 1=1 
ORDER BY
	tr.DocumentNumber ASC

END

