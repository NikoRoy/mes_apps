using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Interfaces
{
    public interface IActivityLogger<T>
    {
        Task LogActivity(T obj, string action, string response);
    }
}
