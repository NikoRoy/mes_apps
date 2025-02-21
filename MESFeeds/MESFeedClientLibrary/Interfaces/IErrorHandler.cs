using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Interfaces
{
    public interface IErrorHandler
    {
        Task LogError(Exception ex, string message);
    }
}
