using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    /// <summary>
    /// Plugin that checks if owner/developer, confirms and deletes all instances of a user
    /// </summary>
    public class StopPlugin : IPlugin
    {
        /// <summary>
        /// Takes in the info from the moderator/owner and removes a user from the group
        /// </summary>
        /// <param name="message">Message to be handled</param>
        /// <param name="service">Service to send/recieve messages through</param>
        public void Run(ParsedMessage message, AWatcherService service, IDBController controller)
        {
            Message msgSender = new Message();

            if (message.Arguments.Count > 0 && message.Arguments[0].Substring(0, 3).ToUpper().Equals("YES"))
            {
                controller.DeleteUser(message.Sender);
                msgSender.FullMessage = "You have been removed from our system. Thank you for using our services.";
            }
            else 
            {
                controller.SuppressUser(message.Sender);
                msgSender.FullMessage = "To remove yourself from Text2Share, please respond with STOP YES. Please keep in mind that this also deletes any groups you own and any plugins you have developed.";
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
            Description = "Plugin for unregistering users",
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