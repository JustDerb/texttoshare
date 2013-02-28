using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t2sBackend;

namespace t2sBackendTest
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void TestLogMessage()
        {
            Logger logger = new Logger();
            Assert.IsTrue(logger.LogMessage("TEST_LOG_MESSAGE", 0));
        }
    }
}
