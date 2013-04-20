﻿using System;
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

            HttpContext.Current.Session["username"] = user.UserName;
            HttpContext.Current.Session["lastName"] = user.LastName;
            HttpContext.Current.Session["firstName"] = user.FirstName;
            HttpContext.Current.Session["carrier"] = user.Carrier.GetName();
            HttpContext.Current.Session["phoneNumber"] = user.PhoneNumber;
            HttpContext.Current.Session["userid"] = user.UserID;
            HttpContext.Current.Session["phoneEmail"] = user.PhoneEmail;
            HttpContext.Current.Session["userDAO"] = user;

            //session information was set no problem, redirect user to home page
            Response.Redirect("Index.aspx");
            return;
        }

        invalidCredentials.Text = "Invalid user name or password.";
        return;
    }
}