using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using t2sDbLibrary;

public partial class Index : BasePage
{
    private UserDAO _currentUser;

    public bool showErrorMessage = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        base.ClearCache();
        
        base.CheckLoginSession();
        _currentUser = HttpContext.Current.Session["userDAO"] as UserDAO;

        PageTitle.Text = "Text2Share - Home";
        retrieveGroups();
        retrievePlugins();

        string errorMessageHTML = Request.QueryString["error"];
        if (errorMessageHTML != null)
        {
            showErrorMessage = true;
            errorMessage.Text = errorMessageHTML;
        }
        
    }

    /// <summary>
    /// Retrieves groups from the database associated with the current user in session.
    /// </summary>
    private void retrieveGroups()
    {
        if (null != _currentUser)
        {
            List<GroupDAO> ownedGroups = new List<GroupDAO>();
            List<GroupDAO> moderatedGroups = new List<GroupDAO>();
            List<GroupDAO> userIsInGroups = new List<GroupDAO>();

            try
            {
                IDBController controller = new SqlController();
                ownedGroups = controller.GetGroupsUserIsOwnerOf(_currentUser.UserID);
                moderatedGroups = controller.GetGroupsUserIsModeratorOf(_currentUser.UserID);
                userIsInGroups = controller.GetGroupsUserIsMemberOf(_currentUser.UserID);
            }
            catch (SqlException)
            {
                groupsUserOwns.Text = "<li>An error occurred gathering group information. Please try again later.</li>";
                return;
            }

            printGroupsToPage(ownedGroups, groupsUserOwns, @"<li>You do not own any groups. Press ""Create Group"" to make a new one!</li>");
            printGroupsToPage(moderatedGroups, groupsUserModerates, "<li>You are not the moderator of any groups.</li>");
            printGroupsToPage(userIsInGroups, groupsUserIsIn, "<li>You are not a user of any groups.</li>");
        }
    }

    /// <summary>
    /// Prints the basic group information to the 
    /// </summary>
    /// <param name="groups"></param>
    /// <param name="pageLiteral"></param>
    /// <param name="zeroGroupCountMessage"></param>
    private void printGroupsToPage(List<GroupDAO> groups, Literal pageLiteral, string zeroGroupCountMessage)
    {
        StringBuilder groupBuilder = new StringBuilder();
        if (0 == groups.Count)
        {
            groupBuilder.Append(zeroGroupCountMessage);
        }
        else
        {
            foreach (GroupDAO group in groups)
            {
                groupBuilder.Append(string.Format(@"<li><a href=""ManageGroup.aspx?grouptag={1}"">{0} ({1})</a></li>", group.Name, group.GroupTag));
            }
        }

        pageLiteral.Text = groupBuilder.ToString();
    }

    public void retrievePlugins()
    {
        List<PluginDAO> plugins = new List<PluginDAO>();

        try
        {
            IDBController controller = new SqlController();
            plugins = controller.GetPluginsOwnedByUser(_currentUser);
        }
        catch (ArgumentNullException)
        {
            // Should not happen
        }

        printPluginsToPage(plugins, pluginsUserOwns, @"<li>You do not own any plugins. Press ""Create Plugin"" to make a new one!</li>");
    }

    public void printPluginsToPage(List<PluginDAO> plugins, Literal pageLiteral, string zeroPluginCountMessage)
    {
        StringBuilder pluginBuilder = new StringBuilder();
        if (0 == plugins.Count)
        {
            pluginBuilder.Append(zeroPluginCountMessage);
        }
        else
        {
            foreach (PluginDAO plugin in plugins)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format(@"<li><a href=""ManagePlugin.aspx?pluginname={0}"">{0} ", plugin.Name));
                if (plugin.IsDisabled)
                {
                    sb.Append(string.Format(@"<span class=""label label-important pull-right"">Disabled</span>"));
                }
                else
                {
                    try
                    {
                        IDBController controller = new SqlController();
                        int errorCount = controller.GetPluginFailedAttemptCount(plugin.PluginID);
                        if (errorCount > 0)
                            sb.Append(string.Format(@"<span class=""badge badge-important pull-right"">{0}</span>", errorCount));
                    }
                    catch (Exception)
                    {
                        // Shh... nothing but tears.
                    }
                }
                sb.Append(string.Format(@"</a></li>"));
                pluginBuilder.Append(sb.ToString());
            }
        }

        pageLiteral.Text = pluginBuilder.ToString();
    }
}