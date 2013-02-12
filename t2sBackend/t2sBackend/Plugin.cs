using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public abstract class APlugin : IPlugin
    {
        public abstract System.IO.File File
        {
            get;
            set;
        }
    }
}
