using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using t2sBackend;

namespace t2sBackendTest
{
    [TestClass]
    public class PairEntriesTest
    {
        private readonly string _testKeyEntry = "TESTKEY";
        private readonly string _testValueEntry1 = "TESTVALUE1";
        private readonly string _testValueEntry2 = "TESTVALUE2";

        [TestCategory("PairEntries")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SettingNullKeyThrowsException()
        {
            SqlController.SetPairEntryValue(null, _testValueEntry1);
        }

        [TestCategory("PairEntries")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SettingNullValueThrowsException()
        {
            SqlController.SetPairEntryValue(_testKeyEntry, null);
        }

        [TestCategory("PairEntries")]
        [TestMethod]
        [ExpectedException(typeof(CouldNotFindException))]
        public void GettingNonExistantKeyThrowsException()
        {
            SqlController.GetPairEntryValue(_testKeyEntry);
        }

        [TestCategory("PairEntries")]
        [TestMethod]
        public void SettingNewPairReturnsTrue()
        {
            Assert.IsTrue(SqlController.SetPairEntryValue(_testKeyEntry, _testValueEntry1));
        }

        [TestCategory("PairEntries")]
        [TestMethod]
        public void GettingPairAfterSettingReturnsSameValue()
        {
            SqlController.SetPairEntryValue(_testKeyEntry, _testValueEntry1);
            Assert.AreEqual(_testValueEntry1, SqlController.GetPairEntryValue(_testKeyEntry));
        }

        [TestCategory("PairEntries")]
        [TestMethod]
        public void SettingKeyEntryWithNewValueUpdatesValue()
        {
            SqlController.SetPairEntryValue(_testKeyEntry, _testValueEntry1);
            string initialValue = SqlController.GetPairEntryValue(_testKeyEntry);

            SqlController.SetPairEntryValue(_testKeyEntry, _testValueEntry2);
            string updatedValue = SqlController.GetPairEntryValue(_testKeyEntry);

            Assert.AreNotEqual(initialValue, updatedValue, "Test value did not updated as expected.");
        }

        [TestCleanup]
        public void TearDown()
        {
            using (SqlConnection conn = new SqlConnection(SqlController.CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "DELETE FROM pairentries WHERE key_entry = @key_entry";
                query.Parameters.AddWithValue("@key_entry", _testKeyEntry);

                conn.Open();
                query.ExecuteNonQuery();
            }
        }
    }
}
