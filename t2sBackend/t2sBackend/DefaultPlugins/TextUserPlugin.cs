using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    /// <summary>
    /// Plugin that messages the specified user
    /// </summary>
    public class TextUserPlugin : IPlugin
    {
        /// <summary>
        /// Takes in a message content to send to a specified user and replies to sender with confirmation
        /// </summary>
        /// <param name="message">Message to be handled</param>
        /// <param name="service">Service to send/recieve messages through</param>
        public void Run(ParsedMessage message, AWatcherService service, IDBController controller)
        {
            Message msgReceiver = new Message();
            msgReceiver.FullMessage = message.ContentMessage;

            Message msgSender = new Message();

            UserDAO receiver = controller.RetrieveUserByUserName(message.Arguments[0]);

            if (receiver == null || receiver.IsBanned)
            {
                msgSender.FullMessage = "Not a valid user. Please check their username and retry.";
            }
            else if(receiver.IsSuppressed)
            {
                msgSender.FullMessage = "User has suppressed messages.";
            }
            else
            {
                msgSender.FullMessage = "Message sent successfully.";
            }

            service.SendMessage(msgSender);
            service.SendMessage(msgReceiver);
        }

        /// <summary>
        /// Stores metadata related to this plugin
        /// </summary>
        public PluginDAO PluginDAO
        {
            get;
            set;
        }
    }
}