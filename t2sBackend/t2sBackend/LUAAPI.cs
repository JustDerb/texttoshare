using LuaInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            {"GetMessageCommand", typeof(LUAAPI).GetMethod("__GetMessageCommand")},
            {"GetMessageArgumentString", typeof(LUAAPI).GetMethod("__GetMessageArgumentString")},
            {"GetMessageArgumentList", typeof(LUAAPI).GetMethod("__GetMessageArgumentList")},
            {"SendMessage", typeof(LUAAPI).GetMethod("__SendMessage")},
            {"GetUserIdList", typeof(LUAAPI).GetMethod("__GetUserIdList")},
            {"GetModeratorIdList", typeof(LUAAPI).GetMethod("__GetModeratorIdList")},
            {"SetValue", typeof(LUAAPI).GetMethod("__SetValue")},
            {"GetValue", typeof(LUAAPI).GetMethod("__GetValue")},
            {"GetOwnerId", typeof(LUAAPI).GetMethod("__GetOwnerId")},
            {"GetSenderId", typeof(LUAAPI).GetMethod("__GetSenderId")},
            {"GetUserFirstName", typeof(LUAAPI).GetMethod("__GetUserFirstName")},
            {"GetUserLastName", typeof(LUAAPI).GetMethod("__GetUserLastName")},
            {"GetUsername", typeof(LUAAPI).GetMethod("__GetUsername")},
            {"GetUserIsSuppressed", typeof(LUAAPI).GetMethod("__GetUserIsSuppressed")},
            {"GetUserIsBanned", typeof(LUAAPI).GetMethod("__GetUserIsBanned")},
            {"GetUserCarrier", typeof(LUAAPI).GetMethod("__GetUserCarrier")},
            {"GetGroupDescription", typeof(LUAAPI).GetMethod("__GetGroupDescription")},
            {"GetGroupName", typeof(LUAAPI).GetMethod("__GetGroupName")},
            {"GetGroupTag", typeof(LUAAPI).GetMethod("__GetGroupTag")},
            {"HTTPDownloadText", typeof(LUAAPI).GetMethod("__HTTPDownloadText")}
        };

        #endregion

        public static void __DebugPrint(String hash, String debugMessage)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            Console.Out.WriteLine(container.plugin.PluginDAO.Name + ": " + debugMessage);
        }

        public static string __GetMessageCommand(String hash)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            return container.message.Command;
        }

        public static string __GetMessageArgumentString(String hash)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            return container.message.ContentMessage;
        }

        public static LuaTable __GetMessageArgumentList(String hash)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            return ToLuaTable(container.message.Arguments, container.plugin.LuaEngine);
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

        public static string __GetGroupTag(String hash)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            return container.message.Group.GroupTag;
        }

        public static string __GetGroupName(String hash)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            return container.message.Group.Name;
        }

        public static string __GetGroupDescription(String hash)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            return container.message.Group.Description;
        }

        public static string __GetUsername(String hash, String userhash)
        {
            if (userhash == null || userhash.Equals(String.Empty))
                return "";

            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            UserDAO user = container.hashToUser[userhash];
            String retValue = "";
            if (user != null)
                retValue = user.UserName;

            return retValue;
        }

        public static string __GetUserFirstName(String hash, String userhash)
        {
            if (userhash == null || userhash.Equals(String.Empty))
                return "";

            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            UserDAO user = container.hashToUser[userhash];
            String retValue = "";
            if (user != null)
                retValue = user.FirstName;

            return retValue;
        }

        public static string __GetUserLastName(String hash, String userhash)
        {
            if (userhash == null || userhash.Equals(String.Empty))
                return "";

            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            UserDAO user = container.hashToUser[userhash];
            String retValue = "";
            if (user != null)
                retValue = user.LastName;

            return retValue;
        }

        public bool __GetUserIsSuppressed(String hash, String userhash)
        {
            if (userhash == null || userhash.Equals(String.Empty))
                return false;

            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            UserDAO user = container.hashToUser[userhash];
            bool retValue = false;
            if (user != null)
                retValue = user.IsSuppressed;

            return retValue;
        }

        public bool __GetUserIsBanned(String hash, String userhash)
        {
            if (userhash == null || userhash.Equals(String.Empty))
                return false;

            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            UserDAO user = container.hashToUser[userhash];
            bool retValue = false;
            if (user != null)
                retValue = user.IsBanned;

            return retValue;
        }

        public string __GetUserCarrier(String hash, String userhash)
        {
            if (userhash == null || userhash.Equals(String.Empty))
                return "";

            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            UserDAO user = container.hashToUser[userhash];
            string retValue = "";
            if (user != null)
                retValue = System.Enum.GetName(typeof(PhoneCarrier), user.Carrier);

            return retValue;
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

        public static string __HTTPDownloadText(String hash, String URL)
        {
            if (URL == null
                || !URL.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                return null;

            string retValue = "";
            try
            {
                WebRequest req = HttpWebRequest.Create(URL);
                req.Method = "HEAD";
                using (WebResponse resp = req.GetResponse())
                {
                    int ContentLength;
                    // Must be text, and we need to be able to parse the length to int
                    if (resp.Headers.Get("Content-Type").IndexOf("text", StringComparison.OrdinalIgnoreCase) >= 0
                        && int.TryParse(resp.Headers.Get("Content-Length"), out ContentLength))
                    {
                        // File size limit
                        if (ContentLength < 100000)
                        {
                            using (WebClient client = new WebClient())
                            {
                                retValue = client.DownloadString(URL);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage("__HTTPDownloadText: [" + URL + "] " + ex.Message, LoggerLevel.WARNING);
                retValue = null;
            }
            return retValue;
        }
    }
}
