CREATE PROCEDURE [dbo].[spQueueNewUsers]
AS
DECLARE @rundate datetime,
		@querydate datetime;
BEGIN
	set @rundate = getdate();

	begin
		select @querydate = LastRunDate 
		from [MESFeedClient].[dbo].[tblMESControl]
		where Interface = 'TrainingRecordNewUser';
	end
	begin transaction
				begin Try

					update a
					set a.EmployeeNumber = i.Employee_number,
						a.DocumentRevision = d.DocCurrentRev,
						a.LastModifiedDate = GETDATE(),
						a.sync = 0,
						a.SyncAttempt = 0
					from [MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] a
					inner join [EmployeeTraining].dbo.tblCurrentTrainingRequirements i
						on COALESCE(i.previous_Employee_number ,i.Employee_Number) = a.EmployeeNumber
						and i.CurrentTrainingDocID = a.DocumentNumber
						
					inner join [EmployeeTraining].[dbo].tblDoc d
						on d.DocID = i.CurrentTrainingDocID
						and d.DocStatus = 'Active'

					INNER JOIN KronosCustomInterfaces.dbo.kEmployee u
						on u.employee_id = COALESCE(i.previous_employee_number ,i.Employee_Number)

					where exists (SELECT e.EmployeeName, c.LastChangeDate
									FROM  [USMER-DMESDB001].[OLTP].[Camstar_Sch].[ChangeStatus] c
									inner join [USMER-DMESDB001].[OLTP].[Camstar_Sch].[Employee] e
										on c.ChangeStatusId = e.ChangeStatusId
									where e.EmployeeName = u.username
									--truncate to seconds
									and c.LastChangeDate >= DATEADD(ms, -DATEPART(ms, @querydate), @querydate) --@querydate
							)
					and Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1) in ('MP','TP','QP','VA');

				-----------------------------------------------------------------------------------------------
				insert into [MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] (EmployeeNumber, DocumentNumber, DocumentRevision, LastModifiedDate, sync, SyncAttempt)
				select i.Employee_Number, i.CurrentTrainingDocID,d.DocCurrentRev, GETDATE(), 0, 0
					from [EmployeeTraining].[dbo].tblCurrentTrainingRequirements i
					inner join [EmployeeTraining].[dbo].tblDoc d
						on d.DocID = i.CurrentTrainingDocID
						and d.DocStatus = 'Active'

					INNER JOIN KronosCustomInterfaces.dbo.kEmployee u
						on u.employee_id = COALESCE(i.previous_employee_number ,i.Employee_Number)


					left join [MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] m
						on COALESCE(i.previous_employee_number ,i.Employee_Number) = m.EmployeeNumber
						and i.CurrentTrainingDocID = m.DocumentNumber

					where m.id is null
					and Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1) in ('MP','TP','QP','VA')
					and exists (SELECT e.EmployeeName, c.LastChangeDate
									FROM  [USMER-DMESDB001].[OLTP].[Camstar_Sch].[ChangeStatus] c
									inner join [USMER-DMESDB001].[OLTP].[Camstar_Sch].[Employee] e
										on c.ChangeStatusId = e.ChangeStatusId
									where e.EmployeeName = u.username
									-- truncate the milliseconds
									and c.LastChangeDate >= DATEADD(ms, -DATEPART(ms, @querydate), @querydate) --@querydate
									)
				end Try
				begin catch
					IF @@TRANCOUNT > 0  
						ROLLBACK TRANSACTION;
				end catch

		IF @@TRANCOUNT > 0  
			COMMIT TRANSACTION;

	begin
		update [MESFeedClient].[dbo].[tblMESControl]
		set LastRunDate = @rundate
		where Interface = 'TrainingRecordNewUser'
	end
END
