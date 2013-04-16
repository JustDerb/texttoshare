using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sDbLibrary
{
    public interface IDBController
    {
        /// <summary>
        /// Creates a new user entry in the database with the given UserDAO.
        /// </summary>
        /// <param name="user">The UserDAO to insert into the database.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>true if the user was successfully added.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO is null.</exception>
        bool CreateUser(UserDAO user, string password);

        /// <summary>
        /// Grabs an individual user based on the given username string.
        /// </summary>
        /// <param name="username">The username to query for.</param>
        /// <returns>A new UserDAO object with data related to the given username.</returns>
        /// <exception cref="ArgumentNullException">If the given string is null.</exception>
        /// <exception cref="CouldNotFindException">If the user for the given username could not be found.</exception>
        UserDAO RetrieveUserByUserName(string username);

        /// <summary>
        /// Grabs an individual user based on the given phone email string. The string should be in a format similar to
        /// <code>String userPhoneEmail = "1234567890@carrier.com"</code>
        /// in order to grab the correct information.
        /// </summary>
        /// <param name="username">The username to query for.</param>
        /// <returns>A new UserDAO object with data related to the given phone email.</returns>
        /// <exception cref="ArgumentNullException">If the given string is null.</exception>
        /// <exception cref="CouldNotFindException">If the user for the given phone email could not be found.</exception>
        UserDAO RetrieveUserByPhoneEmail(string phoneEmail);

        /// <summary>
        /// Grabs an individual user based on the given user id.
        /// </summary>
        /// <param name="userID">The user id to query for.</param>
        /// <returns>A new UserDAO object with data related to the given user id.</returns>
        /// <exception cref="ArgumentNullException">If the given id is null.</exception>
        /// <exception cref="CouldNotFindException">If the user for the given id could not be found.</exception>
        UserDAO RetrieveUser(int? userID);

        /// <summary>
        /// Updates user information in the database.
        /// </summary>
        /// <param name="user">The UserDAO to update in the database</param>
        /// <returns>true if the user was successfully updated.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO or UserDAO.UserID is null.</exception>
        bool UpdateUser(UserDAO user);

        /// <summary>
        /// Deletes an existing user that matches the given UserDAO.  The user should not be admin of a group, or have plugins
        /// </summary>
        /// <param name="user">The UserDAO to delete from the database.</param>
        /// <returns>true if the user was successfully deleted. Returns false if the user cannot be deleted due to them owning a Plugin or Group.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO or UserDAO.UserID is null.</exception>
        bool DeleteUser(UserDAO user);

        /// <summary>
        /// Deletes an existing user that matches the given UserDAO.
        /// </summary>
        /// <param name="user">The UserDAO to delete from the database.</param>
        /// <param name="isOwner">If true, checks to see if the user is an owner of a group or plugin.</param>
        /// <returns>true if the user was successfully deleted. Returns false if the user cannot be deleted due to them owning a Plugin or Group.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO or UserDAO.UserID is null.</exception>
        bool DeleteUser(UserDAO user, bool isOwner);

        /// <summary>
        /// Checks if the given username or phoneEmail exists in the database. Since usernames and phone emails are unique,
        /// this method is useful to see if a user already exists with the name or phone email.
        /// </summary>
        /// <param name="username">The username of the user to check.</param>
        /// <param name="phoneEmail">The phone email of the user to check.</param>
        /// <returns>true if a user exists.</returns>
        bool UserExists(string username, string phoneEmail);

        /// <summary>
        /// Gets all users currently in the system.
        /// </summary>
        /// <returns>A list of all users.</returns>
        /// <exception cref="SqlException">If there is an issue in querying the database.</exception>
        List<UserDAO> GetAllUsers();

        /// <summary>
        /// Suppresses a user to prevent them from receiving texts.
        /// </summary>
        /// <param name="pluginID">The user to suppress.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given user is null.</exception>
        bool SuppressUser(UserDAO user);

        /// <summary>
        /// Unsuppresses a user so they can continue to receive texts.
        /// </summary>
        /// <param name="pluginID">The user to unsuppress.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given user is null.</exception>
        bool UnsuppressUser(UserDAO user);

        /// <summary>
        /// Bans a user to prevent them from sending or receiving texts.
        /// </summary>
        /// <param name="pluginID">The user to ban.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given user is null.</exception>
        bool BanUser(UserDAO user);

        /// <summary>
        /// Unbans a user so they can continue to send and receive texts.
        /// </summary>
        /// <param name="pluginID">The user to unban.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given user is null.</exception>
        bool UnbanUser(UserDAO user);

        /// <summary>
        /// Inserts the given GroupDAO object into the database, along with the different relations
        /// between users, permissions, and plugins.
        /// </summary>
        /// <param name="group">The GroupDAO to insert into the database</param>
        /// <returns>true if the group was successfully added.</returns>
        /// <exception cref="ArgumentNullException">If the given group is null.</exception>
        bool CreateGroup(GroupDAO group);

        /// <summary>
        /// Retrieves a GroupDAO object representing information about a specific group, including users, moderators, and plugins.
        /// </summary>
        /// <param name="groupTag">The group tag of the group to retrieve.</param>
        /// <returns>A new GroupDAO object, complete with lists of users, moderators, and plugins associated with the group.</returns>
        /// <exception cref="ArgumentNullException">If the given group tag is null.</exception>
        /// <exception cref="CouldNotFindException">If a group with the given group tag could not be found.</exception>
        GroupDAO RetrieveGroup(string groupTag);

        /// <summary>
        /// Retrieves basic information for a group in a GroupDAO. Does not include users or plugins for a group.
        /// </summary>
        /// <param name="groupTag">The group tag of the group to retrieve.</param>
        /// <returns>A new GroupDAO object with basic group data.</returns>
        GroupDAO RetrieveGroupMetadata(string groupTag);

        /// <summary>
        /// Updates the metadata, the list of users for the group, and all enabled plugin relationships for the given group.
        /// </summary>
        /// <param name="group">The GroupDAO to update in the database.</param>
        /// <returns>true if successful.</returns>
        bool UpdateGroup(GroupDAO group);

        /// <summary>
        /// Deletes an existing group that matches the given GroupDAO. All users and plugin assocations with the given
        /// GroupDAO will be deleted, however the users and plugins will not be.
        /// </summary>
        /// <param name="user">The PluginDAO to delete from the database.</param>
        /// <returns>true if the plugin was successfully deleted.</returns>
        /// <exception cref="ArgumentNullException">If the given PluginDAO or PluginDAO.PluginID is null.</exception>
        bool DeleteGroup(GroupDAO group);

        /// <summary>
        /// Checks to see if a group with the given name exists in the database.
        /// </summary>
        /// <param name="name">The name of the group to check in the database.</param>
        /// <returns>true if the group already exists.</returns>
        /// <exception cref="ArgumentNullException">If the given name is null.</exception>
        bool GroupExists(string name);

        /// <summary>
        /// Adds a user with a specified administrative level to a group.
        /// </summary>
        /// <param name="groupID">The GroupID of the group to add the user to.</param>
        /// <param name="userID">The UserID of the user to add.</param>
        /// <param name="groupLevel">The level of the user's permissions in the group.</param>
        /// <returns>true if the user was added successfully.</returns>
        /// <exception cref="ArgumentNullException">If the given groupID or userID are null.</exception>
        /// <seealso cref="GroupLevel"/>
        bool AddMemberToGroup(int? groupID, int? userID, GroupLevel groupLevel);

        /// <summary>
        /// Checks to see if the given user is already a member of the given group.
        /// </summary>
        /// <param name="groupID">The GroupID of the group to check.</param>
        /// <param name="userID">The UserID of the user.</param>
        /// <returns>true if the user is already a member of the group.</returns>
        bool IsUserMemberOfGroup(int? groupID, int? userID);

        /// <summary>
        /// Removes an individual user from a group. If the user was not already in the group, this method returns false.
        /// </summary>
        /// <param name="groupID">The GroupID of the group to remove the user from.</param>
        /// <param name="userID">The UserID of the user to remove.</param>
        /// <returns>true if the user was successfully removed</returns>
        bool RemoveMemberFromGroup(int? groupID, int? userID);

        /// <summary>
        /// Gets a list of all members (regardless of group level) for the given groupID.
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns>A list of UserDAOs that is representative of all users for a group.</returns>
        List<UserDAO> GetAllGroupMembers(int? groupID);

        /// <summary>
        /// Adds a plugin as an enabled plugin for a group in the database.
        /// </summary>
        /// <param name="groupID">The ID of the group to add the plugin relationship to.</param>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <param name="isDisabled">Initially set whether or not the plugin can be used by the group.</param>
        /// <returns>true if successful.</returns>
        bool AddPluginToGroup(int? groupID, int? pluginID, bool isDisabled);

        /// <summary>
        /// Checks to see if the group already has the plugin associated with the group.
        /// </summary>
        /// <param name="groupID">The ID of the group.</param>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if there is an associated plugin for the group already.</returns>
        bool IsPluginInGroup(int? groupID, int? pluginID);

        /// <summary>
        /// Removes a plugin from the group entirely. This does not remove the plugin itself, only the relationship between the group and the plugin.
        /// </summary>
        /// <param name="groupID">The ID of the group.</param>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if successful.</returns>
        bool RemovePluginFromGroup(int? groupID, int? pluginID);

        /// <summary>
        /// Enables a plugin for a group if it is not already.
        /// </summary>
        /// <param name="groupID">The ID of the group.</param>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if successful.</returns>
        bool EnablePluginForGroup(int? groupID, int? pluginID);

        /// <summary>
        /// Disables a plugin for a group if it is not already.
        /// </summary>
        /// <param name="groupID">The ID of the group.</param>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if successful.</returns>
        bool DisablePluginForGroup(int? groupID, int? pluginID);

        /// <summary>
        /// Gets a list of all enabled plugins for a group.
        /// </summary>
        /// <param name="groupID">The ID of the group.</param>
        /// <returns>A list of PluginDAOs.</returns>
        List<PluginDAO> GetAllEnabledGroupPlugins(int? groupID);

        /// <summary>
        /// Creates a new plugin entry in the database with the given PluginDAO. The PluginID of the given
        /// PluginDAO will also be set after calling this method.
        /// </summary>
        /// <param name="user">The PluginDAO to insert into the database.</param>
        /// <returns>true if the plugin was successfully added and the PluginID was set</returns>
        /// <exception cref="ArgumentNullException">If the given plugin is null.</exception>
        bool CreatePlugin(PluginDAO plugin);

        /// <summary>
        /// Grabs an individual plugin from the database that matches the given command.
        /// </summary>
        /// <param name="commandText">The command text of the plugin to search for.</param>
        /// <returns>A new PluginDAO object with data related to the given command text.</returns>
        /// <exception cref="ArgumentNullException">If the given commandText is null.</exception>
        /// <exception cref="CouldNotFindException">If the plugin for the given commandText could not be found.</exception>
        PluginDAO RetrievePlugin(string commandText);

        /// <summary>
        /// Grabs an individual plugin from the database that matches the given command.
        /// </summary>
        /// <param name="pluginID">The plugin id to search for.</param>
        /// <returns>A new PluginDAO object with data related to the given plugin id.</returns>
        /// <exception cref="ArgumentNullException">If the given plugin id is null.</exception>
        /// <exception cref="CouldNotFindException">If the plugin for the given plugin id could not be found.</exception>
        PluginDAO RetrievePlugin(int? pluginID);

        /// <summary>
        /// Updates plugin information in the database. If there is no entry in the database that matches the given
        /// PluginDAO.PluginID no entries will be updated and <code>false</code> will be returned.
        /// </summary>
        /// <param name="user">The PluginDAO to update in the database</param>
        /// <returns>true if the plugin was successfully updated.</returns>
        /// <exception cref="ArgumentNullException">If the given PluginDAO or PluginDAO.PluginID is null.</exception>
        bool UpdatePlugin(PluginDAO plugin);

        /// <summary>
        /// Deletes an existing plugin that matches the given PluginDAO.
        /// </summary>
        /// <param name="user">The PluginDAO to delete from the database.</param>
        /// <returns>true if the plugin was successfully deleted.</returns>
        /// <exception cref="ArgumentNullException">If the given PluginDAO or PluginDAO.PluginID is null.</exception>
        bool DeletePlugin(PluginDAO plugin);

        /// <summary>
        /// Checks to see if a plugin already exists with the given command text.
        /// </summary>
        /// <param name="plugin">The command text to search for.</param>
        /// <returns>true if a plugin exists that matches the given string.</returns>
        /// <exception cref="ArgumentNullException">If the given command text is null.</exception>
        bool PluginExists(string commandText);

        /// <summary>
        /// Enables a plugin for global use by all groups.
        /// </summary>
        /// <param name="pluginID">The id of the plugin to enable.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given plugin id is null.</exception>
        bool EnableGlobalPlugin(int? pluginID);

        /// <summary>
        /// Disables a plugin for global use by all groups.
        /// </summary>
        /// <param name="pluginID">The id of the plugin to disable.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given plugin id is null.</exception>
        bool DisableGlobalPlugin(int? pluginID);

        /// <summary>
        /// Increments the failed attempt count of the given pluginID by 1.
        /// </summary>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if successful.</returns>
        bool IncrementPluginFailedAttemptCount(int? pluginID);

        /// <summary>
        /// Gets the current count of failed attempts for the given pluginID.
        /// </summary>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>The failed attempt count of the given pluginID.</returns>
        int GetPluginFailedAttemptCount(int? pluginID);

        /// <summary>
        /// Resets the failed attempt count of the plugin to 0.
        /// </summary>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if successful.</returns>
        bool ResetPluginFailedAttemptCount(int? pluginID);

        /// <summary>
        /// Gets a list of all the names of plugins that are globally enabled
        /// </summary>
        /// <returns>A list containing all plugin names.</returns>
        /// <exception cref="SqlException">If a SQL-related exception is thrown.</exception>
        List<PluginDAO> RetrieveEnabledPlugins();

        /// <summary>
        /// Updates the given plugin's owner with the given user. On completion, updates
        /// the given PluginDAO's owner id with the given user.
        /// </summary>
        /// <param name="group">The group to update the owner of.</param>
        /// <param name="newOwner">The user to set as owner of the given group.</param>
        /// <returns>true if successful.</returns>
        bool UpdatePluginOwner(PluginDAO plugin, UserDAO newOwner);

        /// <summary>
        /// Gets a list of plugins that are owned by the given user.
        /// </summary>
        /// <param name="user">The user to retrieve a list of owned plugins for.</param>
        /// <returns>A list containing the plugins owned by the user. If the user does not own any plugins, the list will return empty.</returns>
        List<PluginDAO> GetPluginsOwnedByUser(UserDAO user);

        #region "Plugin Key/Value Actions"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void UpdatePluginKeyValue(PluginDAO plugin, String key, String value, UserDAO forUser = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string RetrievePluginValue(PluginDAO plugin, String key, UserDAO forUser = null);

        #endregion

        /// <summary>
        /// Gets the associated value for the given key entry.
        /// </summary>
        /// <param name="keyEntry">The key entry to search for.</param>
        /// <returns>The string representation of the value assoviated with the key.</returns>
        /// <exception cref="ArgumentNullException">If the given key entry is null.</exception>
        /// <exception cref="CouldNotFindException">If the key entry does not exist.</exception>
        string GetPairEntryValue(string keyEntry);

        /// <summary>
        /// Performs an upsert on a key-value entry, e.g. if the key-value entry does not exist, it is inserted; 
        /// otherwise it updates the given key's value with the given value.
        /// </summary>
        /// <param name="keyEntry">The key entry to upsert.</param>
        /// <param name="valueEntry">The value to upsert.</param>
        /// <returns>true if successful</returns>
        /// <exception cref="ArgumentNullException">If the given key or value is null.</exception>
        bool SetPairEntryValue(string keyEntry, string valueEntry);

        /// <summary>
        /// Inserts a message into the database with the given message and level of importance.
        /// </summary>
        /// <param name="message">The message for the log.</param>
        /// <param name="level">The level of the given.</param>
        /// <returns>true if the message was successfully logged.</returns>
        /// <exception cref="ArgumentNullException">If the given message is null.</exception>
        /// <seealso cref="LoggerLevel"/>
        bool LogMessage(string message, LoggerLevel level);

        /// <summary>
        /// Checks that a user with the given username and password exists in the database.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="password">The password to check.</param>
        /// <returns>true if the user exists.</returns>
        bool CheckLogin(string username, string password);

        /// <summary>
        /// Registers a new user in the database.
        /// </summary>
        /// <param name="user">The user to be added to the database.</param>
        /// <param name="password">The login password of the user.</param>
        /// <returns>true if the user was successfully added.</returns>
        /// <seealso cref="SqlController#CreateUser"/>
        bool RegisterUser(UserDAO user, string password);
    }

    /// <summary>
    /// Exception class thrown when a specific entry in the database could not be found.
    /// </summary>
    public class CouldNotFindException : Exception
    {
        public CouldNotFindException() : base() {}
        public CouldNotFindException(string message) : base(message) {}
        public CouldNotFindException(string message, Exception innerException) : base(message, innerException) {}
        public CouldNotFindException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }

    /// <summary>
    /// Exception class thrown when a specific entry in the database already exists.
    /// </summary>
    public class EntryAlreadyExistsException : Exception
    {
        public EntryAlreadyExistsException() : base() { }
        public EntryAlreadyExistsException(string message) : base(message) {}
        public EntryAlreadyExistsException(string message, Exception innerException) : base(message, innerException) {}
        public EntryAlreadyExistsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }
}
