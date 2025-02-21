CREATE PROCEDURE [dbo].[spTrainingActionBackFeed]
AS
declare @rundate datetime,
		@querydate datetime;
BEGIN
	set @rundate = getdate();

	begin
		select @querydate = LastRunDate 
		from [MESFeedClient].[dbo].[tblMESControl]
		where Interface = 'TrainingBackFeed';
	end
		begin transaction
				begin Try
					
					with cte as (
							SELECT 
							k.Employee_Id employee_number,
							Container.ContainerName job_number,
							'MES-Job' job_type,
							TrainingRequirementGroup.TrainingRequirementGroupName Bundle,
							TrainingRequirementBase.TrainingRequirementName Doc_ID,
							'Verified' Status,
							row_number() over (partition by k.Employee_Id,
															Container.ContainerName,
															TrainingRequirementGroup.TrainingRequirementGroupName,
															TrainingRequirementBase.TrainingRequirementName order by txndate desc) rn,
							txndate CreateDate,
							0 isTraining, 
							Employee.FullName RecordedBy
   
									  FROM
									  -- start from tasks and gets execution history
										  [USMER-DMESDB001].[OLTP].[Camstar_Sch].TaskItem 
		
										  INNER JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].ExecuteTaskHistory 
										  ON (ExecuteTaskHistory.TaskId=TaskItem.TaskItemId
										  and TaskItem.TrainingReqGroupId = ExecuteTaskHistory.TrainingReqGroupId)

									-- join to mainline and cross reference to reconcile txn
										   INNER JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].HistoryMainline 
										   ON (HistoryMainline.HistoryMainlineId=ExecuteTaskHistory.HistoryMainlineId)

										   INNER JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].HistoryCrossRef 
										   ON (HistoryMainline.HistoryId=HistoryCrossRef.HistoryId
										   and HistoryMainline.TxnId between HistoryCrossRef.StartTxnId and HistoryCrossRef.EndTxnId)
										--get container
											inner join [USMER-DMESDB001].[OLTP].[Camstar_Sch].[Container]
											on (HistoryMainline.ContainerID = Container.ContainerID)

									-- employee that executed the txn
										   INNER JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].Employee
										   on Employee.EmployeeId = HistoryMainline.EmployeeId
									-- training documents executed
										 inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirementGroup 
											ON (TrainingRequirementGroup.TrainingRequirementGroupId = ExecuteTaskHistory.TrainingReqGroupId)

										inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingReqGroupEntries 
											ON (TrainingRequirementGroup.TrainingRequirementGroupId = TrainingReqGroupEntries.TrainingRequirementGroupId)
  
										  inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirement 
											ON (TrainingReqGroupEntries.EntriesId=TrainingRequirement.TrainingRequirementId)

										inner JOIN [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirementBase
											ON (TrainingRequirementBase.TrainingRequirementBaseId=TrainingRequirement.TrainingRequirementBaseId)
									-- tdb document 
										inner join [USMER-PSQL001].[EmployeeTraining].[dbo].[tblDoc] d
											on  d.DocID = TrainingRequirementBase.TrainingRequirementName

										left join [USMER-PSQL001].[EmployeeTraining].[dbo].[TrainingDocumentTypes] t
											on t.TypeCode = Substring(TrainingRequirementBase.TrainingRequirementName, 1,PATINDEX('%[^A-z]%', TrainingRequirementBase.TrainingRequirementName)-1)

									-- kronos emp mapping
										inner join [USMER-PSQL001].[KronosCustomInterfaces].[dbo].[kEmployee] k 
											on k.username = Employee.EmployeeName

										where 1=1
										and txndate >= @querydate
										and ExecuteTaskHistory.Pass = 1
							)
							insert into [EmployeeTraining].[dbo].[xxatr_jobprocedure_tracking]
							(employee_number, job_number, job_type, scanID, bundle, doc_ID, status, createDate, isTraining, recordedBy)
							select employee_number, job_number, job_type, NEWID() scanID, bundle, doc_ID, status, createDate, isTraining, recordedBy 
							from cte 
							where rn = 1

				end Try
				begin catch
					IF @@TRANCOUNT > 0  
						ROLLBACK TRANSACTION;
				end catch

		IF @@TRANCOUNT > 0 
		Begin
			COMMIT TRANSACTION;

			begin
				update [MESFeedClient].[dbo].[tblMESControl]
				set LastRunDate = @rundate
				where Interface = 'TrainingBackFeed'
			end
		End
END
