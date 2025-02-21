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
    public class TrainingMessageUpdater : IMessageUpdater
    {
        private readonly IXmlProcessor _xmlProcessor;
        private readonly ILogger _logger;
        private readonly InterfaceTypes type;

        public TrainingMessageUpdater(IXmlProcessor xmlProcessor, ILogger logger, InterfaceTypes type)
        {
            this._xmlProcessor = xmlProcessor;
            this._logger = logger;
            this.type = type;
        }
        public async Task UpdateAsync(IEnumerable<IMessage> obj, DateTime date = default(DateTime))
        {
            if(obj.Count() == 0) { return; }
            try
            {
                foreach (IMessage item in obj)
                {
                    Console.WriteLine(item.ToXml());
                    string response = await _xmlProcessor.Execute(item.ToXml());
                    await _logger.LogActivity(item, "Post", response);
                    bool sync = WasUpdateSuccessful(response);
                    if (!sync)
                    {
                        if (UNumberException.IsUNumberException(response))
                            throw new UNumberException(response);
                        await this._logger.LogError(new Exception(response), "Failure in UpdateAsync");
                        await this._logger.LogMessage(response, "Failure in UpdateAsync");
                    }
                    await SyncBackTrainingRecord(XElement.Parse(item.ToXml()), sync);
                }
            }
            catch (UNumberException)
            {
                //ignore unumber errors
            }
            catch (Exception ex)
            {
                await this._logger.LogError(ex, "Failure in GetRecordsAsync");
                await this._logger.LogMessage(LoggingMethods.FormatExceptionMessage(ex), "Failure in UpdateAsync");
            }
            finally
            {
            }
        }

        public bool WasUpdateSuccessful(string response)
        {
            return !response.Contains("__errorDescription") || !response.Contains("__errorCode") || !response.Contains("__failureContext") || !response.Contains("__exceptionData");
        }
        private async Task SyncBackTrainingRecord(XElement obj, bool sync)
        {
            using (var context = EntityFactory.GenerateContext())
            {
                var ee = obj.Element("EmployeeName");
                var doc = obj.Element("TrainingRequirementName");

                context.spSyncMESQueue(ee.Value, doc.Value, sync);
                await context.SaveChangesAsync();
            }
        }

    }
}
