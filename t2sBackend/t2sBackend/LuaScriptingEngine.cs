using LuaInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    public static class LuaScriptingEngine
    {
        #region "LUA C# Callbacks"

        private static Dictionary<String, System.Reflection.MethodBase> LuaCallbacks = new Dictionary<string, System.Reflection.MethodBase>()
        {
            {"sendMessage", typeof(TextAPI).GetMethod("SendMessage")}
        };

        #endregion

        /// <summary>
        /// Conatainer to hold all our information that we need for 
        /// </summary>
        private struct LUAPluginContainer
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

        private static LUAPluginContainer getPluginContainerByHash(String hash)
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

        private static String generateLUAPluginCallbackWrappers(Lua pluginEngine)
        {

            return "";
        }

        private static void registerLUAPluginCallbacks(Lua pluginEngine)
        {
            
            pluginEngine.RegisterFunction("sendMessage", null, );

        }
    }
}
