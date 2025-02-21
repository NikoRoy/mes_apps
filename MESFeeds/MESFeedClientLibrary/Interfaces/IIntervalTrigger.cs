using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MESFeedClientLibrary.Interfaces
{
    public interface IIntervalTrigger : IDisposable
    {
        void Start();
        void Stop();

        event ElapsedEventHandler IntervalReached;
    }
}
