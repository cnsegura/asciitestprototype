using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using asciitestingNS;
using wopr.common;

namespace wopr
{
    [TestClass]
    public class SystemCheckTests : TestCommon
    {
        
        //public TestContext TestContext { get; set; }

        [TestMethod]
        public void Ping_Known_Good_Address()
        {
            string host = "dns.google.com";
            int port = 53;
            bool expectedResult = true;
            bool returnedResult;

            PingSpectrumFiveG sp = new PingSpectrumFiveG();
            returnedResult = sp.PingServer(host, port);
            this.LogTest.Info("I'm here");
            Assert.IsTrue(returnedResult == expectedResult, "ping failed");

        }

        //[TestMethod]
        //public void Ping_Bad_Address()
        //{
        //    string host = "dns.google.com";
        //    int port = 80;
        //    bool expectedResult = true;
        //    bool returnedResult;

        //    PingSpectrumFiveG sp = new PingSpectrumFiveG();
        //    returnedResult = sp.PingServer(host, port);

        //    //should throw exception... 
        //    Assert.IsTrue(returnedResult == expectedResult, "ping wrong address");
        //}
    }
}
