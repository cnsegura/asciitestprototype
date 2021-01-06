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
         
        }

        [TestMethod]
        public void Ping_Known_Good_Address()
        {
            string host = "52.156.139.164";
            int port = 8080;

            //PingSpectrumFiveG sp = new PingSpectrumFiveG();
            var sp = PingSpectrumFiveG.PingServer(host, port);
            Assert.IsNotNull(sp, "ping failed");
            if( sp != null)
            {
                this.LogTest.Info("Successfully ping'd host: {0}", host);
                this.LogTest.Info("At port: {0}", port);

                this.LogTest.Info($"Ping result is: {sp.PingSuccess}");
                this.LogTest.Info($"Average response time is: {sp.AveragePingTimeMs} ms");
                this.LogTest.Info($"Max response time is: {sp.MaxPingTimeMs} ms");
                this.LogTest.Info($"Min response time is: {sp.MinPingTimeMs} ms");
                this.LogTest.Info($"Response standard deviation (p) is {sp.PingStandardDeviationS}");
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
