using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{

    public class ParsedMessage
    {
        
        /// <summary>
        /// the UserDA) which sent the message
        /// </summary>
        public UserDAO Sender
        {
            get;
  
            set;



        }


        /// <summary>
        /// contains the group the message is for
        /// </summary>
        public GroupDAO Group
        {
            get;
            
            set;
            
        }


        /// <summary>
        /// contains arg for command, each arg is a entry in list. will be space delimited when arrives from a Message
        /// </summary>
        public List<string> Message
        {
            /// <summary>
            /// getter for the List of string called Message
            /// </summary>
            get;


            set;
           
        }

        /// <summary>
        /// entire message unchanged from user expect for group id having been removed
        /// </summary>
        public string ContentMessage
        {
            get;
            
            set;
            
        }

        /// <summary>
        /// the command that is passed from a Message. Will be space delimited in Message
        /// </summary>
        public string PluginMessage
        {
            get;

            set;
        }
    }
}
