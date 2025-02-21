using MESFeedClientLibrary.Updater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClientUnitTests.FactoryTests
{
    public class TestUpdater : IMessageUpdater
    {
        public string testingstring = "";

        public async  Task UpdateAsync(IEnumerable<MESFeedClientLibrary.Interfaces.IMessage> obj, DateTime date = default(DateTime))
        {
            await Task.Run(() => this.testingstring = "Updated");
        }

        public bool WasUpdateSuccessful(string response)
        {
            throw new NotImplementedException();
        }
    }
}
