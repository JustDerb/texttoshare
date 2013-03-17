using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class LUAPlugin : IPlugin
    {
        public void Run(ParsedMessage message, AWatcherService service)
        {
            throw new NotImplementedException();
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
