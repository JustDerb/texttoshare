using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    /// <summary>
    /// Class which handles errors with plug-ins
    /// </summary>
    public class ErrorPlugin : IPlugin
    {            
        /// <summary>
        /// Takes in the ParsedMessage and sends a message back to the sender with a specified detailed message about the error
        /// </summary>
        /// <param name="message">Message in error to be handled</param>
        /// <param name="service">Service to send/recieve messages through</param>
        public void Run(ParsedMessage message, AWatcherService service, IDBController controller)
        {
            Message msg = new Message();
            msg.FullMessage = message.ContentMessage;
            msg.Reciever.Add(message.Sender.PhoneEmail);
            service.SendMessage(msg);
        }

        /// <summary>
        /// Stores metadata related to this plugin
        /// </summary>
        private PluginDAO _PluginDAO = new PluginDAO()
        {
            Access = PluginAccess.STANDARD,
            Description = "Plugin for reporting error messages",
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