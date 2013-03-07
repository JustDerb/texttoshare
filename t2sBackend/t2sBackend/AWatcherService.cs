using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public abstract class AWatcherService
    {
        public class WatcherServiceEventArgs : EventArgs
        {
            public Message MessageObj { get; set; }
            public String MessageString { get; set; }
        }

        /// <summary>
        /// Event listener to be called when the service recieves a message
        /// </summary>
        public event EventHandler<WatcherServiceEventArgs> RecievedMessage;

        protected virtual void OnRecievedMessage(WatcherServiceEventArgs e)
        {
            EventHandler<WatcherServiceEventArgs> handler = RecievedMessage;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Start the watcher service
        /// </summary>
        abstract public void Start();

        /// <summary>
        /// Stop the watcher service
        /// </summary>
        abstract public void Stop();

        /// <summary>
        /// Sends a message through this watcher service
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="async">Send message asynchronously</param>
        /// <returns>True is sending successful, false otherwise</returns>
        abstract public bool SendMessage(Message message, bool async);

        /// <summary>
        /// Sends a message through this watcher service
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>True is sending successful, false otherwise</returns>
        abstract public bool SendMessage(Message message);

        /// <summary>
        /// The current running state of the thread
        /// </summary>
        /// <returns></returns>
        abstract public bool IsRunning();
    }
}
