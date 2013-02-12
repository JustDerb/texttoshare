using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public abstract class GMailWatcherService : IWatcherService
    {
        public string UserName
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string Password
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public bool UseSSL
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void Login()
        {
            throw new System.NotImplementedException();
        }

        public void Logout()
        {
            throw new System.NotImplementedException();
        }

        private abstract void AddMessage(t2sBackend.Message message);
    }
}
