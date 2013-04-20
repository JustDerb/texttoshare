using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BasePage : System.Web.UI.Page
{
    public void CheckLoginSession()
    {
        if (null == Session["username"])
        {
            Response.Redirect("Login.aspx");
        }
    }
}
