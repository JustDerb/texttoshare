using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    /// <summary>
    /// Plugin that messages the entire specified group
    /// </summary>
    public class TextGroupPlugin : IPlugin
    {
        /// <summary>
        /// Takes in a message content to send to all users in group (excluding sender) and replies to sender with confirmation
        /// </summary>
        /// <param name="message">Message to be handled</param>
        /// <param name="service">Service to send/recieve messages through</param>
        public void Run(ParsedMessage message, AWatcherService service, IDBController controller)
        {
            Message msg = new Message();
            msg.FullMessage = message.ContentMessage;
            Message msgSender = new Message();
            msgSender.Reciever.Add(message.Sender.PhoneEmail);
            List<UserDAO> listPeeps = new List<UserDAO>();

            listPeeps.AddRange(message.Group.Users);
            listPeeps.AddRange(message.Group.Moderators);

            foreach(UserDAO u in listPeeps)
            {
                if(!u.Equals(message.Sender)&&!u.IsBanned&&!u.IsSuppressed)
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
        private PluginDAO _PluginDAO = new PluginDAO()
        {
            Access = PluginAccess.STANDARD,
            Description = "Plugin for texting users",
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