using System.Threading.Tasks;
using MESFeedClientLibrary.Activity;
using MESFeedClientLibrary.Interfaces;

namespace UnitTests.TestClasses
{
    internal class FakeIntakeActivityAdapter<T> : IActivity
    {
        private IActivityLogger<T> serviceBusActivity;
        public T obj;
        public string act;
        public string res;



        public FakeIntakeActivityAdapter(IActivityLogger<T> serviceBusActivity)
        {
            this.serviceBusActivity = serviceBusActivity;
        }

        public async Task LogActivity(IMessage obj, string action, string response)
        {
            this.obj = (T)obj;
            this.act = action;
            this.res = response;
            await Task.CompletedTask;
            //throw new System.NotImplementedException();
        }
    }
}