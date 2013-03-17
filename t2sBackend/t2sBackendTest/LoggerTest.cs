using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using t2sBackend;

namespace t2sBackendTest
{
    [TestClass]
    public class LoggerTest
    {
        private const string _connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\MainDatabase.mdf;Integrated Security=True";

        [TestMethod]
        public void TestLogMessage()
        {
            Assert.IsTrue(Logger.LogMessage("TEST_LOG_MESSAGE", LoggerLevel.DEBUG), "The logger was unable to log the test message.");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestNullMessageThrowsException()
        {
            Logger.LogMessage(null, LoggerLevel.DEBUG);
        }

        [TestCleanup]
        public void Teardown()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
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
