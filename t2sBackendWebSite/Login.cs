using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;
using System.Data.SqlClient;

namespace login
{
    /// <summary>
    /// Summary description for Login
    /// </summary>
    public class Login : Page
    {
        public Login()
        {
        }

        public void Login_Click(Object sender, EventArgs e)
        {
            SqlController controller = new SqlController();
            String userName = Request["userNameBox"];
            String pasword = Request["passwordBox"];

            if (controller.CheckLogin(userName, pasword))
            {
                Response.Write("You're logged In");
                Session["Username"] = userName;
                try
                {
                    UserDAO user = controller.RetrieveUserByUserName(userName);
                   //set session info
                    Session["lastName"]=  user.LastName;
                    Session["firstName"]=  user.FirstName;
                    Session["carrier"] = user.Carrier;
                    Session["phoneNumber"] = user.PhoneNumber;
                    Session["id"] = user.UserID;
                    Session["phoneEmail"] = user.PhoneEmail;
                   // Session["plugins"] = controller.GetPluginsOwnedByUser(user);
                }
                catch (ArgumentNullException error)
                {

                }
                catch (CouldNotFindException error)
                {

                }
                return;
            }
            else
            {
                Response.Write("Incorrect user name or password!");
                return;
            }

        }




        
    }
}