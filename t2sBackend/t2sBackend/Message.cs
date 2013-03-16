using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    /// <summary>
    /// message contains the group tag as first part of message delimited by "." then command and args which are both space delimited 
    /// </summary>
    public class Message
    {
        public string FullMessage
        {
            get;
            set;
        }

        public string Sender
        {
            get;
            set;
        }

        public List<string> Reciever
        {
            get;
            set;
        }
    }
}
