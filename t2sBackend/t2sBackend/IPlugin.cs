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
        /// Stores metadata related to this plugin
        /// </summary>
        PluginDAO PluginDAO
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
