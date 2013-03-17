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
        private const string _connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\MainDatabase.mdf;Integrated Security=True";

        /// <summary>
        /// Creates a new user entry in the database with the given UserDAO.
        /// </summary>
        /// <param name="user">The UserDAO to insert into the database.</param>
        /// <returns>true if the user was successfully added.</returns>
        /// <exception cref="NullReferenceException">If the given UserDAO is null.</exception>
        public bool CreateUser(UserDAO user)
        {
            if (null == user) throw new NullReferenceException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "INSERT INTO users (first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed) VALUES (@first_name, @last_name, @phone, @email_phone, @carrier, @user_level, @banned, @suppressed)";
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

                return 1 == effectedRows;
            }
        }

        /// <summary>
        /// Deletes an existing user that matches the given UserDAO.
        /// </summary>
        /// <param name="user">The UserDAO to delete from the database.</param>
        /// <returns>true if the user was successfully deleted.</returns>
        /// <exception cref="NullReferenceException">If the given UserDAO or UserDAO.UserID is null.</exception>
        public bool DeleteUser(UserDAO user)
        {
            if (null == user || 0 < user.UserID) throw new NullReferenceException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "sp_deleteUser";
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@userid", user.UserID);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 0 < effectedRows;
            }
            //throw new NotImplementedException();
        }

        public bool UpdateUser(UserDAO user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Grabs an individual user based on the given phone email string. The string should be in a format similar to
        /// <code>String userPhoneEmail = "1234567890@carrier.com"</code>
        /// in order to grab the correct information.
        /// </summary>
        /// <param name="userPhoneEmail"></param>
        /// <returns>A new UserDAO object with data related to the given phone email, or null if the user does not exist.</returns>
        /// <exception cref="NullReferenceException">If the given string is null.</exception>
        public UserDAO RetrieveUser(string userPhoneEmail)
        {
            if (null == userPhoneEmail) throw new NullReferenceException();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "SELECT id, first_name, last_name, phone, email_phone, carrier, user_level, banned, suppressed FROM users WHERE email_phone = @phoneEmail";
                query.Parameters.AddWithValue("@phoneEmail", userPhoneEmail);

                conn.Open();
                SqlDataReader reader = query.ExecuteReader();

                UserDAO userDAO = null;
                while (reader.Read())
                {
                    userDAO = new UserDAO();
                    userDAO.UserID = (int) reader["id"];
                    userDAO.FirstName = (string) reader["first_name"];
                    userDAO.LastName = (string) reader["last_name"];
                    userDAO.PhoneNumber = (string) reader["phone"]; // This should be casting to (int) instead of (string)
                    userDAO.PhoneEmail = (string) reader["email_phone"];
                    userDAO.Carrier = (PhoneCarrier) reader["carrier"];
                    userDAO.UserLevel = (UserType) reader["user_level"];
                    userDAO.IsBanned = (bool) reader["banned"];
                    userDAO.IsSuppressed = (bool) reader["suppressed"];
                }

                return userDAO;
            }

            //throw new NotImplementedException();
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

        public bool LogMessage(string message, LoggerLevel level)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand query = conn.CreateCommand())
            {
                query.CommandText = "INSERT INTO eventlog (message, level, created_dt) VALUES (@message, @level, GETDATE())";
                query.Parameters.AddWithValue("@message", message);
                query.Parameters.AddWithValue("@level", (int) level);

                conn.Open();
                int effectedRows = query.ExecuteNonQuery();

                return 1 == effectedRows;
            }
        }
    }
}
