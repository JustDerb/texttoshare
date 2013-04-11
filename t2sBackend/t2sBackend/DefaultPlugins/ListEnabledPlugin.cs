using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    /// <summary>
    /// Plugin that responds to caller with a list of enabled plugins for the specified group
    /// </summary>
    public class ListEnabledPlugins : IPlugin
    {
        /// <summary>
        /// Takes in the ParsedMessage and sends a message back to the sender with a message 
        /// listing the enabled plugins for the group
        /// </summary>
        /// <param name="message">Message in error to be handled</param>
        /// <param name="service">Service to send/recieve messages through</param>
        public void Run(ParsedMessage message, AWatcherService service, IDBController controller)
        {
            Message msg = new Message();
            foreach (PluginDAO d in message.Group.EnabledPlugins)
            {
                if (d.Access == PluginAccess.STANDARD || (d.Access == PluginAccess.MODERATOR && 
                    (message.Group.Moderators.Contains(message.Sender) || message.Group.Owner.Equals(message.Sender))))
                {
                    msg.FullMessage += d.Name + " ";
                }
            }
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