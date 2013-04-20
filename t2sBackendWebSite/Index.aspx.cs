using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using t2sDbLibrary;

public partial class Index : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
    }

    public void retrievePlugins(Object sender, EventArgs e)
    {
        SqlController control = new SqlController();
        try
        {
            //String response = HttpContext.Current.Session["username"].ToString();
            // Response.Write(response);

            UserDAO user = control.RetrieveUserByUserName(Session["username"].ToString());
            List<PluginDAO> plugins = control.GetPluginsOwnedByUser(user);
            String[] names = new String[plugins.Count];
            ListItem[] list = new ListItem[plugins.Count];
            for (int i = 0; i < plugins.Count; i++)
            {
                ListItem item = new ListItem(plugins.ElementAt(i).Name);
                list[i] = item;
            }
            DropDownList PluginsOwned = ((DropDownList)((DropDownList)sender).Parent.FindControl("PluginsOwned"));
            PluginsOwned.Items.AddRange(list);

        }
        catch (ArgumentNullException)
        {

        }
        catch (CouldNotFindException)
        {

        }
    }


}