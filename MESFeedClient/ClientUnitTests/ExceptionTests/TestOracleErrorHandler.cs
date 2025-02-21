using MESFeedClientLibrary.Interfaces;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClientUnitTests.ExceptionTests
{
    public class TestOracleErrorHandler : IErrorHandler
    {
        private readonly string LogFolder;
        private readonly IAlertHandler AlertHandler;

        public Exception Thrown;
        public XElement Parsed;

        public TestOracleErrorHandler(string logfolder, IAlertHandler ah)
        {
            if (!Directory.Exists(logfolder)) { throw new Exception("Log Folder Not Found"); }
            LogFolder = logfolder;
            AlertHandler = ah;
        }
        public virtual async Task LogError(Exception ex, string message)
        {
            try
            {
                //this should be mocked to a actual database
                //simulated error logged
                //RunStubbedLogic(ex, message);
                this.Thrown = ex;
                await this.AlertHandler.SendAlert(message, "TestSubject");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task RunStubbedLogic(Exception ex, string message)
        {
            try
            {
                //using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString))
               // {

                    var h = XElement.Parse(message);

                    this.Parsed = h;

                    //key field = transaction type 
                    //var j = h.DescendantsAndSelf().Where(p => p.Name.LocalName == "__errorDescription");
                    ////message field = job + error message
                    //var k = message + ": " + FormatExceptionMessage(ex);

                    //return k;

                    //using (OracleCommand cmd = cn.CreateCommand())
                    //{
                    //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //    cmd.CommandText = "xxatr_mes_extracts_pkg.xxatr_insert_mes_msg";

                    //    //cmd.Parameters.Add(new OracleParameter("@msgdatetime", a.Timestamp));
                    //    cmd.Parameters.Add(new OracleParameter("in_key_field", j));
                    //    cmd.Parameters.Add(new OracleParameter("in_msg", k));

                    //    cn.Open();
                    //    await cmd.ExecuteNonQueryAsync();
                    //}
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual string GetLogFile()
        {
            string logFileName = string.Format("OracleMESFeedLog-{0}", DateTime.Today.ToString("yyyy-MM-dd"));
            string fullPath = Path.Combine(this.LogFolder, logFileName);
            return fullPath;
        }

        public virtual string FormatExceptionMessage(Exception ex)
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
