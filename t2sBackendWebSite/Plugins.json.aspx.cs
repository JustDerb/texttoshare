using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;

public partial class Plugins_json : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        Response.ContentType = "application/json; charset=utf-8";

        String searchFor = Request.QueryString["search"];

        StringBuilder userJson = new StringBuilder();
        userJson.Append(@"{");
        userJson.Append(@" ""Plugins"" : [ ");

        try
        {
            if (!String.IsNullOrEmpty(searchFor))
            {
                SqlController controller = new SqlController();
                List<PluginDAO> plugins = controller.RetrieveEnabledPlugins();
                foreach (PluginDAO plugin in plugins)
                {
                    userJson.Append(@"""");
                    userJson.Append(plugin.Name);
                    userJson.Append(@"""");
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogMessage("Plugins.json.aspx: " + ex.Message, LoggerLevel.SEVERE);
        }

        userJson.Append(@" ] ");
        userJson.Append(@"}");

        Response.Write(userJson.ToString());
        Response.End();
    }
}