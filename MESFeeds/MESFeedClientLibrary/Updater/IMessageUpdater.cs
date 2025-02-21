using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MESFeedClientLibrary.Updater
{
    // TO DO: rescope IMessage and IQuery to be higher level than TDBMESFEED dll.
    public interface IMessageUpdater
    {
        Task UpdateAsync(IEnumerable<IMessage> obj, DateTime date = default(DateTime));

        bool WasUpdateSuccessful(string response);
    }
}
