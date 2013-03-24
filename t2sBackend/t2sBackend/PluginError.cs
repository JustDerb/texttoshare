using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    /// <summary>
    /// Class which handles errors with plug-ins
    /// </summary>
    public class PluginError : IPlugin
    {
        /// <summary>
        /// Controls access to the system
        /// </summary>
        public PluginAccess Access
        {
            get
            {
                return PluginAccess.MODERATOR;
            }
            set { throw new InvalidOperationException("This variable cannot be set");  }
        }

        /// <summary>
        /// Returns command to call Error plug-in
        /// </summary>
        public string Command
        {
            get
            {
                return "ERROR";
            }
            set { throw new InvalidOperationException("This variable cannot be set"); }
        }

        /// <summary>
        /// Returns information about the Error plug-in
        /// </summary>
        public string HelpText
        {
            get
            {
                return "Notifies User with a specified detail message";
            }
            set { throw new InvalidOperationException("This variable cannot be set"); }
        }

        /// <summary>
        /// Returns that Error plug-in is not disabled
        /// </summary>
        public bool IsDisabled
        {
            get
            {
                return false;
            }
            set { throw new InvalidOperationException("This variable cannot be set"); }
        }

        /// <summary>
        /// Returns that the Error plug-in is not hidden
        /// </summary>
        public bool IsHidden
        {
            get
            {
                return false;
            }
            set { throw new InvalidOperationException("This variable cannot be set"); }
        }

        /// <summary>
        /// Takes in the ParsedMessage and sends a message back to the sender with a specified detailed message about the error
        /// </summary>
        /// <param name="message">Message in error to be handled</param>
        /// <param name="service">Service to send/recieve messages through</param>
        public void Run(ParsedMessage message, AWatcherService service)
        {
            Message msg = new Message();
            msg.FullMessage = message.ContentMessage;
            msg.Reciever.Add(message.Sender.PhoneEmail);
            service.SendMessage(msg);
        }


        public bool AlwaysEnabled
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public PluginDAO PluginDAO
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}