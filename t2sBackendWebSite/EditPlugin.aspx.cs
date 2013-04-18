using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;

public partial class EditPlugin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (null == Session["username"])
        {
            Response.Redirect("Login.aspx");
            return;
        }
        else
        {
            UserDAO user = (UserDAO)Session["userDAO"];
            String pluginName = Request.QueryString["p"];
            StringBuilder title = new StringBuilder();
            title.Append("text2share: Edit Plugin");
            if (pluginName == null)
            {
                
            }
            else
            {
                title.Append(" (");
                title.Append(pluginName);
                title.Append(")");
            }

            PageTitle.Text = title.ToString();
            editorText.InnerText = user.FirstName;
        }
    }
}