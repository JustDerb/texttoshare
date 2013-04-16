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
            StringBuilder fullMsg = new StringBuilder();
            bool isModerator = message.Group.Moderators.Contains(message.Sender);
            bool isOwner = message.Group.Owner.Equals(message.Sender);

            // Put the group tag for
            fullMsg.Append(message.Group.GroupTag);
            fullMsg.Append(": \n");

            bool first = true;
            foreach (PluginDAO d in message.Group.EnabledPlugins)
            {
                if (d.Access == PluginAccess.STANDARD ||
                    (d.Access == PluginAccess.MODERATOR && (isModerator || isOwner)))
                {
                    if (!d.Name.Equals("error", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!first)
                            fullMsg.Append(", ");
                        // Make it look pretty
                        fullMsg.Append(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(d.Name.ToLower()));
                        first = false;
                    }
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