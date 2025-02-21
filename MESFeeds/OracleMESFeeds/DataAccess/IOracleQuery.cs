using OracleMESFeeds.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleMESFeeds.DataAccess
{
    public interface IOracleQuery<IMessage>
    {

        IList<IMessage> GetObject(); 
    }
}
