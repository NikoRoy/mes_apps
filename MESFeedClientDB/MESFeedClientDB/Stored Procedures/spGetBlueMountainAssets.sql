CREATE PROCEDURE [dbo].[spGetBlueMountainAssets]
AS
BEGIN
	with base as (
		-- get assets that havent been sent to mes yet (Base assets)
		select asset.MessageId,asset.AHAssetID, asset.AHAssetDesc, asset.AHAssetStatus, asset.AHStateName, asset.AHScopeName, asset.[AHLastModified], asset.CreationDate
		from [MESFeedClient].[dbo].[tblServiceBusIntakeDtl] asset 
		left join [MESFeedClient].[dbo].[tblBlueMountainOutbound] otb_existing
			on asset.MessageId = otb_existing.MessageID
		where ((otb_existing.Synced = 0 and otb_existing.SyncAttempt < 3) or otb_existing.Synced is null)
	) 
	,joins as (
		select base.*
		, sch.M_AEScheduleType,  sch.[M_EHEVID], sch.[AEDueDate]
		, wo.[M_EH2_UDF15] 
		, ROW_NUMBER() over (partition by sch.messageid, base.ahassetid order by aeduedate, wo.[M_EH2_UDF15], sch.M_AEScheduleType) rn
		from base
		--left join base to get the schedule record rank
		left join [MESFeedClient].[dbo].[tblServiceBusDtlSchedules] sch
			on base.MessageId = sch.messageid
		--left join schedule to work
		left join [MESFeedClient].[dbo].[tblServiceBusDtlWorkOrders] wo
			on sch.messageid = wo.messageid
			and sch.[M_EHEVID] = wo.[M_EHEVID]
			and wo.EHStateName not in (select [StateName]
										from [MESFeedClient].[dbo].[tblStateExclusions] se
										where se.[StateJoinType] = 'Work')
		where sch.aeduedate >= getdate()
	)
	, apply_sched as (
		select joins.*,
			case 
				-- when schedule is null
				when isnull(joins.AeDueDate,'')= ''
				then Cast('12/30/2999' as datetime)
				-- when schedule is not null apply schedule to appropriate date
				else 
					Case  
						when joins.M_AEScheduleType like 'Week%' 
						then [EmployeeTraining].[dbo].[xxatr_fn_addbusinessdays](coalesce(joins.M_EH2_UDF15, AEDueDate) , 3)
						else coalesce(joins.M_EH2_UDF15, AEDueDate)
					End
			end as BufferDate
		from joins 
		where rn = 1
	)
	select apply_sched.MessageId, apply_sched.AHAssetID, AHAssetDesc, AHAssetStatus, AHStateName, min(BufferDate) AENextDueDate , AHLastModified lastModDate
	from apply_sched
	group by apply_sched.MessageId, apply_sched.AHAssetID, AHAssetDesc, AHAssetStatus, AHStateName, AHLastModified, CreationDate
	order by AHAssetID, CreationDate;
END