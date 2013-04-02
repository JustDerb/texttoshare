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

        private MessageController controller;
        private AWatcherService service;

        private BackgroundWorker libraryThread;

        /// <summary>
        /// Constructs a list of plug-ins and initializes the library
        /// </summary>
        public PluginLibrary(MessageController Controller, AWatcherService Service)
        {
            this.controller = Controller;
            this.service = Service;
            ScanForPlugins();
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
        /// Don't do until LUA Plugins
        /// </summary>
        private static void ScanForPlugins()
        {
            return;
        }

        /// <summary>
        /// Grabs the next message from the controller
        /// </summary>
        /// <returns></returns>
        public ParsedMessage GetNextMessage()
        {
            return this.controller.getNextMessage();
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

                // Not a valid group ID
                if (message.Group.Equals(null))
                {
                    message.ContentMessage = INVALID_GROUP_MESSAGE;
                }
                // Not a user within the given group
                else if (!message.Group.Users.Contains(message.Sender))
                {
                    message.ContentMessage = INVALID_USER_MESSAGE;
                }
                // User is banned
                else if (message.Sender.IsBanned)
                {
                    message.ContentMessage = BANNED_USER_MESSAGE;
                    doMessage = false;
                }
                // User is suppressed 
                else if (message.Sender.IsSuppressed)
                {
                    message.ContentMessage = SUPPRESSED_USER_MESSAGE;
                }

                Boolean foundPlugin = false;
                foreach (IPlugin i in message.Group.EnabledPlugins)
                {
                    if (i.PluginDAO.PluginID.Equals(message.Command))
                    {
                        // If the plugin can only be accessed by moderators and the calling user is not a moderator/owner
                        if (i.PluginDAO.Access == PluginAccess.MODERATOR && (message.Group.Moderators.Contains(message.Sender) || message.Sender.Equals(message.Group.Owner)))
                        {
                            message.ContentMessage = RESTRICTED_ACCESS_MESSAGE;
                        }
                        else
                        {
                            foundPlugin = true;
                            plugin = i;
                        }
                    }
                }

                // No valid plugin was found for the given command
                if (!foundPlugin)
                {
                    message.ContentMessage = INVALID_PLUGIN_MESSAGE;
                }

                if (!doMessage)
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
            plugin.Run(message, this.service);
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

        
        // Messages to be sent back to sender when system throws an error or the commands are invalid.
        private static string INVALID_GROUP_MESSAGE = "Invalid group. Please check your message and try again.";
        private static string INVALID_USER_MESSAGE = "You are not a valid member of this group. Please check your message and try again.";
        private static string BANNED_USER_MESSAGE = "";
        private static string SUPPRESSED_USER_MESSAGE = "You have currently suppressed recieving messages. To disable, please reply, \"SUPPRESS OFF\"";
        private static string INVALID_PLUGIN_MESSAGE = "Invalid command. Please check your message and try again.";
        private static string RESTRICTED_ACCESS_MESSAGE = "You are not authorized to use this command. Please check with your group's owner and try again.";
    }
}
