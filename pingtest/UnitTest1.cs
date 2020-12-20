using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using asciitestingNS;

namespace wopr
{
    [TestClass]
    public class connectioncheck
    {
        
        public TestContext TestContext { get; set; }
        /*
        [TestInitialize]
        public void Test_Assesembly_Initialize(TestContext context)
        {
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            string _fileName = "./TestResult_" + DateTime.Now.ToString("yyyyMMddTHHmm") + ".txt";
            try
            {
                ostrm = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open file for writing");
                Console.WriteLine(e.Message);
                return;
            }
        }
        */
        [TestMethod]
        public void Ping_Known_Good_Address()
        {
            string host = "dns.google.com";
            int port = 53;
            bool expectedResult = true;
            bool returnedResult;

            //test file attachment
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            string _fileName = "./TestResult_" + DateTime.Now.ToString("yyyyMMddTHHmm") + ".txt";
            try
            {
                ostrm = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open file for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer);
            Console.WriteLine("Test result for : {0}", TestContext.TestName + DateTime.Now.ToString("yyyyMMdd"));
            Console.WriteLine("Everything is awesome");
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            TestContext.AddResultFile(_fileName);

            PingSpectrumFiveG sp = new PingSpectrumFiveG();
            returnedResult = sp.PingServer(host, port);

            Assert.IsTrue(returnedResult == expectedResult, "ping failed");

        }

        [TestMethod]
        public void Ping_Bad_Address()
        {
            string host = "dns.google.com";
            int port = 80;
            bool expectedResult = true;
            bool returnedResult;

            PingSpectrumFiveG sp = new PingSpectrumFiveG();
            returnedResult = sp.PingServer(host, port);

            //should throw exception... 
            Assert.IsTrue(returnedResult == expectedResult, "ping wrong address");
        }

        //[TestCleanup]
        //public void Test_Assembly_Cleanup(TestContext context)
        //{
        //    //Console.SetOut(oldOut);
        //    //writer.Close();
        //    //ostrm.Close();
        //}
    }
}
