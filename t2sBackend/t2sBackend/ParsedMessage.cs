using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{

    public class ParsedMessage
    {

        /// <summary>
        /// Content Message Types
        /// </summary>
        public enum ContentMessageType
        {
            /// <summary>
            /// User wants to stop recieving messages 
            /// from the system.
            /// </summary>
            STOP,
            /// <summary>
            /// User is banned from the system (drop message)
            /// </summary>
            BAN,
            /// <summary>
            /// User is suppressed from the system (Send note back to user about it)
            /// </summary>
            SUPPRESS,
            /// <summary>
            /// Error parsing message in some way
            /// </summary>
            ERROR,
            /// <summary>
            /// Totally not a invalid message
            /// </summary>
            VALID,
            /// <summary>
            /// No Group Specified/Invalid Group Format
            /// </summary>
            NO_GROUP

        }

        /// <summary>
        /// The type of message this is
        /// </summary>
        public ContentMessageType Type
        {
            get;
            set;
        }
        
        /// <summary>
        /// The UserDAO which sent the message
        /// </summary>
        public UserDAO Sender
        {
            get;
            set;
        }


        /// <summary>
        /// Contains the group the message is for
        /// </summary>
        public GroupDAO Group
        {
            get;
            set;
        }


        /// <summary>
        /// Contains arg for command, each arg is a entry in list. will be space delimited when arrives from a Message
        /// </summary>
        public List<string> Arguments
        {
            get;
            set;
        }

        /// <summary>
        /// The arguments from the users
        /// </summary>
        public string ContentMessage
        {
            get;
            set;
        }

        /// <summary>
        /// The command that is passed from a Message. Will be space delimited in Message
        /// </summary>
        public string Command
        {
            get;
            set;
        }

        public ParsedMessage()
        {
            this.Arguments = new List<string>();
            this.Type = ContentMessageType.ERROR;
        }
    }
}
