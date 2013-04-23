using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;

public partial class ManagePlugin : BasePage
{
    private UserDAO _currentUser;
    private PluginDAO _currentPlugin;

    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
        _currentUser = Session["userDAO"] as UserDAO;

        PageTitle.Text = "Text2Share - Manage Plugin";

        GetPagePlugin();
        editPluginButton.Text = string.Format(@"<a href=""EditPlugin.aspx?pluginname={0}"" id=""editPluginButton"" class=""btn pull-right"">Edit Plugin Code</a>", HttpUtility.HtmlEncode(HttpUtility.UrlEncode(_currentPlugin.Name)));

        popluateList(sender, e);
    }

    private void GetPagePlugin()
    {
        if (null == Request["pluginname"])
        {
            Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"An error occurred retrieving the plugin information")));
            return;
        }

        try
        {
            IDBController controller = new SqlController();
            _currentPlugin = controller.RetrievePlugin(Request["pluginname"]);
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (CouldNotFindException)
        {
            Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"An unknown error occurred loading plugin data. Please try again soon.")));
            return;
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("ManagePlugin: " + ex.Message, LoggerLevel.SEVERE);
            Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"An unknown error occurred loading plugin data. Please try again soon.")));
            return;
        }
    }

    private void ShowError(string error)
    {
        invalidPlugin.Text = error;
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
        catch (SqlException)
        {
            // Logger.LogMessage("SQL exception with Retrieve Enabled plugin");
        }
        catch (ArgumentNullException)
        {

        }
        catch (CouldNotFindException)
        {

        }
    }


    public void updatePlugin_Click(Object sender, EventArgs e)
    {
        String pluginName = Request["pluginNameBox"];
        String pluginDescription = Request["pluginDescripationBox"];
        String pluginHelpText = Request["helpTextBox"];
        String pluginVersion = Request["versionBox"];

        try
        {
            //TODO
            //check session user id to make sure they are the owner of plugin
            if (string.IsNullOrWhiteSpace(pluginName) || pluginName.Length >= PluginDAO.NameMaxLength)
            {
                ShowError("Plugin name cannot be empty or all spaces, and must be less than 64 characters.");
                return;
            }
            if (string.IsNullOrWhiteSpace(pluginDescription) || pluginDescription.Length >= PluginDAO.DescriptionMaxLength)
            {
                ShowError("Plugin description cannot be empty or all spaces.");
                return;
            }
            if (string.IsNullOrWhiteSpace(pluginHelpText) || pluginHelpText.Length >= PluginDAO.HelpTextMaxLength)
            {
                ShowError("Plugin help text cannot be empty or all spaces, and must be less than 160 characters.");
                return;
            }
            if (string.IsNullOrWhiteSpace(pluginVersion) || pluginVersion.Length >= PluginDAO.VersionNumberMaxLength)
            {
                ShowError("Plugin version number cannot be empty or all spaces, and must be less than 32 characters.");
                return;
            }

            // Everything checks out--set the current plugin information
            _currentPlugin.Name = pluginName;
            _currentPlugin.Description = pluginDescription;
            _currentPlugin.HelpText = pluginHelpText;
            _currentPlugin.VersionNum = pluginVersion;

            IDBController controller = new SqlController();
            //controller.UpdatePluginOwner(_currentPlugin, _currentUser);
            controller.UpdatePlugin(_currentPlugin);
        }
        catch (CouldNotFindException)
        {
            // Shouldn't happen
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("ManagePlugin: " + ex.Message, LoggerLevel.SEVERE);
            ShowError("An unknown error occurred loading plugin data. Please try again soon.");
            return;
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