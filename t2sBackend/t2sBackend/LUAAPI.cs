using System;
using System.Collections.Generic;
using System.Linq;
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
        public readonly int MAX_TEXT_SIZE_7BIT  = 160;
        public readonly int MAX_TEXT_SIZE_8BIT  = 140;
        public readonly int MAX_TEXT_SIZE_16BIT = 70;

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

        public static void __SendMessage(String hash, String to, String message)
        {
            LuaScriptingEngine.LUAPluginContainer container = LuaScriptingEngine.getPluginContainerByHash(hash);
            
        }
    }
}
