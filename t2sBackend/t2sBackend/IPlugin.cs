using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    /// <summary>
    /// Backbone for all plug-ins to be used
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Controls what the plug-in has access to in the system
        /// </summary>
        t2sBackend.PluginAccess Access
        {
            get;
            set;
        }

        /// <summary>
        /// Command that identifies plug-in
        /// </summary>
        string Command
        {
            get;
            set;
        }

        /// <summary>
        /// Description of plug-in
        /// </summary>
        string HelpText
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the plug-in will be available to the users
        /// </summary>
        bool IsDisabled
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the plug-in will be displayed to the users
        /// </summary>
        bool IsHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Intializes the plug-in thread
        /// </summary>
        /// <param name="message"></param>
        /// <param name="service"></param>
        void Run(ParsedMessage message, AWatcherService service);
    }
}
