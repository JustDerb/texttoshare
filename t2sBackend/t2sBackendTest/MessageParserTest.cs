using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t2sBackend;
using Rhino.Mocks;

namespace t2sBackendTest
{
    [TestClass]
    public class MessageParserTest
    {
        // TODO: Change to IDBController when the interface is updated
        private SqlController controller;
        private Message msg;

        private GroupDAO _group;
        private UserDAO _user1;
        private UserDAO _user2;

        [TestInitialize]
        public void Setup()
        {
            this.controller = MockRepository.GenerateStub<SqlController>();
            this.msg = new Message();

            //this._group = new GroupDAO();
        }

        [TestCleanup]
        public void TearDown()
        {
            this.controller = null;
            this.msg = null;
        }

        [TestCategory("MessageParser.Texts.Good")]
        [TestMethod]
        public void NormalText()
        {
            //this.controller.Stub(x => x.RetrieveGroup("TEST")).Return();
            msg.Sender = "";
            ParsedMessage pmsg = MessageParser.Parse(this.msg, this.controller);

        }
    }
}
