using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public class Logger
    {
        public bool LogMessage(string message, LoggerLevel level)
        {
            return SQLController.LogMessage(message, level);
        }
    }
}
