using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t2sBackend
{
    public class PluginDAO
    {
        public int PluginID
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

        public bool isDisabled
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

        private UserDAO Owner
        {
            get;
            set;
        }
    }
}
