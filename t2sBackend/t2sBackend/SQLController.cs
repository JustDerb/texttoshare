using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class SQLController : IDBController
    {
        private const string _connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\MainDatabase.mdf;Integrated Security=True";

        public bool CreateUser(UserDAO user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(UserDAO user)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(UserDAO user)
        {
            throw new NotImplementedException();
        }

        public UserDAO RetrieveUser(string userPhoneEmail)
        {
            // return null if not a valid user
            throw new NotImplementedException();
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
            // return null if not a valid user
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
                conn.Close();

                return (1 == effectedRows) ? true : false;
            }
        }
    }
}
