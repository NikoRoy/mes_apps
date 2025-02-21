using MESFeedClientLibrary.Updater;
using MESFeedClientLibrary.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.FeedManagers
{
    public interface IFeedManager : IDisposable
    {
        void Start();
        void Stop();

        Task UpdateAsync();
    }
}
