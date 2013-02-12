using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class PluginLibrary
    {
        private PluginError ErrorPlugin;

        public PluginLibrary()
        {
            throw new System.NotImplementedException();
        }

        public PluginLibrary(MessageParser parser)
        {
            throw new System.NotImplementedException();
        }

        public List<IPlugin> PluginList
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Queue<t2sBackend.ParsedMessage> MessageQueue
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public bool Running
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        private void ScanForPlugins()
        {
            throw new System.NotImplementedException();
        }

        public void AddMessage(ParsedMessage message)
        {
            throw new System.NotImplementedException();
        }

        public ParsedMessage GetNextMessage()
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}
