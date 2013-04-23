using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    /// <summary>
    /// Plugin that user calls when registering with System 
    /// </summary>
    public class RegisterPlugin : IPlugin
    {
        /// <summary>
        /// Takes in the ParsedMessage and makes appropriate calls before responding to user
        /// </summary>
        /// <param name="message">Message containing information</param>
        /// <param name="service">Service to send/recieve messages through</param>
        /// <param name="controller">Database to pull from</param>
        public void Run(ParsedMessage message, AWatcherService service, IDBController controller)
        {
            Message msg = new Message();
            StringBuilder fullMsg = new StringBuilder();
            if (message.Arguments.Count <= 0)
            {
                fullMsg.Append("Please remember to add your verification code after your command.");
            }
            else
            {
                try
                {
                    UserDAO user = controller.GetUserByVerificationCode(message.Arguments[0]);
                    if (user != null)
                    {
                        user.PhoneEmail = message.Sender.PhoneEmail;
                        fullMsg.Append("You have successfully register with Text2Share. Thank you!");
                    }
                }
                catch
                {
                    return;
                }
            }

            msg.FullMessage = fullMsg.ToString();
            msg.Reciever.Add(message.Sender.PhoneEmail);
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