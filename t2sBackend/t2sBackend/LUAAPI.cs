﻿using LuaInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using t2sDbLibrary;

namespace t2sBackend
{
    public class LUAAPI
    {
        public class APIException : Exception, ISerializable
        {
            public APIException() : base()
            {
            }
            public APIException(string message) : base(message)
            {
            }
            public APIException(string message, Exception inner) : base(message, inner)
            {
            }

            // This constructor is needed for serialization.
            protected APIException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        //160 7-bit characters, 140 8-bit characters, or 70 16-bit characters
        public static readonly int MAX_TEXT_SIZE_7BIT  = 160;
        public static readonly int MAX_TEXT_SIZE_8BIT = 140;
        public static readonly int MAX_TEXT_SIZE_16BIT = 70;

        /// <summary>
        /// Tries to find the user by username
        /// </summary>
        /// <param name="username">Username to fetch</param>
        /// <returns>A UserDAO, not null</returns>
        /// <exception cref="APIException">If user cannot be found</exception>
        private static UserDAO grabUserByUsername(LuaScriptingEngine.LUAPluginContainer container, String username)
        {
            UserDAO user = container.controller.RetrieveUserByUserName(username);
            if (user == null)
                throw new APIException("'" + username + "' is not a valid user.");
            return user;
        }

        /// <summary>
        /// Tries to find the user by email
        /// </summary>
        /// <param name="username">Email to fetch</param>
        /// <returns>A UserDAO, not null</returns>
        /// <exception cref="APIException">If user cannot be found</exception>
        private static UserDAO grabUserByEmail(LuaScriptingEngine.LUAPluginContainer container, String email)
        {
            UserDAO user = container.controller.RetrieveUserByUserName(email);
            if (user == null)
                throw new APIException("'" + email + "' is not a valid email.");
            return user;
        }

        /// <summary>
        /// Doc: http://stackoverflow.com/questions/14299634/luainterface-a-function-which-will-return-a-luatable-value
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="lua"></param>
        /// <returns></returns>
        private static LuaTable ToLuaTable(Dictionary<String, String> dict, Lua lua)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("return {");
            bool first = true;
            foreach (KeyValuePair<String, String> keyValue in dict) 
            {
                if (!first)
                    sb.Append(", ");
                sb.Append("[\"");
                sb.Append(keyValue.Key.Replace("\"", "\\\""));
                sb.Append("\"] = \"");
                sb.Append(keyValue.Value.Replace("\"", "\\\""));
                sb.Append("\"");

                if (first)
                    first = false;
            }
            sb.Append("} \n");
            LuaTable table = (LuaTable)lua.DoString(sb.ToString())[0];
            return table;
        }

        /// <summary>
        /// Doc: http://stackoverflow.com/questions/14299634/luainterface-a-function-which-will-return-a-luatable-value
        /// </summary>
        /// <param name="list"></param>
        /// <param name="lua"></param>
        /// <returns></returns>
        private static LuaTable ToLuaTable(List<String> list, Lua lua)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("return {");
            bool first = true;
            int index = 1;
            foreach (String value in list)
            {
                if (!first)
                    sb.Append(", ");
                sb.Append("[");
                sb.Append(index);
                sb.Append("] = \"");
                sb.Append(value.Replace("\"", "\\\""));
                sb.Append("\"\n ");

                if (first)
                    first = false;

                ++index;
            }
            sb.Append("} \n");
            LuaTable table = (LuaTable)lua.DoString(sb.ToString())[0];
            return table;
        }

        #region "LUA C# Callbacks"

        public static readonly Dictionary<String, MethodBase> LuaCallbacks = new Dictionary<string, MethodBase>()
        {
            //{"DebugPrint", typeof(LUAAPI).GetMethod("__DebugPrint")},
            {"SendMessage", typeof(LUAAPI).GetMethod("__SendMessage")},
            {"GetUserIdList", typeof(LUAAPI).GetMethod("__GetUserIdList")},
            {"GetModeratorIdList", typeof(LUAAPI).GetMethod("__GetModeratorIdList")},
            {"SetValue", typeof(LUAAPI).GetMethod("__SetValue")},
            {"GetValue", typeof(LUAAPI).GetMethod("__GetValue")},
            {"GetOwnerId", typeof(LUAAPI).GetMethod("__GetOwnerId")},
            {"GetSenderId", typeof(LUAAPI).GetMethod("__GetSenderId")}
        };

        #endregion

        public static void __DebugPrint(String hash, String debugMessage)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            Console.Out.WriteLine(container.plugin.PluginDAO.Name + ": " + debugMessage);
        }

        public static void __SendMessage(String hash, String toHash, String message)
        {
            if (message == null || message.Equals(String.Empty))
                return;

            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            UserDAO toUser = null;
            if (toHash != null)
                toUser = container.hashToUser[toHash];

            if (toUser != null)
            {
                Message msg = new Message(new string[1] { toUser.PhoneEmail }, message.Substring(0, LUAAPI.MAX_TEXT_SIZE_8BIT));
                container.service.SendMessage(msg);
            }
        }

        public static LuaTable __GetUserIdList(String hash)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            List<string> userIds = new List<string>();
            foreach (UserDAO user in container.message.Group.Users)
            {
                userIds.Add(container.userToHash[user]);
            }

            return ToLuaTable(userIds, container.plugin.LuaEngine);
        }

        public static LuaTable __GetModeratorIdList(String hash)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            List<string> userIds = new List<string>();
            foreach (UserDAO user in container.message.Group.Moderators)
            {
                userIds.Add(container.userToHash[user]);
            }

            return ToLuaTable(userIds, container.plugin.LuaEngine);
        }

        public static string __GetOwnerId(String hash)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            String ownerHash = container.userToHash[container.message.Group.Owner];
            return ownerHash;
        }

        public static string __GetSenderId(String hash)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            String senderHash = container.userToHash[container.message.Sender];
            return senderHash;
        }

        public static void __SetValue(String hash, String key, String value, String userhash)
        {
            if (key == null)
                return;

            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            UserDAO user = null;
            if (userhash != null)
                user = container.hashToUser[userhash];
            container.controller.UpdatePluginKeyValue(container.plugin.PluginDAO, key, value, user);
        }

        public static string __GetValue(String hash, String key, String userhash)
        {
            if (key == null)
                return null;

            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            UserDAO user = null;
            if (userhash != null)
                user = container.hashToUser[userhash];
            return container.controller.RetrievePluginValue(container.plugin.PluginDAO, key, user);
        }

        public static void __DebugPrint(String hash, String debugMessage)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            Console.Out.WriteLine(container.plugin.PluginDAO.Name + ": " + debugMessage);
        }
    }
}
