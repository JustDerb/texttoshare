using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t2sBackend
{
    public class MessageController
    {
        private BlockingQueue<ParsedMessage> queue;
        private AWatcherService watcher;
        private IDBController controller;

        public ParsedMessage getNextMessage()
        {
            return this.queue.Dequeue();
        }

        private void putNextMessage(ParsedMessage Message)
        {
            this.queue.Enqueue(Message);
        }

        public MessageController(AWatcherService Watcher, IDBController Controller)
        {
            this.queue = new BlockingQueue<ParsedMessage>();
            this.watcher = Watcher;
            this.controller = Controller;
            this.watcher.RecievedMessage += Watcher_RecievedMessage;
        }

        void Watcher_RecievedMessage(object sender, AWatcherService.WatcherServiceEventArgs e)
        {
            this.putNextMessage(MessageParser.Parse(e.MessageObj, this.controller));
        }

    }
}
