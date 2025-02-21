
using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Model.BlueMountaion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClientUnitTests.AssetClasses
{
    public class TestAssetXmlUpdater 
    {
        IXmlProcessor Processor = new XmlProcessor("https://usmer-dmesapp01.getingegroup.local:4401/OPEXCRAdapter/Messages?Wait=True");
        public async Task<bool> UpdateAsync(IEnumerable<AssetDownload> obj, DateTime date = default(DateTime))
        {
            return await TestErrorBool(obj);            
        }
        private async Task TestControlTableUpdate(bool b)
        {
            if (b)
            {
                try
                {
                    await TestBusinessExtensions.UpdateBlueMountainControlTime(DateTime.Now);
                }
                catch (Exception ex)
                {
                    throw ex;
                    //await this.ErrorHandler.LogError(ex, "Error Updating Blue Mountain Feed Control Date");
                }

            }
        }
        private async Task<bool> TestErrorBool(IEnumerable<AssetDownload> obj)
        {
            if (obj.Count() <= 0) { return false; }
            bool atleastOneError = false;

            foreach (AssetDownload rec in obj)
            {
                try
                {
                    var response = await Processor.Execute(rec.ToXml());
                    if (!WasResponseSuccessful(response))
                    {
                        atleastOneError = true;
                    }
                }
                catch (Exception ex)
                {
                    atleastOneError = true;
                }
            }
            return atleastOneError;
        }
        private bool WasResponseSuccessful(string response)
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
