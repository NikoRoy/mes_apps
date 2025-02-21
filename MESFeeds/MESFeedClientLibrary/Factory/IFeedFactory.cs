using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.Reader;
using MESFeedClientLibrary.Updater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Factory
{
    public interface IFeedFactory
    {
        IMessageUpdater CreateUpdater();
        IMessageReader CreateReader();
    }
}
