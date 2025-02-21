using System;
using MESFeedClientLibrary.Interfaces;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ClientUnitTests
{
    [TestClass]
    public class LoggerTest
    {

        [TestMethod]
        public void FormatNonNullTest()
        {
            //Arrange
            var ex = new InvalidOperationException("Outer Exception", new InvalidCastException("Inner Exception"));
            //string path = "C:\\Workspace\\Logs";
            //IAlertHandler ah = new EmailAlertHandler("bi@getinge.com", "nicholas.roy@getinge.com", "mailrelay.getingegroup.local");
            //FileLogger fl = new FileLogger(path, ah);

            //Act
            var form = LoggingMethods.FormatExceptionMessage(ex);
            Console.WriteLine(form);
            //Assert
            Assert.IsNotNull(form);
            
        }
        [TestMethod]
        public async Task SendEmailFormattedTested()
        {
            //Debugger.Launch(); 
            //Arrange
            var ex = new InvalidOperationException("Outer Exception", new InvalidCastException("Inner Exception"));
            string path = "C:\\Workspace\\Logs";
            IAlertHandler ah = new EmailAlertHandler("bi@getinge.com", "nicholas.roy@getinge.com", "mailrelay.getingegroup.local");
            
            //FileLogger fl = new FileLogger( LoggingMethods.LogTypes.TrainingRecord.ToString(),path);
            MockLogger count = new MockLogger();

            //Act
            await count.Logger.LogError(ex, "Unit Test");
            //Assert
            Assert.AreEqual(1, count.Count);
        }
        [TestMethod]
        public void TestSendEmail()
        {
            try
            {
                Debugger.Launch();
                IAlertHandler ah = new EmailAlertHandler("nicholas.roy@getinge.com", "nicholas.roy@getinge.com", "mailrelay.getingegroup.local");
                ah.SendAlert("test", "visual studio test");
            }
            catch (Exception ex)
            {
                Console.WriteLine(LoggingMethods.FormatExceptionMessage(ex));
                throw;
            }
            
        }
    }
}
