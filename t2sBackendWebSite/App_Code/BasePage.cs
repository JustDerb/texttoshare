using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public class BasePage : System.Web.UI.Page
{
    public void CheckLoginSession()
    {
        if (null == HttpContext.Current.Session["userDAO"])
        {
            Response.Redirect("Login.aspx");
        }
    }

    public void LogoutUser()
    {
        ClearCache();
        HttpContext.Current.Session.Abandon();
        Response.Redirect("Login.aspx");
    }

    public void ClearCache()
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoServerCaching();
        Response.Cache.SetNoStore();
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
    }
}
