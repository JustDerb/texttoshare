using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AE.Net.Mail;

namespace t2sBackend
{
    /// <summary>
    /// IMAP/POP implmentation used from: https://github.com/andyedinborough/aenetmail
    /// </summary>
    public class GMailWatcherService : AWatcherService
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
        public int IMAPPort
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
        /// Port to use when sending mail to the GMail service
        /// </summary>
        public int SMTPPort
        {
            get;
            set;
        }

        protected bool Running;

        protected ImapClient ImapConnection;

        /// <summary>
        /// <see cref="IWatcherService.Start"/>
        /// </summary>
        /// <exception cref="Exception">Any errors when the IMAP service tries to connect</exception>
        public override void Start()
        {
            this.ImapConnection = new ImapClient(
                this.IMAPServer, 
                this.UserName, 
                this.Password, 
                ImapClient.AuthMethods.Login, 
                this.IMAPPort, 
                this.UseSSL);
            //var msgs = ImapConnection.SearchMessages(SearchCondition.Undeleted());

            ImapConnection.NewMessage += ImapConnection_NewMessage;

        }

        void ImapConnection_NewMessage(object sender, AE.Net.Mail.Imap.MessageEventArgs e)
        {
            // Download the message
            MailMessage msg = ImapConnection.GetMessage(e.MessageCount - 1, false, true);

            Console.WriteLine(msg);

            // Alert the watchers!
            WatcherServiceEventArgs args = new WatcherServiceEventArgs();
            args.MessageString = msg.Body;
            this.OnRecievedMessage(args);

            // Delete the message
            ImapConnection.DeleteMessage(msg);
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public override bool SendMessage(Message message, bool async)
        {
            throw new NotImplementedException();
        }

        public override bool SendMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public override bool IsRunning()
        {
            return Running;
        }
    }
}
