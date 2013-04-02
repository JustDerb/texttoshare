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
        public static readonly char delimiter='.';
       
        /// <summary>
        /// takes a Message and create a ParsedMessage from it and adds it to the queue
        /// </summary>
        /// <param name="message"></param>
        public static ParsedMessage Parse(Message message, IDBController controller)
        {
            //variable to create a parsedMessage
            String grouptag = "";
            String command ="";
            String args ="";

            //both delimiter are required.
             String expression = @"^([0-9a-zA-Z]{3,6})\"+delimiter+@"(\w+)\"+delimiter+@"(.*)$";
            //need to include the delimiter
             //to test in rublar ^([0-9a-zA-Z]{3,6})\.(\w+)\.(.*)$

             Regex reg = new Regex(expression);
             MatchCollection m = reg.Matches(message.FullMessage);
            //check for array size of m being 0, ( message doesn't match the regix)
             if (m.Count == 0)
             {
                 //assuming they had a . before the period and just forgot group tag
                 String noGroupTagExpress = @"^"+delimiter+@"(\w+)\"+delimiter+@"(.*)$";
                 Regex noGroupReg = new Regex(noGroupTagExpress);
                 MatchCollection c = noGroupReg.Matches(message.FullMessage);
                 //check to make sure period is there
                 if (message.FullMessage.IndexOf('.') == 0)
                 {
                     //check to for . before the command
                     //check bounds to make for null pointers
                     if(c.Count>0 && c[0].Groups[1] !=null){
                        grouptag = "";
                         //take out the period before the command
                        String[] commandTemp =  c[0].Groups[1].Value.Split('.');
                        command = commandTemp[1];
                        args = c[0].Groups[1].Value;
                     }
                //completely wrong input, putting all of message into the parsedMessage content
                 }else{
                    grouptag = "";
                    command = "";
                    args = message.FullMessage;
                 }

             }
             //correct input
             else
             {
                 grouptag = m[0].Groups[1].Value;
                 command = m[0].Groups[2].Value;
                 //check grouptag for being stop
                 if (String.Equals("stop", grouptag, StringComparison.CurrentCultureIgnoreCase))
                 {
                     command = "stop";
                 }
                 if (m[0].Groups[3] != null)
                 {
                     args = m[0].Groups[3].Value;
                 }
                 else
                 {
                     args = "";
                 }
                 
             }


             GroupDAO tempGroup;
             UserDAO tempUser;
             try
             {
                 tempGroup = controller.RetrieveGroup(grouptag);
             }
             catch (CouldNotFindException e)
             {
                 tempGroup = null;

             }
             try
             {
                 tempUser = controller.RetrieveUser(message.Sender);
             }
             catch (CouldNotFindException e)
             {
                 tempUser = null;
             }

             ParsedMessage parsed = new ParsedMessage()
             {
                 //set parsed message properties
                 
                 Group = tempGroup,
                 Sender =tempUser,
                 Command = command,
                 ContentMessage = command + " " + args,
             };

             // get and add args to message
             args = args.Trim();
             if (!args.Equals(String.Empty))
             {
                 parsed.Arguments.AddRange(args.Split(' '));
             }
             else
             {
                 args = "";
             }

             return parsed;
            
        }
    }
}
