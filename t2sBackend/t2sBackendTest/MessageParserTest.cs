using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t2sBackend;
using Rhino.Mocks;
using t2sDbLibrary;
using System.Collections.Generic;
using System.Text;

namespace t2sBackendTest
{
    [TestClass]
    public class MessageParserTest
    {
        // TODO: Change to IDBController when the interface is updated
        private SqlController stubbedController;

        private GroupDAO _group;
        private UserDAO _user1;
        private UserDAO _user2;
        private UserDAO _user3;

        [TestInitialize]
        public void Setup()
        {
            this.stubbedController = MockRepository.GenerateStub<SqlController>();

            this._user1 = new UserDAO()
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

            this._user2 = new UserDAO()
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

            this._user3 = new UserDAO()
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

            this._group = new GroupDAO(this._user1)
            {
                Description = "Test description",
                GroupID = 1,
                GroupTag = "TEST",
                Moderators = new List<UserDAO>(),
                Name = "TEST GROUP",
                EnabledPlugins = new List<PluginDAO>(),
                Users = new List<UserDAO>()
            };
            this._group.Users.Add(this._user2);

            this.stubbedController.Stub(x => x.RetrieveGroup(this._group.GroupTag)).Return(this._group);
            this.stubbedController.Stub(x => x.RetrieveUserByPhoneEmail(this._user1.PhoneEmail)).Return(this._user1);
            this.stubbedController.Stub(x => x.RetrieveUserByPhoneEmail(this._user2.PhoneEmail)).Return(this._user2);
        }

        [TestCleanup]
        public void TearDown()
        {
            this._user2 = null;
            this._user1 = null;
            this._group = null;
            this.stubbedController = null;
        }

