using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using t2sDbLibrary;

namespace t2sBackend
{
    ///handle for suppress
    
    /// <summary>
    /// class which parses a message and creates a new Parsed message.
    /// </summary>
    public static class MessageParser
    {
        public static readonly char delimiter='@';
        public static readonly char secondDelimiter = ' ';
       
        /// <summary>
        /// takes a Message and create a ParsedMessage from it and adds it to the queue
        /// </summary>
        /// <param name="message"></param>
        public static ParsedMessage Parse(Message message, IDBController controller)
        {
            String[] cmdArray = message.FullMessage.Split(delimiter);
            String command = cmdArray[0];
            String[] idArray = cmdArray[1].Split(secondDelimiter);
            String groupId = idArray[0];
            String args = "";




            ParsedMessage parsed = new ParsedMessage();
            for (int i = 1; i < idArray.Length; i++)
            {
                parsed.Arguments.Add(idArray[i]);
                args = args + idArray[i];
            }
            parsed.Command = command;
            parsed.ContentMessage = args;
            try
            {
                parsed.Group = controller.RetrieveGroup(groupId);
            }
            catch (Exception)
            {
                parsed.Group = null;
            }
            try
            {
                parsed.Sender = controller.RetrieveUserByPhoneEmail(message.Sender);
            }
            catch (Exception)
            {
                parsed.Sender = null;
            }
             return parsed;
            
        }
    }
}
