using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace t2sDbLibrary
{
    public class LUADefinitions
    {
        static LUADefinitions()
        {
            LuaFolderName = "LUAPLUGINS";
            LuaExtension = ".lua";

            // This needs to be called last because it relies on the above variables
            LuaScriptLocation = getLuaDirectory();
        }

        public static readonly int DisablePluginAboveErrorCount = 10;

        /// <summary>
        /// Location within the filesystem that holds all our Lua Scripts
        /// </summary>
        public static readonly String LuaScriptLocation;

        private static readonly String LuaFolderName;

        private static String getLuaDirectory()
        {
            List<String> topLevelProject = new List<string>()
            {
                "LUAPLUGINS",
                "t2sBackend",
                "t2sBackendWebSite"
            };

            // Start at our executing directory
            DirectoryInfo info = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (info != null)
            {
                int found = 0;
                DirectoryInfo[] folders = info.GetDirectories();
                foreach (DirectoryInfo folder in folders) 
                {
                    bool foundOne = false;
                    foreach (String findFolder in topLevelProject)
                    {
                        if (folder.Name.Equals(findFolder, StringComparison.OrdinalIgnoreCase))
                        {
                            foundOne = true;
                            break;
                        }
                    }

                    if (foundOne)
                    {
                        ++found;
                    }
                }
                if (found == topLevelProject.Count)
                    break;
                else
                    info = info.Parent;
            }

            info = info.CreateSubdirectory(LuaFolderName);

            return info.FullName;
        }

        /// <summary>
        /// Extension for Lua scripts
        /// </summary>
        public static readonly String LuaExtension;

        /// <summary>
        /// gets the default location of where a Lua Script will be loaded from
        /// </summary>
        /// <param name="name">Name of script</param>
        /// <returns></returns>
        public static String getLuaScriptLocation(String name)
        {
            // Make sure we are using the file extension
            name = name.Trim();
            if (!name.EndsWith(LUADefinitions.LuaExtension))
            {
                name += LUADefinitions.LuaExtension;
            }

            // TODO - Make this more elegant and use a File object
            String basePath = LUADefinitions.LuaScriptLocation.Trim().TrimEnd('\\');
            if (name == null)
                return basePath;
            name = name.Trim();
            if (name.Equals(String.Empty))
                return basePath;
            else if (name.StartsWith(@"/") || name.StartsWith(@"\"))
                return basePath + name;
            else
                return basePath + @"\" + name;
        }
    }
}
