using LuaInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    public static class LuaScriptingEngine
    {
        #region "LUA C# Callbacks"

        private static Dictionary<String, MethodBase> LuaCallbacks = new Dictionary<string, MethodBase>()
        {
            {"sendMessage", typeof(LUAAPI).GetMethod("SendMessage")}
        };

        #endregion

        /// <summary>
        /// Conatainer to hold all our information that we need for 
        /// </summary>
        public struct LUAPluginContainer
        {
            public LUAPlugin plugin;
            public AWatcherService service;
            public IDBController controller;
        }

        /// <summary>
        /// Dictionary that holds all running plugins
        /// </summary>
        private static Dictionary<String, LUAPluginContainer> register = new Dictionary<String, LUAPluginContainer>();
        private static Object registerLock = new Object();

        private static String hashVarName = "TTSpluginHashIdentifier";

        /// <summary>
        /// Registers the plugin with the 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="service"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static String registerPlugin(LUAPlugin plugin, AWatcherService service, IDBController controller, Lua pluginEngine = null)
        {
            if (plugin == null ||
                service == null ||
                controller == null)
            {
                throw new ArgumentNullException();
            }

            // 64 byte hash
            byte[] bytes = new byte[64];
            String hash = "";
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                lock (registerLock)
                {
                    // Generate a random secure hash
                    do
                    {
                        rng.GetBytes(bytes);
                        hash = BitConverter.ToString(bytes);
                    }
                    while (!LuaScriptingEngine.register.ContainsKey(hash));
                }
            }

            LUAPluginContainer container = new LUAPluginContainer();
            container.plugin = plugin;
            container.service = service;
            container.controller = controller;
            lock (registerLock)
            {
                LuaScriptingEngine.register.Add(hash, container);
            }

            if (pluginEngine != null)
            {
                pluginEngine.DoString("pluginHashIdentifier = \""+hash+"\"\n");
                LuaScriptingEngine.registerLUAPluginCallbacks(pluginEngine);
            }

            return hash;
        }

        public static bool unregisterPlugin(String hash)
        {
            lock (registerLock)
            {
                return LuaScriptingEngine.register.Remove(hash);
            }
        }

        public static LUAPluginContainer getPluginContainerByHash(String hash)
        {
            LUAPluginContainer plugin;
            lock (registerLock)
            {
                if (LuaScriptingEngine.register.TryGetValue(hash, out plugin))
                {
                    return plugin;
                }
            }

            // Not found
            throw new CouldNotFindException("Hash " + hash + " cannot be found.");
            //return null;
        }

        private static String generateLUAPluginCallbackWrapper(MethodBase function)
        {
            String name = function.Name;
            String userFriendlyName = name.TrimStart('_');
            ParameterInfo[] parameters = function.GetParameters();

            StringBuilder paramsb = new StringBuilder();
            // Add all parameters
            bool first = true;
            foreach (ParameterInfo parameter in parameters)
            {
                if (!first)
                {
                    paramsb.Append(", ");
                }
                paramsb.Append(parameter.Name);

                if (first)
                    first = false;
            }
            String paramString = paramsb.ToString();

            StringBuilder sb = new StringBuilder();
            // Yeah, I do appends here because it's easier to read... so sue me.
            sb.Append(@"function " + userFriendlyName + " (" + paramString + ") \n");
            if (parameters.Length > 0)
            {
                sb.Append(@"    return " + name + "(" + hashVarName + ", " + paramString + ") \n");
            }
            else
            {
                sb.Append(@"    return " + name + "(" + hashVarName + ") \n");
            }
            sb.Append(@"end \n");
            return sb.ToString();
        }

        private static void registerLUAPluginCallbacks(Lua pluginEngine)
        {
            foreach (KeyValuePair<String, MethodBase> entry in LuaCallbacks)
            {
                pluginEngine.RegisterFunction(entry.Key, null, entry.Value);
                pluginEngine.DoString(LuaScriptingEngine.generateLUAPluginCallbackWrapper(entry.Value));
            }
        }
    }
}
