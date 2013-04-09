using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;


namespace users
{
    /// <summary>
    /// Summary description for GetUser
    /// </summary>
    public class AddGroup : Page
    {
        public AddGroup()
        {
         
        }

        protected System.Web.UI.WebControls.Label MyLabel;
        protected System.Web.UI.WebControls.Button MyButton;
        protected System.Web.UI.WebControls.TextBox MyTextBox;


        public void addGroup_Click(Object sender, EventArgs e)
        {
            //TODO
            SqlController controller = new SqlController();
            UserDAO user = new UserDAO();
            GroupDAO group = new GroupDAO();
            //grab input from textboxs
            //get owner from session
          //  String owner = Request["ownerNamebox"];
            String users = Request["usersNameBox"];
            String moderators = Request["moderatorsBox"];
            String enabledPlugins = Request["enabledPluginsBox"];
            String groupName = Request["groupNameBox"];
            String userName = Request["groupTagBox"];
            String descripation = Request["groupDescripationBox"];
            //get owner
            try
            {
              //  controller.RetrieveUserByUserName(owner);

            }
            catch (CouldNotFindException)
            {

            }
            catch (ArgumentNullException)
            {

            }
            //get moderators
            List<UserDAO> userList = new List<UserDAO>();
            String[] usersArray = users.Split(',');
            foreach (String i in usersArray)
            {
                try
                {
                    UserDAO newGroupMember =  controller.RetrieveUserByUserName(i);
                    userList.Add(newGroupMember);
                }
                catch (CouldNotFindException)
                {
                    Response.Write("The user " + i + " could not be found in the database!");
                    return;
                }
                catch (ArgumentNullException)
                {
                    //shouldn't happen i think
                    Response.Write("Null ArgumentException");
                    return;
                }
            }
        }


    }

}