using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t2sBackend;
using t2sDbLibrary;

namespace t2sBackendTest
{
    [TestClass]
    public class SqlControllerGroupTest
    {
        private SqlController _controller;
        
        private GroupDAO _group;

        private UserDAO _owner;
        private UserDAO _moderator;
        private UserDAO _user;

        private PluginDAO _enabledPlugin;
        private PluginDAO _disabledPlugin;

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

            _moderator = new UserDAO()
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

            _user = new UserDAO()
            {
                UserName = "TESTUSER3",
                FirstName = "TEST",
                LastName = "USER",
                PhoneNumber = "1111111113",
                PhoneEmail = "1111111113@test.com",
                Carrier = PhoneCarrier.Verizon,
                UserLevel = UserLevel.User,
                IsBanned = false,
                IsSuppressed = false
            };

            _controller.RegisterUser(_owner, "password");
            _controller.RegisterUser(_moderator, "password");
            _controller.RegisterUser(_user, "password");

            _enabledPlugin = new PluginDAO()
            {
                Name = "EnPlgn",
                Description = "An enabled test plugin",
                IsDisabled = false,
                VersionNum = "1.0.0",
                OwnerID = _user.UserID,
                Access = PluginAccess.STANDARD,
                HelpText = "Help meh, I'm an enabled plugin!"
            };

            _disabledPlugin = new PluginDAO()
            {
                Name = "DsPlgn",
                Description = "A disabled test plugin",
                IsDisabled = true,
                VersionNum = "1.0.0",
                OwnerID = _user.UserID,
                Access = PluginAccess.STANDARD,
                HelpText = "Help meh, I'm a disabled plugin!"
            };

            _controller.CreatePlugin(_enabledPlugin);
            _controller.CreatePlugin(_disabledPlugin);

            _group = new GroupDAO(_owner)
            {
                Name = "Test Group",
                Description = "A test group, for testing",
                GroupTag = "TEST"
            };
        }

        [TestCategory("SqlController.Group")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateNullGroupThrowsException()
        {
            _controller.CreateGroup(null);
        }

        [TestCategory("SqlController.Group")]
        [TestMethod]
        [ExpectedException(typeof(CouldNotFindException))]
        public void CreateGroupWithNoOwnerThrowsException()
        {
            _controller.CreateGroup(new GroupDAO());
        }

        [TestCleanup]
        public void Teardown()
        {
            if (null != _group.GroupID) _controller.DeleteGroup(_group);
            if (null != _enabledPlugin.PluginID) _controller.DeletePlugin(_enabledPlugin);
            if (null != _disabledPlugin.PluginID) _controller.DeletePlugin(_disabledPlugin);
            if (null != _owner.UserID) _controller.DeleteUser(_owner, false);
            if (null != _moderator.UserID) _controller.DeleteUser(_moderator, false);
            if (null != _user.UserID) _controller.DeleteUser(_user, false);
        }
    }
}
