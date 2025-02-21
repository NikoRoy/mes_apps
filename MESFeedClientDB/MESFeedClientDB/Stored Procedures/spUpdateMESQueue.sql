CREATE PROCEDURE [dbo].[spUpdateMESQueue]
AS
declare @rundate datetime,
		@querydate datetime;
BEGIN
	set @rundate = getdate();

	begin
		select @querydate = LastRunDate 
		from [MESFeedClient].[dbo].[tblMESControl]
		where Interface = 'TrainingRecord';
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
					inner join [EmployeeTraining].[dbo].tblTrainingHistory i
						on COALESCE(i.previous_employee_number ,i.Employee_Number) = a.EmployeeNumber
						and i.TrainDocID = a.DocumentNumber
						and i.TrainingHistoryID in (select max(ii.TrainingHistoryID) TrainingHistoryID
												from [EmployeeTraining].[dbo].tblTrainingHistory ii
												where IIF (ii.EnteredDate > ii.ActiveTrainingApprovedOn, ii.EnteredDate, coalesce(ii.ActiveTrainingApprovedOn, ii.EnteredDate)) >= dateadd(minute, datediff(minute, 0, @querydate), 0)-- @querydate
												group by Employee_Number, TrainDocID)
					inner join [EmployeeTraining].[dbo].tblDoc d
						on d.DocID = i.TrainDocID
						and d.DocStatus = 'Active'

					INNER JOIN KronosCustomInterfaces.dbo.kEmployee u
						on u.employee_id = COALESCE(i.previous_employee_number ,i.Employee_Number)

					where exists (select 1 from [USMER-DMESDB001].[OLTP].[Camstar_Sch].Employee oemp
						where oemp.EmployeeName = u.username)
					-- truncate training history datetime to minute
					and IIF (i.EnteredDate > i.ActiveTrainingApprovedOn, i.EnteredDate, coalesce(i.ActiveTrainingApprovedOn, i.EnteredDate)) >= dateadd(minute, datediff(minute, 0, @querydate), 0)-- @querydate
					and Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1) in ('MP','TP','QP','VA');

				-----------------------------------------------------------------------------------------

					insert into [MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] (EmployeeNumber, DocumentNumber, DocumentRevision, LastModifiedDate, sync, SyncAttempt)
					select i.Employee_Number, i.TrainDocID,d.DocCurrentRev, GETDATE(), 0, 0
					from [EmployeeTraining].[dbo].tblTrainingHistory i
					inner join [EmployeeTraining].[dbo].tblDoc d
						on d.DocID = i.TrainDocID
						and d.DocStatus = 'Active'

					INNER JOIN KronosCustomInterfaces.dbo.kEmployee u
						on u.employee_id = COALESCE(i.previous_employee_number ,i.Employee_Number)


					left join [MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] m
						on COALESCE(i.previous_employee_number ,i.Employee_Number) = m.EmployeeNumber
						and i.TrainDocID = m.DocumentNumber

					where m.id is null
					-- truncate training history datetime to minute
					and IIF (i.EnteredDate > i.ActiveTrainingApprovedOn, i.EnteredDate, coalesce(i.ActiveTrainingApprovedOn, i.EnteredDate)) >= dateadd(minute, datediff(minute, 0, @querydate), 0)-- @querydate
					and Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1) in ('MP','TP','QP','VA')
					and exists (select 1 from [USMER-DMESDB001].[OLTP].[Camstar_Sch].Employee oemp
									where oemp.EmployeeName = u.username)
					and i.TrainingHistoryID in (select max(ii.TrainingHistoryID) TrainingHistoryID
												from [EmployeeTraining].[dbo].tblTrainingHistory ii
												where IIF (ii.EnteredDate > ii.ActiveTrainingApprovedOn, ii.EnteredDate, coalesce(ii.ActiveTrainingApprovedOn, ii.EnteredDate)) >= dateadd(minute, datediff(minute, 0, @querydate), 0)-- @querydate
												group by Employee_Number, TrainDocID);


				---------------------------------------------------------------------------------------
				update a
						set a.EmployeeNumber = j.employee_number,
							a.DocumentRevision = d.DocCurrentRev,
							a.LastModifiedDate = GETDATE(),
							a.sync = 0,
							a.SyncAttempt = 0
					from [MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] a

					inner join [EmployeeTraining].[dbo].[tblDoc] d
						on d.DocID = a.DocumentNumber
						and d.DocStatus = 'Active'

					inner join [EmployeeTraining].[dbo].[xxatr_jobprocedure_tracking] j
						on  j.doc_ID = a.DocumentNumber
						and j.employee_number = a.EmployeeNumber
						and j.trackID in (select max(trackID) 
											from [EmployeeTraining].[dbo].[xxatr_jobprocedure_tracking] jj
											where jj.createDate >= @querydate
											and jj.status = 'Verified'
											--and jj.isTraining = 1
											group by employee_number, doc_ID)

					INNER JOIN KronosCustomInterfaces.dbo.kEmployee u
						on u.employee_id = j.Employee_Number

					where j.status = 'Verified'
						--and j.isTraining = 1
						and j.createDate >= @querydate
						and Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1) in ('MP','TP','QP','VA')
						and exists (select 1 from [USMER-DMESDB001].[OLTP].[Camstar_Sch].Employee oemp
						where oemp.EmployeeName = u.username);

				-----------------------------------------------------------------------------------------

				insert into [MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] (EmployeeNumber, DocumentNumber, DocumentRevision, LastModifiedDate, sync, SyncAttempt)
					select j.Employee_Number, j.doc_ID,d.DocCurrentRev, GETDATE(), 0, 0
					from [EmployeeTraining].[dbo].[xxatr_jobprocedure_tracking] j

					INNER JOIN KronosCustomInterfaces.dbo.kEmployee u
						on u.employee_id = j.Employee_Number

					inner join [EmployeeTraining].[dbo].[tblDoc] d
						on d.DocID = j.doc_ID
						and d.DocStatus = 'Active'

					left join [MESFeedClient].[dbo].[tblMesLastTrainingActionTaken] a
						on  j.doc_ID = a.DocumentNumber
						and j.employee_number = a.EmployeeNumber
					
					where a.ID is null
					and j.status = 'Verified'
					--and j.isTraining = 1
					and Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1) in ('MP','TP','QP','VA')
					and exists (select 1 from [USMER-DMESDB001].[OLTP].[Camstar_Sch].Employee oemp
						where oemp.EmployeeName = u.username)
					and j.createDate >= @querydate
					and j.trackID in (select max(trackID) 
											from [EmployeeTraining].[dbo].[xxatr_jobprocedure_tracking] jj
											where jj.createDate >= @querydate
											and jj.status = 'Verified'
											--and jj.isTraining = 1
											group by employee_number, doc_ID);


				end Try
				begin catch
					IF @@TRANCOUNT > 0  
						ROLLBACK TRANSACTION;
						SET @rundate = @querydate;
				end catch

		IF @@TRANCOUNT > 0  
			COMMIT TRANSACTION;

	begin
		update [MESFeedClient].[dbo].[tblMESControl]
		set LastRunDate = @rundate
		where Interface = 'TrainingRecord'
	end
END