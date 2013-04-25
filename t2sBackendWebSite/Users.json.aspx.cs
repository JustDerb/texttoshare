using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;

public partial class Users_json : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        Response.ContentType = "application/json; charset=utf-8";

        String searchFor = Request.QueryString["search"];

        StringBuilder userJson = new StringBuilder();
        userJson.Append(@"{");
        userJson.Append(@" ""Users"" : [ ");

        try
        {
            if (!String.IsNullOrEmpty(searchFor))
            {
                SqlController controller = new SqlController();
                List<UserDAO> users = controller.GetAllUsers();
                foreach (UserDAO user in users)
                {
                    if (user.UserName.IndexOf(searchFor, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        userJson.Append(@"""");
                        userJson.Append(user.UserName);
                        userJson.Append(@"""");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogMessage("Users.json.aspx: " + ex.Message, LoggerLevel.SEVERE);
        }

        userJson.Append(@" ] ");
        userJson.Append(@"}");

        Response.Write(userJson.ToString());
        Response.End();
    }
}