using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using asciitestingNS;
using NLog;

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
            this.Info("START: {0} starts at {1}.", testTitle, this.startTestTime);
        }
        public void LogTestEnding(string testTitle)
        {
            var endTestTime = DateTime.Now;
            var timeInSec = (endTestTime - this.startTestTime).TotalMilliseconds / 1000d;
            this.Info("END: {0} ends at {1} after {2} sec.", testTitle, endTestTime, timeInSec.ToString("##,###"));
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
            this.LogTestStarting(TestContext.TestName);
        }

        [TestCleanup]
        public void AfterTest()
        {
            this.LogTest.LogTestEnding(TestContext.TestName);
            this.TestContext.AddResultFile("./file.txt");
        }
        #endregion
    }

}
