﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using S22.Imap;

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
        /// 
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="UseSSL"></param>
        /// <param name="IMAPServer"></param>
        /// <param name="IMAPPort"></param>
        /// <param name="SMTPServer"></param>
        /// <param name="SMTPPort"></param>
        public GMailWatcherService(String Username, String Password, bool UseSSL, String IMAPServer, int IMAPPort, String SMTPServer, int SMTPPort)
        {
            this.UserName = Username;
            this.Password = Password;

            this.UseSSL = UseSSL;

            this.IMAPServer = IMAPServer;
            this.IMAPPort = IMAPPort;

            this.SMTPServer = SMTPServer;
            this.SMTPPort = SMTPPort;
        }

        ~GMailWatcherService()
        {
            this.Stop();
        }

        /// <summary>
        /// <see cref="IWatcherService.Start"/>
        /// </summary>
        /// <exception cref="Exception">Any errors when the IMAP service tries to connect</exception>
        public override void Start()
        {
            this.ImapConnection = new ImapClient(
                this.IMAPServer, 
                this.IMAPPort,
                this.UserName, 
                this.Password, 
                AuthMethod.Auto, 
                this.UseSSL,
                null);

            this.ImapConnection.NewMessage += ImapConnection_NewMessage;

            // Check for messages that are unread (IDLE only tells you for NEWLY recieve mail)
            uint[] msgs = this.ImapConnection.Search(SearchCondition.Unseen());
            foreach (uint msg in msgs) {
                this.recievedMessage(msg);
            }
        }

        void ImapConnection_NewMessage(object sender, IdleMessageEventArgs e)
        {
            this.recievedMessage(e.MessageUID);
        }

        private void recievedMessage(uint MessageUID)
        {
            // Download the message (Set as seen)
            MailMessage msg = this.ImapConnection.GetMessage(MessageUID, true);

            Console.WriteLine(msg.Body);

            // Alert the watchers!
            WatcherServiceEventArgs args = new WatcherServiceEventArgs();
            Message msgObj = new Message();

            // TODO: Just return a System.Net.MailMessage (It has more info than we'll ever need)
            msgObj.FullMessage = msg.Body;
            msgObj.Sender = msg.From.Address;
            msgObj.Reciever = null;

            args.MessageObj = msgObj;
            args.MessageString = msg.Body;
            this.OnRecievedMessage(args);

            // Delete the message
            //this.ImapConnection.DeleteMessage(MessageUID);
        }


        public override void Stop()
        {
            this.ImapConnection.NewMessage -= ImapConnection_NewMessage;

            this.ImapConnection.Dispose();
            this.ImapConnection = null;
        }

        public override bool SendMessage(Message message, bool async)
        {
            System.Net.Mail.MailMessage messageToSend = new System.Net.Mail.MailMessage();
            foreach (String to in message.Reciever)
                messageToSend.To.Add(to);
            messageToSend.Subject = "";
            messageToSend.From = new System.Net.Mail.MailAddress(message.Sender);
            messageToSend.Body = message.FullMessage;

            SmtpClient smtp = new SmtpClient(this.SMTPServer,this.SMTPPort);
            if (async)
                smtp.SendAsync(messageToSend, null);
            else
                smtp.Send(messageToSend);
            return false;
        }

        public override bool SendMessage(Message message)
        {
            return SendMessage(message, false);
        }

        public override bool IsRunning()
        {
            return Running;
        }
    }
}
