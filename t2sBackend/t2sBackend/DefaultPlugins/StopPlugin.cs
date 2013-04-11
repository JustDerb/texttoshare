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
            Message msgRemovedUser = new Message();
            try
            {
                
            }
            catch (Exception)
            {
                msgSender.FullMessage = "Failed to remove " + message.ContentMessage + " from the group "
                    + message.Group.GroupID + ". Please check user/group and try again.";
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