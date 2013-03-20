using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class SqlController : IDBController
    {
        /*
         * The connection string used for connecting to the database. 
         * 
         * Do NOT modify these values unless the directory of the database changes.
         */
        private const string _connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\MainDatabase.mdf;Integrated Security=True";

        #region UserDAO "CRUD" actions

        /// <summary>
        /// Creates a new user entry in the database with the given UserDAO. The UserID of the given
        /// UserDAO will also be set after calling this method.
        /// </summary>
        /// <param name="user">The UserDAO to insert into the database.</param>
        /// <returns>true if the user was successfully added and the UserID was set</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO is null.</exception>
        /// <exception cref="SqlException">If there is an error querying the database.</exception>
        public bool CreateUser(UserDAO user)
        {
            if (null == user) throw new ArgumentNullException();

            if (UserExists(user)) throw new EntryAlreadyExistsException("User with phone email: " + user.PhoneEmail + " already exists.");

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO users (username, first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed, created_dt) ");
                queryBuilder.Append("VALUES ");
                queryBuilder.Append("(@username, @first_name, @last_name, @phone, @email_phone, @carrier, @user_level, @banned, @suppressed, GETDATE()) ");
                queryBuilder.Append("; SELECT SCOPE_IDENTITY()");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@username", user.UserName);
                query.Parameters.AddWithValue("@first_name", user.FirstName);
                query.Parameters.AddWithValue("@last_name", user.LastName);
                query.Parameters.AddWithValue("@phone", user.PhoneNumber);
                query.Parameters.AddWithValue("@email_phone", user.PhoneEmail);
                query.Parameters.AddWithValue("@carrier", (int)user.Carrier);
                query.Parameters.AddWithValue("@user_level", (int)user.UserLevel);
                query.Parameters.AddWithValue("@banned", user.IsBanned ? 1 : 0);
                query.Parameters.AddWithValue("@suppressed", user.IsSuppressed ? 1 : 0);

                conn.Open();
                int newID = (int)(decimal) query.ExecuteScalar();

                // The SCOPE_IDENTITY() should return the generated UserID of the INSERT statement
                if (1 > newID) return false;

                user.UserID = newID;
                return true;
            }
        }

        /// <summary>
        /// Grabs an individual user based on the given phone email string. The string should be in a format similar to
        /// <code>String userPhoneEmail = "1234567890@carrier.com"</code>
        /// in order to grab the correct information.
        /// </summary>
        /// <param name="userPhoneEmail">The user phone email to query for.</param>
        /// <returns>A new UserDAO object with data related to the given phone email.</returns>
        /// <exception cref="ArgumentNullException">If the given string is null.</exception>
        /// <exception cref="CouldNotFindException">If the user for the given phone email could not be found.</exception>
        public UserDAO RetrieveUser(string userPhoneEmail)
        {
            if (string.IsNullOrEmpty(userPhoneEmail)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, username, first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed ");
                queryBuilder.Append("FROM users ");
                queryBuilder.Append("WHERE email_phone = @phoneEmail");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@phoneEmail", userPhoneEmail);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                UserDAO userDAO = null;

                // If there are no records returned from the select statement, the DataReader will be empty
                while (reader.Read())
                {
                    userDAO = new UserDAO();
                    userDAO.UserID = (int) reader["id"];
                    userDAO.UserName = (string) reader["username"];
                    userDAO.FirstName = (string) reader["first_name"];
                    userDAO.LastName = (string) reader["last_name"];
                    userDAO.PhoneNumber = (string) reader["phone"];
                    userDAO.PhoneEmail = (string) reader["email_phone"];
                    userDAO.Carrier = (PhoneCarrier) reader["carrier"];
                    userDAO.UserLevel = (UserLevel) reader["user_level"];
                    userDAO.IsBanned = (bool) reader["banned"];
                    userDAO.IsSuppressed = (bool) reader["suppressed"];
                }

                if (null == userDAO) throw new CouldNotFindException("Could not find user with userPhoneEmail: " + userPhoneEmail);

                return userDAO;
            }
        }

        /// <summary>
        /// Updates user information in the database. If there is no entry in the database that matches the given
        /// UserDAO.UserID and UserDAO.PhoneEmail, no entries will be updated and <code>false</code> will be returned.
        /// </summary>
        /// <param name="user">The UserDAO to update in the database</param>
        /// <returns>true if the user was successfully updated.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO or UserDAO.UserID is null.</exception>
        public bool UpdateUser(UserDAO user)
        {
            if (null == user || null == user.UserID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("UPDATE users SET ");
                queryBuilder.Append("username = @username ");
                queryBuilder.Append("first_name = @first_name, ");
                queryBuilder.Append("last_name = @last_name, ");
                queryBuilder.Append("phone = @phone, ");
                queryBuilder.Append("email_phone = @email_phone, ");
                queryBuilder.Append("carrier = @carrier, ");
                queryBuilder.Append("user_level = @user_level, ");
                queryBuilder.Append("banned = @banned, ");
                queryBuilder.Append("suppressed = @suppressed ");
                queryBuilder.Append("WHERE id = @userid ");
                queryBuilder.Append("AND email_phone = @email_phone2");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@username", user.UserName);
                query.Parameters.AddWithValue("@first_name", user.FirstName);
                query.Parameters.AddWithValue("@last_name", user.LastName);
                query.Parameters.AddWithValue("@phone", user.PhoneNumber);
                query.Parameters.AddWithValue("@email_phone", user.PhoneEmail);
                query.Parameters.AddWithValue("@carrier", (int) user.Carrier);
                query.Parameters.AddWithValue("@user_level", (int) user.UserLevel);
                query.Parameters.AddWithValue("@banned", user.IsBanned ? 1 : 0);
                query.Parameters.AddWithValue("@suppressed", user.IsSuppressed ? 1 : 0);
                query.Parameters.AddWithValue("@userid", user.UserID);
                query.Parameters.AddWithValue("@email_phone2", user.PhoneEmail);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                // Only one record should have been updated
                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Deletes an existing user that matches the given UserDAO.
        /// </summary>
        /// <param name="user">The UserDAO to delete from the database.</param>
        /// <returns>true if the user was successfully deleted.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO or UserDAO.UserID is null.</exception>
        public bool DeleteUser(UserDAO user)
        {
            if (null == user || null == user.UserID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "sp_deleteUser";
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@userid", user.UserID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                /* One or more records should have been deleted:
                 * The user record itself (1), and any additional groupmember entries (0 or more)
                 */
                return 0 < effectedRows;
            }
        }

        /// <summary>
        /// Checks that the given UserDAO exists in the database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool UserExists(UserDAO user)
        {
            try
            {
                return RetrieveUser(user.PhoneEmail).PhoneEmail.Equals(user.PhoneEmail);
            }
            catch (CouldNotFindException)
            {
                return false;
            }
        }

        #endregion

        #region GroupDAO "CRUD" actions

        /// <summary>
        /// Inserts the given GroupDAO object into the database, along with the different relations
        /// between users, permissions, and plugins.
        /// </summary>
        /// <param name="group">The GroupDAO to insert into the database</param>
        /// <returns>true if the group was successfully added.</returns>
        /// <exception cref="ArgumentNullException">If the given group is null.</exception>
        public bool CreateGroup(GroupDAO group)
        {
            if (null == group) throw new ArgumentNullException();

            /*
             * In order to prevent foreign key issues, data must be inserted in a specific order:
             * 1) GroupDAO metadata -> groups
             * 2) GroupDAO members -> groupmembers
             * 3) GroupDAO plugins -> groupplugins
             */

            // Make sure the group owner exists first
            if (!UserExists(group.Owner)) CreateUser(group.Owner);

            return (InsertGroup(group) &&
                InsertGroupMember(group.GroupID, group.Owner.UserID, GroupLevel.Owner) &&
                InsertGroupMembers(group.GroupID, group.Moderators, GroupLevel.Moderator) &&
                InsertGroupMembers(group.GroupID, group.Users, GroupLevel.User) &&
                InsertGroupPlugins(group.GroupID, group.EnabledPlugins));
        }

        /// <summary>
        /// Inserts the group metadata into the "groups" table.
        /// </summary>
        /// <param name="group">The GroupDAO to insert into the database.</param>
        private bool InsertGroup(GroupDAO group)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO groups (name, description, owner_id, created_dt, grouptag) ");
                queryBuilder.Append("VALUES ");
                queryBuilder.Append("(@name, @description, @owner_id, GETDATE(), @grouptag) ");
                queryBuilder.Append("; SELECT SCOPE_IDENTITY() ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@name", group.Name);
                query.Parameters.AddWithValue("@description", group.Description);
                query.Parameters.AddWithValue("@owner_id", group.Owner);
                query.Parameters.AddWithValue("@grouptag", group.GroupTag);

                conn.Open();
                int newID = (int)(decimal) query.ExecuteScalar();

                if (1 > newID) return false;

                group.GroupID = newID;
                return true;
            }
        }

        /// <summary>
        /// Inserts a list of records into the "groupmembers" table for a specific group and group level.
        /// </summary>
        /// <param name="groupID">The associated group for a list of users.</param>
        /// <param name="users">The list of users in the group.</param>
        /// <param name="groupLevel">The group level of the users.</param>
        private bool InsertGroupMembers(int groupID, List<UserDAO> users, GroupLevel groupLevel)
        {
            foreach(UserDAO user in users)
            {
                if (!UserExists(user)) CreateUser(user);
                if (!InsertGroupMember(groupID, user.UserID, groupLevel)) return false;
            }

            return true;
        }

        /// <summary>
        /// Inserts an individual record into the "groupmembers" table for a specific group and group level.
        /// </summary>
        /// <param name="groupID">The associated group for a user.</param>
        /// <param name="userID">The userID for a specific user.</param>
        /// <param name="groupLevel">The group level of the user.</param>
        private bool InsertGroupMember(int groupID, int? userID, GroupLevel groupLevel)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO groupmembers (group_id, user_id, group_level, added_dt) ");
                queryBuilder.Append("VALUES ");
                queryBuilder.Append("(@group_id, @user_id, @group_level, GETDATE())");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@group_id", groupID);
                query.Parameters.AddWithValue("@user_id", userID);
                query.Parameters.AddWithValue("@group_level", (int) groupLevel);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }

        private bool InsertGroupPlugins(int groupID, List<IPlugin> plugins)
        {
            foreach (IPlugin plugin in plugins)
            {
                if (!PluginExists(plugin.PluginDAO)) CreatePlugin(plugin.PluginDAO);
                if (!InsertGroupPlugin(groupID, plugin)) return false;
            }

            return true;
        }

        private bool InsertGroupPlugin(int groupID, IPlugin plugin)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO groupplugins (group_id, user_id, group_level, added_dt) ");
                queryBuilder.Append("VALUES ");
                queryBuilder.Append("(@group_id, @user_id, GETDATE())");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@group_id", groupID);
                query.Parameters.AddWithValue("@plugin_id", plugin.PluginDAO.PluginID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }

        public GroupDAO RetrieveGroup(string groupTag)
        {
            // return null if not a valid group
            // when retrieving enabled plugins for the group, return plugins that are selected by the group AND are not disabled
            // when retrieving users in the group, only return users that are not suppressed or banned
            throw new NotImplementedException();
        }

        public bool UpdateGroup(GroupDAO group)
        {
            throw new NotImplementedException();
        }

        public bool DeleteGroup(GroupDAO group)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region PluginDAO "CRUD" actions

        /// <summary>
        /// Creates a new plugin entry in the database with the given PluginDAO. The PluginID of the given
        /// PluginDAO will also be set after calling this method.
        /// </summary>
        /// <param name="user">The PluginDAO to insert into the database.</param>
        /// <returns>true if the plugin was successfully added and the PluginID was set</returns>
        /// <exception cref="ArgumentNullException">If the given plugin is null.</exception>
        public bool CreatePlugin(PluginDAO plugin)
        {
            if (null == plugin) throw new ArgumentNullException();

            if (PluginExists(plugin)) throw new EntryAlreadyExistsException("Plugin with command: " + plugin.Name + " already exists.");

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO plugin (name, description, disabled, version_num, owner_id, created_dt, plugin_access, help_text) ");
                queryBuilder.Append("VALUES ");
                queryBuilder.Append("(@name, @description, @disabled, @version_num, @owner_id, GETDATE(), @plugin_access, @help_text) ");
                queryBuilder.Append("; SELECT SCOPE_IDENTITY()");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@name", plugin.PluginID);
                query.Parameters.AddWithValue("@description", plugin.PluginID);
                query.Parameters.AddWithValue("@disabled", plugin.IsDisabled ? 1 : 0);
                query.Parameters.AddWithValue("@version_num", plugin.VersionNum);
                query.Parameters.AddWithValue("@owner_id", plugin.OwnerID);
                query.Parameters.AddWithValue("@plugin_access", (int)plugin.Access);
                query.Parameters.AddWithValue("@help_text", plugin.HelpText);

                conn.Open();
                int newID = (int)(decimal)query.ExecuteScalar();

                // The SCOPE_IDENTITY() should return the generated PluginID of the INSERT statement
                if (1 > newID) return false;

                plugin.PluginID = newID;
                return true;
            }
        }

        /// <summary>
        /// Grabs an individual plugin from the database that matches the given command.
        /// </summary>
        /// <param name="commandText">The command text of the plugin to search for.</param>
        /// <returns>A new PluginDAO object with data related to the given command text.</returns>
        /// <exception cref="ArgumentNullException">If the given commandText is null.</exception>
        /// <exception cref="CouldNotFindException">If the plugin for the given commandText could not be found.</exception>
        public PluginDAO RetrievePlugin(string commandText)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, name, description, disabled, version_num, owner_id, plugin_access, help_text ");
                queryBuilder.Append("FROM plugins ");
                queryBuilder.Append("WHERE name = @commandText");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@commandText", commandText);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                PluginDAO pluginDAO = null;

                // If there are no records returned from the select statement, the DataReader will be empty
                while (reader.Read())
                {
                    pluginDAO = new PluginDAO();
                    pluginDAO.PluginID = (int)reader["id"];
                    pluginDAO.Name = (string)reader["name"];
                    pluginDAO.Description = (string)reader["description"];
                    pluginDAO.IsDisabled = (bool)reader["disabled"];
                    pluginDAO.VersionNum = (string)reader["version_num"];
                    pluginDAO.OwnerID = (int)reader["owner_id"];
                    pluginDAO.Access = (PluginAccess)reader["plugin_access"];
                    pluginDAO.HelpText = (string)reader["help_text"];
                }

                if (null == pluginDAO) throw new CouldNotFindException("Could not find plugin with command: " + commandText);

                return pluginDAO;
            }
        }

        /// <summary>
        /// Updates plugin information in the database. If there is no entry in the database that matches the given
        /// PluginDAO.PluginID no entries will be updated and <code>false</code> will be returned.
        /// </summary>
        /// <param name="user">The PluginDAO to update in the database</param>
        /// <returns>true if the plugin was successfully updated.</returns>
        /// <exception cref="ArgumentNullException">If the given PluginDAO or PluginDAO.PluginID is null.</exception>
        public bool UpdatePlugin(PluginDAO plugin)
        {
            if (null == plugin || null == plugin.PluginID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("UPDATE plugins SET ");
                queryBuilder.Append("name = @name, ");
                queryBuilder.Append("description = @description, ");
                queryBuilder.Append("disabled = @disabled, ");
                queryBuilder.Append("version_num = @version_num, ");
                queryBuilder.Append("owner_id = @owner_id, ");
                queryBuilder.Append("plugin_access = @plugin_access, ");
                queryBuilder.Append("help_text = @help_text ");
                queryBuilder.Append("WHERE id = @pluginid");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@name", plugin.Name);
                query.Parameters.AddWithValue("@description", plugin.Description);
                query.Parameters.AddWithValue("@disabled", plugin.IsDisabled ? 1 : 0);
                query.Parameters.AddWithValue("@version_num", plugin.VersionNum);
                query.Parameters.AddWithValue("@owner_id", plugin.OwnerID);
                query.Parameters.AddWithValue("@plugin_access", (int)plugin.Access);
                query.Parameters.AddWithValue("@help_text", plugin.HelpText);
                query.Parameters.AddWithValue("@pluginid", plugin.PluginID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                // Only one record should have been updated
                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Deletes an existing plugin that matches the given PluginDAO.
        /// </summary>
        /// <param name="user">The PluginDAO to delete from the database.</param>
        /// <returns>true if the plugin was successfully deleted.</returns>
        /// <exception cref="ArgumentNullException">If the given PluginDAO or PluginDAO.PluginID is null.</exception>
        public bool DeletePlugin(PluginDAO plugin)
        {
            if (null == plugin || null == plugin.PluginID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "sp_deletePlugin";
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@pluginid", plugin.PluginID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                /* One or more records should have been deleted:
                 * The plugin record itself (1), and any additional groupplugin entries (0 or more)
                 */
                return 0 < effectedRows;
            }
        }

        private bool PluginExists(PluginDAO plugin)
        {
            try
            {
                return RetrievePlugin(plugin.Name).Name.Equals(plugin.Name);
            }
            catch (CouldNotFindException)
            {
                return false;
            }
        }

        #endregion

        #region Other Miscellaneous database interactions

        /// <summary>
        /// Inserts a message into the database with the given message and level of importance.
        /// </summary>
        /// <param name="message">The message for the log.</param>
        /// <param name="level">The level of the given.</param>
        /// <returns>true if the message was successfully logged.</returns>
        /// <exception cref="ArgumentNullException">If the given message is null.</exception>
        /// <seealso cref="LoggerLevel"/>
        public bool LogMessage(string message, LoggerLevel level)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "INSERT INTO eventlog (message, level, created_dt) VALUES (@message, @level, GETDATE())";
                query.Parameters.AddWithValue("@message", message);
                query.Parameters.AddWithValue("@level", (int) level);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                // Only one record should have been inserted
                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Checks that a user with the given username and password exists in the database.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="password">The password to check.</param>
        /// <returns>true if the user exists.</returns>
        public bool CheckLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return false;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "SELECT * FROM users WHERE username = @username AND password = UNHEX(SHA1(@password))";
                query.Parameters.AddWithValue("@username", username);
                query.Parameters.AddWithValue("@password", password);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                int count = 0;
                while (reader.Read()) ++count;

                // Only one record should have been returned
                return 1 == count;
            }
        }

        #endregion
    }
}
