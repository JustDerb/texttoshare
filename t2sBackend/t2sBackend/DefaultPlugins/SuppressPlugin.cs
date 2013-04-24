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
                msgSender.FullMessage = "You have been unsuppressed and can now receive messages. To suppress yourself, please reply SUPPRESS to disable messages.";
            }
            else
            {
                controller.SuppressUser(message.Sender);
                msgSender.FullMessage = "You have been suppressed. If you would like to unsuppress yourself, please reply SUPPRESS OFF to remove.";
            }
            msgSender.Reciever.Add(message.Sender.PhoneEmail);
            service.SendMessage(msgSender);
        }

        /// <summary>
        /// Stores metadata related to this plugin
        /// </summary>
        private PluginDAO _PluginDAO = new PluginDAO()
        {
            Access = PluginAccess.STANDARD,
            Description = "Plugin for suppressing users",
            HelpText = "",
            VersionNum = "1.0"
        };

        public PluginDAO PluginDAO
        {
            get { return _PluginDAO; }
            set { _PluginDAO = value; }
        }
    }
}