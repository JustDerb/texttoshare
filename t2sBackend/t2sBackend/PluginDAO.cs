using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t2sBackend
{
    public class PluginDAO
    {
        public int? PluginID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool IsDisabled
        {
            get;
            set;
        }

        public string VersionNum
        {
            get;
            set;
        }

        public int OwnerID
        {
            get;
            set;
        }

        public PluginAccess Access
        {
            get;
            set;
        }

        public string HelpText
        {
            get;
            set;
        }
    }
}
