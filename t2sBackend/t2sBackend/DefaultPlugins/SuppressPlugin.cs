using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    /// <summary>
    /// Plugin that removes a specified user from the specified group
    /// </summary>
    public class SuppressPlugin : IPlugin
    {
        /// <summary>
        /// Takes in the info from the moderator/owner and removes a user from the group
        /// </summary>
        /// <param name="message">Message in error to be handled</param>
        /// <param name="service">Service to send/recieve messages through</param>
        public void Run(ParsedMessage message, AWatcherService service, IDBController controller)
        {
            Message msgSender = new Message();

            if (message.Arguments.Count > 0 && message.Arguments[0].Substring(0, 3).ToUpper().Equals("OFF"))
            {
                controller.UnsuppressUser(message.Sender);
                msgSender.FullMessage = "You have been unsupressed and can now receive messages. To supress yourself, please reply SUPRESS to disable messages.";
            }
            else
            {
                controller.SuppressUser(message.Sender);
                msgSender.FullMessage = "You have been suppressed. If you would like to unsupress yourself, please reply SUPRESS OFF to remove.";
            }
            msgSender.Reciever.Add(message.Sender.PhoneEmail);
            service.SendMessage(msgSender);
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