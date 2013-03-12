using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace t2sBackend
{
    /// <summary>
    /// class which parses a message and creates a new Parsed message.
    /// </summary>
    public static class MessageParser
    {
        private static readonly char delimiter='.';

        /// <summary>
        /// takes a Message and create a ParsedMessage from it and adds it to the queue
        /// </summary>
        /// <param name="message"></param>
        public static ParsedMessage Parse(Message message, IDBController controller)
        {
             String expression = @"^([0-9a-zA-Z]{3,6})\"+delimiter+@"(\w+)\"+delimiter+@"(.*)$";

             Regex reg = new Regex(expression);
             MatchCollection m = reg.Matches(message.FullMessage);

             String grouptag = m[0].Groups[0].Value;
            
             String command = m[0].Groups[1].Value;
            //check grouptag for being stop
            if(String.Equals("stop", grouptag, StringComparison.CurrentCultureIgnoreCase)){
                command = "stop";
            }
             String args = m[0].Groups[2].Value;

             ParsedMessage parsed = new ParsedMessage();

            //set parsed message properties
             parsed.Group = controller.RetrieveGroup(grouptag);
             parsed.Sender = controller.RetrieveUser(message.Sender);
             parsed.PluginMessage = command;
             parsed.Message[0] = command;
             parsed.ContentMessage = command+" "+args;
             // get and add args to message
             String[]  listContent = args.Split(' ');
             for(int i=0; i<listContent.Length; i++){
                 parsed.Message[i+1] = listContent[i];
             }

             return parsed;
            
        }
    }
}
