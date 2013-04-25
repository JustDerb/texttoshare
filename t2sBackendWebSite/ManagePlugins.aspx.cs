using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;

public partial class ManagePlugins : BasePage
{
    public GroupDAO _currentGroup;
    private UserDAO _currentUser;

    public bool showErrorMessage = false;
    public bool showSuccessMessage = false;

    /// <summary>
    /// functions which are run on page load.
    /// It checks that the user is login and sets the page title as
    /// well as filling in the group information for the group being managed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
        _currentUser = Session["userDAO"] as UserDAO;

        PageTitle.Text = "Text2Share - Manage Group Plugins";

        string errorMessageHTML = Request.QueryString["error"];
        if (errorMessageHTML != null)
        {
            showErrorMessage = true;
            errorMessage.Text = errorMessageHTML;
        }

        string successMessageHTML = Request.QueryString["success"];
        if (successMessageHTML != null)
        {
            showSuccessMessage = true;
            successMessage.Text = successMessageHTML;
        }

        GetGroupData();

        if (!Page.IsPostBack)
        {
            SetGroupData();
        }
    }

    private void GetGroupData()
    {
        try
        {
            IDBController controller = new SqlController();
            _currentGroup = controller.RetrieveGroup(Request["grouptag"]);
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (CouldNotFindException)
        {
            Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"An unknown error occurred. Please try again soon.")));
            return;
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("ManageGroup.aspx.cs: " + ex.Message, LoggerLevel.SEVERE);
            Response.Redirect(string.Format("ManagePlugins.aspx?error={0}", HttpUtility.UrlEncode("An unknown error occurred. Please try again soon.")));
            return;
        }
    }

    /// <summary>
    /// Uses the grouptag GET parameter and retrieves the group metadata.
    /// Populates the "Group Information" section as well.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the given string is null.</exception>
    /// <exception cref="CouldNotFindException">If the user for the given username could not be found.</exception>
    /// <exception cref="SQL exception">For an unknown SQL error.</exception>
    private void SetGroupData()
    {
        SqlController controller = new SqlController();
        List<PluginDAO> DisabledPlugins = controller.GetAllDisabledGroupPlugins(_currentGroup.GroupID);
        List<PluginDAO> EnabledPlugins = controller.GetAllEnabledGroupPlugins(_currentGroup.GroupID);

        bool first = true;
        foreach (PluginDAO plug in DisabledPlugins)
        {
            if (!first)
                disabledPlugins.Text += ", ";
            disabledPlugins.Text += plug.Name;

            first = false;
        }

        first = true;
        foreach (PluginDAO plug in EnabledPlugins)
        {
            if (!first)
                enabledPlugins.Text += ", ";
            enabledPlugins.Text += plug.Name;

            first = false;
        }
    }

    /// <summary>
    /// Updates the group's metadata in the database 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void submitPluginsButton_Click(object sender, EventArgs e)
    {
        bool isMod = false;
        string groupTag = Request.QueryString["grouptag"];
        SqlController control = new SqlController();
        GroupDAO group = control.RetrieveGroup(groupTag);
        List<GroupDAO> groupList = control.GetGroupsUserIsModeratorOf(_currentUser.UserID);
        foreach (GroupDAO x in groupList)
        {
            if (x.GroupID == group.GroupID)
            {
                isMod = true;
            }
        }

        if (_currentGroup.Owner.UserID != _currentUser.UserID && !isMod)
        {
            Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"You cannot edit plugins in groups you do not own.")));
            return;
        }

        try
        {
            IDBController controller = new SqlController();
            
            _currentGroup.EnabledPlugins = ParseFromTextArea(enabledPlugins);

            control.UpdateGroupPlugins(_currentGroup);
            
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (CouldNotFindException)
        {
            // Shouldn't happen
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("ManagePlugins.aspx: " + ex.Message, LoggerLevel.SEVERE);
            Response.Redirect(string.Format("ManagePlugins.aspx?grouptag={0}&error={1}",
                HttpUtility.UrlEncode(_currentGroup.GroupTag),
                HttpUtility.UrlEncode("An error occurred connecting to the server. Please try again soon.")));
            return;
        }

        Response.Redirect(string.Format("ManageGroup.aspx?grouptag={0}&success={1}",
                HttpUtility.UrlEncode(_currentGroup.GroupTag),
                HttpUtility.UrlEncode("Plugins successfully updated!")));
    }

    /// <summary>
    /// Splits up the user names in the given TextBox input, finds them in the database and adds them to a HashSet.
    /// </summary>
    /// <param name="textarea"></param>
    /// <returns></returns>
    private HashSet<PluginDAO> ParseFromTextArea(TextBox textarea)
    {
        string[] pluginsSplit;
        if (textarea.Text.IndexOf(',') < 0)
        {
            pluginsSplit = new string[] { textarea.Text.Trim() };
        }
        else
        {
            pluginsSplit = textarea.Text.Split(',');
        }

        HashSet<PluginDAO> plugins = new HashSet<PluginDAO>();

        try
        {
            IDBController controller = new SqlController();
            foreach (string plug in pluginsSplit)
            {
                try
                {
                    plugins.Add(controller.RetrievePlugin(plug.Trim()));
                }
                catch (CouldNotFindException)
                {
                    Response.Redirect(string.Format("ManagePlugins.aspx?grouptag={0}&error={1}{2}{3}",
                    HttpUtility.UrlEncode(_currentGroup.GroupTag),
                    HttpUtility.UrlEncode("Could not find plugin '"),
                    HttpUtility.UrlEncode(plug),
                    HttpUtility.UrlEncode("'")));
                    return null;
                }
            }
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("ManagePlugins.aspx: " + ex.Message, LoggerLevel.SEVERE);
            Response.Redirect(string.Format("ManagePlugins.aspx?grouptag={0}&error={1}",
                HttpUtility.UrlEncode(_currentGroup.GroupTag),
                HttpUtility.UrlEncode("An error occurred connecting to the server. Please try again soon.")));
            return null;
        }

        return plugins;
    }
}