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
            // Fix up the string a little
            message.FullMessage = message.FullMessage.Trim();

            ParsedMessage parsed = new ParsedMessage();
            TryFindSender(parsed, message, controller);

            parsed.Command = FindCommand(message.FullMessage, controller);
            parsed.ContentMessage = FindArguments(message.FullMessage, controller);
            if (!parsed.ContentMessage.Trim().Equals(String.Empty))
            {
                parsed.Arguments.AddRange(parsed.ContentMessage.Split(' '));
            }

            String groupId = FindGroup(message.FullMessage, controller);

            //Strip out any leading, trailing delimiters
            groupId = groupId.TrimStart(new char[] { MessageParser.delimiter });
            groupId = groupId.TrimEnd(new char[] { MessageParser.secondDelimiter });

            TryFindGroup(parsed, groupId, controller);        
            
            // Set our type flag (by importance)
            if (parsed.Command.Equals("stop", StringComparison.OrdinalIgnoreCase)) {
                parsed.Type = ParsedMessage.ContentMessageType.STOP;
            }
            else if (parsed.Sender != null &&
                parsed.Sender.IsBanned)
            {
                parsed.Type = ParsedMessage.ContentMessageType.BAN;
            }
            else if (parsed.Command.Equals("register", StringComparison.OrdinalIgnoreCase))
            {
                // Since they are not in the database, 
                // we need to make a temp UserDAO as the sender
                // and populate their phone email for the Register plugin
                parsed.Sender = new UserDAO();
                parsed.Sender.PhoneEmail = message.Sender;
                parsed.Type = ParsedMessage.ContentMessageType.REGISTER;
            }
            else if (parsed.Sender != null &&
                parsed.Sender.IsSuppressed)
            {
                parsed.Type = ParsedMessage.ContentMessageType.SUPPRESS;
            }
            else if (parsed.Sender == null) 
            {
                parsed.Type = ParsedMessage.ContentMessageType.NO_SENDER;
            }
            else if (parsed.Command.Equals(String.Empty))
            {
                parsed.Type = ParsedMessage.ContentMessageType.NO_COMMAND;
            }
            else if (parsed.Group == null)
            {
                parsed.Type = ParsedMessage.ContentMessageType.NO_GROUP;
            }
            else
            {
                parsed.Type = ParsedMessage.ContentMessageType.VALID;
            }

            return parsed;
        }

        private static void TryFindSender(ParsedMessage parsed, Message message, IDBController controller)
        {
            try
            {
                parsed.Sender = controller.RetrieveUserByPhoneEmail(message.Sender);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is CouldNotFindException)
                {
                    parsed.Sender = null;
                }
                else
                {
                    // Not an exception we deal with, throw so we don't hide bugs
                    throw;
                }
            }
        }

        private static void TryFindGroup(ParsedMessage parsed, String groupId, IDBController controller)
        {
            try
            {
                parsed.Group = controller.RetrieveGroup(groupId);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException ||
                    ex is CouldNotFindException)
                {
                    parsed.Group = null;
                }
                else
                {
                    // Not an exception we deal with, throw so we don't hide bugs
                    throw;
                }
            }
        }

        private static String FindCommand(String message, IDBController controller)
        {
            // Try and find our plugin at the start
            List<PluginDAO> plugins = controller.RetrieveEnabledPlugins();
            foreach (PluginDAO plugin in plugins)
            {
                if (message.StartsWith(plugin.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return plugin.Name;
                }
            }

            // Get our best guess by seeing if there is the first delimeter
            int delimiter1Index = message.IndexOf(MessageParser.delimiter);
            if (delimiter1Index < 0)
            {
                // No group specified, so take up until second delimeter
                int delimiter2Index = message.IndexOf(MessageParser.secondDelimiter);
                if (delimiter2Index < 0)
                {
                    // No group stuff so just return the whole message
                    return message;
                }
                else
                {
                    return message.Substring(0, delimiter2Index).Trim();
                }
            }
            else
            {
                // Do we have spaces? (Plugins shouldn't have spaces)
                string pluginMaybe = message.Substring(0, delimiter1Index).Trim();

                if (pluginMaybe.Contains(' '))
                {
                    return "";
                }
                else
                {
                    // We found an plugin! (Hopefully)
                    return pluginMaybe.Trim();
                }
            }
        }

        private static String FindGroup(String message, IDBController controller)
        {
            Tuple<int, int> indexes = MessageParser.FindGroupRange(message, controller);
            if (indexes == null)
            {
                return "";
            }
            else
            {
                if (indexes.Item2 <= indexes.Item1)
                {
                    return "";
                }
                message = message.Substring(indexes.Item1, indexes.Item2 - indexes.Item1 + 1).Trim();
                return message;
            }
        }

        private static Tuple<int, int> FindGroupRange(String message, IDBController controller)
        {
            // Look for our first delimiter
            int firstDelimiterIndex = message.IndexOf(MessageParser.delimiter);
            if (firstDelimiterIndex < 0)
            {
                return null;
            }
            firstDelimiterIndex++;
            // Return if the delimiter is the last character
            if (firstDelimiterIndex >= message.Length)
            {
                return null;
            }
            else
            {
                message = message.Substring(firstDelimiterIndex);
                int secondDelimiterIndex = message.IndexOf(MessageParser.secondDelimiter);
                if (secondDelimiterIndex < 0)
                {
                    return new Tuple<int, int>(firstDelimiterIndex, firstDelimiterIndex + message.Length - 1);
                }

                // Grab that section
                return new Tuple<int, int>(firstDelimiterIndex, firstDelimiterIndex + secondDelimiterIndex);
            }
        }

        private static String FindArguments(String message, IDBController controller)
        {
            // Find the arguments right after the group ID
            Tuple<int, int> indexes = MessageParser.FindGroupRange(message, controller);
            if (indexes == null)
            {
                // Try and find the second delimeter
                int secondIndex = message.IndexOf(MessageParser.secondDelimiter);
                if (secondIndex < 0)
                {
                    return "";
                }
                else
                {
                    if (secondIndex + 1 < message.Length)
                    {
                        return message.Substring(secondIndex + 1, message.Length - (secondIndex + 1));
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            else
            {
                // Don't include last character
                int argStartIndex = indexes.Item2 + 1;
                if (argStartIndex >= message.Length)
                {
                    return "";
                }
                else
                {
                    message = message.Substring(argStartIndex).Trim();
                    return message;
                }
            }
        }
    }
}
