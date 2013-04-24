using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace t2sDbLibrary
{
    public class SqlController : IDBController
    {
        /// <summary>
        /// The connection string used for connecting to the database. 
        /// Do NOT modify these values unless the directory of the database changes.
        /// </summary>
        private static string _CONNECTION_STRING = @"Data Source=(LocalDB)\v11.0;Integrated Security=True;AttachDbFilename=|DataDirectory|\MainDatabase.mdf";
        public static string CONNECTION_STRING
        {
            get { return _CONNECTION_STRING; }
            set { _CONNECTION_STRING = value; }
        }


        #region UserDAO "CRUD" actions

        /// <summary>
        /// Creates a new user entry in the database with the given UserDAO.
        /// The UserID of the given UserDAO will also be set after calling this method.
        /// </summary>
        /// <param name="user">The UserDAO to insert into the database.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>true if the user was successfully added and the UserID was set</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO or password is null.</exception>
        /// <exception cref="SqlException">If there is an error querying the database.</exception>
        public bool CreateUser(UserDAO user, string password)
        {
            if (null == user || string.IsNullOrEmpty(password)) throw new ArgumentNullException();

            if (UserExists(user.UserName, user.PhoneEmail)) throw new EntryAlreadyExistsException("User with username: " + user.UserName + " or phone email: " + user.PhoneEmail + " already exists.");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO users (username, password, salt, first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed, created_dt) ");
                queryBuilder.Append("VALUES ");
                queryBuilder.Append("(@username, CONVERT(VARBINARY, HASHBYTES('SHA1', @password)), @salt, @first_name, @last_name, @phone, @email_phone, @carrier, @user_level, @banned, @suppressed, GETDATE()) ");
                queryBuilder.Append("; SELECT SCOPE_IDENTITY()");

                String salt = GenerateSalt(128);

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@username", user.UserName);
                query.Parameters.AddWithValue("@password", password + salt);
                query.Parameters.AddWithValue("@salt", salt);
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

        private static Random _random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Used to generate 128 character strings to act as a random salt for passwords
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string GenerateSalt(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Grabs an individual user based on the given username string.
        /// </summary>
        /// <param name="username">The username to query for.</param>
        /// <returns>A new UserDAO object with data related to the given username.</returns>
        /// <exception cref="ArgumentNullException">If the given string is null.</exception>
        /// <exception cref="CouldNotFindException">If the user for the given username could not be found.</exception>
        public virtual UserDAO RetrieveUserByUserName(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, username, first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed ");
                queryBuilder.Append("FROM users ");
                queryBuilder.Append("WHERE username = @username");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@username", username);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // If there are no records returned from the select statement, the DataReader will be empty
                if (reader.Read()) return BuildUserDAO(reader);
                else throw new CouldNotFindException("Could not find user with username: " + username);
            }
        }

        /// <summary>
        /// Grabs an individual user based on the given phone email string. The string should be in a format similar to
        /// <code>String userPhoneEmail = "1234567890@carrier.com"</code>
        /// in order to grab the correct information.
        /// </summary>
        /// <param name="username">The username to query for.</param>
        /// <returns>A new UserDAO object with data related to the given phone email.</returns>
        /// <exception cref="ArgumentNullException">If the given string is null.</exception>
        /// <exception cref="CouldNotFindException">If the user for the given phone email could not be found.</exception>
        public virtual UserDAO RetrieveUserByPhoneEmail(string phoneEmail)
        {
            if (string.IsNullOrEmpty(phoneEmail)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, username, first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed ");
                queryBuilder.Append("FROM users ");
                queryBuilder.Append("WHERE email_phone = @email_phone");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@email_phone", phoneEmail);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // If there are no records returned from the select statement, the DataReader will be empty
                if (reader.Read()) return BuildUserDAO(reader);
                else throw new CouldNotFindException("Could not find user with username: " + phoneEmail);
            }
        }

        /// <summary>
        /// Grabs an individual user based on the given user id.
        /// </summary>
        /// <param name="userID">The user id to query for.</param>
        /// <returns>A new UserDAO object with data related to the given user id.</returns>
        /// <exception cref="ArgumentNullException">If the given id is null.</exception>
        /// <exception cref="CouldNotFindException">If the user for the given id could not be found.</exception>
        public UserDAO RetrieveUser(int? userID)
        {
            if (null == userID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, username, first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed ");
                queryBuilder.Append("FROM users ");
                queryBuilder.Append("WHERE id = @id");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@id", userID);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // If there are no records returned from the select statement, the DataReader will be empty
                if (reader.Read()) return BuildUserDAO(reader);
                else throw new CouldNotFindException("Could not find user with userID: " + userID);
            }
        }

        private UserDAO BuildUserDAO(SqlDataReader reader)
        {
            UserDAO userDAO = new UserDAO();
            userDAO.UserID = (int)reader["id"];
            userDAO.UserName = (string)reader["username"];
            userDAO.FirstName = (string)reader["first_name"];
            userDAO.LastName = (string)reader["last_name"];
            userDAO.PhoneNumber = (string)reader["phone"];
            userDAO.PhoneEmail = (string)reader["email_phone"];
            userDAO.Carrier = (PhoneCarrier)(int)reader["carrier"];
            userDAO.UserLevel = (UserLevel)reader["user_level"];
            userDAO.IsBanned = (bool)reader["banned"];
            userDAO.IsSuppressed = (bool)reader["suppressed"];
            return userDAO;
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

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
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
        /// Deletes an existing user that matches the given UserDAO.  The user should not be admin of a group, or have plugins
        /// </summary>
        /// <param name="user">The UserDAO to delete from the database.</param>
        /// <returns>true if the user was successfully deleted. Returns false if the user cannot be deleted due to them owning a Plugin or Group.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO or UserDAO.UserID is null.</exception>
        public bool DeleteUser(UserDAO user)
        {
            // We should always check before deleting
            return DeleteUser(user, true);
        }

        /// <summary>
        /// Deletes an existing user that matches the given UserDAO.
        /// </summary>
        /// <param name="user">The UserDAO to delete from the database.</param>
        /// <param name="checkIsOwnerBeforeRemoving">If true, checks to see if the user is an owner of a group or plugin.</param>
        /// <returns>true if the user was successfully deleted. Returns false if the user cannot be deleted due to them owning a Plugin or Group.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO or UserDAO.UserID is null.</exception>
        public bool DeleteUser(UserDAO user, bool checkIsOwnerBeforeRemoving)
        {
            if (null == user || null == user.UserID) throw new ArgumentNullException();

            if (checkIsOwnerBeforeRemoving && CheckIfOwnerOfGroupOrPlugin(user))
                return false;

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
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
                return effectedRows > 0;
            }
        }

        /// <summary>
        /// Checks if the given user is the owner of a group or plugin.
        /// </summary>
        /// <param name="user">The user to check in the database.</param>
        /// <returns>true if the user is an owner.</returns>
        private bool CheckIfOwnerOfGroupOrPlugin(UserDAO user)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "SELECT COUNT(*) FROM groups, plugins WHERE owner_id = @userid";
                query.Parameters.AddWithValue("@userid", user.UserID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                /* If 1 or more records exist, then the user owns a group or plugin
                 */
                return effectedRows > 0;
            }
        }

        /// <summary>
        /// Checks if the given username or phoneEmail exists in the database. Since usernames and phone emails are unique,
        /// this method is useful to see if a user already exists with the name or phone email.
        /// </summary>
        /// <param name="username">The username of the user to check.</param>
        /// <param name="phoneEmail">The phone email of the user to check.</param>
        /// <returns>true if a user exists.</returns>
        public virtual bool UserExists(string username, string phoneEmail)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(phoneEmail)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
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

        public List<UserDAO> GetAllUsers()
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, username, first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed FROM users ");

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                List<UserDAO> userList = new List<UserDAO>();

                while (reader.Read())
                {
                    userList.Add(new UserDAO()
                    {
                        UserID = (int?)reader["id"],
                        UserName = (string)reader["username"],
                        FirstName = (string)reader["firstname"],
                        LastName = (string)reader["lastname"],
                        PhoneNumber = (string)reader["phone"],
                        PhoneEmail = (string)reader["email_phone"],
                        Carrier = (PhoneCarrier)reader["carrier"],
                        UserLevel = (UserLevel)reader["user_level"],
                        IsBanned = (bool)reader["banned"],
                        IsSuppressed = (bool)reader["suppressed"]
                    });
                }

                return userList;
            }
        }

        /// <summary>
        /// Suppresses a user to prevent them from receiving texts.
        /// </summary>
        /// <param name="pluginID">The user to suppress.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given user is null.</exception>
        public bool SuppressUser(UserDAO user)
        {
            return ToggleUserSuppression(user, true);
        }

        /// <summary>
        /// Unsuppresses a user so they can continue to receive texts.
        /// </summary>
        /// <param name="pluginID">The user to unsuppress.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given user is null.</exception>
        public bool UnsuppressUser(UserDAO user)
        {
            return ToggleUserSuppression(user, false);
        }

        private bool ToggleUserSuppression(UserDAO user, bool value)
        {
            if (null == user) throw new ArgumentNullException("Cannot update suppression for null user.");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("UPDATE users ");
                queryBuilder.Append("SET suppressed = @value ");
                queryBuilder.Append("WHERE id = @user_id ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@value", value);
                query.Parameters.AddWithValue("@user_id", user.UserID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Bans a user to prevent them from sending or receiving texts.
        /// </summary>
        /// <param name="pluginID">The user to ban.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given user is null.</exception>
        public bool BanUser(UserDAO user)
        {
            return ToggleUserBanned(user, true);
        }

        /// <summary>
        /// Unbans a user so they can continue to send and receive texts.
        /// </summary>
        /// <param name="pluginID">The user to unban.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given user is null.</exception>
        public bool UnbanUser(UserDAO user)
        {
            return ToggleUserBanned(user, false);
        }

        private bool ToggleUserBanned(UserDAO user, bool value)
        {
            if (null == user) throw new ArgumentNullException("Cannot update banning for null user.");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("UPDATE users ");
                queryBuilder.Append("SET banned = @value ");
                queryBuilder.Append("WHERE id = @user_id ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@value", value);
                query.Parameters.AddWithValue("@user_id", user.UserID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Gets a list of groups that the given user is a moderator for
        /// </summary>
        /// <param name="userid">The id of the user to search for</param>
        /// <returns>A list containing all groups the user is a moderator of</returns>
        public List<GroupDAO> GetGroupsUserIsModeratorOf(int? userid)
        {
            return GetGroupsForUserWithGroupLevel(userid, GroupLevel.Moderator);
        }

        /// <summary>
        /// Gets a list of groups that the given user is the owner of
        /// </summary>
        /// <param name="userid">The id of the user to search for</param>
        /// <returns>A list containing all groups the user is the owner of</returns>
        public List<GroupDAO> GetGroupsUserIsOwnerOf(int? userid)
        {
            return GetGroupsForUserWithGroupLevel(userid, GroupLevel.Owner);
        }

        /// <summary>
        /// Gets a list of groups that the given user is a user for
        /// </summary>
        /// <param name="userid">The id of the user to search for</param>
        /// <returns>A list containing all groups the user is a user of</returns>
        public List<GroupDAO> GetGroupsUserIsMemberOf(int? userid)
        {
            return GetGroupsForUserWithGroupLevel(userid, GroupLevel.User);
        }

        /// <summary>
        /// Gets a list of groups for a particular user based on a specific GroupLevel the user might be.
        /// </summary>
        /// <param name="userid">The id of the user to search for.</param>
        /// <param name="level">The GroupLevel the user is at within each group.</param>
        /// <returns>A list containing all groups with the user at the given level.</returns>
        private List<GroupDAO> GetGroupsForUserWithGroupLevel(int? userid, GroupLevel level)
        {
            if (null == userid) throw new ArgumentNullException("Cannot get information for null user.");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT grouptag FROM groups g ");
                queryBuilder.Append("INNER JOIN groupmembers gm ON g.id = gm.group_id ");
                queryBuilder.Append("WHERE gm.user_id = @userid AND gm.group_level = @grouplevel ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@userid", userid);
                query.Parameters.AddWithValue("@grouplevel", level);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                List<GroupDAO> groups = new List<GroupDAO>();
                while (reader.Read())
                {
                    groups.Add(RetrieveGroup((string)reader["grouptag"]));
                }

                return groups;
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
            if (null == group.Owner) throw new CouldNotFindException("Group Owner is null, cannot find in database.");
            if (!UserExists(group.Owner.UserName, group.Owner.PhoneEmail)) 
                throw new CouldNotFindException("User with username: " + group.Owner.UserName + " needs to be created before the group can be created.");

            if (GroupExists(group.Name))
                throw new EntryAlreadyExistsException("Group with name: " + group.Name + " already exists.");

            return (InsertGroupMetadata(group) &&
                InsertGroupMember(group.GroupID, group.Owner.UserID, GroupLevel.Owner) &&
                InsertGroupMembers(group.GroupID, group.Moderators, GroupLevel.Moderator) &&
                InsertGroupMembers(group.GroupID, group.Users, GroupLevel.User) &&
                InsertGroupPlugins(group.GroupID, group.EnabledPlugins));
        }

        /// <summary>
        /// Inserts the group metadata into the "groups" table.
        /// </summary>
        /// <param name="group">The GroupDAO to insert into the database.</param>
        private bool InsertGroupMetadata(GroupDAO group)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
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
                query.Parameters.AddWithValue("@owner_id", group.Owner.UserID);
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
        private bool InsertGroupMembers(int? groupID, HashSet<UserDAO> users, GroupLevel groupLevel)
        {
            foreach(UserDAO user in users)
            {
                if (!UserExists(user.UserName, user.PhoneEmail)) throw new CouldNotFindException("User with username: " + user.UserName + " needs to be created before being added to the group.");
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
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO groupmembers (group_id, user_id, group_level, added_dt) ");
                queryBuilder.Append("VALUES ");
                queryBuilder.Append("(@group_id, @user_id, @group_level, GETDATE())");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@group_id", groupID);
                query.Parameters.AddWithValue("@user_id", userID);
                query.Parameters.AddWithValue("@group_level", (int)groupLevel);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }

        private bool InsertGroupPlugins(int? groupID, HashSet<PluginDAO> plugins)
        {
            foreach (PluginDAO plugin in plugins)
            {
                if (!PluginExists(plugin.Name)) CreatePlugin(plugin);
                if (!InsertGroupPlugin(groupID, plugin.PluginID, plugin.IsDisabled)) return false;
            }

            return true;
        }

        private bool InsertGroupPlugin(int? groupID, int? pluginID, bool isDisabled)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO groupplugins (group_id, plugin_id, disabled, created_dt) ");
                queryBuilder.Append("VALUES ");
                queryBuilder.Append("(@group_id, @plugin_id, @disabled, GETDATE())");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@group_id", groupID);
                query.Parameters.AddWithValue("@plugin_id", pluginID);
                query.Parameters.AddWithValue("@disabled", isDisabled);

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
        public virtual GroupDAO RetrieveGroup(string groupTag)
        {
            if (string.IsNullOrEmpty(groupTag)) throw new ArgumentNullException();

            GroupDAO groupDAO = RetrieveGroupMetadata(groupTag);
            // when retrieving users in the group, only return users that are not suppressed or banned
            RetrieveGroupUsers(groupDAO);
            // when retrieving enabled plugins for the group, return plugins that are selected by the group AND are not disabled
            RetrieveGroupPlugins(groupDAO);

            return groupDAO;
        }

        /// <summary>
        /// Retrieves basic information for a group in a GroupDAO. Does not include users or plugins for a group.
        /// </summary>
        /// <param name="groupTag">The group tag of the group to retrieve.</param>
        /// <returns>A new GroupDAO object with basic group data.</returns>
        public GroupDAO RetrieveGroupMetadata(string groupTag)
        {
            if (string.IsNullOrEmpty(groupTag)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, name, description, owner_id, grouptag ");
                queryBuilder.Append("FROM groups ");
                queryBuilder.Append("WHERE grouptag = @grouptag");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@grouptag", groupTag);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                GroupDAO group = null;

                // If there are no records returned from the select statement, the DataReader will be empty
                if (reader.Read())
                {
                    int groupID = (int)reader["id"];
                    string name = (string)reader["name"];
                    string description = (string)reader["description"];
                    int ownerID = (int)reader["owner_id"];
                    string groupTagReader = (string)reader["grouptag"];

                    group = new GroupDAO(RetrieveUser(ownerID));
                    group.GroupID = groupID;
                    group.Name = name;
                    group.Description = description;
                    group.GroupTag = groupTagReader;

                    return group;
                }
                else throw new CouldNotFindException("Could not find user with groupTag: " + groupTag);
            }
        }

        private void RetrieveGroupUsers(GroupDAO group)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT u.id, u.username, u.first_name, u.last_name, u.phone, u.email_phone, u.carrier, u.user_level, u.banned, u.suppressed, gm.group_level ");
                queryBuilder.Append("FROM users u ");
                queryBuilder.Append("INNER JOIN groupmembers gm ON u.id = gm.user_id ");
                queryBuilder.Append("WHERE gm.group_id = @groupid ");
                queryBuilder.Append("AND u.suppressed = 0;");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@groupid", group.GroupID);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // If there are no records returned from the select statement, the DataReader will be empty
                while (reader.Read())
                {
                    UserDAO userDAO = BuildUserDAO(reader);

                    switch ((int)reader["group_level"])
                    {
                        // Moderator
                        case 1:
                            group.AddModerator(userDAO);
                            break;
                        // Owner
                        case 2:
                            // The owner should have been added in RetrieveGroupMetadata(groupTag)
                            break;
                            //group.Owner = userDAO; // This needs to be addressed, since the GroupDAO.Owner setter is private
                            //break;
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
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
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
                    group.AddPlugin(BuildPluginDAO(reader));
                }
            }

        }

        /// <summary>
        /// Updates the metadata, the list of users for the group, and all enabled plugin relationships for the given group.
        /// </summary>
        /// <param name="group">The GroupDAO to update in the database.</param>
        /// <returns>true if successful.</returns>
        public bool UpdateGroup(GroupDAO group)
        {
            if (null == group || null == group.GroupID) throw new ArgumentNullException();

            return (UpdateGroupMetadata(group) &&
                UpdateGroupMembers(group) &&
                UpdateGroupPlugins(group));
        }

        /// <summary>
        /// Updates the metadata (in the groups table) only for the given group.
        /// </summary>
        /// <param name="group">The group to update</param>
        /// <returns>true if successful</returns>
        public bool UpdateGroupMetadata(GroupDAO group)
        {
            if (null == group || null == group.GroupID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("UPDATE groups SET ");
                queryBuilder.Append("name = @name, ");
                queryBuilder.Append("description = @description, ");
                queryBuilder.Append("owner_id = @owner_id, ");
                queryBuilder.Append("grouptag = @grouptag ");
                queryBuilder.Append("WHERE id = @group_id");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@name", group.Name);
                query.Parameters.AddWithValue("@description", group.Description);
                query.Parameters.AddWithValue("@owner_id", group.Owner.UserID);
                query.Parameters.AddWithValue("@grouptag", group.GroupTag);
                query.Parameters.AddWithValue("@group_id", group.GroupID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                // Only one record should have been updated
                return 1 == effectedRows;
            }
        }

        private bool UpdateGroupMembers(GroupDAO group)
        {
            CheckForUserListModifications(group.GroupID, group.Users, GroupLevel.User);
            CheckForUserListModifications(group.GroupID, group.Moderators, GroupLevel.Moderator);

            return true;
        }

        private void CheckForUserListModifications(int? groupID, HashSet<UserDAO> userList, GroupLevel groupLevel)
        {
            // Get the current list of all users for a specific group level
            List<int?> userIDList = GetAllGroupMemberIDs(groupID, true, groupLevel);

            // For each user in the current group object
            foreach (UserDAO user in userList)
                // If the user is not already a member in the database, add them
                if (!userIDList.Contains(user.UserID)) AddMemberToGroup(groupID, user.UserID, groupLevel);
                // Otherwise, remove their ID from the list
                else userIDList.Remove(user.UserID);

            // At this point, userIDList contains user IDs of members who are no longer in the group
            if (userIDList.Count > 0)
                // For each user no longer in the group
                foreach (int? id in userIDList)
                    // Remove them from the group in the database
                    RemoveMemberFromGroup(groupID, id);
        }

        private bool UpdateGroupPlugins(GroupDAO group)
        {
            CheckForPluginListModifications(group.GroupID, group.EnabledPlugins);

            return true;
        }

        private void CheckForPluginListModifications(int? groupID, HashSet<PluginDAO> pluginList)
        {
            // Get the current list of all plugins for a group
            List<int?> pluginIDList = GetAllEnabledGroupPluginIDs(groupID);

            // For each plugin in the current group object
            foreach (PluginDAO plugin in pluginList)
                // If the plugin is not already a part of the group in the database, add it
                if (!pluginIDList.Contains(plugin.PluginID)) AddPluginToGroup(groupID, plugin.PluginID, false);
                // Otherwise, remove the plugin ID from the list
                else pluginIDList.Remove(plugin.PluginID);

            // At this point, pluginIDList contains plugin IDs of plugins no longer associated with the group
            if (pluginIDList.Count > 0)
                // For each plugin no longer in the group
                foreach (int? id in pluginIDList)
                    // Remove them from the group in the database
                    RemovePluginFromGroup(groupID, id);
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

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "sp_deleteGroup";
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@groupid", group.GroupID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                /* One or more records should have been deleted:
                 * The group record itself (1), any groupmember references (>= 0), and any groupplugin references (>= 0)
                 */
                return 0 < effectedRows;
            }
        }

        /// <summary>
        /// Checks to see if a group with the given grouptag exists in the database.
        /// </summary>
        /// <param name="name">The grouptag of the group to check in the database.</param>
        /// <returns>true if the group already exists.</returns>
        /// <exception cref="ArgumentNullException">If the given grouptag is null.</exception>
        public virtual bool GroupExists(string grouptag)
        {
            if (string.IsNullOrWhiteSpace(grouptag)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "SELECT * FROM groups WHERE grouptag = @grouptag";
                query.Parameters.AddWithValue("@grouptag", grouptag);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // Returns true if there is something to read
                return reader.Read();
            }
        }

        /// <summary>
        /// Checks to see if a group with the given grouptag that isn't related to the
        /// given group id exists.
        /// 
        /// This method is useful when updating an existing group's metadata and verifying
        /// initially if there is a group that already exists with the same grouptag. If
        /// GroupExists(grouptag) were simply run when trying to update a group with the given
        /// grouptag, then the method would return true and the group would not be able to be
        /// updated correctly.
        /// </summary>
        /// <param name="grouptag">The grouptag to check for.</param>
        /// <param name="groupid">The groupid to check against.</param>
        /// <returns>true if a grouptag that doesn't correspond with the given groupid exists.</returns>
        public bool GroupExists(string grouptag, int? groupid)
        {
            if (string.IsNullOrWhiteSpace(grouptag)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "SELECT * FROM groups WHERE grouptag = @grouptag AND id <> @groupid";
                query.Parameters.AddWithValue("@grouptag", grouptag);
                query.Parameters.AddWithValue("@groupid", groupid);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // Returns true if there is something to read
                return reader.Read();
            }
        }

        /// <summary>
        /// Adds a user with a specified administrative level to a group.
        /// </summary>
        /// <param name="groupID">The GroupID of the group to add the user to.</param>
        /// <param name="userID">The UserID of the user to add.</param>
        /// <param name="groupLevel">The level of the user's permissions in the group.</param>
        /// <returns>true if the user was added successfully.</returns>
        /// <exception cref="ArgumentNullException">If the given groupID or userID are null.</exception>
        /// <seealso cref="GroupLevel"/>
        public bool AddMemberToGroup(int? groupID, int? userID, GroupLevel groupLevel)
        {
            if (null == groupID || null == userID) throw new ArgumentNullException("Cannot add null userID, or add user to null groupID.");

            return InsertGroupMember(groupID, userID, groupLevel);
        }

        /// <summary>
        /// Checks to see if the given user is already a member of the given group.
        /// </summary>
        /// <param name="groupID">The GroupID of the group to check.</param>
        /// <param name="userID">The UserID of the user.</param>
        /// <returns>true if the user is already a member of the group.</returns>
        public bool IsUserMemberOfGroup(int? groupID, int? userID)
        {
            if (null == groupID || null == userID) return false;

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM groupmembers ");
                queryBuilder.Append("WHERE group_id = @group_id ");
                queryBuilder.Append("AND user_id = @user_id ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@group_id", groupID);
                query.Parameters.AddWithValue("@user_id", userID);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                return reader.Read();
            }
        }

        /// <summary>
        /// Removes an individual user from a group. If the user was not already in the group, this method returns false.
        /// </summary>
        /// <param name="groupID">The GroupID of the group to remove the user from.</param>
        /// <param name="userID">The UserID of the user to remove.</param>
        /// <returns>true if the user was successfully removed</returns>
        public bool RemoveMemberFromGroup(int? groupID, int? userID)
        {
            if (null == groupID || null == userID) throw new ArgumentNullException("Cannot remove null userID, or remove user from null groupID.");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("DELETE FROM groupmembers ");
                queryBuilder.Append("WHERE group_id = @group_id ");
                queryBuilder.Append("AND user_id = @user_id");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@group_id", groupID);
                query.Parameters.AddWithValue("@user_id", userID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Gets a list of all members (regardless of group level) for the given groupID.
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns>A list of UserDAOs that is representative of all users for a group.</returns>
        public List<UserDAO> GetAllGroupMembers(int? groupID)
        {
            return GetAllGroupMembers(groupID, false, 0);
        }

        private List<UserDAO> GetAllGroupMembers(int? groupID, bool byGroupLevel, GroupLevel groupLevel)
        {
            List<int?> userIDList = GetAllGroupMemberIDs(groupID, byGroupLevel, groupLevel);
            List<UserDAO> userList = new List<UserDAO>();
            foreach (int? id in userIDList) userList.Add(RetrieveUser(id));

            return userList;
        }

        private List<int?> GetAllGroupMemberIDs(int? groupID, bool byGroupLevel, GroupLevel groupLevel)
        {
            if (null == groupID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT user_id FROM groupmembers ");
                queryBuilder.Append("WHERE group_id = @group_id ");
                if (byGroupLevel)
                    queryBuilder.Append("AND group_level = @group_level");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@group_id", groupID);
                if (byGroupLevel)
                    query.Parameters.AddWithValue("@group_level", (int)groupLevel);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                List<int?> userList = new List<int?>();
                while (reader.Read())
                {
                    userList.Add((int?)reader["user_id"]);
                }

                return userList;
            }
        }

        /// <summary>
        /// Adds a plugin as an enabled plugin for a group in the database.
        /// </summary>
        /// <param name="groupID">The ID of the group to add the plugin relationship to.</param>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <param name="isDisabled">Initially set whether or not the plugin can be used by the group.</param>
        /// <returns>true if successful.</returns>
        public bool AddPluginToGroup(int? groupID, int? pluginID, bool isDisabled)
        {
            if (null == groupID || null == pluginID) throw new ArgumentNullException("Cannot add null pluginID, or add plugin to null groupID.");

            return InsertGroupPlugin(groupID, pluginID, isDisabled);
        }

        /// <summary>
        /// Checks to see if the group already has the plugin associated with the group.
        /// </summary>
        /// <param name="groupID">The ID of the group.</param>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if there is an associated plugin for the group already.</returns>
        public bool IsPluginInGroup(int? groupID, int? pluginID)
        {
            if (null == groupID || null == pluginID) throw new ArgumentNullException("Cannot check null pluginID, or check plugin with null groupID.");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT * FROM groupplugins ");
                queryBuilder.Append("WHERE group_id = @group_id AND plugin_id = @plugin_id");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@group_id", groupID);
                query.Parameters.AddWithValue("@plugin_id", pluginID);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                return reader.Read();
            }
        }

        /// <summary>
        /// Removes a plugin from the group entirely. This does not remove the plugin itself, only the relationship between the group and the plugin.
        /// </summary>
        /// <param name="groupID">The ID of the group.</param>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if successful.</returns>
        public bool RemovePluginFromGroup(int? groupID, int? pluginID)
        {
            if (null == groupID || null == pluginID) throw new ArgumentNullException("Cannot remove null pluginID, or remove plugin from null groupID.");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("DELETE FROM groupplugins ");
                queryBuilder.Append("WHERE group_id = @group_id AND plugin_id = @plugin_id");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@group_id", groupID);
                query.Parameters.AddWithValue("@plugin_id", pluginID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Enables a plugin for a group if it is not already.
        /// </summary>
        /// <param name="groupID">The ID of the group.</param>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if successful.</returns>
        public bool EnablePluginForGroup(int? groupID, int? pluginID)
        {
            return ToggleGroupPluginDisableStatus(groupID, pluginID, false);
        }

        /// <summary>
        /// Disables a plugin for a group if it is not already.
        /// </summary>
        /// <param name="groupID">The ID of the group.</param>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if successful.</returns>
        public bool DisablePluginForGroup(int? groupID, int? pluginID)
        {
            return ToggleGroupPluginDisableStatus(groupID, pluginID, true);
        }

        private bool ToggleGroupPluginDisableStatus(int? groupID, int? pluginID, bool isDisabled)
        {
            if (null == groupID || null == pluginID) throw new ArgumentNullException("Cannot update status for null plugin or group.");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("UPDATE groupmembers ");
                queryBuilder.Append("SET disabled = @disabled ");
                queryBuilder.Append("WHERE group_id = @group_id ");
                queryBuilder.Append("AND plugin_id = @plugin_id ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@disabled", isDisabled);
                query.Parameters.AddWithValue("@group_id", groupID);
                query.Parameters.AddWithValue("@plugin_id", pluginID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Gets a list of all enabled plugins for a group.
        /// </summary>
        /// <param name="groupID">The ID of the group.</param>
        /// <returns>A list of PluginDAOs.</returns>
        public List<PluginDAO> GetAllEnabledGroupPlugins(int? groupID)
        {
            List<int?> pluginIDList = GetAllEnabledGroupPluginIDs(groupID);
            List<PluginDAO> pluginList = new List<PluginDAO>();
            foreach (int? id in pluginIDList) pluginList.Add(RetrievePlugin(id));

            return pluginList;
        }

        private List<int?> GetAllEnabledGroupPluginIDs(int? groupID)
        {
            if (null == groupID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT plugin_id FROM groupplugins ");
                queryBuilder.Append("WHERE group_id = @group_id ");
                queryBuilder.Append("AND disabled = 0 ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@group_id", groupID);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                List<int?> pluginList = new List<int?>();
                while (reader.Read())
                {
                    pluginList.Add((int?)reader["plugin_id"]);
                }

                return pluginList;
            }
        }

        /// <summary>
        /// Updates the given group's owner with the given user. On completion, updates
        /// the given GroupDAO's owner object with the given user.
        /// </summary>
        /// <param name="group">The group to update the owner of.</param>
        /// <param name="newOwner">The user to set as owner of the given group.</param>
        /// <returns>true if successful.</returns>
        public bool UpdateGroupOwner(GroupDAO group, UserDAO newOwner)
        {
            if (null == group || null == newOwner) throw new ArgumentNullException("Cannot update null group or user");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "UPDATE groups SET owner_id = @userid WHERE id = @groupid";
                query.Parameters.AddWithValue("@userid", newOwner.UserID);
                query.Parameters.AddWithValue("@groupid", group.GroupID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                if (1 == effectedRows)
                {
                    // Update the GroupDAO reference with the new UserDAO
                    group = new GroupDAO(newOwner)
                    {
                        GroupID = group.GroupID,
                        Name = group.Name,
                        Description = group.Description,
                        GroupTag = group.GroupTag,
                        Moderators = group.Moderators,
                        Users = group.Users,
                        EnabledPlugins = group.EnabledPlugins
                    };

                    return true;
                }

                return false;
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

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO plugins (name, description, disabled, version_num, owner_id, created_dt, plugin_access, help_text) ");
                queryBuilder.Append("VALUES ");
                queryBuilder.Append("(@name, @description, @disabled, @version_num, @owner_id, GETDATE(), @plugin_access, @help_text) ");
                queryBuilder.Append("; SELECT SCOPE_IDENTITY()");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@name", plugin.Name);
                query.Parameters.AddWithValue("@description", plugin.Description);
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

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
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

                // If there are no records returned from the select statement, the DataReader will be empty
                if (reader.Read()) return BuildPluginDAO(reader);
                else throw new CouldNotFindException("Could not find plugin with command: " + commandText);
            }
        }

        /// <summary>
        /// Grabs an individual plugin from the database that matches the given command.
        /// </summary>
        /// <param name="pluginID">The plugin id to search for.</param>
        /// <returns>A new PluginDAO object with data related to the given plugin id.</returns>
        /// <exception cref="ArgumentNullException">If the given plugin id is null.</exception>
        /// <exception cref="CouldNotFindException">If the plugin for the given plugin id could not be found.</exception>
        public PluginDAO RetrievePlugin(int? pluginID)
        {
            if (null == pluginID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, name, description, disabled, version_num, owner_id, plugin_access, help_text ");
                queryBuilder.Append("FROM plugins ");
                queryBuilder.Append("WHERE id = @plugin_id");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@plugin_id", pluginID);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // If there are no records returned from the select statement, the DataReader will be empty
                if (reader.Read()) return BuildPluginDAO(reader);
                else throw new CouldNotFindException("Could not find plugin with id: " + pluginID);
            }
        }

        private PluginDAO BuildPluginDAO(SqlDataReader reader)
        {
            PluginDAO pluginDAO = new PluginDAO();
            pluginDAO.PluginID = (int)reader["id"];
            pluginDAO.Name = (string)reader["name"];
            pluginDAO.Description = (string)reader["description"];
            pluginDAO.IsDisabled = (bool)reader["disabled"];
            pluginDAO.VersionNum = (string)reader["version_num"];
            pluginDAO.OwnerID = (int)reader["owner_id"];
            pluginDAO.Access = (PluginAccess)reader["plugin_access"];
            pluginDAO.HelpText = (string)reader["help_text"];
            return pluginDAO;
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

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
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

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
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

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
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

        /// <summary>
        /// Enables a plugin for global use by all groups.
        /// </summary>
        /// <param name="pluginID">The id of the plugin to enable.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given plugin id is null.</exception>
        public bool EnableGlobalPlugin(int? pluginID)
        {
            return ToggleGlobalPluginDisableStatus(pluginID, false);
        }

        /// <summary>
        /// Disables a plugin for global use by all groups.
        /// </summary>
        /// <param name="pluginID">The id of the plugin to disable.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given plugin id is null.</exception>
        public bool DisableGlobalPlugin(int? pluginID)
        {
            return ToggleGlobalPluginDisableStatus(pluginID, true);
        }

        /// <summary>
        /// Enables or disables a plugin depending on the boolean flag isDisabled (true = disabled).
        /// </summary>
        /// <param name="pluginID">The id of the plugin to toggle.</param>
        /// <param name="isDisabled">Set to true if the given plugin is to be disabled.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the given plugin id is null.</exception>
        private bool ToggleGlobalPluginDisableStatus(int? pluginID, bool isDisabled)
        {
            if (null == pluginID) throw new ArgumentNullException("Cannot update status for null plugin.");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("UPDATE plugins ");
                queryBuilder.Append("SET disabled = @disabled ");
                queryBuilder.Append("WHERE id = @plugin_id ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@disabled", isDisabled);
                query.Parameters.AddWithValue("@plugin_id", pluginID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Increments the failed attempt count of the given pluginID by 1.
        /// </summary>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if successful.</returns>
        public bool IncrementPluginFailedAttemptCount(int? pluginID)
        {
            if (null == pluginID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("UPDATE plugins ");
                queryBuilder.Append("SET attempts_failed = attempts_failed + 1 ");
                queryBuilder.Append("WHERE id = @plugin_id ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@plugin_id", pluginID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Gets the current count of failed attempts for the given pluginID.
        /// </summary>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>The failed attempt count of the given pluginID.</returns>
        public int GetPluginFailedAttemptCount(int? pluginID)
        {
            if (null == pluginID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT attempts_failed FROM plugins ");
                queryBuilder.Append("WHERE id = @plugin_id ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@plugin_id", pluginID);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                if (reader.Read()) return (int)reader["attempts_failed"];
                else throw new CouldNotFindException("Could not find plugin with id: " + pluginID);
            }
        }

        /// <summary>
        /// Resets the failed attempt count of the plugin to 0.
        /// </summary>
        /// <param name="pluginID">The ID of the plugin.</param>
        /// <returns>true if successful.</returns>
        public bool ResetPluginFailedAttemptCount(int? pluginID)
        {
            if (null == pluginID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("UPDATE plugins ");
                queryBuilder.Append("SET attempts_failed = 0 ");
                queryBuilder.Append("WHERE id = @plugin_id ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@plugin_id", pluginID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Gets a list of all the names of plugins that are globally enabled
        /// </summary>
        /// <returns>A list containing all plugin names.</returns>
        /// <exception cref="SqlException">If a SQL-related exception is thrown.</exception>
        public List<PluginDAO> RetrieveEnabledPlugins()
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, name, description, disabled, version_num, owner_id, plugin_access, help_text ");
                queryBuilder.Append("FROM plugins ");
                queryBuilder.Append("WHERE disabled = 0 ");

                query.CommandText = queryBuilder.ToString();

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                List<PluginDAO> pluginList = new List<PluginDAO>();
                while (reader.Read())
                {
                    pluginList.Add(new PluginDAO()
                    {
                        PluginID = (int?)reader["id"],
                        Name = (string)reader["name"],
                        Description = (string)reader["description"],
                        IsDisabled = (bool)reader["disabled"],
                        VersionNum = (string)reader["version_num"],
                        OwnerID = (int?)reader["owner_id"],
                        Access = (PluginAccess)reader["plugin_access"],
                        HelpText = (string)reader["help_text"]
                    });
                }

                return pluginList;
            }
        }

        /// <summary>
        /// Gets a list of all the names of plugins that are system plugins
        /// </summary>
        /// <returns>A list containing all plugins.</returns>
        /// <exception cref="SqlException">If a SQL-related exception is thrown.</exception>s>
        public List<PluginDAO> RetrieveSystemPlugins()
        {
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, name, description, disabled, version_num, owner_id, plugin_access, help_text ");
                queryBuilder.Append("FROM plugins ");
                queryBuilder.Append("WHERE system = 1 ");

                query.CommandText = queryBuilder.ToString();

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                List<PluginDAO> pluginList = new List<PluginDAO>();
                while (reader.Read())
                {
                    pluginList.Add(new PluginDAO()
                    {
                        PluginID = (int?)reader["id"],
                        Name = (string)reader["name"],
                        Description = (string)reader["description"],
                        IsDisabled = (bool)reader["disabled"],
                        VersionNum = (string)reader["version_num"],
                        OwnerID = (int?)reader["owner_id"],
                        Access = (PluginAccess)reader["plugin_access"],
                        HelpText = (string)reader["help_text"]
                    });
                }

                return pluginList;
            }
        }

        /// <summary>
        /// Updates the given plugin's owner with the given user. On completion, updates
        /// the given PluginDAO's owner id with the given user.
        /// </summary>
        /// <param name="group">The group to update the owner of.</param>
        /// <param name="newOwner">The user to set as owner of the given group.</param>
        /// <returns>true if successful.</returns>
        public bool UpdatePluginOwner(PluginDAO plugin, UserDAO newOwner)
        {
            if (null == plugin || null == newOwner) throw new ArgumentNullException("Cannot update null plugin or user");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "UPDATE plugins SET owner_id = @userid WHERE id = @pluginid";
                query.Parameters.AddWithValue("@userid", newOwner.UserID);
                query.Parameters.AddWithValue("@pluginid", plugin.PluginID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                if (1 == effectedRows)
                {
                    // Update the PluginDAO reference with the new UserDAO
                    plugin.OwnerID = newOwner.UserID;

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a list of plugins that are owned by the given user.
        /// </summary>
        /// <param name="user">The user to retrieve a list of owned plugins for.</param>
        /// <returns>A list containing the plugins owned by the user. If the user does not own any plugins, the list will return empty.</returns>
        public List<PluginDAO> GetPluginsOwnedByUser(UserDAO user)
        {
            if (null == user || null == user.UserID) throw new ArgumentNullException("Cannot get plugins for null owner");

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, name, description, disabled, version_num, owner_id, plugin_access, help_text ");
                queryBuilder.Append("FROM plugins ");
                queryBuilder.Append("WHERE owner_id = @ownerid ");
                query.CommandText = queryBuilder.ToString();

                query.Parameters.AddWithValue("@ownerid", user.UserID);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                List<PluginDAO> plugins = new List<PluginDAO>();
                while (reader.Read())
                {
                    plugins.Add(BuildPluginDAO(reader));
                }

                return plugins;
            }
        }

        #endregion

        #region "Plugin Key/Value Actions"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdatePluginKeyValue(PluginDAO plugin, String key, String value, UserDAO forUser = null)
        {
            if (null == plugin
                || null == key) 
                throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                // Using SQL Server, so this isn't supported
                //queryBuilder.Append("INSERT INTO pluginkeyvalue (plugin_id, key_string, value_object) ");
                //queryBuilder.Append("VALUES ");
                //queryBuilder.Append("(@pluginid, @keystring, @valueobj) ");
                //queryBuilder.Append("ON DUPLICATE KEY UPDATE ");
                //queryBuilder.Append(" value_object = @valueobj ");

                queryBuilder.Append("begin tran \n");
                queryBuilder.Append("if exists (select * from pluginkeyvalue with (updlock,serializable) where key_string = @keystring and plugin_id = @pluginid) \n");
                queryBuilder.Append("   begin \n");
                queryBuilder.Append("   update pluginkeyvalue set value_object = @valueobj \n");
                queryBuilder.Append("   where key_string = @keystring and plugin_id = @pluginid \n");
                queryBuilder.Append("end \n");
                queryBuilder.Append("else \n");
                queryBuilder.Append("begin \n");
                queryBuilder.Append("   insert pluginkeyvalue (plugin_id, user_id, key_string, value_object) \n");
                queryBuilder.Append("   values (@pluginid, @userid, @keystring, @valueobj) \n");
                queryBuilder.Append("end \n");
                queryBuilder.Append("commit tran \n");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@pluginid", plugin.PluginID.Value);
                if (forUser == null)
                    query.Parameters.AddWithValue("@userid", null);
                else
                    query.Parameters.AddWithValue("@userid", forUser.UserID.Value);
                query.Parameters.AddWithValue("@keystring", key);
                query.Parameters.AddWithValue("@valueobj", value);

                conn.Open();
                query.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string RetrievePluginValue(PluginDAO plugin, String key, UserDAO forUser = null)
        {
            if (null == plugin
                || null == key)
                throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT value_object ");
                queryBuilder.Append("FROM pluginkeyvalue ");
                queryBuilder.Append("WHERE plugin_id = @pluginid ");
                queryBuilder.Append(" AND user_id = @userid ");
                queryBuilder.Append(" AND key_string = @keystring ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@pluginid", plugin.PluginID.Value);
                if (forUser == null)
                    query.Parameters.AddWithValue("@userid", null);
                else
                    query.Parameters.AddWithValue("@userid", forUser.UserID.Value);
                query.Parameters.AddWithValue("@keystring", key);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // If there are no records returned from the select statement, the DataReader will be empty
                if (reader.Read())
                {
                    return (string)reader["value_object"];
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region PairEntries Getter/Setter actions

        /// <summary>
        /// Gets the associated value for the given key entry.
        /// </summary>
        /// <param name="keyEntry">The key entry to search for.</param>
        /// <returns>The string representation of the value assoviated with the key.</returns>
        /// <exception cref="ArgumentNullException">If the given key entry is null.</exception>
        /// <exception cref="CouldNotFindException">If the key entry does not exist.</exception>
        public string GetPairEntryValue(string keyEntry)
        {
            if (string.IsNullOrEmpty(keyEntry)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "SELECT value_entry FROM pairentries WHERE key_entry = @key_entry";
                query.Parameters.AddWithValue("@key_entry", keyEntry);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // Only one record should have been inserted
                if (reader.Read()) return (string)reader["value_entry"];
                else throw new CouldNotFindException("Could not find a value that matched the given key: " + keyEntry);
            }
        }

        /// <summary>
        /// Performs an upsert on a key-value entry, e.g. if the key-value entry does not exist, it is inserted; 
        /// otherwise it updates the given key's value with the given value.
        /// </summary>
        /// <param name="keyEntry">The key entry to upsert.</param>
        /// <param name="valueEntry">The value to upsert.</param>
        /// <returns>true if successful</returns>
        /// <exception cref="ArgumentNullException">If the given key or value is null.</exception>
        public bool SetPairEntryValue(string keyEntry, string valueEntry)
        {
            if (string.IsNullOrEmpty(keyEntry) || string.IsNullOrEmpty(valueEntry)) throw new ArgumentNullException();
            
            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "sp_upsertPairEntry";
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@key_entry", keyEntry);
                query.Parameters.AddWithValue("@value_entry", valueEntry);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                /* A key-value entry should have either been inserted or updated.
                 * Either way, only one row should have been effected.
                 */
                return 1 == effectedRows;
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

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
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

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();

                query.CommandText = "SELECT username FROM users WHERE username = @username AND password = CONVERT(VARBINARY, HASHBYTES('SHA1', @password + salt))";
                query.Parameters.AddWithValue("@username", username);
                query.Parameters.AddWithValue("@password", password);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                if (reader.Read()) return true;
                else return false;
            }
        }

        /// <summary>
        /// Sets the verification code for the given user
        /// </summary>
        /// <param name="verificationCode">The 6-character verification code.</param>
        /// <param name="user">The user to set the verification code for.</param>
        /// <returns>true if successful.</returns>
        /// <exception cref="ArgumentNullException">If the verification is null or empty, or the userDAO or UserDAO.UserID are null.</exception>
        public bool SetVerificationCodeForUser(string verificationCode, UserDAO user)
        {
            if ((null != verificationCode && verificationCode.Trim().Equals(string.Empty)) || null == user || null == user.UserID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("UPDATE users ");
                queryBuilder.Append("SET verification_code = @verificationCode ");
                queryBuilder.Append("WHERE id = @userid ");

                query.CommandText = queryBuilder.ToString();
                if (null == verificationCode)
                    query.Parameters.AddWithValue("@verificationCode", DBNull.Value);
                else
                    query.Parameters.AddWithValue("@verificationCode", verificationCode);
                query.Parameters.AddWithValue("@userid", user.UserID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                // Only one record should have been affected
                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Returns a UserDAO that corresponds to the given verification code.
        /// </summary>
        /// <param name="verificationCode">The verification code to query the database for.</param>
        /// <returns>The UserDAO that matches the given verification code.</returns>
        /// <exception cref="ArgumentNullException">If the given verification code is null or empty.</exception>
        /// <exception cref="CouldNotFindException">If no user could be found for the given code.</exception>
        public UserDAO GetUserByVerificationCode(string verificationCode)
        {
            if (string.IsNullOrWhiteSpace(verificationCode)) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, username, first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed ");
                queryBuilder.Append("FROM users ");
                queryBuilder.Append("WHERE verification_code = @verificationcode ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@verificationcode", verificationCode);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // If there are no records returned from the select statement, the DataReader will be empty
                if (reader.Read()) return BuildUserDAO(reader);
                else throw new CouldNotFindException("Could not find user with verification code: " + verificationCode);
            }
        }

        /// <summary>
        /// Gets the current verification code for the given user. Useful for determining if a user
        /// has been verified in the system completely.
        /// </summary>
        /// <param name="user">The user to check in the database.</param>
        /// <returns>the database string with either "-1" (new user), a 6-character verification code (registering), or NULL (verified).</returns>
        public string GetCurrentVerificationValueForUser(UserDAO user)
        {
            if (null == user || null == user.UserID) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(CONNECTION_STRING))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT verification_code ");
                queryBuilder.Append("FROM users ");
                queryBuilder.Append("WHERE id = @userid ");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@userid", user.UserID);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                // If there are no records returned from the select statement, the DataReader will be empty
                if (reader.Read())
                {
                    if (DBNull.Value.Equals(reader["verification_code"])) return null;
                    else return reader["verification_code"] as string;
                }
                else throw new CouldNotFindException("Could not find user with userid: " + user.UserID);
            }
        }

        #endregion
    }
}
