using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using t2sDbLibrary;

/// <summary>
/// class which contains the codebehind for ManageGroup.aspx
/// </summary>
public partial class ManageGroup : BasePage
{
    private GroupDAO _currentGroup;
    private UserDAO _currentUser;

    public bool showErrorMessage = false;
    public bool showSuccessMessage = false;

    private List<string> usersNotFound = new List<string>();

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

        PageTitle.Text = "Text2Share - Manage Group";

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
            RetrieveUsers();
            RetrievePlugins();
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
            Response.Redirect(string.Format("ManageGroup.aspx?error={0}", HttpUtility.UrlEncode("An unknown error occurred. Please try again soon.")));
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
        groupNameBox.Text = _currentGroup.Name;
        groupTagBox.Text = _currentGroup.GroupTag;
        groupDescriptionBox.Text = _currentGroup.Description;
    }

    /// <summary>
    /// Updates the group's metadata in the database 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdateGroupMetadata_Click(object sender, EventArgs e)
    {
        if (_currentGroup.Owner.UserID != _currentUser.UserID)
        {
            Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"You cannot edit groups you do not own.")));
            return;
        }

        // Check that they are not updating to empty values
        if (string.IsNullOrWhiteSpace(groupNameBox.Text))
        {
            Response.Redirect(string.Format("ManageGroup.aspx?grouptag={0}&error={1}",
                HttpUtility.UrlEncode(_currentGroup.GroupTag),
                HttpUtility.UrlEncode("Cannot update group name to be empty or whitespace.")));
            groupNameBox.Focus();
            return;
        }
        else if (string.IsNullOrWhiteSpace(groupTagBox.Text))
        {
            Response.Redirect(string.Format("ManageGroup.aspx?grouptag={0}&error={1}",
                HttpUtility.UrlEncode(_currentGroup.GroupTag),
                HttpUtility.UrlEncode("Cannot update group tag to be empty or whitespace.")));
            groupTagBox.Focus();
            return;
        }
        else if (string.IsNullOrWhiteSpace(groupDescriptionBox.Text))
        {
            Response.Redirect(string.Format("ManageGroup.aspx?grouptag={0}&error={1}",
                HttpUtility.UrlEncode(_currentGroup.GroupTag),
                HttpUtility.UrlEncode("Cannot update group description to be empty or whitespace.")));
            groupDescriptionBox.Focus();
            return;
        }
        else if (string.IsNullOrWhiteSpace(groupOwner.Text))
        {
            Response.Redirect(string.Format("ManageGroup.aspx?grouptag={0}&error={1}",
                HttpUtility.UrlEncode(_currentGroup.GroupTag),
                HttpUtility.UrlEncode("Cannot update group owner to be empty or whitespace.")));
            groupOwner.Focus();
            return;
        }

        try
        {
            IDBController controller = new SqlController();
            // Check first that the group tag isn't already being used in the database by a different group
            if (!controller.GroupExists(groupTagBox.Text, _currentGroup.GroupID))
            {
                // If ok, set the current groupDAO reference to the group tag and update the database
                _currentGroup.Name = groupNameBox.Text;
                _currentGroup.GroupTag = groupTagBox.Text;
                _currentGroup.Description = groupDescriptionBox.Text;

                controller.UpdateGroupMetadata(_currentGroup);

                _currentGroup.Moderators = ParseUsersFromTextArea(groupModerators);
                _currentGroup.Users = ParseUsersFromTextArea(groupUsers);

                controller.UpdateGroup(_currentGroup);
            }
            else
            {
                // Tell the user they can't use the group tag
                Response.Redirect(string.Format("ManageGroup.aspx?grouptag={0}&error={1}",
                    HttpUtility.UrlEncode(_currentGroup.GroupTag),
                    HttpUtility.UrlEncode(string.Format(@"A group with grouptag ""{0}"" already exists.", HttpUtility.HtmlEncode(groupTagBox.Text)))));
                return;
            }
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
            Logger.LogMessage("ManageGroup.aspx: " + ex.Message, LoggerLevel.SEVERE);
            Response.Redirect(string.Format("ManageGroup.aspx?grouptag={0}&error={1}", 
                HttpUtility.UrlEncode(_currentGroup.GroupTag),
                HttpUtility.UrlEncode("An error occurred connecting to the server. Please try again soon.")));
            return;
        }

        if (usersNotFound.Count > 0)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string user in usersNotFound)
            {
                builder.Append(user + " ");
            }

            Response.Redirect(string.Format("ManageGroup.aspx?grouptag={0}&error={1}",
                HttpUtility.UrlEncode(_currentGroup.GroupTag),
                HttpUtility.UrlEncode("The following users were not found in the database and were not added to the group: " + builder.ToString())));
        }
    }

    /// <summary>
    /// Splits up the user names in the given TextBox input, finds them in the database and adds them to a HashSet.
    /// </summary>
    /// <param name="textarea"></param>
    /// <returns></returns>
    private HashSet<UserDAO> ParseUsersFromTextArea(TextBox textarea)
    {
        string[] usernames;
        if (textarea.Text.IndexOf(',') < 0)
        {
            usernames = new string[] { textarea.Text.Trim() };
        }
        else
        {
            usernames = textarea.Text.Split(',');
        }

        HashSet<UserDAO> users = new HashSet<UserDAO>();

        try
        {
            IDBController controller = new SqlController();
            foreach (string username in usernames)
            {
                try
                {
                    users.Add(controller.RetrieveUserByUserName(username));
                }
                catch (CouldNotFindException)
                {
                    usersNotFound.Add(username);
                }
            }
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("ManageGroup.aspx: " + ex.Message, LoggerLevel.SEVERE);
            Response.Redirect(string.Format("ManageGroup.aspx?grouptag={0}&error={1}",
                HttpUtility.UrlEncode(_currentGroup.GroupTag),
                HttpUtility.UrlEncode("An error occurred connecting to the server. Please try again soon.")));
            return null;            
        }

        return users;
    }

    /// Retrieves groups from the database associated with the current user in session.
    /// </summary>
    private void RetrieveUsers()
    {
        if (null != _currentGroup)
        {
            PrintUsersToPage(_currentGroup.Users, groupUsers);
            PrintUsersToPage(_currentGroup.Moderators, groupModerators);
            PrintUsersToPage(new HashSet<UserDAO> { _currentGroup.Owner }, groupOwner);
        }
    }

    /// <summary>
    /// Prints the basic group information to the 
    /// </summary>
    /// <param name="groups"></param>
    /// <param name="pageLiteral"></param>
    /// <param name="zeroGroupCountMessage"></param>
    private void PrintUsersToPage(HashSet<UserDAO> users, Control control)
    {
        StringBuilder userBuilder = new StringBuilder();

        if (users.Count > 0)
        {
            foreach (UserDAO user in users)
            {
                userBuilder.Append(string.Format(@"{0}, ", user.UserName));
            }
            userBuilder.Remove(userBuilder.Length - 2, 2);
        }

        ((TextBox) control).Text = userBuilder.ToString();
    }

    public void RetrievePlugins()
    {
        if (null != _currentGroup)
        {
            PrintPluginsToPage(_currentGroup.EnabledPlugins, groupPluginList, @"<li>This group has no specific plugins enabled for it.</li>");
        }
    }

    public void PrintPluginsToPage(HashSet<PluginDAO> plugins, Literal pageLiteral, string zeroPluginCountMessage)
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
                pluginBuilder.Append(string.Format(@"<li>{0}</li>", plugin.Name));
            }
        }

        pageLiteral.Text = pluginBuilder.ToString();
    }

    protected void deleteGroupButton_Click(object sender, EventArgs e)
    {
        if (null != _currentGroup)
        {
            if (_currentGroup.Owner.UserID != _currentUser.UserID)
            {
                Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"You cannot edit groups you do not own.")));
                return;
            }

            try
            {
                IDBController controller = new SqlController();
                if (controller.DeleteGroup(_currentGroup))
                {
                    Response.Redirect(string.Format(@"Index.aspx?success={0}", HttpUtility.UrlEncode(@"The group has been deleted.")));
                }
            }
            catch (ArgumentNullException)
            {
                // Shouldn't happen
            }
            catch (SqlException ex)
            {
                Logger.LogMessage("ManageGroup.aspx: " + ex.Message, LoggerLevel.SEVERE);
                Response.Redirect(string.Format("ManageGroup.aspx?grouptag={0}&error={1}",
                    HttpUtility.UrlEncode(_currentGroup.GroupTag),
                    HttpUtility.UrlEncode("An error occurred connecting to the server. Please try again soon.")));
                return;
            }
        }

        SetGroupData();
        RetrieveUsers();
        RetrievePlugins();
    }
}