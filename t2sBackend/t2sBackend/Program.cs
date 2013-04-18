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
            Logger.LogMessage("Established connection to SQL server", LoggerLevel.DEBUG);
            try
            {
                // Try and add data, if it errors we probably already have it in the DB
                PrivateInfo.addTestData(database);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ADDING PRIVATE INFO: " + ex.Message);
            }
            AWatcherService gmailServ = new GMailWatcherService(
                PrivateInfo.Email.UserName,
                PrivateInfo.Email.Password,
                true,
                "imap.gmail.com",
                993,
                "smtp.gmail.com",
                587);

            MessageControllerOverride controller = new MessageControllerOverride(gmailServ, database);
            PluginLibrary pluginLib = new PluginLibrary(controller, gmailServ, database);

            pluginLib.Start();
            Logger.LogMessage("Started PluginLibrary", LoggerLevel.DEBUG);
            gmailServ.Start();
            Logger.LogMessage("Started AWatcherService", LoggerLevel.DEBUG);


            // Add fake emails (For testing)
            List<Message> msgArray = new List<Message>();
            //msgArray.Add(new Message(...));

            foreach (Message msg in msgArray)
                controller.putNextMessage(MessageParser.Parse(msg, database));

            Logger.LogMessage("Waiting for messages...", LoggerLevel.DEBUG);

            // BAD
            while (true) ;
        }
    }
}
