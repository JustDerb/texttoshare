using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class RemoveUserPlugin : IPlugin
    {
        public t2sDbLibrary.PluginDAO PluginDAO
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

        public void Run(ParsedMessage message, AWatcherService service)
        {
            throw new NotImplementedException();
        }
    }
}
