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

    protected void Page_Load(object sender, EventArgs e)
    {
        base.ClearCache();
        
        base.CheckLoginSession();
        _currentUser = HttpContext.Current.Session["userDAO"] as UserDAO;

        PageTitle.Text = "Text2Share - Home";
        retrieveGroups();
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

            printGroupsToPage(ownedGroups, groupsUserOwns, "<li>You do not own any groups. Press the \"+\" button to create a new one.</li>");
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

    public void retrievePlugins(Object sender, EventArgs e)
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

        printPluginsToPage(plugins, pluginsUserOwns, "<li>You do not own any plugins. Press the \"+\" button to create a new one.</li>");
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
                pluginBuilder.Append(string.Format(@"<li><a href=""ManagePlugin.aspx?pluginid={1}"">{0}</li>", plugin.Name, plugin.PluginID));
            }
        }

        pageLiteral.Text = pluginBuilder.ToString();
    }
}