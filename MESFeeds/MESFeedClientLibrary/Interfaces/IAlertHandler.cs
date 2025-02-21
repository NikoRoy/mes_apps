using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Interfaces
{
    public interface IAlertHandler
    {
        Task SendAlert(string message, string subject);
    }
}
