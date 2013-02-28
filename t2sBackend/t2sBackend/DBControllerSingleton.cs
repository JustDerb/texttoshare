using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public static class DBControllerSingleton{
    

        private static IDBController _Controller;

        private static IDBController Controller
        {
            get
            {
                if (_Controller == null)
                {
                    _Controller = new SQLController();
                }
                return _Controller;
            }
            set
            {
                _Controller = value;
            }
        }

        public static IDBController GetInstance()
        {
            return Controller;
        }
    }
}
