using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using asciitestingNS;
using wopr.common;
using System.Runtime.CompilerServices;

namespace wopr
{
    [TestClass]
    public class SystemCheckTests : TestCommon
    {
        //ClassInitalize and ClassCleanup cannot be inherited from TestCommon
        [ClassInitialize]
        public static void Before_Test_ClassRun(TestContext context)
        {
            File.Delete(@"./logfile.txt");
        }
        [ClassCleanup]
        public static void After_Tests_Run()
        {
            //File.Delete(@"./logfile.txt");
        }

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
        
        [TestMethod]
        public void Download_Speed_Test()
        {
            string dlUrl = @"http://asciiserver.westus2.cloudapp.azure.com:8080/downloads/bigJsonFile.json";
            var dlSpeedtest = DownloadSpeedTest.Download(dlUrl, ".");
            Assert.IsNotNull(dlSpeedtest, "download failed");

            if (dlSpeedtest != null)
            {
                this.LogTest.Info($"Download Size: {dlSpeedtest.Size} bytes");
                this.LogTest.Info($"Time taken: {dlSpeedtest.TimeTaken,6:f} s");
                this.LogTest.Info($"Download speed: {dlSpeedtest.DownloadSpeed,6:f} Mbps");
            }
        }
    }
}
