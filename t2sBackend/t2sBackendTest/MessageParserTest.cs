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
        private Message msg;

        private GroupDAO _group;
        private UserDAO _user1;
        private UserDAO _user2;
        private UserDAO _user3;

        [TestInitialize]
        public void Setup()
        {
            this.stubbedController = MockRepository.GenerateStub<SqlController>();
            this.msg = new Message();

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
            this.stubbedController.Stub(x => x.RetrieveUser(this._user1.PhoneEmail)).Return(this._user1);
            this.stubbedController.Stub(x => x.RetrieveUser(this._user2.PhoneEmail)).Return(this._user2);
        }

        [TestCleanup]
        public void TearDown()
        {
            this._user2 = null;
            this._user1 = null;
            this._group = null;
            this.stubbedController = null;
            this.msg = null;
        }

        [TestCategory("MessageParser.Texts.Good")]
        [TestMethod]
        public void NormalTextNoArgs()
        {
            msg.Sender = this._user1.PhoneEmail;
            StringBuilder sbuilder = new StringBuilder();
            StringBuilder sbuilderMessage = new StringBuilder();
            String command = "command";
            List<string> args = new List<string>()
            {
            };
            sbuilder.Append(this._group.GroupTag);
            sbuilder.Append(MessageParser.delimiter);

            // Main message
            sbuilderMessage.Append(command);
            sbuilderMessage.Append(" ");
            foreach (string arg in args) 
            {
                sbuilderMessage.Append(arg);
                sbuilderMessage.Append(" ");
            }

            //Tack on to text
            sbuilder.Append(sbuilderMessage);

            msg.FullMessage = sbuilder.ToString();
            ParsedMessage pmsg = MessageParser.Parse(this.msg, this.stubbedController);

           // Assert.AreEqual(command, pmsg.Command, true);
            String outputContentMsg = sbuilderMessage.ToString();
            int firstDelimeter = outputContentMsg.IndexOf(MessageParser.delimiter);
            outputContentMsg = outputContentMsg.Substring(0, firstDelimeter) + " " + outputContentMsg.Substring(firstDelimeter + 1);
            Assert.AreEqual(outputContentMsg, pmsg.ContentMessage, true);
            CollectionAssert.AreEquivalent(args, pmsg.Arguments);
            Assert.AreEqual(sbuilderMessage.ToString(), pmsg.ContentMessage, true);
            // TODO: Check for correct UserDAO and GroupDAO
        }

        [TestCategory("MessageParser.Texts.Good")]
        [TestMethod]
        public void NormalTextWithArguments()
        {
            msg.Sender = this._user1.PhoneEmail;
            StringBuilder sbuilder = new StringBuilder();
            StringBuilder sbuilderMessage = new StringBuilder();
            String command = "command";
            List<string> args = new List<string>()
            {
                "this",
                "is",
                "an",
                "argument"
            };
            sbuilder.Append(this._group.GroupTag);
            sbuilder.Append(MessageParser.delimiter);

            // Main message
            sbuilderMessage.Append(command);
            sbuilderMessage.Append(" ");
            foreach (string arg in args)
            {
                sbuilderMessage.Append(arg);
                sbuilderMessage.Append(" ");
            }

            //Tack on to text
            sbuilder.Append(sbuilderMessage);

            msg.FullMessage = sbuilder.ToString();
            ParsedMessage pmsg = MessageParser.Parse(this.msg, this.stubbedController);

           // Assert.AreEqual(command, pmsg.Command, true);
            String outputContentMsg = sbuilderMessage.ToString();
            int firstDelimeter = outputContentMsg.IndexOf(MessageParser.delimiter);
            outputContentMsg = outputContentMsg.Substring(0, firstDelimeter) + " " + outputContentMsg.Substring(firstDelimeter + 1);
            Assert.AreEqual(outputContentMsg, pmsg.ContentMessage, true);
            CollectionAssert.AreEquivalent(args, pmsg.Arguments);
            Assert.AreEqual(sbuilderMessage.ToString(), pmsg.ContentMessage, true);
            // TODO: Check for correct UserDAO and GroupDAO
        }

        [TestCategory("MessageParser.Texts.Good")]
        [TestMethod]
        public void StopText()
        {
            msg.Sender = this._user1.PhoneEmail;
            StringBuilder sbuilder = new StringBuilder();
            StringBuilder sbuilderMessage = new StringBuilder();
            String command = "stop";
            List<string> args = new List<string>()
            {
            };

            // Main message
            sbuilderMessage.Append(command);
            sbuilder.Append(MessageParser.delimiter);
          //  sbuilderMessage.Append(" ");
            foreach (string arg in args)
            {
                sbuilderMessage.Append(arg);
                sbuilderMessage.Append(" ");
            }

            //Tack on to text
            sbuilder.Append(sbuilderMessage);

            msg.FullMessage = sbuilder.ToString();
            ParsedMessage pmsg = MessageParser.Parse(this.msg, this.stubbedController);

           // Assert.AreEqual(command, pmsg.Command, true);
            String outputContentMsg = sbuilderMessage.ToString();
            int firstDelimeter = outputContentMsg.IndexOf(MessageParser.delimiter);
            outputContentMsg = outputContentMsg.Substring(0, firstDelimeter) + " " + outputContentMsg.Substring(firstDelimeter + 1);
            Assert.AreEqual(outputContentMsg, pmsg.ContentMessage, true);
            CollectionAssert.AreEquivalent(args, pmsg.Arguments);
            Assert.AreEqual(sbuilderMessage.ToString(), pmsg.ContentMessage, true);
            // TODO: Check for correct UserDAO and GroupDAO
        }

        [TestCategory("MessageParser.Texts.Good")]
        [TestMethod]
        public void StopTextWithArguments()
        {
            msg.Sender = this._user1.PhoneEmail;
            StringBuilder sbuilder = new StringBuilder();
            StringBuilder sbuilderMessage = new StringBuilder();
            String command = "stop";
            List<string> args = new List<string>()
            {
                "sending",
                "me",
                "messages",
                "now!"
            };

            // Main message
            sbuilderMessage.Append(command);
            //changed this from " " to "." to account for second delimiter we are requirign
            sbuilder.Append(MessageParser.delimiter);
            foreach (string arg in args)
            {
                sbuilderMessage.Append(arg);
                sbuilderMessage.Append(" ");
            }

            //Tack on to text
            sbuilder.Append(sbuilderMessage);

            msg.FullMessage = sbuilder.ToString();
            ParsedMessage pmsg = MessageParser.Parse(this.msg, this.stubbedController);

           // Assert.AreEqual(command, pmsg.Command, true);
            String outputContentMsg = sbuilderMessage.ToString();
            int firstDelimeter = outputContentMsg.IndexOf(MessageParser.delimiter);
            outputContentMsg = outputContentMsg.Substring(0, firstDelimeter) + " " + outputContentMsg.Substring(firstDelimeter + 1);
            Assert.AreEqual(outputContentMsg, pmsg.ContentMessage, true);
            CollectionAssert.AreEquivalent(args, pmsg.Arguments);
            Assert.AreEqual(sbuilderMessage.ToString(), pmsg.ContentMessage, true);
            // TODO: Check for correct UserDAO and GroupDAO
        }

        [TestCategory("MessageParser.Texts.Bad")]
        [TestMethod]
        public void IncorrectGroupTagNoCommandOrArguments()
        {
            string fakeGroupTag = "HAHA";
            
            
            this.stubbedController.Stub(x => x.RetrieveGroup(fakeGroupTag)).Throw(new CouldNotFindException("Could not find group"));

            msg.Sender = this._user1.PhoneEmail;
            StringBuilder sbuilder = new StringBuilder();
            StringBuilder sbuilderMessage = new StringBuilder();
            String command = "";
            List<string> args = new List<string>()
            {
            };
            sbuilder.Append(fakeGroupTag);
            sbuilder.Append(MessageParser.delimiter);

            // Main message
            sbuilderMessage.Append(command);
            sbuilderMessage.Append(MessageParser.delimiter);
           // sbuilderMessage.Append(" ");
            foreach (string arg in args)
            {
                sbuilderMessage.Append(arg);
                sbuilderMessage.Append(" ");
            }

            //Tack on to text
            sbuilder.Append(sbuilderMessage);

            msg.FullMessage = sbuilder.ToString();
            ParsedMessage pmsg = MessageParser.Parse(this.msg, this.stubbedController);

           // Assert.AreEqual(command, pmsg.Command, true);
            String outputContentMsg = sbuilderMessage.ToString();
            int firstDelimeter = outputContentMsg.IndexOf(MessageParser.delimiter);
            outputContentMsg = outputContentMsg.Substring(0, firstDelimeter) + " " + outputContentMsg.Substring(firstDelimeter + 1);
            Assert.AreEqual(outputContentMsg, pmsg.ContentMessage, true);
            CollectionAssert.AreEquivalent(args, pmsg.Arguments);
            Assert.AreEqual(sbuilderMessage.ToString(), pmsg.ContentMessage, true);
            // TODO: Check for correct UserDAO and GroupDAO
            Assert.AreEqual(null, pmsg.Group);
        }

        [TestCategory("MessageParser.Texts.Bad")]
        [TestMethod]
        public void IncorrectGroupTagNoArguments()
        {
            string fakeGroupTag = "HAHA";

            this.stubbedController.Stub(x => x.RetrieveGroup(fakeGroupTag)).Throw(new CouldNotFindException("Could not find group"));

            msg.Sender = this._user1.PhoneEmail;
            StringBuilder sbuilder = new StringBuilder();
            StringBuilder sbuilderMessage = new StringBuilder();
            String command = "command";
            List<string> args = new List<string>()
            {
            };
            sbuilder.Append(fakeGroupTag);
            sbuilder.Append(MessageParser.delimiter);

            // Main message
            sbuilderMessage.Append(command);
            //changed this from " " to "." to account for second delimiter we are requirign
            sbuilderMessage.Append(MessageParser.delimiter);
            foreach (string arg in args)
            {
                sbuilderMessage.Append(arg);
                sbuilderMessage.Append(" ");
            }

            //Tack on to text
            sbuilder.Append(sbuilderMessage);

            msg.FullMessage = sbuilder.ToString();
            ParsedMessage pmsg = MessageParser.Parse(this.msg, this.stubbedController);

           // Assert.AreEqual(command, pmsg.Command, true);
            CollectionAssert.AreEquivalent(args, pmsg.Arguments);
            String outputContentMsg = sbuilderMessage.ToString();
            int firstDelimeter = outputContentMsg.IndexOf(MessageParser.delimiter);
            outputContentMsg = outputContentMsg.Substring(0, firstDelimeter) + " " + outputContentMsg.Substring(firstDelimeter + 1);
            Assert.AreEqual(outputContentMsg, pmsg.ContentMessage, true);
            // TODO: Check for correct UserDAO and GroupDAO
            Assert.AreEqual(null, pmsg.Group);
        }

        [TestCategory("MessageParser.Texts.Bad")]
        [TestMethod]
        public void IncorrectGroupTagWithCommandAndArguments()
        {
            string fakeGroupTag = "HAHA";

            this.stubbedController.Stub(x => x.RetrieveGroup(fakeGroupTag)).Throw(new CouldNotFindException("Could not find group"));

            msg.Sender = this._user1.PhoneEmail;
            StringBuilder sbuilder = new StringBuilder();
            StringBuilder sbuilderMessage = new StringBuilder();
            String command = "command";
            List<string> args = new List<string>()
            {
                "this",
                "is",
                "an",
                "argument"
            };
            sbuilder.Append(fakeGroupTag);
            sbuilder.Append(MessageParser.delimiter);

            // Main message
            sbuilderMessage.Append(command);
            //changed this from " " to "." to account for second delimiter we are requirign
            sbuilder.Append(MessageParser.delimiter);
            foreach (string arg in args)
            {
                sbuilderMessage.Append(arg);
                sbuilderMessage.Append(" ");
            }

            //Tack on to text
            sbuilder.Append(sbuilderMessage);

            msg.FullMessage = sbuilder.ToString();
            ParsedMessage pmsg = MessageParser.Parse(this.msg, this.stubbedController);

           // Assert.AreEqual(command, pmsg.Command, true);
            String outputContentMsg = sbuilderMessage.ToString();
            int firstDelimeter = outputContentMsg.IndexOf(MessageParser.delimiter);
            outputContentMsg = outputContentMsg.Substring(0, firstDelimeter) + " " + outputContentMsg.Substring(firstDelimeter + 1);
            Assert.AreEqual(outputContentMsg, pmsg.ContentMessage, true);
            CollectionAssert.AreEquivalent(args, pmsg.Arguments);
            Assert.AreEqual(sbuilderMessage.ToString(), pmsg.ContentMessage, true);
            // TODO: Check for correct UserDAO and GroupDAO
            Assert.AreEqual(null, pmsg.Group);
        }
    }
}
