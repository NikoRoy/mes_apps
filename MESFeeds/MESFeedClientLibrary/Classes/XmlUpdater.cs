using MESFeedClientLibrary.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Classes
{
    public enum ApiRoutingAdjustment
    {
        oracleworkorder,
        oracleinventory,
        oracleitem,
        momworkorder,
        mominventory,
        momitem
    }
    public abstract class XmlUpdater<T> : Interfaces.IUpdater<T> 
    {
        private readonly IXmlProcessor Processor;
        private readonly IActivityLogger<T> ActivityLogger;
        private readonly IErrorHandler ErrorHandler;
        public XmlUpdater(IXmlProcessor xmlProcessor, IActivityLogger<T> activityLogger, IErrorHandler errorHandler)
        {
            this.Processor = xmlProcessor;
            this.ActivityLogger = activityLogger;
            this.ErrorHandler = errorHandler;
        }

        public abstract Task UpdateAsync(IEnumerable<T> objlist, DateTime date = default(DateTime));
       
        public virtual bool WasResponseSuccessful(string response)
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
            if(k != null)
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
