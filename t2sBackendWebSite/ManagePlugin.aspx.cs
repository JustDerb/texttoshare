using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using t2sDbLibrary;

public partial class ManagePlugin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        popluateList(sender, e);
    }

     public void popluateList(Object sender, EventArgs e)
        {


            try
            {
                
                SqlController controller = new SqlController();
                List<PluginDAO> plugins =  controller.RetrieveEnabledPlugins();
                String[] names = new String[plugins.Count];
                ListItem[] list = new ListItem[plugins.Count];
                for (int i = 0; i < plugins.Count; i++){
                    ListItem item = new ListItem(plugins.ElementAt(i).Name);
                    list[i] = item;
                   // names[i] = plugins.ElementAt(i).Name;
                }
               // DropDownList menu = new DropDownList();
                DropDownList ddlPlugin = ((DropDownList)((DropDownList)sender).Parent.FindControl("ddlPlugin"));

                ddlPlugin.Items.AddRange(list);
               // menu.Items.AddRange(list);

            }catch(SqlException error){
               // Logger.LogMessage("SQL exception with Retrieve Enabled plugin");
            }
        }


}