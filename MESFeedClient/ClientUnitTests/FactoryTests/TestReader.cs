using MESFeedClientLibrary.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MESFeedClientLibrary.Interfaces;

namespace ClientUnitTests.FactoryTests
{
    public class TestReader : IMessageReader
    {
        private readonly string connection;
        private readonly int milSec;
        private readonly int retries;

        public TestReader(string connection, int milSec, int retries)
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                throw new ArgumentException("message", nameof(connection));
            }

            this.connection = connection;
            this.milSec = milSec;
            this.retries = retries;
        }


        public async Task<IEnumerable<IMessage>> GetRecordsAsync()
        {
            return await Task.Run(() => new List<IMessage>());
        }


    }
}
