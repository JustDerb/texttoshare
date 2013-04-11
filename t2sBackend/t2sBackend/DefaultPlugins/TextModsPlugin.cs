using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    /// <summary>
    /// Plugin that messages the moderators of a specified group
    /// </summary>
    public class TextModsPlugin : IPlugin
    {
        /// <summary>
        /// Takes in a message content to send to all moderators in group (excluding sender) and replies to sender with confirmation
        /// </summary>
        /// <param name="message">Message to be handled</param>
        /// <param name="service">Service to send/recieve messages through</param>
        public void Run(ParsedMessage message, AWatcherService service, IDBController controller)
        {
            Message msg = new Message();
            msg.FullMessage = message.ContentMessage;
            Message msgSender = new Message();
            foreach (UserDAO u in message.Group.Moderators)
            {
                if (!u.Equals(message.Sender) && !u.IsBanned && !u.IsSuppressed)
                {
                    msg.Reciever.Add(u.PhoneEmail);
                }
            }
            msgSender.FullMessage = "Message sent successfully.";
            service.SendMessage(msgSender);
            service.SendMessage(msg);
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