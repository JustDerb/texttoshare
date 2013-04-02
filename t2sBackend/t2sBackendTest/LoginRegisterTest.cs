using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t2sBackend;
using t2sDbLibrary;
using Rhino.Mocks;

namespace t2sBackendTest
{
    [TestClass]
    public class LoginRegisterTest
    {
        private SqlController _controller;
        private UserDAO _userDAO1;

        [TestInitialize]
        public void Setup()
        {
            _controller = new SqlController();

            _userDAO1 = new UserDAO()
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
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        public void CheckLoginWithNullUsernameReturnsFalse()
        {
            Assert.IsFalse(_controller.CheckLogin(null, "password"));
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        public void CheckLoginWithNullPasswordReturnsFalse()
        {
            Assert.IsFalse(_controller.CheckLogin("TESTUSER1", null));
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        public void CheckLoginOnNonExistingUserReturnsFalse()
        {
            Assert.IsFalse(_controller.CheckLogin("TESTUSER1", "password"));
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterNullUserThrowsException()
        {
            _controller.RegisterUser(null, "password");
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterNullPasswordThrowsException()
        {
            _controller.RegisterUser(_userDAO1, null);
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        public void RegisterNewUserReturnsTrue()
        {
            Assert.IsTrue(_controller.RegisterUser(_userDAO1, "password"));
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        [ExpectedException(typeof(EntryAlreadyExistsException))]
        public void RegisterDuplicateUsersThrowsException()
        {
            _controller.RegisterUser(_userDAO1, "password");
            _controller.RegisterUser(_userDAO1, "password");
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void RegisterDuplicateUsersBypassUniquenessCheckViolatesSqlConstraintAndThrowsException()
        {
            SqlController stubbedController = MockRepository.GenerateStub<SqlController>();

            stubbedController.Stub(x => x.UserExists(_userDAO1.UserName, _userDAO1.PhoneEmail)).Return(false);

            stubbedController.RegisterUser(_userDAO1, "password");
            stubbedController.RegisterUser(_userDAO1, "password");
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        public void CheckBadLoginOnExistingUserReturnsFalse()
        {
            _controller.RegisterUser(_userDAO1, "password");
            Assert.IsFalse(_controller.CheckLogin("TESTUSER1", "passowrd"));
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        public void CheckGoodLoginOnExistingUserReturnsTrue()
        {
            _controller.RegisterUser(_userDAO1, "password");
            Assert.IsTrue(_controller.CheckLogin("TESTUSER1", "password"));
        }

        [TestCleanup]
        public void TearDown()
        {
            using (SqlConnection conn = new SqlConnection(SqlController.CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "DELETE FROM users WHERE email_phone LIKE @email_phone";
                query.Parameters.AddWithValue("@email_phone", "111111111%@test.com");

                conn.Open();
                query.ExecuteNonQuery();
            }
        }
    }
}
