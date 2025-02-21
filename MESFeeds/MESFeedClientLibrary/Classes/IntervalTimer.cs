using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.Classes
{
    public class IntervalTimer : IIntervalTrigger
    {
        private Timer Timer;

        public event ElapsedEventHandler IntervalReached;

        public IntervalTimer(double milliseconds)
        {
            Timer = new Timer(milliseconds);
            Timer.Elapsed += Reached;
        }
        
        private void Reached(object sender, ElapsedEventArgs e)
        {
            //invoke unassigned delegate - the invocation list of IntervalReached is empty
            IntervalReached(sender, e);
        }
        public void Dispose()
        {
            Timer.Elapsed -= Reached;
            Timer.Dispose();
        }

        public void Start()
        {
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
        }
    }
}
