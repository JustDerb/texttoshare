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
            GroupDAO group=null;
            UserDAO owner = new UserDAO();
            String ownerUsername = Request["groupOwner"];
            try
            {

               owner =  controller.RetrieveUserByUserName(ownerUsername);
            }
            catch (ArgumentNullException)
            {
                Response.Write("Owner username is null! ");
            }
            catch (CouldNotFindException)
            {
                Response.Write("Could not find the username in database! ");
            }

            if (owner != null)
            {
                group = new GroupDAO(owner);
                group.Name = Request["groupNameBox"];
                group.GroupTag = Request["groupTagBox"];
                group.Description = Request["groupDescripationBox"];
            }
            bool added=false;
            try
            {
                added = controller.CreateGroup(group);
            }
            catch (ArgumentNullException)
            {
                Response.Write("The group being inserted is NULL ");
            }
            catch (EntryAlreadyExistsException)
            {
                Response.Write("This group already exists! ");
            }
            if(added){
                Response.Write("Your group was created successfully! ");
            }else{
                Response.Write("Your group was not created successfully. Please try again!");
            }

            //get moderators
          //  List<UserDAO> userList = new List<UserDAO>();
          /*  String[] usersArray = users.Split(',');
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
            }*/
        }


    }

}