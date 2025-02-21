using MESFeedClientLibrary.FeedManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightlyTrainingRecordFeedClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IFeedManager manager = new MESFeedClientLibrary.Factory.NightlyTrainingFeedManagerFactory().Create();
            manager.Start();
        }
    }
}
