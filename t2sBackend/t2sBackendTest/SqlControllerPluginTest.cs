using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Data.SqlClient;
using System.Text;
using t2sBackend;
using t2sDbLibrary;

namespace t2sBackendTest
{
    [TestClass]
    public class SqlControllerPluginTest
    {
        private SqlController _controller;
        private UserDAO _owner;
        private PluginDAO _plugin1;
        private PluginDAO _plugin2;
        private PluginDAO _nullPlugin;

        [TestInitialize]
        public void Setup()
        {
            _controller = new SqlController();

            _owner = new UserDAO()
            {
                UserName = "TESTUSER1",
                FirstName = "TEST",
                LastName = "USER",
                PhoneNumber = "1111111111",
                PhoneEmail = "1111111111@test.com",
                Carrier = PhoneCarrier.Verizon,
                UserLevel = UserLevel.User,
                IsBanned = false,
                IsSuppressed = false
            };
            _controller.CreateUser(_owner, "password");

            _plugin1 = new PluginDAO()
            {
                Name = "TEST1",
                Description = "A test plugin",
                HelpText = "A simple test plugin",
                IsDisabled = false,
                VersionNum = "1",
                Access = PluginAccess.STANDARD,
                OwnerID = (int) _owner.UserID
            };

            _plugin2 = new PluginDAO()
            {
                Name = "TEST2",
                Description = "A test plugin 2",
                HelpText = "A simple test plugin 2",
                IsDisabled = false,
                VersionNum = "1",
                Access = PluginAccess.STANDARD,
                OwnerID = (int)_owner.UserID
            };

            _nullPlugin = new PluginDAO()
            {
                Name = null,
                Description = null,
                HelpText = null,
                IsDisabled = false,
                VersionNum = null,
                Access = PluginAccess.STANDARD,
                OwnerID = (int)_owner.UserID
            };
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateNullPluginThrowsException()
        {
            _controller.CreatePlugin(null);
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatePluginNullPropertiesThrowsException()
        {
            _controller.CreatePlugin(_nullPlugin);
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void CreatingPluginWithNonExistantOwnerViolatesConstraintsAndThrowsException()
        {
            _controller.DeleteUser(_owner);
            _controller.CreatePlugin(_plugin1);
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void CreatesPluginSuccessfully()
        {
            _controller.CreatePlugin(_plugin1);
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        [ExpectedException(typeof(EntryAlreadyExistsException))]
        public void CallingCreatePluginOnSamePluginTwiceThrowsException()
        {
            _controller.CreatePlugin(_plugin1);
            _controller.CreatePlugin(_plugin1);
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        [ExpectedException(typeof(EntryAlreadyExistsException))]
        public void CreatingDuplicatePluginsThrowsException()
        {
            _controller.CreatePlugin(_plugin1);
            _plugin1.PluginID = null;
            _controller.CreatePlugin(_plugin1);
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void CreateDuplicateUsersBypassUniquenessCheckViolatesSqlConstraintAndThrowsException()
        {
            SqlController stubbedController = MockRepository.GenerateStub<SqlController>();

            stubbedController.Stub(x => x.PluginExists(_plugin1.Name)).Return(false);

            stubbedController.CreatePlugin(_plugin1);
            stubbedController.CreatePlugin(_plugin1);
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RetrieveNullPluginThrowsException()
        {
            _controller.RetrievePlugin("");
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        [ExpectedException(typeof(CouldNotFindException))]
        public void RetrieveNonExistantPluginThrowsException()
        {
            _controller.RetrievePlugin(_plugin1.Name);
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void CreateAndRetrieveShouldReturnSamePlugin()
        {
            _controller.CreatePlugin(_plugin1);
            PluginDAO retPluginDAO = _controller.RetrievePlugin(_plugin1.Name);

            Assert.AreEqual(_plugin1.PluginID, retPluginDAO.PluginID, "PluginIDs are not equal");
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void CreateAndRetrieveMultiplePluginsShouldReturnSamePlugins()
        {
            _controller.CreatePlugin(_plugin1);
            _controller.CreatePlugin(_plugin2);
            PluginDAO p1 = _controller.RetrievePlugin(_plugin1.Name);
            PluginDAO p2 = _controller.RetrievePlugin(_plugin2.Name);

            Assert.AreEqual(_plugin1.PluginID, p1.PluginID);
            Assert.AreEqual(_plugin2.PluginID, p2.PluginID);
            Assert.AreNotEqual(_plugin1.PluginID, _plugin2.PluginID);
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateNullPluginShouldThrowException()
        {
            _controller.UpdatePlugin(null);
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void UpdateNonExistingPluginShouldNotChangeDatabase()
        {
            _plugin1.PluginID = -1;
            Assert.IsFalse(_controller.UpdatePlugin(_plugin1));
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void UpdatePluginWithSameInfoShouldReturnSamePlugin()
        {
            _controller.CreatePlugin(_plugin1);
            _controller.UpdatePlugin(_plugin1);
            _plugin2 = _controller.RetrievePlugin(_plugin1.Name);

            Assert.AreEqual(_plugin1, _plugin2, "Updating plugin with same value did not return same information.");
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteNullPluginShouldThrowException()
        {
            _controller.DeletePlugin(null);
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void DeleteNonExistingPluginShouldNotChangeDatabase()
        {
            _plugin1.PluginID = -1;
            Assert.IsFalse(_controller.DeletePlugin(_plugin1), "There was a plugin with an invalid ID already in the database.");
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void DeletePluginShouldUpdateDatabase()
        {
            _controller.CreatePlugin(_plugin1);
            Assert.IsTrue(_controller.DeletePlugin(_plugin1), "Test plugin was not deleted from the database.");
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void DeleteMultiplePluginsShouldUpdateDatabase()
        {
            _controller.CreatePlugin(_plugin1);
            _controller.CreatePlugin(_plugin2);

            int count = 2;
            _controller.DeletePlugin(_plugin1);
            if (!_controller.PluginExists(_plugin1.Name)) --count;

            _controller.DeletePlugin(_plugin2);
            if (!_controller.PluginExists(_plugin2.Name)) --count;

            Assert.AreEqual(0, count, "Not all test plugins were deleted from the database.");
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void GetInitialFailedAttemptCountIsZero()
        {
            _controller.CreatePlugin(_plugin1);
            Assert.AreEqual(0, _controller.GetPluginFailedAttemptCount(_plugin1.PluginID));
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void IncrementingNewPluginFailedAttemptCountSetsToOne()
        {
            _controller.CreatePlugin(_plugin1);
            _controller.IncrementPluginFailedAttemptCount(_plugin1.PluginID);
            Assert.AreEqual(1, _controller.GetPluginFailedAttemptCount(_plugin1.PluginID));
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void MultiplePluginFailedAttemptIncrementsSuccessfullyAddOne()
        {
            _controller.CreatePlugin(_plugin1);
            _controller.IncrementPluginFailedAttemptCount(_plugin1.PluginID);
            Assert.AreEqual(1, _controller.GetPluginFailedAttemptCount(_plugin1.PluginID));

            _controller.IncrementPluginFailedAttemptCount(_plugin1.PluginID);
            Assert.AreEqual(2, _controller.GetPluginFailedAttemptCount(_plugin1.PluginID));

            _controller.IncrementPluginFailedAttemptCount(_plugin1.PluginID);
            Assert.AreEqual(3, _controller.GetPluginFailedAttemptCount(_plugin1.PluginID));
        }

        [TestCategory("SqlController.Plugin")]
        [TestMethod]
        public void ResettingPluginFailedAttemptAfterIncrementingResetsToZero()
        {
            _controller.CreatePlugin(_plugin1);
            _controller.IncrementPluginFailedAttemptCount(_plugin1.PluginID);
            Assert.AreEqual(1, _controller.GetPluginFailedAttemptCount(_plugin1.PluginID));

            _controller.ResetPluginFailedAttemptCount(_plugin1.PluginID);
            Assert.AreEqual(0, _controller.GetPluginFailedAttemptCount(_plugin1.PluginID));
        }

        [TestCleanup]
        public void TearDown()
        {
            using (SqlConnection conn = new SqlConnection(SqlController.CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("DELETE FROM plugins WHERE name LIKE @name ");
                queryBuilder.Append("; DELETE FROM users WHERE email_phone LIKE @email_phone ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@name", "TEST%");
                query.Parameters.AddWithValue("@email_phone", "1111111111@test.com");

                conn.Open();
                query.ExecuteNonQuery();
            }
        }
    }
}
