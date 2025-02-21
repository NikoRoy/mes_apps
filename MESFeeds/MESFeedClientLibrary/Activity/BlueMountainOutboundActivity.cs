using System;
using System.Linq;
using System.Threading.Tasks;
using MESFeedClientEFModel;
using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.Activity
{
    public class BlueMountainOutboundActivity : IActivity
    {
        public async Task LogActivity(IMessage obj, string messageid, string response)
        {

            using (var context = EntityFactory.GenerateContext())
            {
                var j = context.tblBlueMountainOutbounds.Where(m => m.MessageID == messageid).FirstOrDefault();

                if (j is null)
                {
                    var i = new tblBlueMountainOutbound()
                    {
                        MessageID = messageid,
                        Response = response,
                        Synced = false,
                        SyncAttempt = 0,
                        CreationDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Xml = obj.ToXml()
                    };
                    context.tblBlueMountainOutbounds.Add(i);
                }
                else
                {
                    j.UpdateDate = DateTime.Now;
                    j.Xml += obj.ToXml();
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
