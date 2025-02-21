using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESFeedClientLibrary.FeedManagers;

namespace MESFeedClientLibrary.Factory
{
    public abstract class FeedManagerFactory 
    {
        protected abstract IFeedManager Create();
        public IFeedManager MakeManager()
        {
            return this.Create();
        }
    }
}
