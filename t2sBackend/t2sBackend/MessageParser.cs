using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class MessageParser
    {
        private Queue<ParsedMessage> MessageQueue;
        public event EventHandler parsedMessage;

        public MessageParser(IWatcherService watcherService)
        {
            throw new System.NotImplementedException();
        }

        public void addMessage(Message message)
        {
           // creating the parsed messge to be added
            ParsedMessage parsed = new ParsedMessage();
            parsed.Sender = DBControllerSingleton.GetInstance().RetrieveUser(message.Sender);
            
           // String groupTag = message.
            String[] groupTag = message.FullMessage.Split('.');
           
        }

        public ParsedMessage getNextMessage()
        {
            lock (MessageQueue)
            {
                return MessageQueue.Dequeue();
            }
        }
    }
}
