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
        }
    }
}
