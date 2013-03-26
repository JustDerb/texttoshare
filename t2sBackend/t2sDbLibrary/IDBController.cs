using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sDbLibrary
{
    public interface IDBController
    {
        /// <summary>
        /// Creates a new user entry in the database with the given UserDAO.
        /// </summary>
        /// <param name="user">The UserDAO to insert into the database.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>true if the user was successfully added.</returns>
        /// <exception cref="ArgumentNullException">If the given UserDAO is null.</exception>
        bool CreateUser(UserDAO user, string password);

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
        /// <returns>A new UserDAO object with data related to the given phone email.</returns>
        /// <exception cref="ArgumentNullException">If the given string is null.</exception>
        /// <exception cref="CouldNotFindException">If the user for the given phone email could not be found.</exception>
        UserDAO RetrieveUser(string userPhoneEmail);

        bool CreateGroup(GroupDAO group);

        GroupDAO RetrieveGroup(string groupTag);

        bool UpdateGroup(GroupDAO group);

        bool DeleteGroup(GroupDAO group);

        /// <summary>
        /// Creates a new plugin entry in the database with the given PluginDAO. The PluginID of the given
        /// PluginDAO will also be set after calling this method.
        /// </summary>
        /// <param name="user">The PluginDAO to insert into the database.</param>
        /// <returns>true if the plugin was successfully added and the PluginID was set</returns>
        /// <exception cref="ArgumentNullException">If the given plugin is null.</exception>
        bool CreatePlugin(PluginDAO plugin);

        /// <summary>
        /// Grabs an individual plugin from the database that matches the given command.
        /// </summary>
        /// <param name="commandText">The command text of the plugin to search for.</param>
        /// <returns>A new PluginDAO object with data related to the given command text.</returns>
        /// <exception cref="ArgumentNullException">If the given commandText is null.</exception>
        /// <exception cref="CouldNotFindException">If the plugin for the given commandText could not be found.</exception>
        PluginDAO RetrievePlugin(string commandText);

        /// <summary>
        /// Updates plugin information in the database. If there is no entry in the database that matches the given
        /// PluginDAO.PluginID no entries will be updated and <code>false</code> will be returned.
        /// </summary>
        /// <param name="user">The PluginDAO to update in the database</param>
        /// <returns>true if the plugin was successfully updated.</returns>
        /// <exception cref="ArgumentNullException">If the given PluginDAO or PluginDAO.PluginID is null.</exception>
        bool UpdatePlugin(PluginDAO plugin);

        /// <summary>
        /// Deletes an existing plugin that matches the given PluginDAO.
        /// </summary>
        /// <param name="user">The PluginDAO to delete from the database.</param>
        /// <returns>true if the plugin was successfully deleted.</returns>
        /// <exception cref="ArgumentNullException">If the given PluginDAO or PluginDAO.PluginID is null.</exception>
        bool DeletePlugin(PluginDAO plugin);

        /// <summary>
        /// Inserts a message into the database with the given message and level of importance.
        /// </summary>
        /// <param name="message">The message for the log.</param>
        /// <param name="level">The level of the given.</param>
        /// <returns>true if the message was successfully logged.</returns>
        /// <exception cref="ArgumentNullException">If the given message is null.</exception>
        /// <seealso cref="LoggerLevel"/>
        bool LogMessage(string message, LoggerLevel level);

        /// <summary>
        /// Checks that a user with the given username and password exists in the database.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="password">The password to check.</param>
        /// <returns>true if the user exists.</returns>
        bool CheckLogin(string username, string password);

        /// <summary>
        /// Registers a new user in the database.
        /// </summary>
        /// <param name="user">The user to be added to the database.</param>
        /// <param name="password">The login password of the user.</param>
        /// <returns>true if the user was successfully added.</returns>
        /// <seealso cref="SqlController#CreateUser"/>
        bool RegisterUser(UserDAO user, string password);
    }

    /// <summary>
    /// Exception class thrown when a specific entry in the database could not be found.
    /// </summary>
    public class CouldNotFindException : Exception
    {
        public CouldNotFindException() : base() {}
        public CouldNotFindException(string message) : base(message) {}
        public CouldNotFindException(string message, Exception innerException) : base(message, innerException) {}
        public CouldNotFindException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }

    /// <summary>
    /// Exception class thrown when a specific entry in the database already exists.
    /// </summary>
    public class EntryAlreadyExistsException : Exception
    {
        public EntryAlreadyExistsException() : base() { }
        public EntryAlreadyExistsException(string message) : base(message) {}
        public EntryAlreadyExistsException(string message, Exception innerException) : base(message, innerException) {}
        public EntryAlreadyExistsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }
}
