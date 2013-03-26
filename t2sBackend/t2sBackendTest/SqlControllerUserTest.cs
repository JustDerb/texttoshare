using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t2sBackend;
using Rhino.Mocks;
using System.Data.SqlClient;
using t2sDbLibrary;

namespace t2sBackendTest
{
    [TestClass]
    public class SqlControllerUserTest
    {
        private SqlController _controller;
        private UserDAO _userDAO1;
        private UserDAO _userDAO2;
        private UserDAO _nullUserDAO;

        [TestInitialize]
        public void Setup()
        {
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

            _userDAO2 = new UserDAO()
            {
                UserName = "TESTUSER2",
                FirstName = "TEST",
                LastName = "USER",
                PhoneNumber = "1111111112",
                PhoneEmail = "1111111112@test.com",
                Carrier = PhoneCarrier.Verizon,
                UserLevel = UserLevel.User,
                IsBanned = false,
                IsSuppressed = false
            };

            _nullUserDAO = new UserDAO()
            {
                UserName = null,
                FirstName = null,
                LastName = null,
                PhoneNumber = null,
                PhoneEmail = null,
                Carrier = PhoneCarrier.Verizon,
                UserLevel = UserLevel.User,
                IsBanned = false,
                IsSuppressed = false
            };

            _controller = new SqlController();
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUserNullThrowsException()
        {
            _controller.CreateUser(null, "password");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateNullPasswordThrowsException()
        {
            _controller.CreateUser(_nullUserDAO, null);
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void CreateUserSuccessfully()
        {
            Assert.IsTrue(_controller.CreateUser(_userDAO1, "password"), "User was not inserted into the database.");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(EntryAlreadyExistsException))]
        public void CallingCreateUserOnSameUserTwiceThrowsException()
        {
            _controller.CreateUser(_userDAO1, "password");
            _controller.CreateUser(_userDAO1, "password");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(EntryAlreadyExistsException))]
        public void CreatingDuplicateUsersThrowsException()
        {
            _controller.CreateUser(_userDAO1, "password");
            _userDAO1.UserID = null;
            _controller.CreateUser(_userDAO1, "password");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void CreateDuplicateUsersBypassUniquenessCheckViolatesSqlConstraintAndThrowsException()
        {
            SqlController stubbedController = MockRepository.GenerateStub<SqlController>();

            stubbedController.Stub(x => x.UserExists(_userDAO1.UserName, _userDAO1.PhoneEmail)).Return(false);

            stubbedController.CreateUser(_userDAO1, "password");
            stubbedController.CreateUser(_userDAO1, "password");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RetrieveNullUserShouldThrowException()
        {
            _controller.RetrieveUser("");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(CouldNotFindException))]
        public void RetreiveNonExistingUserShouldThrowException()
        {
            _controller.RetrieveUser(_userDAO1.PhoneEmail);
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void CreateAndRetrieveShouldReturnSameUser()
        {
            _controller.CreateUser(_userDAO1, "password");
            UserDAO retUserDAO = _controller.RetrieveUser(_userDAO1.PhoneEmail);

            Assert.AreEqual(_userDAO1.UserID, retUserDAO.UserID, "UserIDs do not match.");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void CreateAndRetrieveMultipleUsersShouldReturnDifferentUsers()
        {
            _controller.CreateUser(_userDAO1, "password");
            _controller.CreateUser(_userDAO2, "password");
            UserDAO u1 = _controller.RetrieveUser(_userDAO1.PhoneEmail);
            UserDAO u2 = _controller.RetrieveUser(_userDAO2.PhoneEmail);

            Assert.AreEqual(_userDAO1.UserID, u1.UserID);
            Assert.AreEqual(_userDAO2.UserID, u2.UserID);
            Assert.AreNotEqual(u1.UserID, u2.UserID);
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateNullUserShouldThrowException()
        {
            _controller.UpdateUser(null);
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void UpdateNonExistingUserShouldNotChangeDatabase()
        {
            _userDAO1.UserID = -1;
            Assert.IsFalse(_controller.UpdateUser(_userDAO1));
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void UpdateUserWithSameInfoShouldReturnSameUser()
        {
            _controller.CreateUser(_userDAO1, "password");
            _controller.UpdateUser(_userDAO1);
            _userDAO2 = _controller.RetrieveUser(_userDAO1.PhoneEmail);

            Assert.AreEqual(_userDAO1.UserName, _userDAO2.UserName, "UserNames do not match");
            Assert.AreEqual(_userDAO1.FirstName, _userDAO2.FirstName, "FirstNames do not match.");
            Assert.AreEqual(_userDAO1.LastName, _userDAO2.LastName, "LastNames do not match.");
            Assert.AreEqual(_userDAO1.PhoneNumber, _userDAO2.PhoneNumber, "PhoneNumbers do not match.");
            Assert.AreEqual(_userDAO1.PhoneEmail, _userDAO2.PhoneEmail, "PhoneEmails do not match.");
            Assert.AreEqual(_userDAO1.UserLevel, _userDAO2.UserLevel, "UserLevels do not match.");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteNullUserShouldThrowException()
        {
            _controller.DeleteUser(null);
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void DeleteNonExistingUserShouldNotChangeDatabase()
        {
            _userDAO1.UserID = -1;
            Assert.IsFalse(_controller.DeleteUser(_userDAO1), "There was a user with an invalid ID already in the database.");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void DeleteUserShouldUpdateDatabase()
        {
            _controller.CreateUser(_userDAO1, "password");
            Assert.IsTrue(_controller.DeleteUser(_userDAO1), "Test user was not deleted from the database.");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void DeleteMultipleUsersShouldUpdateDatabase()
        {
            _controller.CreateUser(_userDAO1, "password");
            _controller.CreateUser(_userDAO2, "password");

            int count = 2;
            _controller.DeleteUser(_userDAO1);
            if (!_controller.UserExists(_userDAO1.UserName, _userDAO1.PhoneEmail)) --count;
            
            _controller.DeleteUser(_userDAO1);
            if (!_controller.UserExists(_userDAO1.UserName, _userDAO1.PhoneEmail)) --count;

            Assert.AreEqual(0, count, "Not all test users were deleted from the database.");
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
