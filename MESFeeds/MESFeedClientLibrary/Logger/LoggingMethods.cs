using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESFeedClientLibrary.Logger
{
    public static class LoggingMethods
    {
        public enum LogTypes
        {
            TrainingRecord,
            TrainingBundle,
            TrainingDoc,
            OracleWorkOrder,
            OracleItem,
            OracleInventory
        }
        public static async Task LogToConsole(string msg)
        {
            await Console.Out.WriteLineAsync(msg);
        }

        public static string FormatExceptionMessage(Exception ex)
        {
            string message = $"Message: {ex.Message} StackTrace: {ex.StackTrace}";
            if (ex.InnerException != null)
            {
                message = message + $"Inner Exception: {FormatExceptionMessage(ex.InnerException)}";
            }
            return message;
        }
    }
}
