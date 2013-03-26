using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using t2sBackend;
using t2sDbLibrary;

namespace t2sBackendTest
{
    [TestClass]
    public class LoggerTest
    {
        [TestCategory("Logger")]
        [TestMethod]
        public void TestLogMessage()
        {
            Assert.IsTrue(Logger.LogMessage("TEST_LOG_MESSAGE", LoggerLevel.DEBUG), "The logger was unable to log the test message.");
        }

        [TestCategory("Logger")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullMessageThrowsException()
        {
            Logger.LogMessage(null, LoggerLevel.DEBUG);
        }

        [TestCategory("Logger")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestEmptyMessageThrowsException()
        {
            Logger.LogMessage("", LoggerLevel.DEBUG);
        }

        [TestCleanup]
        public void Teardown()
        {
            using (SqlConnection conn = new SqlConnection(SqlController.CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "DELETE FROM eventlog WHERE message = 'TEST_LOG_MESSAGE' AND level = @level";
                query.Parameters.AddWithValue("@level", LoggerLevel.DEBUG);

                conn.Open();
                query.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
