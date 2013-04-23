using System;
using System.Collections.Generic;
using System.Linq;
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
    private IDBController controller = new SqlController();

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
        PageTitle.Text = "Text2Share - Manage Group";

        GetGroupAndSetData();
    }

    /// <summary>
    /// Uses the grouptag GET parameter and retrieves the group metadata.
    /// Populates the "Group Information" section as well.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the given string is null.</exception>
    /// <exception cref="CouldNotFindException">If the user for the given username could not be found.</exception>
    /// <exception cref="SQL exception">For an unknown SQL error.</exception>
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
                invalidEntries.Text = string.Format(@"A group with grouptag ""{0}"" already exists.", HttpUtility.HtmlEncode(groupTagBox.Text));
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

    /// <summary>
    /// updates the group information
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="ArgumentNullException">If the given string is null.</exception>
    /// <exception cref="CouldNotFindException">If the user for the given username could not be found.</exception>
    /// <exception cref="SQLException">If an unknown databasae exception happends.</exception>
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













    

}