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

public partial class ManagePlugin : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
        popluateList(sender, e);
    }

     public void popluateList(Object sender, EventArgs e)
        {

            try
            {

                SqlController controller = new SqlController();
                UserDAO user = controller.RetrieveUserByUserName((String)Session["username"]);
                List<PluginDAO> pluginsOwned = controller.GetPluginsOwnedByUser(user);

                Response.Write("plugins owned is: " + pluginsOwned.Count);

              
                String[] names = new String[pluginsOwned.Count];
                ListItem[] list = new ListItem[pluginsOwned.Count];
                for (int i = 0; i < pluginsOwned.Count; i++)
                {
                    ListItem item = new ListItem(pluginsOwned.ElementAt(i).Name);
                    list[i] = item;
                    // names[i] = plugins.ElementAt(i).Name;
                }
                // DropDownList menu = new DropDownList();
               Control con = FindControl("Plugins");
               // DropDownList Plugins = ((DropDownList)((DropDownList)sender).Parent.FindControl("Plugins"));

               DropDownList Plugins = (DropDownList)con;

                Plugins.Items.AddRange(list);
                
                // menu.Items.AddRange(list);

            }
            catch (SqlException error)
            {
                // Logger.LogMessage("SQL exception with Retrieve Enabled plugin");
            }
            catch (ArgumentNullException error)
            {

            }
            catch (CouldNotFindException error)
            {

            }
        }


     public void updatePlugin_Click(Object sender, EventArgs e)
     {
         String pluginName = Request["pluginNameBox"];
         String pluginOwner = Request["pluginOwner"];
         String helptext = Request["helpTextBox"];
         String plugDescrip = Request["pluginDescripationBox"];
         String version = Request["versionBox"];
         SqlController control = new SqlController();
         try
         {
             //TODO
             //check session user id to make sure they are the owner of plugin
             PluginDAO plugin = control.RetrievePlugin(pluginName);
             if (!plugin.Description.Equals(""))
             {
                 plugin.Description = plugDescrip;
             }
             if (!plugin.VersionNum.Equals(""))
             {
                 plugin.VersionNum = version;
             } if (plugin.HelpText.Equals(""))
             {
                 plugin.HelpText = helptext;
             }
             //plugin.OwnerID = control.RetrieveUserByUserName(pluginOwner).UserID;
             control.UpdatePluginOwner(plugin, control.RetrieveUserByUserName(pluginOwner));
             control.UpdatePlugin(plugin);
         }
         catch (CouldNotFindException error)
         {

         }
         catch (ArgumentNullException error)
         {

         }

     }

     /*public void popluateList(Object sender, EventArgs e)
     {

         try
         {
             SqlController controller = new SqlController();
             List<PluginDAO> plugins = controller.RetrieveEnabledPlugins();
             String[] names = new String[plugins.Count];
             ListItem[] list = new ListItem[plugins.Count];
             for (int i = 0; i < plugins.Count; i++)
             {
                 ListItem item = new ListItem(plugins.ElementAt(i).Name);
                 list[i] = item;
             }
             DropDownList ddlPlugin = ((DropDownList)((DropDownList)sender).Parent.FindControl("ddlPlugin"));
             ddlPlugin.Items.AddRange(list);
         }
         catch (SqlException error)
         {
             // Logger.LogMessage("SQL exception with Retrieve Enabled plugin");
         }
     }*/



}