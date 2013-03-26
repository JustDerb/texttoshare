using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    public static class LuaScriptingEngine
    {
        public static void runPlugin(LUAPlugin plugin, ParsedMessage message, AWatcherService service)
        {
            // LUAPlugin is self-contained, so just run it.
            plugin.Run(message, service);
        }
    }
}
