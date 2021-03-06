﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Data.SqlClient;
using t2sDbLibrary;

namespace t2sBackendTest
{
    [TestClass]
    public class SqlControllerUserTest
    {
        private SqlController _controller;
        private UserDAO _user1;
        private UserDAO _user2;
        private UserDAO _nullUser;

        [TestInitialize]
        public void Setup()
        {
            _user1 = new UserDAO()
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

            _user2 = new UserDAO()
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

            _nullUser = new UserDAO()
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
            _controller.CreateUser(_user1, null);
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUserNullPropertiesThrowsException()
        {
            _controller.CreateUser(_nullUser, "password");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void CreateUserSuccessfully()
        {
            Assert.IsTrue(_controller.CreateUser(_user1, "password"), "User was not inserted into the database.");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(EntryAlreadyExistsException))]
        public void CallingCreateUserOnSameUserTwiceThrowsException()
        {
            _controller.CreateUser(_user1, "password");
            _controller.CreateUser(_user1, "password");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(EntryAlreadyExistsException))]
        public void CreatingDuplicateUsersThrowsException()
        {
            _controller.CreateUser(_user1, "password");
            _user1.UserID = null;
            _controller.CreateUser(_user1, "password");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void CreateDuplicateUsersBypassUniquenessCheckViolatesSqlConstraintAndThrowsException()
        {
            SqlController stubbedController = MockRepository.GenerateStub<SqlController>();

            stubbedController.Stub(x => x.UserExists(_user1.UserName, _user1.PhoneEmail)).Return(false);

            stubbedController.CreateUser(_user1, "password");
            stubbedController.CreateUser(_user1, "password");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RetrieveNullUserShouldThrowException()
        {
            _controller.RetrieveUserByUserName("");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(CouldNotFindException))]
        public void RetreiveNonExistingUserShouldThrowException()
        {
            _controller.RetrieveUserByUserName(_user1.UserName);
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RetrieveNullUserByPhoneEmailShouldThrowException()
        {
            _controller.RetrieveUserByPhoneEmail("");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        [ExpectedException(typeof(CouldNotFindException))]
        public void RetreiveNonExistingUserByPhoneEmailShouldThrowException()
        {
            _controller.RetrieveUserByPhoneEmail(_user1.PhoneEmail);
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void CreateAndRetrieveByUserNameShouldReturnSameUser()
        {
            _controller.CreateUser(_user1, "password");
            UserDAO retUserDAO = _controller.RetrieveUserByUserName(_user1.UserName);

            Assert.AreEqual(_user1.UserID, retUserDAO.UserID, "UserIDs do not match.");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void CreateAndRetrieveMultipleUsersShouldReturnDifferentUsers()
        {
            _controller.CreateUser(_user1, "password");
            _controller.CreateUser(_user2, "password");
            UserDAO u1 = _controller.RetrieveUserByUserName(_user1.UserName);
            UserDAO u2 = _controller.RetrieveUserByUserName(_user2.UserName);

            Assert.AreEqual(_user1.UserID, u1.UserID);
            Assert.AreEqual(_user2.UserID, u2.UserID);
            Assert.AreNotEqual(u1.UserID, u2.UserID);
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void CreateAndRetrieveByPhoneEmailShouldReturnSameUser()
        {
            _controller.CreateUser(_user1, "password");
            UserDAO retUserDAO = _controller.RetrieveUserByPhoneEmail(_user1.PhoneEmail);

            Assert.AreEqual(_user1.UserID, retUserDAO.UserID, "UserIDs do not match.");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void CreateAndRetrieveMultipleUsersByPhoneEmailShouldReturnDifferentUsers()
        {
            _controller.CreateUser(_user1, "password");
            _controller.CreateUser(_user2, "password");
            UserDAO u1 = _controller.RetrieveUserByPhoneEmail(_user1.PhoneEmail);
            UserDAO u2 = _controller.RetrieveUserByPhoneEmail(_user2.PhoneEmail);

            Assert.AreEqual(_user1.UserID, u1.UserID);
            Assert.AreEqual(_user2.UserID, u2.UserID);
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
            _user1.UserID = -1;
            Assert.IsFalse(_controller.UpdateUser(_user1));
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void UpdateUserWithSameInfoShouldReturnSameUser()
        {
            _controller.CreateUser(_user1, "password");
            _controller.UpdateUser(_user1);
            _user2 = _controller.RetrieveUserByUserName(_user1.UserName);

            Assert.AreEqual(_user1.UserName, _user2.UserName, "UserNames do not match");
            Assert.AreEqual(_user1.FirstName, _user2.FirstName, "FirstNames do not match.");
            Assert.AreEqual(_user1.LastName, _user2.LastName, "LastNames do not match.");
            Assert.AreEqual(_user1.PhoneNumber, _user2.PhoneNumber, "PhoneNumbers do not match.");
            Assert.AreEqual(_user1.PhoneEmail, _user2.PhoneEmail, "PhoneEmails do not match.");
            Assert.AreEqual(_user1.UserLevel, _user2.UserLevel, "UserLevels do not match.");
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
            _user1.UserID = -1;
            Assert.IsFalse(_controller.DeleteUser(_user1, false), "There was a user with an invalid ID already in the database.");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void DeleteUserShouldUpdateDatabase()
        {
            _controller.CreateUser(_user1, "password");
            Assert.IsTrue(_controller.DeleteUser(_user1, false), "Test user was not deleted from the database.");
        }

        [TestCategory("SqlController.User")]
        [TestMethod]
        public void DeleteMultipleUsersShouldUpdateDatabase()
        {
            _controller.CreateUser(_user1, "password");
            _controller.CreateUser(_user2, "password");

            int count = 2;
            _controller.DeleteUser(_user1, false);
            if (!_controller.UserExists(_user1.UserName, _user1.PhoneEmail)) --count;
            
            _controller.DeleteUser(_user1, false);
            if (!_controller.UserExists(_user1.UserName, _user1.PhoneEmail)) --count;

            Assert.AreEqual(0, count, "Not all test users were deleted from the database.");
        }

        [TestCleanup]
        public void TearDown()
        {
            if (null != _user1.UserID) _controller.DeleteUser(_user1, false);
            if (null != _user2.UserID) _controller.DeleteUser(_user2, false);

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
