using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class GroupDAO
    {
        public List<UserDAO> Users
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int GroupID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string Name
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string Description
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<UserDAO> Moderators
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public UserDAO Owner
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<IPlugin> EnabledPlugins
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string GroupTag
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public bool AddUserToGroup(UserDAO user)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveUserFromGroup(UserDAO user)
        {
            throw new System.NotImplementedException();
        }

        public bool AddModerator(UserDAO user)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveModerator(UserDAO user)
        {
            throw new System.NotImplementedException();
        }

        public bool AddPlugin(IPlugin plugin)
        {
            throw new System.NotImplementedException();
        }

        public bool RemovePlugin(IPlugin plugin)
        {
            throw new System.NotImplementedException();
        }
    }
}
