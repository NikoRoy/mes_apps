using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Interfaces
{
    public interface IFeedManager<T> : IDisposable
    {
        void Start();
        void Stop();

        Task UpdateAsync(IFeedReader<T> reader, IUpdater<T> updater); 
    }
}
