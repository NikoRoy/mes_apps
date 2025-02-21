
using MESFeedClientLibrary.Factory;
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
    class TestFeedManagerFactory : IFeedManagerFactory
    {
        public MESFeedClientLibrary.FeedManagers.IFeedManager Create()
        {
            IMessageReader reader = new TestReader("connection string", 10000, 3);
            IMessageUpdater updater = new TestUpdater();
            IFeedManager bfm = new TestFeedManager(reader, updater);

            return bfm;
        }


    }
}
