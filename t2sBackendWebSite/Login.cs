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