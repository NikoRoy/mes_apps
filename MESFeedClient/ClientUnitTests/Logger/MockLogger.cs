using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUnitTests
{
    public class MockLogger : IErrorHandler
    {
        public int Count { get; private set; }
        public ILogger Logger;

        public MockLogger()
        {
            var logger = new LoggerBuilder().AttachErrorWriter(this).Build();
            Logger = logger;
            Count = 0;
        }
        public async Task SwallowErrorButCount(Exception ex, string s)
        {
            //ignore and increment
            await Task.Factory.StartNew(() => Count += 1);
        }

        public async Task LogError(Exception ex, string message)
        {
            await Task.Factory.StartNew(() => Count += 1);
        }
    }
}
