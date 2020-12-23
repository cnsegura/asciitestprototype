using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using System;
using System.IO;

namespace wopr.common
{
    public class TestCommon
    {

        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public TestContext TestContext { get; set; }
        //public string TestTitle { get; set; }
        private TestCommon logTest;
        public TestCommon LogTest
        {
            get
            {
                return this.logTest ?? (this.logTest = new TestCommon());
            }

            set
            {
                this.logTest = value;
            }
        }

        private DateTime startTestTime;

        #region Logger Setup
        public void LogTestStarting(string testTitle)
        {
            this.startTestTime = DateTime.Now;
            this.Info("*************************************************************************************");
            this.Info("TEST: {0} starts at {1}.", testTitle, this.startTestTime);
        }
        public void LogTestEnding(string testTitle, DateTime startTest)
        {
            var endTestTime = DateTime.Now;
            var timeInSec = (endTestTime - startTest).TotalMilliseconds / 1000d;
            this.Info("TEST: {0} ends at {1} after {2} sec.", testTitle, endTestTime, timeInSec.ToString("#0.###"));
            this.Info("*************************************************************************************");
        }
        public void Info(string message, params object[] args)
        {
            this.log.Info(message, args);
        }
        public void Warn(string message, params object[] args)
        {
            this.log.Warn(message, args);
        }
        public void Error(string message, params object[] args)
        {
            this.log.Error(message, args);
        }
        public void LogError(Exception e)
        {
            this.Error("Error occurred: {0}", e);
            throw e;
        }
        #endregion
        #region Before/After test setup
        [TestInitialize]
        public void BeforeTest()
        {
            if (File.Exists(@"./logfile.txt"))
            {
                File.Delete(@"./logfile.txt");
            }
            this.LogTestStarting(TestContext.TestName);
        }

        [TestCleanup]
        public void AfterTest()
        {
            this.LogTest.LogTestEnding(TestContext.TestName, startTestTime);
            this.TestContext.AddResultFile("./logfile.txt");
            NLog.LogManager.Shutdown();
        }
        #endregion
    }

}
