using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public interface IWatcherService
    {
        event EventHandler RecievedMessage;

        bool Running
        {
            get;
            set;
        }

        List<Message> MessageQueue
        {
            get;
            set;
        }

        void Start();

        void Stop();

        Message GetNextMessage();

        bool SendMessage(t2sBackend.Message message, bool async);

        bool SendMessage(t2sBackend.Message message);
    }
}
