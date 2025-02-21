using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using MESFeedClientLibrary.Classes;
using MESFeedClientLibrary.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientUnitTests.ExceptionTests
{
    [TestClass]
    public class TestErrorHandlers
    {
        [TestMethod]
        public async Task TestOracleErrorHandler()
        {
            //arrange
            TestEmailAlertHandler oraAlert = new TestEmailAlertHandler("FromEmail", "Nicholas.Roy@getinge.com", "SMTP");
            TestOracleErrorHandler oraHandler = new TestOracleErrorHandler("C:\\Workspace\\Logs", oraAlert);
            //act
            await oraHandler.LogError(new Exception("Test-Exception"), "TestMessage");
            //assert
            Assert.AreEqual(oraAlert.testfinal, string.Concat("TestMessage", "TestSubject"));
            Assert.AreEqual(oraHandler.Thrown.Message, "Test-Exception");
        }
        [TestMethod]
        public async Task TestBadNodeXML()
        {
            string x = @"<Message>
                    <TransactionType><![CDATA[RemovalUpload]]></TransactionType>
                    <TransactionId><![CDATA[76e510f2 - e6b0 - 445b - bd9b - 4745ca3d65b6]]></TransactionId>
                    <TransactionDateTime><![CDATA[2021 - 06 - 21T12: 08:52.000]]></TransactionDateTime>
                            <WorkOrderName><![CDATA[12345678]]></WorkOrderName>
                            <badnode>
                                <Detail>
                                    <RouteStepName ><![CDATA[GDN_ERPStep_1]]></RouteStepName>
                                    <ProductName><![CDATA[GDN_Component_2]]></ProductName>
                                    <LotNumRemoveToInv><![CDATA[Cont2]]></LotNumRemoveToInv>
                                    <InvLocToRemoveTo><![CDATA[FL]]></InvLocToRemoveTo>
                                    <QtyToRemove><![CDATA[13]]></QtyToRemove>
                                    <ManufacturingProcedure><![CDATA[MP000005]]></ManufacturingProcedure>
                                    <LossReason><![CDATA[Glue dot too big]]></LossReason>
                                </Detail>
                            </badnode>
                        </Message>";

            var xx = XElement.Parse(x);

            TestEmailAlertHandler oraAlert = new TestEmailAlertHandler("FromEmail", "Nicholas.Roy@getinge.com", "SMTP");
            TestOracleErrorHandler oraHandler = new TestOracleErrorHandler("C:\\Workspace\\Logs", oraAlert);


            await oraHandler.RunStubbedLogic(new Exception("Test-Exception"), x);

            Assert.AreEqual(oraHandler.Parsed.ToString(), xx.ToString());

        }
    }
}
