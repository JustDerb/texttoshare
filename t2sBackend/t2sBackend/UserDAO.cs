using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDAO
    {
        /// <summary>
        /// The web username for the User
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// The first name of the User
        /// </summary>
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// The last name of the User
        /// </summary>
        public string LastName
        {
            get;
            set;
        }

        /// <summary>
        /// The email address to send text messages to the User
        /// </summary>
        public string PhoneEmail
        {
            get;
            set;
        }

        /// <summary>
        /// The phone number of the User to text
        /// </summary>
        public string PhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// The User's phone carrier to determine email address
        /// </summary>
        public PhoneCarrier Carrier
        {
            get;
            set;
        }

        /// <summary>
        /// If the User has been banned for misuse of the application/website
        /// </summary>
        public bool IsBanned
        {
            get;
            set;
        }

        /// <summary>
        /// Controls whether User receives text messages. Is reset at the end of the calendar month
        /// </summary>
        public bool IsSuppressed
        {
            get;
            set;
        }

        /// <summary>
        /// Keeps track of when User was banned for logging and adminstrative reason
        /// </summary>
        public DateTime DateBanned
        {
            get;
            set;
        }

        /// <summary>
        /// Keeps track of when the User chose to stop text messages. Will be used to determine at the end of what month to resume messaging
        /// </summary>
        public DateTime SuppressedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Controls how long before resetting the User to begin recieving messages again.
        /// </summary>
        public TimeSpan SuppressedLength
        {
            get;
            set;
        }

        /// <summary>
        /// Used to manage the User in the Database
        /// </summary>
        public int? UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Controls the access for the User to certain privileges
        /// </summary>
        public UserType UserLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Calculates the Date to re-enable the User to begin recieving text messages
        /// </summary>
        /// <returns></returns>
        public DateTime GetUnsuppressedDate()
        {
            return SuppressedDate.Add(SuppressedLength);
        }
    }
}
