using MESFeedClientEFModel;
using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MESFeedClientLibrary.Activity
{
    public class BundleActivity : IActivity
    {
        public async Task LogActivity(IMessage obj,string action, string response)
        {
            using (var context = EntityFactory.GenerateContext())
            {
                XElement x = XElement.Parse(obj.ToXml());
                var id = x.Element("TransactionID");
                var dt = x.Element("TransactionDateTime");
                var tt = x.Element("TransactionType");

                var i = new tblBundleFeedLog()
                {
                    Action = action,
                    TransactionDate = DateTime.Parse(dt.Value),
                    TransactionID = id.Value,
                    TransactionType = tt.Value,
                    XmlRequest = obj.ToXml(),
                    XmlResponse = response
                };
                context.tblBundleFeedLogs.Add(i);
                await context.SaveChangesAsync();
            }
        }
    }
}
