using MESFeedClientLibrary;
using MESFeedClientLibrary.Factory;
using MESFeedClientLibrary.FeedManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Mock
{
    //failed business extension call
    public class TestBFMBLFactory : IFeedManagerFactory
    {
        public IFeedManager Create()
        {
            return new TestBundleFeedManagerBLFail();
        }

    }
    //failed update call
    public class TestBFMUpdateFactory : IFeedManagerFactory
    {
        public IFeedManager Create()
        {
            return new TestBundleFeedManagerUpdateFail();
        }

    }

    public class TestBundleFeedManagerBLFail : IFeedManager
    {
        public int Count { get; set; }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            ProcessFeed();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void CallBusinessLogic()
        {       
            throw new StoredProcedureException("unit test");
        }

        public async Task UpdateAsync()
        {
            await Task.Delay(10);
        }
        private void ProcessFeed()
        {
            try
            {
                CallBusinessLogic();
                Task.WaitAll(UpdateAsync());
            }
            catch (AggregateException ex)
            {
                foreach (var iEx in ex.Flatten().InnerExceptions)
                {
                    Task.Run(() => this.Count++).Wait();
                }
            }
            catch (StoredProcedureException ex)
            {
                throw ex;
                //Task.Run(() => this.Count++).Wait();
            }
            catch (Exception ex)
            {
                Task.Run(() => this.Count++).Wait();
            }
        }
    }

    public class TestBundleFeedManagerUpdateFail : IFeedManager
    {
        public int Count { get; set; }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            ProcessFeed();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void CallBusinessLogic()
        {
            return;
            //throw new StoredProcedureException("unit test");
        }

        public async Task UpdateAsync()
        {
            await Task.Run(() => throw new Exception("The Ado.net reader threw an exception"));
            await Task.Run(() => Task.Delay(10));
        }
        private void ProcessFeed()
        {
            try
            {
                CallBusinessLogic();
                Task.WaitAll(UpdateAsync());
            }
            catch (AggregateException ex)
            {
                throw ex;
            }
            catch (StoredProcedureException ex)
            {
                Task.Run(() => this.Count++).Wait();
            }
            catch (Exception ex)
            {
                Task.Run(() => this.Count++).Wait();
            }
        }
    }

}
