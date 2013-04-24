using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    /// <summary>
    /// Plugin that responds to the sender with information about a specific plugin 
    /// </summary>
    public class HelpPlugin : IPlugin
    {
        /// <summary>
        /// Takes in the ParsedMessage and sends a message back to the sender with a message containing the description for the specified plugin
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
                fullMsg.Append("Please specify which plugin you would like information about.");
            }
            else
            {
                PluginDAO plugin = controller.RetrievePlugin(message.Arguments[0]);
                if ((controller.GetAllEnabledGroupPlugins(message.Group.GroupID)).Contains(plugin))
                {
                    fullMsg.Append(plugin.Name);
                    fullMsg.Append(": ");
                    fullMsg.Append(plugin.Description);
                }
                else
                {
                    fullMsg.Append("This plugin is not enabled for this group.");
                }
            }

            msg.FullMessage = fullMsg.ToString();
            msg.Reciever.Add(message.Sender.PhoneEmail);
            service.SendMessage(msg);
        }

        /// <summary>
        /// Stores metadata related to this plugin
        /// </summary>
        private PluginDAO _PluginDAO = new PluginDAO()
        {
            Access = PluginAccess.STANDARD,
            Description = "Plugin for reporting help messages",
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