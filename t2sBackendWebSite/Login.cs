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
                try
                {
                    UserDAO user = controller.RetrieveUserByUserName(userName);
                    //set session info
                    Session["username"] = user.UserName;
                    Session["lastName"] = user.LastName;
                    Session["firstName"] = user.FirstName;
                    Session["carrier"] = user.Carrier;
                    Session["phoneNumber"] = user.PhoneNumber;
                    Session["userid"] = user.UserID;
                    Session["phoneEmail"] = user.PhoneEmail;
                    // Session["plugins"] = controller.GetPluginsOwnedByUser(user);
                }
                catch (ArgumentNullException)
                {
                    Response.Write("Incorrect user name or password!");
                    return;
                }
                catch (CouldNotFindException)
                {
                    Response.Write("Incorrect user name or password!");
                    return;
                }

                //session information was set no problem, redirect user to home page
                Response.Redirect("");
                return;
            }

            Response.Write("Incorrect user name or password!");
            return;
        }
    }
}