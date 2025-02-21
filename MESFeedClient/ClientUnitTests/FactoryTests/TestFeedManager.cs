using MESFeedClientLibrary.FeedManagers;
using MESFeedClientLibrary.Reader;
using MESFeedClientLibrary.Updater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUnitTests.FactoryTests
{
    class TestFeedManager : IFeedManager
    {
        private IMessageReader reader;
        private IMessageUpdater updater;
        public string testupdatestring = "";

        public TestFeedManager(IMessageReader reader, IMessageUpdater updater)
        {
            this.reader = reader;
            this.updater = updater;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync()
        {
            await Task.Run(() => this.testupdatestring = "UpdatedAsync");
        }
    }
}
