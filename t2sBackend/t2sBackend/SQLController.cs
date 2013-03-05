using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class SQLController : IDBController
    {
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
            throw new NotImplementedException();
        }

        public bool UpdateGroup(GroupDAO group)
        {
            throw new NotImplementedException();
        }

        public bool LogMessage(string message, LoggerLevel level)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                using (SqlCommand query = conn.CreateCommand())
                {
                    query.CommandText = "INSERT INTO eventlog (message, level, created_dt) VALUES (@message, @information, GETDATE())";
                    query.Parameters.Add("@message", message);
                    query.Parameters.Add("@level", (int) level);

                    conn.Open();
                    int effectedRows = query.ExecuteNonQuery();
                    conn.Close();

                    return (1 == effectedRows) ? true : false;
                }
            }
        }
    }
}
