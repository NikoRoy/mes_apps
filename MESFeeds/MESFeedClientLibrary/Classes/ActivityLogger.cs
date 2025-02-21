using MESFeedClientLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Classes
{
    public abstract class ActivityLogger<T> : IActivityLogger<T>
    {
        private readonly string ConnectionString;
        private readonly Interfaces.IErrorHandler ErrorHandler;
        public ActivityLogger(string connectionString, Interfaces.IErrorHandler errorHandler)
        {
            this.ConnectionString = connectionString;
            this.ErrorHandler = errorHandler;
        }

        public abstract Task LogActivity(T obj, string action, string response);
    }
}
