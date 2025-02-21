
using MESFeedClientLibrary.FeedManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Factory
{
    public interface IFeedManagerFactory
    {
        IFeedManager Create();
    }
}
