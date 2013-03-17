using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public interface IDBController
    {
        /// <summary>
        /// Creates a new user entry in the database with the given UserDAO.
        /// </summary>
        /// <param name="user">The UserDAO to insert into the database.</param>
        /// <returns>true if the user was successfully added.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO is null.</exception>
        bool CreateUser(UserDAO user);

        /// <summary>
        /// Deletes an existing user that matches the given UserDAO.
        /// </summary>
        /// <param name="user">The UserDAO to delete from the database.</param>
        /// <returns>true if the user was successfully deleted.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO or UserDAO.UserID is null.</exception>
        bool DeleteUser(UserDAO user);

        /// <summary>
        /// Updates user information in the database.
        /// </summary>
        /// <param name="user">The UserDAO to update in the database</param>
        /// <returns>true if the user was successfully updated.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO or UserDAO.UserID is null.</exception>
        bool UpdateUser(UserDAO user);

        /// <summary>
        /// Grabs an individual user based on the given phone email string. The string should be in a format similar to
        /// <code>String userPhoneEmail = "1234567890@carrier.com"</code>
        /// in order to grab the correct information.
        /// </summary>
        /// <param name="userPhoneEmail">The user phone email to query for.</param>
        /// <returns>A new UserDAO object with data related to the given phone email, or null if the user does not exist.</returns>
        /// <exception cref="ArgumentNullException">If the given string is null.</exception>
        UserDAO RetrieveUser(string userPhoneEmail);

        bool CreateGroup(GroupDAO group);

        bool DeleteGroup(GroupDAO group);

        GroupDAO RetrieveGroup(string groupTag);

        bool UpdateGroup(GroupDAO group);

        /// <summary>
        /// Inserts a message into the database with the given message and level of importance.
        /// </summary>
        /// <param name="message">The message for the log.</param>
        /// <param name="level">The level of the given.</param>
        /// <returns>true if the message was successfully logged.</returns>
        /// <exception cref="ArgumentNullException">If the given message is null.</exception>
        /// <seealso cref="LoggerLevel"/>
        bool LogMessage(string message, LoggerLevel level);
    }
}
