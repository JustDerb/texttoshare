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

        public void retrieveEnabledGroupPlugins(Object sender, EventArgs e)
        {
            try
            {
                SqlController controller = new SqlController();
                GroupDAO group = controller.RetrieveGroup(Request["groupTagBox"]);
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



    }



}