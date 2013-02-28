using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    /// <summary>
    /// IMAP/POP implmentation used from: https://github.com/andyedinborough/aenetmail
    /// </summary>
    public class GMailWatcherService : IWatcherService
    {
        /// <summary>
        /// User to use when logging into the GMail service
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Password to use when logging into the GMail service
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Use SSL when logging into the GMail service
        /// </summary>
        public bool UseSSL
        {
            get;
            set;
        }

        /// <summary>
        /// Use SSL when logging into the GMail service
        /// </summary>
        public bool IsLoggedIn
        {
            get;
            private set;
        }

        /// <summary>
        /// Port to use when logging into the GMail service
        /// </summary>
        public int Port
        {
            get;
            set;
        }

        /// <summary>
        /// Address to the IMAP server (Incoming)
        /// </summary>
        public string IMAPServer
        {
            get;
            set;
        }

        /// <summary>
        /// Address to the SMTP server (Outgoing)
        /// </summary>
        public string SMTPServer
        {
            get;
            set;
        }

        /// <summary>
        /// Messages that have been recieved
        /// </summary>
        private List<Message> MessageQueue
        {
            get;
            set;
        }

        public void Login()
        {
            throw new System.NotImplementedException();
        }

        public void Logout()
        {
            throw new System.NotImplementedException();
        }

        private void AddMessage(t2sBackend.Message message)
        {
            
        }

    }
}
