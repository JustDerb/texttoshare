using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using S22.Imap;
using t2sDbLibrary;
using System.Net;
using System.IO;
using System.Timers;

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

        private Timer reconnectTimer;

        private static readonly double timeTillReconnect = 10 * 60 * 1000;

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
            if (!IsRunning())
            {
                if (this.reconnectTimer != null)
                {
                    this.reconnectTimer.Stop();
                    this.reconnectTimer.Close();

                    this.reconnectTimer = null;
                }

                this.reconnectTimer = new Timer(timeTillReconnect);
                this.reconnectTimer.Elapsed += reconnectTimer_Elapsed;

                this.reconnectTimer.Start();

                this.Running = true;
            }
        }

        void reconnectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            startUpConnection();
        }

        private void startUpConnection()
        {
            if (this.IsRunning())
            {
                stopConnection();
            }

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
            foreach (uint msg in msgs)
            {
                this.recievedMessage(msg);
            }
        }

        private void stopConnection()
        {
            this.ImapConnection.NewMessage -= ImapConnection_NewMessage;

            this.ImapConnection.Dispose();
            this.ImapConnection = null;
        }

        void ImapConnection_NewMessage(object sender, IdleMessageEventArgs e)
        {
            this.recievedMessage(e.MessageUID);
        }

        private void recievedMessage(uint MessageUID)
        {
            // Download the message (Set as seen)
            MailMessage msg = this.ImapConnection.GetMessage(MessageUID, true);
            
            // Side case for iPhones/"Smart Phones"
            if (msg.Attachments.Count > 0)
            {
                foreach (Attachment a in msg.Attachments) 
                {
                    // Only get text files
                    if (a.ContentType.MediaType.Equals("text/plain", StringComparison.OrdinalIgnoreCase))
                    {
                        Stream dataStream = a.ContentStream;
                        byte[] dataBuffer = new byte[dataStream.Length];
                        dataStream.Position = 0;
                        dataStream.Read(dataBuffer, 0, dataBuffer.Length);
                        String textData = Encoding.ASCII.GetString(dataBuffer);
                        // Do it twice in-case theres a different order
                        textData = textData.TrimEnd('\r');
                        textData = textData.TrimEnd('\n');
                        textData = textData.TrimEnd('\r');
                        textData = textData.TrimEnd('\n');

                        msg.Body += textData;
                    }
                }
            }

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
            this.Running = false;

            this.reconnectTimer.Stop();

            if (this.reconnectTimer != null)
            {
                this.reconnectTimer.Stop();
                this.reconnectTimer.Close();

                this.reconnectTimer = null;
            }
        }

        public override bool SendMessage(Message message, bool async)
        {
            if (message.Sender == null || message.Sender.Equals(String.Empty))
            {
                message.Sender = this.UserName;
            }

            System.Net.Mail.MailMessage messageToSend = new System.Net.Mail.MailMessage();
            foreach (String to in message.Reciever)
                messageToSend.To.Add(to);
            messageToSend.Subject = "";
            //messageToSend.From = new System.Net.Mail.MailAddress(message.Sender);
            messageToSend.From = new System.Net.Mail.MailAddress(this.UserName);
            messageToSend.Body = message.FullMessage;

            SmtpClient smtp = new SmtpClient()
            {
                Host = this.SMTPServer,
                Port = this.SMTPPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(this.UserName, this.Password)
            };
            try
            {
                if (async)
                    smtp.SendAsync(messageToSend, null);
                else
                    smtp.Send(messageToSend);
            }
            catch (Exception ex)
            {
                Logger.LogMessage("GMailWatcherService.SendMessage: SendMessage failed: " + ex.Message, LoggerLevel.WARNING);
                return false;
            }
            return true;
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
