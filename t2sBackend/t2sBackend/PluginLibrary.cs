using System;
using System.Collections.Generic;
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
        /// Parser to be used by the library to break down commands from the sender
        /// </summary>
        private MessageParser _Parser;

        /// <summary>
        /// Service to send/recieve messages to users
        /// </summary>
        private AWatcherService _Watcher;

        /// <summary>
        /// List of plug-ins available
        /// </summary>
        private List<IPlugin> _PluginList;

        /* Information to initialize message service*/
        private string _User;
        private string _Password;
        private bool _UseSSL;
        private string _IMAPServer;
        private int _IMAPPort;
        private string _SMTPServer;
        private int _SMTPPort;

        /// <summary>
        /// Database controller to user to access plug-ins and group/user information
        /// </summary>
        private SQLController _SQLControl;


        /// <summary>
        /// Constructs a list of plug-ins and initializes the library
        /// </summary>
        public PluginLibrary()
        {
            _PluginList = new List<IPlugin>(/*default plugins*/);
            _Watcher = new GMailWatcherService(_User, _Password, _UseSSL, _IMAPServer, _IMAPPort, _SMTPServer, _SMTPPort);
            _SQLControl = new SQLController();
            _Parser = new MessageParser(_Watcher, _SQLControl);
        }

        /// <summary>
        /// Constructs a list of plug-ins and initializes the library with a message parser given by user
        /// </summary>
        /// <param name="parser"></param>
        public PluginLibrary(MessageParser parser)
        {
            _PluginList = new List<IPlugin>(/*default plugins*/);
        }

        /// <summary>
        /// Returns the list of plug-ins
        /// </summary>
        public List<IPlugin> PluginList
        {
            get;
            private set;
        }

        /// <summary>
        /// ????????
        /// </summary>
        public Queue<t2sBackend.ParsedMessage> MessageQueue
        {
            get;
            set;
        }

        /// <summary>
        /// Checks whether thread of plug-in library is running
        /// </summary>
        private bool _Running;
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
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// ????????????????
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(ParsedMessage message)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// ??????????????????????????
        /// </summary>
        /// <returns></returns>
        public ParsedMessage GetNextMessage()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Begins the plug-in library thread
        /// </summary>
        public void Start()
        {
            _Running = true;
        }

        /// <summary>
        /// Terminates the plug-in library thread
        /// </summary>
        public void Stop()
        {
            _Running = false;
        }
    }
}
