using System;
using System.Threading.Tasks;
using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.Logger
{
    public interface ILogger
    {
        Task LogMessage(string msg, string subject);
        Task LogActivity(IMessage obj, string action, string response);
        Task LogError(Exception ex, string message);

    }
}