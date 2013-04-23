using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using t2sDbLibrary;


public partial class ManageGroup : BasePage
{
    private GroupDAO _currentGroup;
    private IDBController controller = new SqlController();

    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
        PageTitle.Text = "Text2Share - Manage Group";

        GetGroupAndSetData();
    }

    /// <summary>
    /// Uses the grouptag GET parameter and retrieves the group metadata.
    /// Populates the "Group Information" section as well.
    /// </summary>
    private void GetGroupAndSetData()
    {
        try
        {
            _currentGroup = controller.RetrieveGroup(Request["grouptag"]);
        }
        catch (ArgumentNullException)
        {
            Response.Write("Grouptag field is null!");
        }catch(CouldNotFindException){
            Response.Write("Group could not be found!");
        }
        catch (SqlException err)
        {
            Response.Write("An unknown error has happened");
            Logger.LogMessage("ManageGroup.aspx " + err.Message, LoggerLevel.SEVERE);       
        }

        groupNameBox.Text = _currentGroup.Name;
        groupTagBox.Text = _currentGroup.GroupTag;
        groupDescriptionBox.Text = _currentGroup.Description;
    }

    /// <summary>
    /// Updates the group's metadata in the database 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateGroupMetadata_Click(Object sender, EventArgs e)
    {
        // Check that they are not updating to empty values
        if (string.IsNullOrWhiteSpace(groupNameBox.Text))
        {
            invalidEntries.Text = "Cannot update group name to be empty or whitespace.";
            groupNameBox.Focus();
        }
        else if (string.IsNullOrWhiteSpace(groupTagBox.Text))
        {
            invalidEntries.Text = "Cannot update group tag to be empty or whitespace.";
            groupTagBox.Focus();
        }
        else if (string.IsNullOrWhiteSpace(groupDescriptionBox.Text))
        {
            invalidEntries.Text = "Cannot update group description to be empty or whitespace.";
            groupDescriptionBox.Focus();
        }

        try
        {
            // Check first that the group tag isn't already being used in the database by a different group
            if (!controller.GroupExists(groupTagBox.Text, _currentGroup.GroupID))
            {
                // If ok, set the current groupDAO reference to the group tag and update the database
                _currentGroup.Name = groupNameBox.Text;
                _currentGroup.GroupTag = groupTagBox.Text;
                _currentGroup.Description = groupDescriptionBox.Text;
                controller.UpdateGroupMetadata(_currentGroup);
            }
            else
            {
                // Tell the user they can't use the group tag
                invalidEntries.Text = string.Format(@"A group with grouptag ""{0}"" already exists.", groupTagBox.Text);
                return;
            }
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (CouldNotFindException)
        {
            //Shouldn't happen
            Response.Write("Could find group!");
        }
        catch (SqlException)
        {
            invalidEntries.Text = "An error occurred connecting to the server. Please try again soon.";
            return;
        }


    }

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
            controller.UpdateGroup(group);

        }
        catch (ArgumentNullException)
        {
            Response.Write("Argument is null!");
        }
        catch (CouldNotFindException)
        {
            Response.Write("Could not find Group");
        }
        catch (SqlException err)
        {
            Response.Write("An unknown error has happened");
            Logger.LogMessage("ManageGroup.aspx " + err.Message, LoggerLevel.SEVERE);
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

            GroupDAO group = controller.RetrieveGroup(Request["grouptag"].ToString());
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