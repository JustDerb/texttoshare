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
            this.Users = new HashSet<UserDAO>();
            this.Moderators = new HashSet<UserDAO>();
            this.EnabledPlugins = new List<PluginDAO>();
        }

        public GroupDAO() { }

        /// <summary>
        /// The List of Users in the Group. Managed by the Group Moderators
        /// </summary>
        public HashSet<UserDAO> Users
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
        public HashSet<UserDAO> Moderators
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
            return Users.Add(user);
        }

        /// <summary>
        /// Removes a User from the Group
        /// </summary>
        /// <param name="user">User to be removed</param>
        /// <returns>True if User was removed successfully, false if otherwise</returns>
        public bool RemoveUserFromGroup(UserDAO user)
        {
            return Users.Remove(user);
        }

        /// <summary>
        /// Adds a Moderator to the Group
        /// </summary>
        /// <param name="user">User to be added as a Moderator</param>
        /// <returns>True if User is added as a Moderator successfully, false if otherwise</returns>
        public bool AddModerator(UserDAO user)
        {
            return Moderators.Add(user);
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            GroupDAO g = obj as GroupDAO;

            return (
                this.GroupID == g.GroupID &&
                this.Name.Equals(g.Name) &&
                this.Description.Equals(g.Description) &&
                this.Users.Equals(g.Users) &&
                this.Moderators.Equals(g.Moderators) &&
                this.Owner.Equals(g.Owner) &&
                this.EnabledPlugins.Equals(g.EnabledPlugins) &&
                this.GroupTag.Equals(g.GroupTag)
            );
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + (null == GroupID ? 0 : GroupID.GetHashCode());
                hash = hash * 23 + (null == Name ? 0 : Name.GetHashCode());
                hash = hash * 23 + (null == Description ? 0 : Description.GetHashCode());
                hash = hash * 23 + (null == Users ? 0 : Users.GetHashCode());
                hash = hash * 23 + (null == Moderators ? 0 : Moderators.GetHashCode());
                hash = hash * 23 + (null == Owner ? 0 : Owner.GetHashCode());
                hash = hash * 23 + (null == EnabledPlugins ? 0 : EnabledPlugins.GetHashCode());
                hash = hash * 23 + (null == GroupTag ? 0 : GroupTag.GetHashCode());

                return hash;
            }
        }
    }
}
