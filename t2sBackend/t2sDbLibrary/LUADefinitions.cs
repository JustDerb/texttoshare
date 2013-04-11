using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t2sDbLibrary
{
    public class LUADefinitions
    {
        /// <summary>
        /// Location within the filesystem that holds all our Lua Scripts
        /// </summary>
        protected static readonly String LuaScriptLocation = @"C:\LUAPlugins\";

        /// <summary>
        /// Extension for Lua scripts
        /// </summary>
        public static readonly String LuaExtension = ".lua";
    }
}
