using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using t2sDbLibrary;

namespace t2sBackend
{
    /// <summary>
    /// Plugin that removes a specified user from the specified group
    /// </summary>
    public class RemoveUserPlugin : IPlugin
    {
        /// <summary>
        /// Takes in the info from the moderator/owner and removes a user from the group
        /// </summary>
        /// <param name="message">Message to be handled</param>
        /// <param name="service">Service to send/recieve messages through</param>
        public void Run(ParsedMessage message, AWatcherService service, IDBController controller)
        {
            Message msgSender = new Message();
            msgSender.Reciever.Add(message.Sender.PhoneEmail);
            Message msgRemovedUser = new Message();
            try
            {
                UserDAO userToBeRemoved = controller.RetrieveUserByUserName(message.ContentMessage);
                if (userToBeRemoved == null || !message.Group.Users.Contains(userToBeRemoved))
                {
                    msgSender.FullMessage = "Failed to remove " + message.Arguments[0] + " from the group "
                        + message.Group.GroupID + ". Please check user/group and try again.";
                }
                else
                {
                    msgRemovedUser.Reciever.Add(userToBeRemoved.PhoneEmail);
                    controller.RemoveMemberFromGroup(message.Group.GroupID, userToBeRemoved.UserID);
                    msgSender.FullMessage = "Successfully removed " + message.ContentMessage + " from the group "
                        + message.Group.Name + ".";
                    msgRemovedUser.FullMessage = "You have been removed from group " + message.Group.Name + ".";
                    service.SendMessage(msgRemovedUser);
                }
            }
            catch (Exception)
            {
                msgSender.FullMessage = "Failed to remove " + message.ContentMessage + " from the group "
                    + message.Group.GroupID + ". Please check user/group and try again.";
            }
            msgSender.Reciever.Add(message.Sender.PhoneEmail);
            service.SendMessage(msgSender);
        }

        /// <summary>
        /// Stores metadata related to this plugin
        /// </summary>
        public PluginDAO PluginDAO
        {
            get;
            set;
        }
    }
}