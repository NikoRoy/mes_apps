using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;

namespace MESFeedClientLibrary.Updater
{
    public class BlueMountainUpdater : IMessageUpdater
    {
        private IXmlProcessor _xmlProcessor;
        private ILogger _logger;

        public BlueMountainUpdater(IXmlProcessor xmlProcessor, ILogger logger)
        {
            this._xmlProcessor = xmlProcessor;
            this._logger = logger;
        }
        public async Task UpdateAsync(IEnumerable<IMessage> obj, DateTime date = default(DateTime))
        {
            if (obj.Count() <= 0) return;
            bool atleastOneError = false;

            foreach (var item in obj)
            {
                try
                {
                    var response = await _xmlProcessor.Execute(item.ToXml());

                    await _logger.LogActivity(item, "Post Request", response);
                    if (!WasUpdateSuccessful(response))
                    {
                        atleastOneError = true;
                        await this._logger.LogError(new Exception(response), "Blue Mountain Feed Was Unsuccessful");
                    }
                }
                catch (Exception ex)
                {
                    atleastOneError = true;
                    await this._logger.LogError(ex, "Error Processing Blue Mountain Asset");
                }
            }

            if (!atleastOneError)
            {
                try
                {
                    await BusinessExtensions.UpdateControlTime(date, InterfaceTypes.BlueMountain);
                }
                catch (Exception ex)
                {
                    await this._logger.LogError(ex, "Error Updating Blue Mountain Feed Control Date");
                }
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
