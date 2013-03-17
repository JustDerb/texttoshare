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

        /// <summary>
        /// Creates a new user entry in the database with the given UserDAO.
        /// </summary>
        /// <param name="user">The UserDAO to insert into the database.</param>
        /// <returns>true if the user was successfully added.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO is null.</exception>
        public bool CreateUser(UserDAO user)
        {
            if (null == user) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("INSERT INTO users (first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed, created_dt) ");
                queryBuilder.Append("VALUES ");
                queryBuilder.Append("(@first_name, @last_name, @phone, @email_phone, @carrier, @user_level, @banned, @suppressed, GETDATE())");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@first_name", user.FirstName);
                query.Parameters.AddWithValue("@last_name", user.LastName);
                query.Parameters.AddWithValue("@phone", user.PhoneNumber);
                query.Parameters.AddWithValue("@email_phone", user.PhoneEmail);
                query.Parameters.AddWithValue("@carrier", user.Carrier.ToString());
                query.Parameters.AddWithValue("@user_level", (int) user.UserLevel);
                query.Parameters.AddWithValue("@banned", user.IsBanned ? 1 : 0);
                query.Parameters.AddWithValue("@suppressed", user.IsSuppressed ? 1 : 0);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                // Only one record should have been inserted
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

                /* One or more records should have been deleted
                 * 
                 * The user record itself (1), and any additional groupmember entries (0 or more)
                 */
                return 0 < effectedRows;
            }
        }

        /// <summary>
        /// Updates user information in the database.
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
                queryBuilder.Append("first_name = @first_name, ");
                queryBuilder.Append("last_name = @last_name, ");
                queryBuilder.Append("phone = @phone, ");
                queryBuilder.Append("email_phone = @email_phone, ");
                queryBuilder.Append("carrier = @carrier, ");
                queryBuilder.Append("user_level = @user_level, ");
                queryBuilder.Append("banned = @banned, ");
                queryBuilder.Append("suppressed = @suppressed ");
                queryBuilder.Append("WHERE id = @userid");

                query.CommandText = queryBuilder.ToString();
                query.Parameters.AddWithValue("@first_name", user.FirstName);
                query.Parameters.AddWithValue("@last_name", user.LastName);
                query.Parameters.AddWithValue("@phone", user.PhoneNumber);
                query.Parameters.AddWithValue("@email_phone", user.PhoneEmail);
                query.Parameters.AddWithValue("@carrier", user.Carrier.ToString());
                query.Parameters.AddWithValue("@user_level", (int) user.UserLevel);
                query.Parameters.AddWithValue("@banned", user.IsBanned ? 1 : 0);
                query.Parameters.AddWithValue("@suppressed", user.IsSuppressed ? 1 : 0);
                query.Parameters.AddWithValue("@userid", user.UserID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                // Only one record should have been updated
                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Grabs an individual user based on the given phone email string. The string should be in a format similar to
        /// <code>String userPhoneEmail = "1234567890@carrier.com"</code>
        /// in order to grab the correct information.
        /// </summary>
        /// <param name="userPhoneEmail">The user phone email to query for.</param>
        /// <returns>A new UserDAO object with data related to the given phone email, or null if the user does not exist.</returns>
        /// <exception cref="ArgumentNullException">If the given string is null.</exception>
        public UserDAO RetrieveUser(string userPhoneEmail)
        {
            if (null == userPhoneEmail) throw new ArgumentNullException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT id, first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed ");
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
                    userDAO.FirstName = (string) reader["first_name"];
                    userDAO.LastName = (string) reader["last_name"];
                    userDAO.PhoneNumber = (string) reader["phone"];
                    userDAO.PhoneEmail = (string) reader["email_phone"];
                    userDAO.Carrier = (PhoneCarrier) reader["carrier"];
                    userDAO.UserLevel = (UserLevel) reader["user_level"];
                    userDAO.IsBanned = (bool) reader["banned"];
                    userDAO.IsSuppressed = (bool) reader["suppressed"];
                }

                return userDAO;
            }
        }

        public bool CreateGroup(GroupDAO group)
        {
            throw new NotImplementedException();
        }

        public bool DeleteGroup(GroupDAO group)
        {
            throw new NotImplementedException();
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
    }
}
