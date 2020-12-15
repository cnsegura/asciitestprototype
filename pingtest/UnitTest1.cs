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
            string host = "1.1.1.1";
            bool expectedResult = true;
            bool returnedResult;

            SpectrumFiveG sp = new SpectrumFiveG();
            returnedResult = sp.PingServer(host);

            Assert.IsTrue(returnedResult == expectedResult, "ping succeeded");

        }

        [TestMethod]
        public void Ping_Bad_Address()
        {
            string host = "0.0.0.0";
            bool expectedResult = true;
            bool returnedResult;

            SpectrumFiveG sp = new SpectrumFiveG();
            returnedResult = sp.PingServer(host);

            //should throw exception... 
            Assert.IsTrue(returnedResult == expectedResult, "ping succeeded");
        }
    }
}
