using S22.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using t2sDbLibrary;

namespace t2sBackend
{
    class Program
    {
        static void Main(string[] args)
        {
            IDBController database = new SqlController();
            AWatcherService gmailServ = new GMailWatcherService(
                PrivateInfo.Email.UserName,
                PrivateInfo.Email.Password,
                true,
                "imap.gmail.com",
                993,
                "smtp.gmail.com",
                465);

            MessageControllerOverride controller = new MessageControllerOverride(gmailServ, database);
            PluginLibrary pluginLib = new PluginLibrary(controller, gmailServ);

            pluginLib.Start();
            gmailServ.Start();

            // Add fake emails (For testing)
            List<Message> msgArray = new List<Message>();
            //msgArray.add(...)

            foreach (Message msg in msgArray)
                controller.putNextMessage(MessageParser.Parse(msg, database));

            // BAD
            while (true) ;
        }
    }
}
