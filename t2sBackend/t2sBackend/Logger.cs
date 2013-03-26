using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{

    /// <summary>
    /// Static class which logs messages to the database
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// private static IDBController variable
        /// </summary>
        private static SqlController controller = new SqlController();

        /// <summary>
        /// Inserts a message into the database with the given message and level of importance.
        /// </summary>
        /// <param name="message">The message for the log.</param>
        /// <param name="level">The level of the given.</param>
        /// <returns>true if the message was successfully logged.</returns>
        /// <seealso cref="LoggerLevel"/>
        public static bool LogMessage(string message, LoggerLevel level)
        {
            return controller.LogMessage(message, level);
        }
    }
}
