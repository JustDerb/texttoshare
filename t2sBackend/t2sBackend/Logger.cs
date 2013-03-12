using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace t2sBackend
{

    /// <summary>
    /// static class which logs messages to the database
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// private static IDBController variable
        /// </summary>
        private static IDBController controller = new SQLController();
        /// <summary>
        /// takes the IDBController instance and logs a message with the LoggerLevel
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static bool LogMessage(string message, LoggerLevel level)
        {
            return controller.LogMessage(message, level);
        }
    }
}
