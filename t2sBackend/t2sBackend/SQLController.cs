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
        /// <param name="password">The password for the user.</param>
        /// <returns>true if the user was successfully added and the UserID was set</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO is null.</exception>
        /// <exception cref="SqlException">If there is an error querying the database.</exception>
        public bool CreateUser(UserDAO user, string password)
        {
            if (null == user) throw new ArgumentNullException();

            if (UserExists(user.UserName, user.PhoneEmail)) throw new EntryAlreadyExistsException("User with username: " + user.UserName + " or phone email: " + user.PhoneEmail + " already exists.");

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO users (username, password, first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed, created_dt) ");
                queryBuilder.Append("VALUES ");
                queryBuilder.Append("(@username, CONVERT(VARBINARY, HASHBYTES('SHA1', @password)), @first_name, @last_name, @phone, @email_phone, @carrier, @user_level, @banned, @suppressed, GETDATE()) ");
                queryBuilder.Append("; SELECT SCOPE_IDENTITY()");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@username", user.UserName);
                query.Parameters.AddWithValue("@password", password);
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
                queryBuilder.Append("username = @username, ");
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
        /// Checks if the given username or phoneEmail exists in the database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual bool UserExists(string username, string phoneEmail)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(phoneEmail)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "SELECT * FROM users WHERE username = @username OR email_phone = @email_phone";
                query.Parameters.AddWithValue("@username", username);
                query.Parameters.AddWithValue("@email_phone", phoneEmail);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // Returns true if there is something to read
                return reader.Read();
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
            if (!UserExists(group.Owner.UserName, group.Owner.PhoneEmail)) 
                throw new CouldNotFindException("User with username: " + group.Owner.UserName + " needs to be created before the group can be created.");

            if (GroupExists(group.Name))
                throw new EntryAlreadyExistsException("Group with name: " + group.Name + " already exists.");

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
        private bool InsertGroupMembers(int? groupID, List<UserDAO> users, GroupLevel groupLevel)
        {
            foreach(UserDAO user in users)
            {
                if (!UserExists(user.UserName, user.PhoneEmail)) throw new CouldNotFindException("User with username: " + user.UserName + " needs to be created before being added to the group");
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
        private bool InsertGroupMember(int? groupID, int? userID, GroupLevel groupLevel)
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

        private bool InsertGroupPlugins(int? groupID, List<IPlugin> plugins)
        {
            foreach (IPlugin plugin in plugins)
            {
                if (!PluginExists(plugin.PluginDAO.Name)) CreatePlugin(plugin.PluginDAO);
                if (!InsertGroupPlugin(groupID, plugin)) return false;
            }

            return true;
        }

        private bool InsertGroupPlugin(int? groupID, IPlugin plugin)
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

        /// <summary>
        /// Retrieves a GroupDAO object representing information about a specific group, including users, moderators, and plugins.
        /// </summary>
        /// <param name="groupTag">The group tag of the group to retrieve.</param>
        /// <returns>A new GroupDAO object, complete with lists of users, moderators, and plugins associated with the group.</returns>
        /// <exception cref="ArgumentNullException">If the given group tag is null.</exception>
        /// <exception cref="CouldNotFindException">If a group with the given group tag could not be found.</exception>
        public GroupDAO RetrieveGroup(string groupTag)
        {
            if (string.IsNullOrEmpty(groupTag)) throw new ArgumentNullException();

            GroupDAO groupDAO = RetrieveGroupMetadata(groupTag);
            // when retrieving users in the group, only return users that are not suppressed or banned
            RetrieveGroupUsers(groupDAO);
            // when retrieving enabled plugins for the group, return plugins that are selected by the group AND are not disabled
            RetrieveGroupPlugins(groupDAO);

            return groupDAO;
        }

        private GroupDAO RetrieveGroupMetadata(string groupTag)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, name, description, owner_id ");
                queryBuilder.Append("FROM groups ");
                queryBuilder.Append("WHERE grouptag = @grouptag");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@grouptag", groupTag);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                GroupDAO group = null;

                // If there are no records returned from the select statement, the DataReader will be empty
                while (reader.Read())
                {
                    group = new GroupDAO();
                    group.GroupID = (int)reader["id"];
                    group.Name = (string)reader["name"];
                    group.Description = (string)reader["description"];
                    group.Owner.UserID = (int)reader["owner_id"];
                }

                if (null == group) throw new CouldNotFindException("Could not find user with groupTag: " + groupTag);

                return group;
            }
        }

        private void RetrieveGroupUsers(GroupDAO group)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT u.id, u.username, u.first_name, u.last_name, u.phone, u.email_phone, u.carrier, u.user_level, u.banned, u.suppressed, gm.group_level ");
                queryBuilder.Append("FROM users u ");
                queryBuilder.Append("INNER JOIN groupmembers gm ON u.id = gm.user_id ");
                queryBuilder.Append("WHERE gm.group_id = 2 ");
                queryBuilder.Append("AND u.suppressed = 0;");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@groupid", group.GroupID);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // If there are no records returned from the select statement, the DataReader will be empty
                while (reader.Read())
                {
                    UserDAO userDAO = new UserDAO();
                    userDAO.UserID = (int)reader["id"];
                    userDAO.UserName = (string)reader["username"];
                    userDAO.FirstName = (string)reader["first_name"];
                    userDAO.LastName = (string)reader["last_name"];
                    userDAO.PhoneNumber = (string)reader["phone"];
                    userDAO.PhoneEmail = (string)reader["email_phone"];
                    userDAO.Carrier = (PhoneCarrier)reader["carrier"];
                    userDAO.UserLevel = (UserLevel)reader["user_level"];
                    userDAO.IsBanned = (bool)reader["banned"];
                    userDAO.IsSuppressed = (bool)reader["suppressed"];

                    switch ((int)reader["group_level"])
                    {
                        // Moderator
                        case 1:
                            group.AddModerator(userDAO);
                            break;
                        // Owner
                        case 2:
                            //group.Owner = userDAO; // This needs to be addressed, since the GroupDAO.Owner setter is private
                            break;
                        // User
                        default:
                            group.AddUserToGroup(userDAO);
                            break;
                    }
                }
            }
        }

        private void RetrieveGroupPlugins(GroupDAO group)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT p.id, p.name, p.description, p.disabled, p.version_num, p.owner_id, p.plugin_access, p.help_text ");
                queryBuilder.Append("FROM plugins p ");
                queryBuilder.Append("INNER JOIN groupplugins gm ON p.id = gm.plugin_id ");
                queryBuilder.Append("WHERE gm.group_id = @groupid ");
                queryBuilder.Append("AND gm.disabled = 0 ");
                queryBuilder.Append("AND p.disabled = 0");               

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@groupid", group.GroupID);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // If there are no records returned from the select statement, the DataReader will be empty
                while (reader.Read())
                {
                    //This needs to be addressed, as there is no current way of adding PluginDAOs to a GroupDAO
                    //group.AddPlugin(new IPlugin() {
                    //    new PluginDAO() {
                    //        PluginID = (int)reader["id"],
                    //        Name = (string)reader["name"],
                    //        Description = (string)reader["description"],
                    //        IsDisabled = (bool)reader["disabled"],
                    //        VersionNum = (string)reader["version_num"],
                    //        OwnerID = (int)reader["owner_id"],
                    //        Access = (PluginAccess)reader["plugin_access"],
                    //        HelpText = (string)reader["help_text"]
                    //    }
                    //});
                }
            }

        }

        public bool UpdateGroup(GroupDAO group)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes an existing group that matches the given GroupDAO. All users and plugin assocations with the given
        /// GroupDAO will be deleted, however the users and plugins will not be.
        /// </summary>
        /// <param name="user">The PluginDAO to delete from the database.</param>
        /// <returns>true if the plugin was successfully deleted.</returns>
        /// <exception cref="ArgumentNullException">If the given PluginDAO or PluginDAO.PluginID is null.</exception>
        public bool DeleteGroup(GroupDAO group)
        {
            if (null == group || null == group.GroupID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "sp_deleteGroup";
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@groupid", group.GroupID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                /* One or more records should have been deleted:
                 * The plugin record itself (1), and any additional groupplugin entries (0 or more)
                 */
                return 0 < effectedRows;
            }
        }

        /// <summary>
        /// Checks to see if a group with the given name exists in the database.
        /// </summary>
        /// <param name="name">THe name of the group to check in the database.</param>
        /// <returns>true if the group already exists.</returns>
        /// <exception cref="ArgumentNullException">If the given name is null.</exception>
        public virtual bool GroupExists(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "SELECT * FROM groups WHERE name = @name";
                query.Parameters.AddWithValue("@name", name);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // Returns true if there is something to read
                return reader.Read();
            }
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
        /// <exception cref="EntryAlreadyExistsException">If a plugin already exists with the given PluginDAO.Name.</exception>
        public bool CreatePlugin(PluginDAO plugin)
        {
            if (null == plugin) throw new ArgumentNullException();

            if (PluginExists(plugin.Name)) throw new EntryAlreadyExistsException("Plugin with command: " + plugin.Name + " already exists.");

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

        /// <summary>
        /// Checks to see if a plugin already exists with the given command text.
        /// </summary>
        /// <param name="plugin">The command text to search for.</param>
        /// <returns>true if a plugin exists that matches the given string.</returns>
        /// <exception cref="ArgumentNullException">If the given command text is null.</exception>
        public virtual bool PluginExists(string commandText)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "SELECT * FROM plugins WHERE name = @commandText";
                query.Parameters.AddWithValue("@commandText", commandText);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // Returns true if there is something to read
                return reader.Read();
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
