using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sDbLibrary
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
        public UserLevel UserLevel
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

        /// <summary>
        /// Creates a new UserDAO object. By default the UserID is set to null because we do not know
        /// if there is a reference to this UserDAO in the database.
        /// </summary>
        public UserDAO()
        {
            this.UserID = null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            UserDAO u = obj as UserDAO;

            return (
                this.UserID == u.UserID &&
                this.FirstName.Equals(u.FirstName) &&
                this.LastName.Equals(u.LastName) &&
                this.UserName.Equals(u.UserName) &&
                this.PhoneNumber.Equals(u.PhoneNumber) &&
                this.PhoneEmail.Equals(u.PhoneEmail) &&
                this.Carrier.Equals(u.Carrier) &&
                this.IsBanned == u.IsBanned &&
                this.DateBanned.Equals(u.DateBanned) &&
                this.IsSuppressed == u.IsSuppressed &&
                this.SuppressedDate.Equals(u.SuppressedDate) &&
                this.SuppressedLength.Equals(u.SuppressedLength) &&
                this.UserLevel.Equals(u.UserLevel)
            );
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + (null == UserID ? 0 : UserID.GetHashCode());
                hash = hash * 23 + (null == FirstName ? 0 : FirstName.GetHashCode());
                hash = hash * 23 + (null == LastName ? 0 : LastName.GetHashCode());
                hash = hash * 23 + (null == UserName ? 0 : UserName.GetHashCode());
                hash = hash * 23 + (null == PhoneNumber ? 0 : PhoneNumber.GetHashCode());
                hash = hash * 23 + (null == PhoneEmail ? 0 : PhoneEmail.GetHashCode());
                hash = hash * 23 + Carrier.GetHashCode();
                hash = hash * 23 + IsBanned.GetHashCode();
                hash = hash * 23 + (null == DateBanned ? 0 : DateBanned.GetHashCode());
                hash = hash * 23 + IsSuppressed.GetHashCode();
                hash = hash * 23 + (null == SuppressedDate ? 0 : SuppressedDate.GetHashCode());
                hash = hash * 23 + (null == SuppressedLength ? 0 : SuppressedLength.GetHashCode());
                hash = hash * 23 + UserLevel.GetHashCode();

                return hash;
            }
        }
    }
}
