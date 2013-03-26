using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sDbLibrary
{
    public class GroupDAO
    {
        public GroupDAO(UserDAO Owner)
        {
            this.Owner = Owner;
        }

        public GroupDAO() { }

        /// <summary>
        /// The List of Users in the Group. Managed by the Group Moderators
        /// </summary>
        public List<UserDAO> Users
        {
            get;
            set;
        }

        /// <summary>
        /// Used to manage the Group in the Database
        /// </summary>
        public int? GroupID
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the Group
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The description to be displayed about the Group
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// The List of Users that will have access to the Group's Users and PlugIns
        /// </summary>
        public List<UserDAO> Moderators
        {
            get;
            set;
        }

        /// <summary>
        /// The creator of the Group
        /// </summary>
        public UserDAO Owner
        {
            get;
            private set;
        }

        /// <summary>
        /// The List of Plugins that the Group's Moderators have chosen to enable for the Group
        /// </summary>
        public List<PluginDAO> EnabledPlugins
        {
            get;
            set;
        }

        /// <summary>
        /// The Tag for the Group that will be used to identify in command messages
        /// </summary>
        public string GroupTag
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a User to the Group
        /// </summary>
        /// <param name="user">User to be added</param>
        /// <returns>True if User was added successfully, False if otherwise</returns>
        public bool AddUserToGroup(UserDAO user)
        {
            if (user == null)
            {
                return false;
            }
            Users.Add(user);
            return true;
        }

        /// <summary>
        /// Removes a User from the Group
        /// </summary>
        /// <param name="user">User to be removed</param>
        /// <returns>True if User was removed successfully, false if otherwise</returns>
        public bool RemoveUserFromGroup(UserDAO user)
        {
            if (!Users.Contains(user))
            {
                return false;
            }
            Users.Remove(user);
            return true;
        }

        /// <summary>
        /// Adds a Moderator to the Group
        /// </summary>
        /// <param name="user">User to be added as a Moderator</param>
        /// <returns>True if User is added as a Moderator successfully, false if otherwise</returns>
        public bool AddModerator(UserDAO user)
        {
            if (!Users.Contains(user) || Moderators.Contains(user))
            {
                return false;
            }
            Moderators.Add(user);
            return true;
        }

        /// <summary>
        /// Removes a User as a Moderator for the Group
        /// </summary>
        /// <param name="user">User to be removed from Moderators</param>
        /// <returns>True if User is removed successfully, false if otherwise</returns>
        public bool RemoveModerator(UserDAO user)
        {
            return Moderators.Remove(user);
        }

        /// <summary>
        /// Adds a Plugin to the Groups List of enabled Plugins
        /// </summary>
        /// <param name="plugin">Plugin to be added</param>
        /// <returns>True if the Plugin is added successfully, false if otherwise</returns>
        public bool AddPlugin(PluginDAO plugin)
        {
            if (EnabledPlugins.Contains(plugin))
            {
                return false;
            }
            EnabledPlugins.Add(plugin);
            return true;
        }

        /// <summary>
        /// Removes a Plugin from the Group's List of enabled Plugins
        /// </summary>
        /// <param name="plugin">Plugin to be removed</param>
        /// <returns>True if the Plugin is removed successfully, false if otherwise</returns>
        public bool RemovePlugin(PluginDAO plugin)
        {
            return EnabledPlugins.Remove(plugin);
        }
    }
}
