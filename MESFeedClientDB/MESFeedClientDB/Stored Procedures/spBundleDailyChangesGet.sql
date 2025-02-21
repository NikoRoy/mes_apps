CREATE PROCEDURE [dbo].[spBundleDailyChangesGet]
AS
DECLARE @controldate datetime
BEGIN
	BEGIN
		Select @controldate = lastrundate 
		from [MesFeedClient].[dbo].[tblMESControl]
		where interface = 'TrainingGroup'
	END
	BEGIN
		with cte as (
		  select distinct rg.TrainingRequirementGroupName [TGName]
					from [USMER-PSQL001].[EmployeeTraining].[dbo].[tblDoc] d,   
					[USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirementGroup rg
					,[USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingReqGroupEntries ge
					,[USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirementBase b
					,[USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirement tr
					where ge.TrainingRequirementGroupId=rg.TrainingRequirementGroupId
					and tr.TrainingRequirementId=ge.EntriesId
					and b.TrainingRequirementBaseId=tr.TrainingRequirementBaseId					
					and b.trainingrequirementname = d.DocID
					--truncate control date to day and only grab group entries that are not at the current rev
					and tr.TrainingRequirementRevision != d.DocCurrentRev
				and RevUpdateDate >= CAST(@controldate AS DATE)
				and d.DocStatus = 'Active'
				and Substring(d.DocID, 1,PATINDEX('%[^A-z]%', d.DocID)-1) in ('MP','TP','QP','VA')
		)
		select distinct rg.TrainingRequirementGroupName , coalesce(rg.Description,rg.TrainingRequirementGroupName) [Description],  b.trainingrequirementname Document, d.DocCurrentRev
		from cte, 
				[USMER-PSQL001].[EmployeeTraining].[dbo].[tblDoc] d,   
				[USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirementGroup rg
				,[USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingReqGroupEntries ge
				,[USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirementBase b
				,[USMER-DMESDB001].[OLTP].[Camstar_Sch].TrainingRequirement tr
				where ge.TrainingRequirementGroupId=rg.TrainingRequirementGroupId
				and b.TrainingRequirementBaseId=tr.TrainingRequirementBaseId
				and tr.TrainingRequirementId=ge.EntriesId
				and b.trainingrequirementname = d.DocID
				and rg.TrainingRequirementGroupName = cte.TGName
	END
END
