using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    /// <summary>
    /// Class to manage all plug-ins
    /// </summary>
    public class PluginLibrary
    {
        /// <summary>
        /// Plug-in to use when other Plug-ins throw errors
        /// </summary>
        private PluginError ErrorPlugin;

        /// <summary>
        /// List of plug-ins available
        /// </summary>
        private List<IPlugin> _PluginList;
        /// <summary>
        /// Returns the list of plug-ins
        /// </summary>
        public List<IPlugin> PluginList
        {
            get;
            private set;
        }

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
            this._PluginList = new List<IPlugin>(/*default plugins*/);
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
        /// ???????????????????????
        /// </summary>
        private void ScanForPlugins()
        {
            // Don't do until LUA Plugins
            return;
        }

        /// <summary>
        /// ??????????????????????????
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

        private void libraryThread_DoWork(object sender, DoWorkEventArgs e)
        {
            while (_Running)
            {
                // Will block until a new message has arrived
                ParsedMessage message = GetNextMessage();
                // Logic here
                //   Find plugin
                //   Pass shit


                // NOTE: Make sure thread is not disposed after running because it lost scope
                BackgroundWorker pluginThread = new BackgroundWorker();
                pluginThread.DoWork += pluginThread_DoWork;
                Object[] parameters = new Object[] { /*PLUGIN GOES HERE*/ null, message};
                pluginThread.RunWorkerAsync(parameters);
            }
        }

        void pluginThread_DoWork(object sender, DoWorkEventArgs e)
        {
            Object[] parameters = (Object[])e.Argument;
            IPlugin plugin = (IPlugin)parameters[0];
            ParsedMessage message = (ParsedMessage)parameters[1];

            // Do plugin work
            plugin.Run(message, this.service);
            throw new NotImplementedException();
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
    }
}
