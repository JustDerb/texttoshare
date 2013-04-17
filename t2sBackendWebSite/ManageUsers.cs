using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using t2sDbLibrary;



namespace ManageUsers
{
    /// <summary>
    /// Summary description for manageUsers
    /// </summary>
    public class manageUsers : Page
    {
        public manageUsers()
        {

        }

        protected System.Web.UI.WebControls.Label MyLabel;
        protected System.Web.UI.WebControls.Button MyButton;
        protected System.Web.UI.WebControls.TextBox MyTextBox;

        public void update_Click(Object sender, EventArgs e)
        {
            SqlController controller = new SqlController();
            UserDAO user;
            String firstName = Request["firstNameBox"];
            String lastName = Request["lastNameBox"];
            String userName = Request["userNameBox"];
            String phoneNumber = Request["phoneNumberBox"];
            String carrier = Request["carrierBox"];
            //String password = Request["passwordBox"];
            user= controller.RetrieveUserByUserName(userName);
            user.UserName = userName;
            user.PhoneNumber = phoneNumber;
            user.FirstName = firstName;
            //user.Carrier = new PhoneCarrier(carrier);
            try
            {
                controller.UpdateUser(user);

            }catch(ArgumentNullException error){

            }

            
        }


        public void popluatePluginList(Object sender, EventArgs e)
        {
            try
            {

               SqlController controller = new SqlController();
               // controller.
              /*  List<PluginDAO> plugins = controller.ge;
                String[] names = new String[plugins.Count];
                ListItem[] list = new ListItem[plugins.Count];
                for (int i = 0; i < plugins.Count; i++)
                {
                    ListItem item = new ListItem(plugins.ElementAt(i).Name);
                    list[i] = item;
                }
                DropDownList groupPlugin = ((DropDownList)((DropDownList)sender).Parent.FindControl("groupPlugin"));
                groupPlugin.Items.AddRange(list);*/
            }
            catch (SqlException error)
            {
                // Logger.LogMessage("SQL exception with Retrieve Enabled plugin");
            }

        }


        public void retrieveEnabledPlugins(Object sender, EventArgs e)
        {

        }

        public void fillFirstNameBox(Object sender, EventArgs args)
        {
            TextBox firstName = ((TextBox)((TextBox)sender).Parent.FindControl("firstNameBox"));
            firstName.Text = Session["firstName"].ToString();
            //return Session["firstName"].ToString();

        }




    }
}
