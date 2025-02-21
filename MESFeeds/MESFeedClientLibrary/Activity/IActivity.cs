using MESFeedClientLibrary.Interfaces;
using System.Threading.Tasks;


namespace MESFeedClientLibrary.Activity
{
    public interface IActivity
    {
        Task LogActivity(IMessage obj,string action, string response);
    }
}