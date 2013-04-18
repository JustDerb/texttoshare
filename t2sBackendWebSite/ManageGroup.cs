using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using t2sDbLibrary;


namespace manageGroup
{
    /// <summary>
    /// Summary description for ManageGroup
    /// </summary>
    public class ManageGroup : Page
    {
        public ManageGroup()
        {
        }


        protected System.Web.UI.WebControls.Label MyLabel;
        protected System.Web.UI.WebControls.Button MyButton;
        protected System.Web.UI.WebControls.TextBox MyTextBox;


        public void updateGroup_Click(Object sender, EventArgs e)
        {

            SqlController controller = new SqlController();
            GroupDAO group;
            String groupName = Request["groupNameBox"];
            String groupTag = Request["groupTagBox"];
            String groupDescription = Request["groupDescripationBox"];
            try
            {
                group = controller.RetrieveGroup(groupTag);
                group.Name = groupName;
                group.GroupTag = groupTag;
                group.Description = groupDescription;
                //update the group
                try
                {
                    controller.UpdateGroup(group);
                }
                catch (ArgumentNullException error)
                {

                }
            }
            catch (ArgumentNullException error)
            {

            }
            catch (CouldNotFindException error)
            {

            }
           
           
            
            

        }

        /// <summary>
        /// called onload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void popluateGroupList(Object sender, EventArgs e)
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
                DropDownList groupPlugin = ((DropDownList)((DropDownList)sender).Parent.FindControl("groupPlugin"));
                groupPlugin.Items.AddRange(list);
            }
            catch (SqlException error)
            {
                // Logger.LogMessage("SQL exception with Retrieve Enabled plugin");
            }
        }

        /// <summary>
        /// called onload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void retrieveEnabledGroupPlugins(Object sender, EventArgs e)
        {
            try
            {
                //TODO
                //need to get groupid from session
                SqlController controller = new SqlController();
                GroupDAO group = controller.RetrieveGroup(Session["groupTag"].ToString());
               // GroupDAO group = controller.RetrieveGroup(Request["groupTagBox"]);
                List<PluginDAO> plugins = controller.GetAllEnabledGroupPlugins(group.GroupID);
                String[] names = new String[plugins.Count];
                ListItem[] list = new ListItem[plugins.Count];
                for (int i = 0; i < plugins.Count; i++)
                {
                    ListItem item = new ListItem(plugins.ElementAt(i).Name);
                    list[i] = item;
                }
                DropDownList enabledPlugins = ((DropDownList)((DropDownList)sender).Parent.FindControl("enabledPlugins"));
                enabledPlugins.Items.AddRange(list);
            }
            catch (SqlException error)
            {
                // Logger.LogMessage("SQL exception with Retrieve Enabled plugin");
            }
        }



        public void unEnablePlugin_Click(Object sender, EventArgs e)
        {
            //TODO
            //remove from dropdown
            SqlController control = new SqlController();
            DropDownList unEnable = ((DropDownList)((DropDownList)sender).Parent.FindControl("unEnable"));
            ListItem plugin = unEnable.SelectedItem;
            String pluginName = plugin.Text;
            try
            {
                PluginDAO toUnEnable = control.RetrievePlugin(pluginName);
                try
                {
                    GroupDAO group = control.RetrieveGroup(Request["groupTagBox"]);
                    control.DisablePluginForGroup(group.GroupID, toUnEnable.PluginID);
                }
                catch (ArgumentNullException)
                {

                }
                catch (CouldNotFindException)
                {

                }

            }
            catch (ArgumentNullException error)
            {

            }
            catch (CouldNotFindException error)
            {

            }
        }




        public void enablePlugin(Object sender, EventArgs e)
        {
            //TOOD
            //add to other dropdown list
            SqlController control = new SqlController();
            DropDownList enabledPlugins = ((DropDownList)((DropDownList)sender).Parent.FindControl("enabledPlugins"));
            ListItem plugin = enabledPlugins.SelectedItem;
            String pluginName = plugin.Text;
            try
            {
                PluginDAO toEnable = control.RetrievePlugin(pluginName);
                try
                {
                    GroupDAO group = control.RetrieveGroup(Request["groupTagBox"]);
                    control.EnablePluginForGroup(group.GroupID, toEnable.PluginID);
                }
                catch (ArgumentNullException)
                {

                }
                catch (CouldNotFindException)
                {

                }
                
            }
            catch (ArgumentNullException error)
            {

            }
            catch (CouldNotFindException error)
            {

            }

        }

    }



}