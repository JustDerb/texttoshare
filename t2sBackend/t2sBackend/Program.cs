using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t2sBackend
{
    class Program
    {
        static void Main(string[] args)
        {
            IDBController database = new SQLController();
            AWatcherService gmailServ = new GMailWatcherService(
                PrivateInfo.Email.UserName,
                PrivateInfo.Email.Password,
                true,
                "imap.gmail.com",
                993,
                "smtp.gmail.com",
                465);

            MessageController controller = new MessageController(gmailServ, database);
            PluginLibrary pluginLib = new PluginLibrary(controller, gmailServ);

            pluginLib.Start();
            gmailServ.Start();

            // BAD
            while (true) ;
        }
    }
}
