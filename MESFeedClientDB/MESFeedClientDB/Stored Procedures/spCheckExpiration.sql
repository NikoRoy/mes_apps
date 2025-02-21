CREATE PROCEDURE [dbo].[spCheckExpiration]
AS
BEGIN
select sub.Employee_number EmployeeNumber
	  ,sub.[Username] UNumber
	  ,sub.[FIRST_NAME]
	  ,sub.[LAST_NAME]
      ,sub.CurrentTrainingDocID DocumentNumber
      ,sub.DocCurrentRev DocumentRevision
      ,Getdate() [LastModifiedDate]
	  ,'Disallowed' [TrainingStatus]
	  ,0 [ID]
	  ,cast(0 as bit) [sync]
	  ,0 [SyncAttempt]
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
				[EmployeeTraining].[dbo].[fn_CalculateRequiredTrainingLevel](c.Employee_number, d.DocID, d.DocCurrentRev) rtl,
				e.REHIRE_ASG_DATE
				from 
					[EmployeeTraining].[dbo].tblCurrentTrainingRequirements c		

				INNER JOIN [EmployeeTraining].[dbo].tblDoc d
					on c.CurrentTrainingDocID = d.DocID 

				INNER JOIN [EmployeeTraining].[dbo].vwCurrentEmployees2 e
					on c.Employee_number = e.EMPLOYEE_NUMBER 

				INNER JOIN KronosCustomInterfaces.dbo.kEmployee u
					on u.employee_id = e.EMPLOYEE_NUMBER 

				where exists (select 1 from [USMER-DMESDB001].[OLTP].[Camstar_Sch].Employee oemp
					where oemp.EmployeeName = u.username)
				and d.DocStatus = 'Active'
				and Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1) in ('MP','TP','QP','VA')

		) sub
	where sub.rtl ='PR'

END