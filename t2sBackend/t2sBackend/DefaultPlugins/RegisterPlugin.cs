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

            // Only reply to the user if it is valid.  Else, they can abuse the system
            if (message.Arguments.Count > 0
                // Also make sure they are not trying to bypass us by not going to the verification page
                && !message.Arguments[0].Equals("-1"))
            {
                try
                {
                    UserDAO user = controller.GetUserByVerificationCode(message.Arguments[0]);
                    if (user != null)
                    {
                        // Set their wanted phone email
                        user.PhoneEmail = message.Sender.PhoneEmail;
                        // Update our user
                        controller.UpdateUser(user);
                        // Reset verification to nothing
                        controller.SetVerificationCodeForUser(null, user);
                        fullMsg.Append("You have successfully registered with Text2Share. Thank you!");
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