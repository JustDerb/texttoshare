﻿using LuaInterface;
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

        /// <summary>
        /// Conatainer to hold all our information that we need for 
        /// </summary>
        public struct LUAPluginContainer
        {
            public LUAPlugin plugin;
            public AWatcherService service;
            public IDBController controller;
            public ParsedMessage message;
            public Dictionary<String, UserDAO> hashToUser;
            public Dictionary<UserDAO, String> userToHash;
        }

        /// <summary>
        /// Dictionary that holds all running plugins
        /// </summary>
        private static Dictionary<String, LUAPluginContainer> register = new Dictionary<String, LUAPluginContainer>();
        private static Object registerLock = new Object();

        private static String hashVarName = "TTSpluginHashIdentifier";

        private static String generateUniqueHash<T>(Dictionary<String, T> dict)//, T value)
        {
            //if (dict.ContainsValue(value))
            //    throw new EntryAlreadyExistsException();

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
                    while (dict.ContainsKey(hash));
                }
            }
            return hash;
        }

        /// <summary>
        /// Registers the plugin with the 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="service"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static String registerPlugin(LUAPlugin plugin, ParsedMessage message, AWatcherService service, IDBController controller, Lua pluginEngine = null)
        {
            if (plugin == null ||
                service == null ||
                controller == null)
            {
                throw new ArgumentNullException();
            }

            LUAPluginContainer container = new LUAPluginContainer();

            String hash = generateUniqueHash<LUAPluginContainer>(LuaScriptingEngine.register);
            container.plugin = plugin;
            container.service = service;
            container.controller = controller;
            container.message = message;
            container.hashToUser = new Dictionary<string, UserDAO>();
            container.userToHash = new Dictionary<UserDAO, string>();
            // Populate our tables
            foreach (UserDAO user in message.Group.Users)
            {
                String userHash = generateUniqueHash<UserDAO>(container.hashToUser);
                if (!container.hashToUser.ContainsKey(userHash))
                    container.hashToUser.Add(userHash, user);
                if (!container.userToHash.ContainsKey(user))
                    container.userToHash.Add(user, userHash);
            }
            foreach (UserDAO user in message.Group.Moderators)
            {
                String modHash = generateUniqueHash<UserDAO>(container.hashToUser);
                if (!container.hashToUser.ContainsKey(modHash))
                    container.hashToUser.Add(modHash, user);
                if (!container.userToHash.ContainsKey(user))
                    container.userToHash.Add(user, modHash);
            }
            String ownerHash = generateUniqueHash<UserDAO>(container.hashToUser);
            if (!container.hashToUser.ContainsKey(ownerHash))
                container.hashToUser.Add(ownerHash, message.Group.Owner);
            if (!container.userToHash.ContainsKey(message.Group.Owner))
                container.userToHash.Add(message.Group.Owner, ownerHash);

            lock (registerLock)
            {
                LuaScriptingEngine.register.Add(hash, container);
            }

            if (pluginEngine != null)
            {
                pluginEngine.DoString(hashVarName + " = \""+hash+"\"\n");
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

        private static String generateLUAPluginCallbackWrapper(String luaFunctionName, MethodBase function)
        {
            String name = function.Name;
            ParameterInfo[] parameters = function.GetParameters();

            StringBuilder paramsb = new StringBuilder();
            // Add all parameters
            bool first = true;
            // Skip the first parameter (we enter the hash)
            for (int p = 1; p < parameters.Length; ++p)
            {
                ParameterInfo parameter = parameters[p];
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
            sb.Append("function " + luaFunctionName + " (" + paramString + ") \n");
            if (parameters.Length > 1)
            {
                sb.Append("    return " + name + "(" + hashVarName + ", " + paramString + ") \n");
            }
            else
            {
                sb.Append("    return " + name + "(" + hashVarName + ") \n");
            }
            sb.Append("end \n");
            return sb.ToString();
        }

        private static void registerLUAPluginCallbacks(Lua pluginEngine)
        {
            foreach (KeyValuePair<String, MethodBase> entry in LUAAPI.LuaCallbacks)
            {
                if (entry.Value != null)
                {
                    pluginEngine.RegisterFunction(entry.Value.Name, null, entry.Value);
                    pluginEngine.DoString(LuaScriptingEngine.generateLUAPluginCallbackWrapper(entry.Key, entry.Value));
                }
                else
                {
                    // This will get called a lot, so lets not log it, just print out to console
                    Console.WriteLine(entry.Value + " is not a valid API call!");
                }
            }
        }
    }
}
