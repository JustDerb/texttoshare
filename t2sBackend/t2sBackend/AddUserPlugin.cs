using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class AddUserPlugin : IPlugin
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


        public void Run(ParsedMessage message, AWatcherService service, t2sDbLibrary.IDBController controller)
        {
            throw new NotImplementedException();
        }
    }
}
