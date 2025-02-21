using MESFeedClientLibrary.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Reader
{
    // TO DO: rescope IMessage and IQuery to be higher level than TDBMESFEED dll.
    public interface IMessageReader
    {
        Task<IEnumerable<IMessage>> GetRecordsAsync();
    }
}