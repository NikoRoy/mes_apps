using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MESFeedClientEFModel;
using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.BlueMountaion;

namespace MESFeedClientLibrary.Activity
{
    public class ServiceBusActivityDetail : IActivityLogger<MessageAdapter>
    {
        public async Task LogActivity(MessageAdapter obj, string message, string response)
        {
            using (var context = EntityFactory.GenerateContext())
            {
                if (obj._body.Schedule.ScheduleList.Count > 0)
                {
                    foreach (var item in obj._body.Schedule.ScheduleList)
                    {
                        var i = new tblServiceBusDtlSchedule()
                        {
                            MessageId = obj._received.MessageId,
                            AEDueDate = item.AEDueDate,
                            M_AEScheduleType = item.ScheduleType,
                            M_EHEVID = item.EventID,
                            M_EVRTName = item.EventType,
                            SchedRowID = item.RowID,
                            CreationDate = DateTime.Now
                            //UpdateDate = DateTime.Now
                        };
                        context.tblServiceBusDtlSchedules.Add(i);
                    }
                }
                if (obj._body.History.WorkOrders.Count > 0)
                {
                    foreach (var item in obj._body.History.WorkOrders)
                    {
                        var i = new tblServiceBusDtlWorkOrder()
                        {
                            MessageId = obj._received.MessageId,
                            WorkOrderRowID = item.RowID,
                            M_EHRTName = item.M_EHRTName,
                            M_EHHistoryID = item.M_EHHistoryID,
                            M_EHEVID = item.M_EHEVID,
                            EHStateName = item.EHStateName,
                            M_EHDueDate = item.EHDueDate,
                            EHLastModified = item.EHLastModifiedDate,
                            M_EH2_UDF15 = item.M_EH2_UDF15,
                            CreationDate = DateTime.Now
                            //UpdateDate = DateTime.Now
                        };
                        context.tblServiceBusDtlWorkOrders.Add(i);
                    }
                }

                var asset = new tblServiceBusIntakeDtl()
                {
                    MessageId = obj._received.MessageId,
                    AHAssetID = obj._body.Equipment.AssetID,
                    AHAssetDesc = obj._body.Equipment.AssetDescription,
                    AHAssetStatus = obj._body.Equipment.AssetStatus,
                    AHScopeName = obj._body.Equipment.AssetScope,
                    AHStateName = obj._body.Equipment.AssetState,
                    AHLastModified = obj._body.Equipment.AssetLastModified,
                    CreationDate = DateTime.Now,
                    UpdateAction = obj._body.Action
                };
                context.tblServiceBusIntakeDtls.Add(asset);

                await context.SaveChangesAsync();
            }
        }
    }
}
