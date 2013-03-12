using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t2sBackend
{
    class MessageControllerOverride : MessageController
    {
        public MessageControllerOverride(AWatcherService Watcher, IDBController Controller) : base(Watcher, Controller)
        {
        }

        public new void putNextMessage(ParsedMessage Message)
        {
            base.putNextMessage(Message);
        }
    }
}
