using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MESFeedClientLibrary.Updater
{
    public class MessageUpdater : IMessageUpdater 
    {
        private readonly IXmlProcessor _xmlProcessor;
        private readonly ILogger _logger;
        private readonly InterfaceTypes type;

        public MessageUpdater(IXmlProcessor xmlProcessor, ILogger logger, InterfaceTypes type)
        {
            this._xmlProcessor = xmlProcessor;
            this._logger = logger;
            this.type = type;
        }

        public async Task UpdateAsync(IEnumerable<IMessage> obj, DateTime date = default(DateTime))
        {
            if (obj.Count() == 0) { return; }
            int errct = 0;
            try
            {
                foreach (IMessage item in obj)
                {
                    //Console.WriteLine(item.ToXml());
                    string response = await _xmlProcessor.Execute(item.ToXml());
                    await _logger.LogActivity(item, "Post", response);
                    if (!WasUpdateSuccessful(response))
                    {
                        errct++;
                        //TO DO: implement logerror  
                        await this._logger.LogError(new Exception(response), "Failure in UpdateAsync");
                        await this._logger.LogMessage(response, "Failure in UpdateAsync");
                    }
                }
            }
            catch (Exception ex)
            {
                errct++;
                await this._logger.LogError(ex, "Failure in UpdateAsync");
                await this._logger.LogMessage(LoggingMethods.FormatExceptionMessage(ex), "Failure in UpdateAsync");
            }
            finally
            {
                if (errct == 0)
                    await BusinessExtensions.UpdateControlTime(date, type);
            }
        }

        public bool WasUpdateSuccessful(string response)
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


            var k = h.DescendantsAndSelf().Where(p => p.Name.LocalName == "__errorDescription").SingleOrDefault();
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
