using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public interface IPlugin
    {

        t2sBackend.PluginAccess Access
        {
            get;
            set;
        }

        string Command
        {
            get;
            set;
        }

        string HelpText
        {
            get;
            set;
        }

        bool IsDisabled
        {
            get;
            set;
        }

        bool IsHidden
        {
            get;
            set;
        }

        void Run(ParsedMessage message);
    }
}
