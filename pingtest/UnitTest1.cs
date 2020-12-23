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
        
        [TestMethod]
        public void Ping_Known_Good_Address()
        {
            string host = "dns.google.com";
            int port = 53;
            bool expectedResult = true;
            bool returnedResult;

            PingSpectrumFiveG sp = new PingSpectrumFiveG();
            returnedResult = sp.PingServer(host, port);
            Assert.IsTrue(returnedResult == expectedResult, "ping failed");
            if( returnedResult == true)
            {
                this.LogTest.Info("Successfully ping'd host: {0}", host);
                this.LogTest.Info("At port: {0}", port);
            }

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
