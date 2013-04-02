using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using t2sBackend;
using t2sDbLibrary;

namespace t2sBackendTest
{
    [TestClass]
    public class PairEntriesTest
    {
        private readonly string _testKeyEntry = "TESTKEY";
        private readonly string _testValueEntry1 = "TESTVALUE1";
        private readonly string _testValueEntry2 = "TESTVALUE2";

        private SqlController _controller;

        [TestInitialize]
        public void Setup()
        {
            _controller = new SqlController();
        }

        [TestCategory("PairEntries")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SettingNullKeyThrowsException()
        {
            _controller.SetPairEntryValue(null, _testValueEntry1);
        }

        [TestCategory("PairEntries")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SettingNullValueThrowsException()
        {
            _controller.SetPairEntryValue(_testKeyEntry, null);
        }

        [TestCategory("PairEntries")]
        [TestMethod]
        [ExpectedException(typeof(CouldNotFindException))]
        public void GettingNonExistantKeyThrowsException()
        {
            _controller.GetPairEntryValue(_testKeyEntry);
        }

        [TestCategory("PairEntries")]
        [TestMethod]
        public void SettingNewPairReturnsTrue()
        {
            Assert.IsTrue(_controller.SetPairEntryValue(_testKeyEntry, _testValueEntry1));
        }

        [TestCategory("PairEntries")]
        [TestMethod]
        public void GettingPairAfterSettingReturnsSameValue()
        {
            _controller.SetPairEntryValue(_testKeyEntry, _testValueEntry1);
            Assert.AreEqual(_testValueEntry1, _controller.GetPairEntryValue(_testKeyEntry));
        }

        [TestCategory("PairEntries")]
        [TestMethod]
        public void SettingKeyEntryWithNewValueUpdatesValue()
        {
            _controller.SetPairEntryValue(_testKeyEntry, _testValueEntry1);
            string initialValue = _controller.GetPairEntryValue(_testKeyEntry);

            _controller.SetPairEntryValue(_testKeyEntry, _testValueEntry2);
            string updatedValue = _controller.GetPairEntryValue(_testKeyEntry);

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
