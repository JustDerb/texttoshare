using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public interface IWatcherService
    {
        /// <summary>
        /// Event listener to be called when the service recieves a message
        /// </summary>
        event EventHandler RecievedMessage;

        /// <summary>
        /// The current running state of the thread
        /// </summary>
        bool Running
        {
            get;
            private set;
        }

        /// <summary>
        /// Start the watcher service
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the watcher service
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets the next message in the queue and removes it from the queue.
        /// </summary>
        /// <returns>Next meesage in the queue</returns>
        Message GetNextMessage();

        /// <summary>
        /// Sends a message through this watcher service
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="async">Send message asynchronously</param>
        /// <returns>True is sending successful, false otherwise</returns>
        bool SendMessage(t2sBackend.Message message, bool async);

        /// <summary>
        /// Sends a message through this watcher service
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <returns>True is sending successful, false otherwise</returns>
        bool SendMessage(t2sBackend.Message message);
    }
}
