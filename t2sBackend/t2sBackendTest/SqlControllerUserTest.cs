using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t2sBackend;
using Rhino.Mocks;
using System.Data.SqlClient;

namespace t2sBackendTest
{
    [TestClass]
    public class SqlControllerUserTest
    {
        private const string _connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\MainDatabase.mdf;Integrated Security=True";

        private IDBController _controller;
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUserNullThrowsException()
        {
            _controller.CreateUser(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUserWithNullValuesThrowsException()
        {
            _controller.CreateUser(_nullUserDAO);
        }

        [TestMethod]
        public void CreateUserSuccessfully()
        {
            Assert.IsTrue(_controller.CreateUser(_userDAO1), "User was not inserted into the database.");
        }

        [TestMethod]
        [ExpectedException(typeof(EntryAlreadyExistsException))]
        public void CallingCreateUserOnSameUserTwiceThrowsException()
        {
            _controller.CreateUser(_userDAO1);
            _controller.CreateUser(_userDAO1);
        }

        [TestMethod]
        [ExpectedException(typeof(EntryAlreadyExistsException))]
        public void CreatingDuplicateUsersThrowsException()
        {
            _controller.CreateUser(_userDAO1);
            _userDAO1.UserID = null;
            _controller.CreateUser(_userDAO1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RetrieveNullUserShouldThrowException()
        {
            _controller.RetrieveUser(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CouldNotFindException))]
        public void RetreiveNonExistingUserShouldThrowException()
        {
            _controller.RetrieveUser(_userDAO1.PhoneEmail);
        }

        [TestMethod]
        public void CreateAndRetrieveShouldReturnSameUser()
        {
            _controller.CreateUser(_userDAO1);
            UserDAO retUserDAO = _controller.RetrieveUser(_userDAO1.PhoneEmail);

            Assert.AreEqual(_userDAO1.UserID, retUserDAO.UserID, "UserIDs do not match.");
        }

        [TestMethod]
        public void CreateAndRetrieveMultipleUsersShouldReturnDifferentUsers()
        {
            _controller.CreateUser(_userDAO1);
            _controller.CreateUser(_userDAO2);
            UserDAO u1 = _controller.RetrieveUser(_userDAO1.PhoneEmail);
            UserDAO u2 = _controller.RetrieveUser(_userDAO2.PhoneEmail);

            Assert.AreEqual(_userDAO1.UserID, u1.UserID);
            Assert.AreEqual(_userDAO2.UserID, u2.UserID);
            Assert.AreNotEqual(u1.UserID, u2.UserID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateNullUserShouldThrowException()
        {
            _controller.UpdateUser(null);
        }

        [TestMethod]
        public void UpdateNonExistingUserShouldNotChangeDatabase()
        {
            _userDAO1.UserID = -1;
            Assert.IsFalse(_controller.UpdateUser(_userDAO1));
        }

        [TestMethod]
        public void UpdateUserWithSameInfoShouldReturnSameUser()
        {
            _controller.CreateUser(_userDAO1);
            _controller.UpdateUser(_userDAO1);
            _userDAO2 = _controller.RetrieveUser(_userDAO1.PhoneEmail);

            Assert.AreEqual(_userDAO1.UserName, _userDAO2.UserName, "UserNames do not match");
            Assert.AreEqual(_userDAO1.FirstName, _userDAO2.FirstName, "FirstNames do not match.");
            Assert.AreEqual(_userDAO1.LastName, _userDAO2.LastName, "LastNames do not match.");
            Assert.AreEqual(_userDAO1.PhoneNumber, _userDAO2.PhoneNumber, "PhoneNumbers do not match.");
            Assert.AreEqual(_userDAO1.PhoneEmail, _userDAO2.PhoneEmail, "PhoneEmails do not match.");
            Assert.AreEqual(_userDAO1.UserLevel, _userDAO2.UserLevel, "UserLevels do not match.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteNullUserShouldThrowException()
        {
            _controller.DeleteUser(null);
        }

        [TestMethod]
        public void DeleteNonExistingUserShouldNotChangeDatabase()
        {
            _userDAO1.UserID = -1;
            Assert.IsFalse(_controller.DeleteUser(_userDAO1), "There was a user with an invalid ID already in the database.");
        }

        [TestMethod]
        public void DeleteUserShouldUpdateDatabase()
        {
            _controller.CreateUser(_userDAO1);
            Assert.IsTrue(_controller.DeleteUser(_userDAO1), "Test user was not deleted from the database.");
        }

        [TestMethod]
        public void DeleteMultipleUsersShouldUpdateDatabase()
        {
            _controller.CreateUser(_userDAO1);
            _controller.CreateUser(_userDAO2);

            int count = 2;
            if (_controller.DeleteUser(_userDAO1)) --count;
            if (_controller.DeleteUser(_userDAO2)) --count;

            Assert.AreEqual(0, count, "Not all test users were deleted from the database.");
        }

        [TestCleanup]
        public void TearDown()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "DELETE FROM users WHERE email_phone LIKE @email_phone";
                query.Parameters.AddWithValue("@email_phone", "111111111%@test.com");

                conn.Open();
                query.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
