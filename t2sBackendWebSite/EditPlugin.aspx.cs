using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;

public partial class EditPlugin : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();

        StringBuilder title = new StringBuilder();
        title.Append("Edit Plugin");
        if (Request.HttpMethod.Equals("post", StringComparison.OrdinalIgnoreCase))
        {
            if (!doPOST())
                return;
        }
        else if (Request.HttpMethod.Equals("get", StringComparison.OrdinalIgnoreCase))
        {
            if (!doGET())
                return;
        }

        String pluginName = Request.QueryString["p"];
        if (pluginName != null)
        {
            title.Append(" (");
            title.Append(pluginName);
            title.Append(")");
        }

        PageTitle.Text = title.ToString();
    }

    private bool doPOST()
    {
        UserDAO user = (UserDAO)Session["userDAO"];
        String pluginName = Request.QueryString["p"];

        return true;
    }

    private bool doGET()
    {
        UserDAO user = (UserDAO)Session["userDAO"];
        String pluginName = Request.QueryString["p"];
        if (pluginName == null)
        {
            // Redirect them back
            SendErrorMessage("Please specify a plugin");
            return false;
        }
        IDBController controller = new SqlController();
        PluginDAO plugin = null;
        try
        {
            plugin = controller.RetrievePlugin(pluginName);
        }
        catch (CouldNotFindException)
        {
            SendErrorMessage("That is not a valid plugin");
            return false;
        }

        if (!plugin.OwnerID.Equals(user.UserID))
        {
            SendErrorMessage("That is not a plugin you have written");
            return false;
        }

        String luacodeFileLoc = LUADefinitions.getLuaScriptLocation("remember");//plugin.Name);

        // See if it's there
        if (!File.Exists(luacodeFileLoc))
        {
            SendErrorMessage("Could not find plugin " + pluginName);
            return false;
        }
        String luacode = "";
        try
        {
            luacode = File.ReadAllText(luacodeFileLoc, Encoding.UTF8);
        }
        catch (Exception)
        {
            SendErrorMessage("Could not find plugin " + pluginName);
            return false;
        }

        editorText.InnerText = luacode;

        return true;
    }

    private void SendErrorMessage(string message)
    {
        Response.Write(message);
    }
}