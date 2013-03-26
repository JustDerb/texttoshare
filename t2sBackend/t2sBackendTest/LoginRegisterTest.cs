using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t2sBackend;
using t2sDbLibrary;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckLoginWithNullUsernameThrowsException()
        {
            _controller.CheckLogin(null, "password");
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckLoginWithNullPasswordThrowsException()
        {
            _controller.CheckLogin("TESTUSER", null);
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        public void CheckLoginOnNonExistingUserReturnsFalse()
        {
            Assert.IsFalse(_controller.CheckLogin("TESTUSER", "password"));
        }

        [TestCategory("LoginRegister")]
        [TestMethod]
        public void CheckBadLoginOnExistingUserReturnsFalse()
        {
            _controller.CreateUser(_userDAO1, "password");
            Assert.IsFalse(_controller.CheckLogin("TESTUSER", "password"));
        }
    }
}
