using MESFeedClientLibrary.Model.BlueMountaion;
using MESFeedClientLibrary.Model.BlueMountaion.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MESFeedClientLibrary.BusinessLayer
{
    public class BlueMountainController
    {
        public static AssetDownload GetAssetDownload (ServiceBusDownload serviceBusDownload)
        {
            //select the minimum schedule date 
            EVSchedule schedule = serviceBusDownload.Schedule.ScheduleList
                                        //.Where(d => d.AEDueDate >= DateTime.Now)
                                        .OrderByDescending(a => a.AEDueDate)
                                        .FirstOrDefault();

            //select necessary dates
            DateTime? dueDate = schedule?.AEDueDate;
            DateTime? approvedExtensions = serviceBusDownload.History.WorkOrders?
                                                .Where(j => j.M_EHEVID == schedule?.EventID)
                                                .Select(i => i.M_EH2_UDF15)
                                                .SingleOrDefault();

            //if extension exists use that date
            if (approvedExtensions != null)
            {
                dueDate = approvedExtensions;
            }

            //if schedule type is weekly, add 3 business days
            if (schedule.ScheduleType == "Weekly")
            {
                dueDate = dueDate.Value.AddDays(3);

                if (dueDate.Value.DayOfWeek == DayOfWeek.Sunday)
                    dueDate = dueDate.Value.AddDays(1);
                else if (dueDate.Value.DayOfWeek == DayOfWeek.Saturday)
                    dueDate = dueDate.Value.AddDays(2);
                    
            }                
            return new AssetDownload(serviceBusDownload.Equipment.AssetID, serviceBusDownload.Equipment.AssetDescription, serviceBusDownload.Equipment.AssetState, dueDate);         
        }


        public static IEnumerable<AssetDownload> GetAssetDownloads()
        {
            //return the Asset download conversion with the message ID from the service bus received message
            using (var ctx = EntityFactory.GenerateContext())
            {
                foreach (var item in ctx.spGetBlueMountainAssets())
                {
                    yield return new AssetDownload(item.MessageId, item.AHAssetID, item.AHAssetDesc, item.AHStateName, item.AENextDueDate);
                } 
            }
        }

        public static async Task SyncDownloadMessage(string messageId)
        {
            using (var ctx = EntityFactory.GenerateContext())
            {
                var outbound = ctx.tblBlueMountainOutbounds.Where(i => i.MessageID == messageId).ToList();
                foreach (var item in outbound)
                {
                    item.SyncAttempt += 1;
                    item.Synced = true;
                }
                //ctx.Entry(outbound).State = System.Data.Entity.EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }
        public static async Task IncrementSyncAttempt(string messageId)
        {
            using (var ctx = EntityFactory.GenerateContext())
            {
                var outbound = ctx.tblBlueMountainOutbounds.Where(i => i.MessageID == messageId).ToList();
                foreach (var item in outbound)
                {
                    item.SyncAttempt += 1;
                }
                //ctx.Entry(outbound).State = System.Data.Entity.EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }

        public static bool WasResponseSuccessful(string response)
        {

            XDocument xmlDoc = XDocument.Parse(response);

            var isresponse = from x in xmlDoc.Root.Elements()
                             where x.Name.LocalName == "IsResponse"
                             select x.Value;

            if (isresponse.SingleOrDefault() == "false")
            {
                return false;
            }


            var content = from x in xmlDoc.Root.Elements()
                          where x.Name.LocalName == "Contents"
                          select x.Value;

            var h = XElement.Parse(content.SingleOrDefault());


            var k = h.DescendantsAndSelf().Where(p => p.Name.LocalName == "ErrorDescription").SingleOrDefault();
            if (k != null)
            {
                return false;
            }

            var g = h.DescendantsAndSelf().Where(p => p.Name.LocalName == "CompletionMsg").SingleOrDefault();
            if (g != null)
            {
                return true;
            }

            return false;

        }

    }
}
