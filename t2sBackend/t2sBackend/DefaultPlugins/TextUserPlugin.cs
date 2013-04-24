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
            String msgToSend = "";
            for (int i = 1; i < message.Arguments.Count; i++)
            {
                msgToSend += message.Arguments[i];
            }
            msgReceiver.FullMessage = msgToSend;

            Message msgSender = new Message();

            UserDAO receiver = controller.RetrieveUserByUserName(message.Arguments[0]);
            msgSender.Reciever.Add(message.Sender.PhoneEmail);


            if (!message.Group.Users.Contains(receiver) || receiver == null || receiver.IsBanned)
            {
                msgSender.FullMessage = "Not a valid user. Please check their username and retry.";
                service.SendMessage(msgSender);
            }
            else if(receiver.IsSuppressed)
            {
                msgSender.FullMessage = "User has suppressed messages.";
                service.SendMessage(msgSender);
            }
            else
            {
                msgReceiver.Reciever.Add(receiver.PhoneEmail);
                bool sent = service.SendMessage(msgReceiver);
                if (sent)
                {
                    msgSender.FullMessage = "Message sent successfully.";
                }
                else
                {
                    msgSender.FullMessage = "Message was unable to send to user.";
                }
                service.SendMessage(msgSender);
            }

        }

        /// <summary>
        /// Stores metadata related to this plugin
        /// </summary>
        private PluginDAO _PluginDAO = new PluginDAO()
        {
            Access = PluginAccess.STANDARD,
            Description = "Plugin for texting a single user",
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