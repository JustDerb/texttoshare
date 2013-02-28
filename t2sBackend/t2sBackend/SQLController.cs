using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class SQLController : IDBController
    {
        public static bool CreateUser(UserDAO user)
        {
            throw new NotImplementedException();
        }

        public static bool DeleteUser(UserDAO user)
        {
            throw new NotImplementedException();
        }

        public static bool UpdateUser(UserDAO user)
        {
            throw new NotImplementedException();
        }

        public static UserDAO RetrieveUser(string userPhoneEmail)
        {
            throw new NotImplementedException();
        }

        public static bool CreateGroup(GroupDAO group)
        {
            throw new NotImplementedException();
        }

        public static bool DeleteGroup(GroupDAO group)
        {
            throw new NotImplementedException();
        }

        public static GroupDAO RetrieveGroup(string groupTag)
        {
            throw new NotImplementedException();
        }

        public static bool UpdateGroup(GroupDAO group)
        {
            throw new NotImplementedException();
        }

        public static bool LogMessage(string message, LoggerLevel level)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                using (SqlCommand query = conn.CreateCommand())
                {
                    query.CommandText = "INSERT INTO eventlog (message, level, created_dt) VALUES (@message, @information, GETDATE())";
                    query.Parameters.Add("@message", message);
                    query.Parameters.Add("@level", (int)level);

                    conn.Open();
                    int effectedRows = query.ExecuteNonQuery();
                    conn.Close();

                    return (1 == effectedRows) ? true : false;
                }
            }
        }
    }
}