        private Message getMessage(string sender, string[] reciever, string command, string group, string args)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(command);
            builder.Append(MessageParser.delimiter);
            builder.Append(group);
            builder.Append(MessageParser.secondDelimiter);
            builder.Append(args);
            string fullmessage = builder.ToString();
            return new Message(sender, reciever, fullmessage);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextWithNoInvalidFields()
        {
            // Set our variables for testing
            String command = "command";
            String group = this._group.GroupTag;
            String args = "these are arguments";
            String[] argsArr = args.Split(' ');

            // Create our test message
            Message msg = getMessage(this._user1.PhoneEmail, new string[0], command, group, args);

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(args, pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(argsArr, pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(this._group, pmsg.Group);
            Assert.AreEqual(this._user1, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.VALID, pmsg.Type);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextInvalidFieldGroup()
        {
            // Set our variables for testing
            String command = "command";
            String group = "NOGROUP";
            String args = "these are arguments";
            String[] argsArr = args.Split(' ');

            // Create our test message
            Message msg = getMessage(this._user1.PhoneEmail, new string[0], command, group, args);

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(args, pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(argsArr, pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(null, pmsg.Group);
            Assert.AreEqual(this._user1, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.NO_GROUP, pmsg.Type);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextInvalidFieldCommand()
        {
            // Set our variables for testing
            String command = "";
            String group = this._group.GroupTag;
            String args = "these are arguments";
            String[] argsArr = args.Split(' ');

            // Create our test message
            Message msg = getMessage(this._user1.PhoneEmail, new string[0], command, group, args);

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(args, pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(argsArr, pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(this._group, pmsg.Group);
            Assert.AreEqual(this._user1, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.NO_COMMAND, pmsg.Type);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextWithNoArgs()
        {
            // Set our variables for testing
            String command = "command";
            String group = this._group.GroupTag;
            String args = "";
            String[] argsArr = new string[0];

            // Create our test message
            Message msg = getMessage(this._user1.PhoneEmail, new string[0], command, group, args);

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(args, pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(argsArr, pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(this._group, pmsg.Group);
            Assert.AreEqual(this._user1, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.VALID, pmsg.Type);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextWithNoArgsGroup()
        {
            // Set our variables for testing
            String command = "command";
            String group = "";
            String args = "";
            String[] argsArr = new string[0];

            // Create our test message
            Message msg = getMessage(this._user1.PhoneEmail, new string[0], command, group, args);

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(args, pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(argsArr, pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(null, pmsg.Group);
            Assert.AreEqual(this._user1, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.NO_GROUP, pmsg.Type);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextBlank()
        {
            // Create our test message
            Message msg = new Message(this._user1.PhoneEmail, new string[0], "");

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual("", pmsg.Command, true);
            Assert.AreEqual("", pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(new List<string>(), pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(null, pmsg.Group);
            Assert.AreEqual(this._user1, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.NO_COMMAND, pmsg.Type);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextInvalidUser()
        {
            // Set our variables for testing
            String command = "command";
            String group = this._group.GroupTag;
            String args = "these are arguments";
            String[] argsArr = args.Split(' ');

            // Create our test message
            Message msg = getMessage("nope@nospam.net", new string[0], command, group, args);

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(args, pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(argsArr, pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(this._group, pmsg.Group);
            Assert.AreEqual(null, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.NO_SENDER, pmsg.Type);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextWithNoCommand()
        {
            // Set our variables for testing
            String command = "";
            String group = this._group.GroupTag;
            String args = "these are arguments";
            String[] argsArr = args.Split(' ');

            // Create our test message
            Message msg = getMessage(this._user1.PhoneEmail, new string[0], command, group, args);

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(args, pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(argsArr, pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(this._group, pmsg.Group);
            Assert.AreEqual(this._user1, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.NO_COMMAND, pmsg.Type);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextWithFirstDelimiterTwice()
        {
            // Set our variables for testing
            String command = "command";
            String group = MessageParser.delimiter + this._group.GroupTag;
            String args = "these are arguments";
            String[] argsArr = args.Split(' ');

            // Create our test message
            Message msg = getMessage(this._user1.PhoneEmail, new string[0], command, group, args);

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(args, pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(argsArr, pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(this._group, pmsg.Group);
            Assert.AreEqual(this._user1, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.VALID, pmsg.Type);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextWithFirstDelimiterATonOfTimes()
        {
            // Set our variables for testing
            String command = "command";
            String group = "";
            for (int i = 0; i < 200; ++i)
                group += MessageParser.delimiter;
            group += this._group.GroupTag;
            String args = "these are arguments";
            String[] argsArr = args.Split(' ');

            // Create our test message
            Message msg = getMessage(this._user1.PhoneEmail, new string[0], command, group, args);

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(args, pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(argsArr, pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(this._group, pmsg.Group);
            Assert.AreEqual(this._user1, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.VALID, pmsg.Type);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextWithSecondDelimiterTwice()
        {
            // Set our variables for testing
            String command = "command";
            String group = this._group.GroupTag + MessageParser.secondDelimiter;
            String args = "these are arguments";
            String[] argsArr = args.Split(' ');

            // Create our test message
            Message msg = getMessage(this._user1.PhoneEmail, new string[0], command, group, args);

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(args, pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(argsArr, pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(this._group, pmsg.Group);
            Assert.AreEqual(this._user1, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.VALID, pmsg.Type);
        }

        [TestCategory("MessageParser")]
        [TestMethod]
        public void TextWithSecondDelimiterATonOfTimes()
        {
            // Set our variables for testing
            String command = "command";
            String group = this._group.GroupTag;
            for (int i = 0; i < 200; ++i)
                group += MessageParser.secondDelimiter;
            String args = "these are arguments";
            String[] argsArr = args.Split(' ');

            // Create our test message
            Message msg = getMessage(this._user1.PhoneEmail, new string[0], command, group, args);

            //Tack on to text
            ParsedMessage pmsg = MessageParser.Parse(msg, this.stubbedController);

            // Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(command, pmsg.Command, true);
            Assert.AreEqual(args, pmsg.ContentMessage);
            CollectionAssert.AreEquivalent(argsArr, pmsg.Arguments);

            // Check DAO's
            Assert.AreEqual(this._group, pmsg.Group);
            Assert.AreEqual(this._user1, pmsg.Sender);

            // Check type
            Assert.AreEqual(ParsedMessage.ContentMessageType.VALID, pmsg.Type);
        }
    }
}
