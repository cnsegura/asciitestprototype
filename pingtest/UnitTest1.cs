using Microsoft.VisualStudio.TestTools.UnitTesting;
using asciitestingNS;

namespace wopr
{
    [TestClass]
    public class connectioncheck
    {
        [TestMethod]
        public void Ping_Known_Good_Address()
        {
            string host = "dns.google.com";
            int port = 53;
            bool expectedResult = true;
            bool returnedResult;

            SpectrumFiveG sp = new SpectrumFiveG();
            returnedResult = sp.PingServer(host, port);

            Assert.IsTrue(returnedResult == expectedResult, "ping succeeded");

        }

        [TestMethod]
        public void Ping_Bad_Address()
        {
            string host = "dns.google.com";
            int port = 80;
            bool expectedResult = true;
            bool returnedResult;

            SpectrumFiveG sp = new SpectrumFiveG();
            returnedResult = sp.PingServer(host, port);

            //should throw exception... 
            Assert.IsTrue(returnedResult == expectedResult, "ping succeeded");
        }
    }
}
