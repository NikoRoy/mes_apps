CREATE PROCEDURE [dbo].[spDocDailyChangesGet]
AS
DECLARE @controldate datetime
BEGIN
	BEGIN
		Select @controldate = lastrundate 
		from [MesFeedClient].[dbo].[tblMESControl]
		where interface = 'TrainingDocument'
	END
	BEGIN
		SELECT 
			DocID, 
			DocDesc,
			DocPath,
			DocCurrentRev
		
		FROM [EmployeeTraining].[dbo].[tblDoc] d
		WHERE docstatus = 'Active'
		-- truncate control date to day
		AND revupdatedate >= CAST(@controldate AS DATE)
		and Substring(DocID, 1,PATINDEX('%[^A-z]%', DocID)-1) in ('MP','TP','QP','VA')
		and not exists (select b.TrainingRequirementName, tr.TrainingRequirementRevision
						from [USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirementBase b
							,[USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirement tr 
						where b.TrainingRequirementBaseId=tr.TrainingRequirementBaseId
							and b.RevOfRcdId = tr.TrainingRequirementId
							and b.TrainingRequirementName = d.DocID
							and tr.TrainingRequirementRevision = d.DocCurrentRev
					)
	END
END
