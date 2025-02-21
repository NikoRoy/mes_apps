using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MESFeedClientEFModel;
using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.Activity
{
    public class BlueMountainActivity : IActivity
    {
        public async Task LogActivity(IMessage obj, string action, string response)
        {
            using(var context = EntityFactory.GenerateContext())
            {
                XElement x = XElement.Parse(obj.ToXml());
                var id = x.Element("TransactionID");
                var dt = x.Element("TransactionDateTime");
                var tt = x.Element("TransactionType");

                var i = new tblBlueMountainFeedLog()
                {
                    Action = action,
                    TransactionDate = DateTime.Parse(dt.Value),
                    TransactionID = id.Value,
                    TransactionType = tt.Value,
                    XmlRequest = obj.ToXml(),
                    XmlResponse = response
                };
                context.tblBlueMountainFeedLogs.Add(i);
                await context.SaveChangesAsync();
            }
        }
    }
}
