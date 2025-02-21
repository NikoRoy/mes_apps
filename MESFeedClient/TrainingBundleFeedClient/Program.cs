using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.FeedManagers;
using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDBMESFeeds.Messages;
using TrainingBundleFeedClient.Classes;

namespace TrainingBundleFeedClient
{
    class Program
    {
        private const double MILLISECONDS_PER_MINUTE = 1000 * 60;
        static void Main(string[] args)
        {
            IFeedManager manager = new MESFeedClientLibrary.Factory.BundleFeedManagerFactory().Create();
            manager.Start();
        }
    }
}
