using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using t2sDbLibrary;

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

    /// <summary>
    /// Checks if the given user is verified in the database. If they are not, they are redirected to the
    /// Verification page. Otherwise, they are sent to the Index page. Users are always sent to the
    /// Verification page on first registering with the application.
    /// </summary>
    /// <param name="currentUser">The user to check in the database.</param>
    /// <returns>true if the user is already verified</returns>
    /// <exception cref="SqlException">If there is an issue connecting to the database.</exception>
    public bool isVerified(UserDAO currentUser)
    {
        try
        {
            IDBController controller = new SqlController();
            string val = controller.GetCurrentVerificationValueForUser(currentUser);
            return null == val;
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (CouldNotFindException)
        {
            // User was literally just created, shouldn't be a problem
        }
        // Let the other pages handle SqlExceptions, for displaying to users

        return false;
    }
}
