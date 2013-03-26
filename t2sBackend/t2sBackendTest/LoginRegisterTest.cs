using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t2sBackend;

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
            _controller.CreateUser();
            Assert.IsFalse(_controller.CheckLogin("TESTUSER", "password"));
        }
    }
}
