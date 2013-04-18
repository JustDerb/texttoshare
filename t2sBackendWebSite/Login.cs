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
    public class Login : Page, IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                    HttpContext.Current.Session["username"] = user.UserName;
                    HttpContext.Current.Session["lastName"] = user.LastName;
                    HttpContext.Current.Session["firstName"] = user.FirstName;
                    HttpContext.Current.Session["carrier"] = user.Carrier.GetName();
                    HttpContext.Current.Session["phoneNumber"] = user.PhoneNumber;
                    HttpContext.Current.Session["userid"] = user.UserID;
                    HttpContext.Current.Session["phoneEmail"] = user.PhoneEmail;
                    Session["userDAO"] = user;
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
                Response.Redirect("Index.aspx");
                return;
            }

            Response.Write("Incorrect user name or password!");
            return;
        }
    }
}