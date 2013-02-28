using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public interface IDBController
    {
        bool CreateUser(UserDAO user);

        bool DeleteUser(UserDAO user);

        bool UpdateUser(UserDAO user);

        UserDAO RetrieveUser(string userPhoneEmail);

        bool CreateGroup(GroupDAO group);

        bool DeleteGroup(GroupDAO group);

        GroupDAO RetrieveGroup(string groupTag);

        bool UpdateGroup(GroupDAO group);

        bool LogMessage(string message, LoggerLevel level);
    }
}
