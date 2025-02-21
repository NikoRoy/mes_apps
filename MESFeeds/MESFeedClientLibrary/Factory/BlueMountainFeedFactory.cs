using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Logger;
using MESFeedClientLibrary.Reader;
using MESFeedClientLibrary.Updater;

namespace MESFeedClientLibrary.Factory
{
    public class BlueMountainFeedFactory : IFeedFactory
    {
        private IQuery _query;
        private IXmlProcessor _processor;
        private ILogger _logger;

        public BlueMountainFeedFactory(IQuery query, IXmlProcessor processor, ILogger logger)
        {
            this._query = query;
            this._processor = processor;
            this._logger = logger;
        }

        public IMessageReader CreateReader()
        {
            return new BlueMountainReader(_query, _logger);
        }

        public IMessageUpdater CreateUpdater()
        {
            return new BlueMountainUpdater(_processor, _logger);
        }
    }
}
