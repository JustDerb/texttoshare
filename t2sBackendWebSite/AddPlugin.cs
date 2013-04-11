using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;


namespace plugin
{
    /// <summary>
    /// Summary description for AddPlugin
    /// </summary>
    public class AddPlugin : Page
    {
        public AddPlugin()
        {

        }

        public void AddPlugin_Click(Object sender, EventArgs e)
        {
            SqlController controller = new SqlController();
            PluginDAO plugin = new PluginDAO();
            UserDAO owner= new UserDAO();
            plugin.Description=Request["pluginDescripationBox"];
            plugin.HelpText = Request["helpTextBox"];
            plugin.IsDisabled = false;
            plugin.VersionNum = Request["versionBox"];
            plugin.Name = Request["pluginNameBox"];
                           Request.fil
            String ownerUserName = Request["pluginOwner"];
            try{
                owner = controller.RetrieveUserByUserName(ownerUserName);
            }catch(ArgumentNullException){
                Response.Write("Owner username is Null");
            }catch(CouldNotFindException){
                Response.Write("Could not find the owner");
            }
            if (owner != null)
            {
                plugin.OwnerID = owner.UserID;
            }

        }


    }


    

}