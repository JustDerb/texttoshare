using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class MessageParser
    {
        private Queue<string> MessageQueue;
        public event EventHandler parsedMessage;

        public MessageParser(IWatcherService watcherService)
        {
            throw new System.NotImplementedException();
        }

        public void addMessage(string message, string sender)
        {
            throw new System.NotImplementedException();
        }

        public void getNextMessage()
        {
            throw new System.NotImplementedException();
        }
    }
}
