using System;
using MESFeedClientLibrary.Activity;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;

namespace MESFeedClientLibrary.Logger
{
    public class LoggerBuilder
    {
        private readonly Logger _logger;

        public LoggerBuilder()
        {
            _logger = new Logger();
        }
        public LoggerBuilder AttachMessageWriter(IAlertHandler emailAlertHandler)
        {
            this._logger.AttachMessageWriter(emailAlertHandler);
            return this;
        }
        public LoggerBuilder AttachErrorWriter(IErrorHandler eh)
        {
            this._logger.AttachErrorWriter(eh);
            return this;
        }
        public LoggerBuilder AttachActivityWriter(IActivity al)
        {
            this._logger.AttachActivityWriter(al);
            return this;
        }
        public ILogger Build()
        {
            return _logger;
        }
    }
}