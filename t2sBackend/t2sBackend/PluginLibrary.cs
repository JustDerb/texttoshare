﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    /// <summary>
    /// Class to manage all plug-ins
    /// </summary>
    public class PluginLibrary
    {

        private MessageController messageController;
        private AWatcherService service;
        private IDBController idbController;

        private BackgroundWorker libraryThread;

        /// <summary>
        /// Constructs a list of plug-ins and initializes the library
        /// </summary>
        public PluginLibrary(MessageController messageController, AWatcherService Service, IDBController idbController)
        {
            this.messageController = messageController;
            this.service = Service;
            this.idbController = idbController;
        }


        /// <summary>
        /// Checks whether thread of plug-in library is running
        /// </summary>
        private volatile bool _Running;
        public bool Running
        {
            get
            {
                return _Running;
            }
            set
            {
                _Running = value;
            }
        }

        /// <summary>
        /// Grabs the next message from the controller
        /// </summary>
        /// <returns></returns>
        public ParsedMessage GetNextMessage()
        {
            return this.messageController.getNextMessage();
        }

        /// <summary>
        /// Begins the plug-in library thread
        /// </summary>
        public void Start()
        {
            if (!_Running)
            {
                _Running = true;
                this.libraryThread = new BackgroundWorker();
                this.libraryThread.DoWork += libraryThread_DoWork;
                this.libraryThread.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Handles the library and makes the appropriate plug-in with the passed information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void libraryThread_DoWork(object sender, DoWorkEventArgs e)
        {
            while (_Running)
            {
                // Will block until a new message has arrived
                ParsedMessage message = GetNextMessage();

                IPlugin plugin = new ErrorPlugin();
                
                bool doMessage = true;

                switch (message.Type)
                {
                    // Not a valid group ID
                    case ParsedMessage.ContentMessageType.NO_GROUP:
                        message.ContentMessage = INVALID_GROUP_MESSAGE;
                        break;
                    // User is banned
                    case ParsedMessage.ContentMessageType.BAN:
                        doMessage = false;
                        break;
                    // User is suppressed
                    case ParsedMessage.ContentMessageType.SUPPRESS:
                        message.ContentMessage = SUPPRESSED_USER_MESSAGE;
                        break;
                    case ParsedMessage.ContentMessageType.NO_COMMAND:
                        message.ContentMessage = INVALID_COMMAND_MESSAGE;
                        break;
                    case ParsedMessage.ContentMessageType.NO_SENDER:
                        message.ContentMessage = INVALID_SENDER_MESSAGE;
                        break;
                    case ParsedMessage.ContentMessageType.STOP:
                        message.ContentMessage = STOP_MESSAGE;
                        idbController.DeleteUser(message.Sender);
                        break;
                    case ParsedMessage.ContentMessageType.VALID:
                        break;
                }
                
                    
                    // Not a user within the given group
                if (!message.Group.Users.Contains(message.Sender) && 
                    !message.Group.Moderators.Contains(message.Sender) &&
                    !message.Group.Owner.Equals(message.Sender))
                {
                    message.ContentMessage = INVALID_USER_MESSAGE;
                }

                Boolean foundPlugin = false;
                if (doMessage)
                {
                    foreach (PluginDAO d in message.Group.EnabledPlugins)
                    {
                        if (d.Name.Equals(message.Command, StringComparison.OrdinalIgnoreCase))
                        {
                            // If the plugin can only be accessed by moderators and the calling user is not a moderator/owner
                            if (d.Access == PluginAccess.MODERATOR && 
                                !(message.Group.Moderators.Contains(message.Sender) || message.Group.Owner.Equals(message.Sender)))
                            {
                                message.ContentMessage = RESTRICTED_ACCESS_MESSAGE;
                            }
                            else
                            {
                                foundPlugin = true;
                                if (defaultPlugins.ContainsKey(d.Name.ToUpperInvariant()))
                                {
                                    plugin = defaultPlugins[d.Name.ToUpperInvariant()];
                                    break;
                                }
                                else
                                {
                                    try
                                    {
                                        plugin = new LUAPlugin(d);
                                    }
                                    catch (Exception ex)
                                    {
                                        // Fails if the file cannot be found
                                        Logger.LogMessage("LUA Plugin (" + d.Name + ") failed.  Cannot load plugin: " + ex.Message, LoggerLevel.SEVERE);
                                        message.ContentMessage = INVALID_COMMAND_MESSAGE;
                                    }
                                }
                            }
                            // Set the DAO for the plugin
                            plugin.PluginDAO = d;
                            break;
                        }
                    }
                }

                // No valid plugin was found for the given command
                if (!foundPlugin)
                {
                    message.ContentMessage = INVALID_PLUGIN_MESSAGE;
                }

                if (doMessage)
                {
                    // NOTE: Make sure thread is not disposed after running because it lost scope
                    BackgroundWorker pluginThread = new BackgroundWorker();
                    pluginThread.DoWork += pluginThread_DoWork;
                    Object[] parameters = new Object[] { plugin, message };
                    pluginThread.RunWorkerAsync(parameters);
                }
            }
        }

        void pluginThread_DoWork(object sender, DoWorkEventArgs e)
        {
            Object[] parameters = (Object[])e.Argument;
            IPlugin plugin = (IPlugin)parameters[0];
            ParsedMessage message = (ParsedMessage)parameters[1];

            // Do plugin work
            try
            {
                plugin.Run(message, this.service, this.idbController);
            }
            catch (Exception ex)
            {
                // Add logic to increment LUA Plugin Error count
                // And disable, if necessary
                Logger.LogMessage(plugin.GetType().ToString() + " - " + ex.Message, LoggerLevel.SEVERE);
            }
        }

        /// <summary>
        /// Terminates the plug-in library thread
        /// </summary>
        public void Stop()
        {
            if (_Running)
            {
                _Running = false;
            }
        }

        private readonly static Dictionary<string, IPlugin> defaultPlugins = new Dictionary<string, IPlugin>()
        {
            {"ERROR".ToUpperInvariant(), new ErrorPlugin()}, {"TEXTUSER".ToUpperInvariant(), new TextUserPlugin()},
            {"TEXTGROUP".ToUpperInvariant(), new TextGroupPlugin()}, {"LISTPLUGINS".ToUpperInvariant(), new ListEnabledPlugins()},
            {"REMOVEUSER".ToUpperInvariant(), new RemoveUserPlugin()}
        };
        
        // Messages to be sent back to sender when system throws an error or the commands are invalid.
        private static string INVALID_GROUP_MESSAGE = "Invalid group. Please check your message and try again.";
        private static string INVALID_USER_MESSAGE = "You are not a valid member of this group. Please check your message and try again.";
        private static string SUPPRESSED_USER_MESSAGE = "You have currently suppressed recieving messages. To disable, please reply, \"SUPPRESS OFF\"";
        private static string INVALID_PLUGIN_MESSAGE = "Invalid command. Please check your message and try again.";
        private static string RESTRICTED_ACCESS_MESSAGE = "You are not authorized to use this command. Please check with your group's owner and try again.";
        private static string INVALID_COMMAND_MESSAGE = "Invalid command. Please check your message and try again.";
        private static string INVALID_SENDER_MESSAGE = "Invalid Sender. Please check message or register with System to use services.";
        private static string STOP_MESSAGE = "You have been removed from the System. Thank you.";
    }
}
