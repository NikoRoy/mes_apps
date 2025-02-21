using MESFeedClientLibrary.BusinessLayer;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MESFeedClientLibrary.FeedManagers
{
    public class BlueMountainManager : IFeedManager
    {
        public Timer Timer;
        private IXmlProcessor processor;
        private ILogger logger;
        private bool _disposed;
        public bool IsProcessing { get; private set; }


        public BlueMountainManager(IXmlProcessor xmlProcessor, ILogger logger)
        {

            this.processor = xmlProcessor;
            this.logger = logger;

            this.Timer = new Timer(300000) { AutoReset = true };
            this.Timer.Elapsed += ElapsedTimerHandler;

            this.IsProcessing = false;
        }

        ~BlueMountainManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this.Timer?.Dispose();
                    this.Timer = null;

                    this.processor.Dispose();
                    this.processor = null;

                    this.logger = null;
                }
            }
            _disposed = true;
        }
        


        public void Start()
        {
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
            int ct = 0;
            while (this.IsProcessing && ct <= 30)
            {
                Task.Delay(1000).Wait();
                ct += 1;
            } 
        }

        public async Task UpdateAsync()
        {
            try
            {
                this.IsProcessing = true;
                //generate list of records to send
                //send the records to mes
                foreach (var item in BlueMountainController.GetAssetDownloads())
                {
                    Console.WriteLine(item.ToXml());
                    var response = await this.processor.Execute(item.ToXml());
                    await this.logger.LogActivity(item, item.MessageID, response);
                    try
                    {
                        if (!BlueMountainController.WasResponseSuccessful(response))
                        {
                            await BlueMountainController.IncrementSyncAttempt(item.MessageID);
                            //await this.logger.LogError(new Exception(response), "Blue Mountain Feed response was unsuccessful");
                        }
                        else
                        {
                            await BlueMountainController.SyncDownloadMessage(item.MessageID);
                        }
                    }
                    catch (Exception ex)
                    {
                        await BlueMountainController.IncrementSyncAttempt(item.MessageID);
                        await this.logger.LogError(ex, "Blue Mountain Feed response was unsuccessful");
                    }                    
                }
            }
            catch (Exception ex)
            {
                await this.logger.LogError(ex, "BlueMountain Update Failure");
                await this.logger.LogMessage(ex.Message, "BlueMountain Update Failure");
            }
            finally
            {
                this.IsProcessing = false;
            }
        }

        private async void ElapsedTimerHandler(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine("Event handled");
            //update accumulated records
            await this.UpdateAsync().ConfigureAwait(false);
            //throw new NotImplementedException();
        }
    }
}
