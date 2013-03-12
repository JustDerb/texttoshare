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
            Logger logger = new Logger();
            Assert.IsTrue(logger.LogMessage("TEST_LOG_MESSAGE", 0));
        }

        [TestCleanup]
        public void Teardown()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "DELETE FROM eventlog WHERE message = @message AND level = @level";
                query.Parameters.AddWithValue("@message", "TEST_LOG_MESSAGE");
                query.Parameters.AddWithValue("@level", 0);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
