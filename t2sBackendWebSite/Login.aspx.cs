using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;
using System.Data.SqlClient;

public partial class Login : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PageTitle.Text = "Text2Share - Login";
    }

    public void Login_Click(Object sender, EventArgs e)
    {
        SqlController controller = new SqlController();
        String userName = Request["userNameBox"];
        String pasword = Request["passwordBox"];

        if (controller.CheckLogin(userName, pasword))
        {
            UserDAO user;
            try
            {
                user = controller.RetrieveUserByUserName(userName);
            }
            catch (ArgumentNullException)
            {
                invalidCredentials.Text = "Invalid user name or password.";
                return;
            }
            catch (CouldNotFindException)
            {
                invalidCredentials.Text = "Invalid user name or password.";
                return;
            }

            HttpContext.Current.Session["userDAO"] = user;

            // Check if the user's phone-email is already verified in the system
            try
            {
                if (!base.isVerified(user))
                {
                    Response.Redirect("Verification.aspx");
                }
                else
                {
                    Response.Redirect("Index.aspx");
                }
            }
            catch (SqlException ex)
            {
                Logger.LogMessage("Verification.aspx: " + ex.Message, LoggerLevel.SEVERE);
                invalidCredentials.Text = "An unknown error occured. Please try again later.";
            }

            return;
        }

        invalidCredentials.Text = "Invalid user name or password.";
        return;
    }
}