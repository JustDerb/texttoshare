using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using t2sDbLibrary;
/// <summary>
/// class which contains the code behind for AddGroup.aspx
/// </summary>
public partial class GetUser : BasePage
{
    /// <summary>
    /// functions which are excueted on page load
    /// checks to make sure user is login and sets the page title
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
        PageTitle.Text = "TextToShare - Add Group";
    }

    /// <summary>
    /// adds a new group to the database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="ArgumentNullException">If the given string is null.</exception>
    /// <exception cref="CouldNotFindException">If the user for the given username could not be found.</exception>
    /// <exception cref="EntryAlreadyExistsException">If the group already exists in the database.</exception>
    /// <exception cref="SQLException">An unknown SQL happened.</exception>
    public void addGroup_Click(Object sender, EventArgs e)
    {

        SqlController controller = new SqlController();
        GroupDAO group = null;
        UserDAO owner = new UserDAO();
        String ownerUsername = Request["groupOwner"];
        try
        {
            owner = controller.RetrieveUserByUserName(ownerUsername);
        }
        catch (ArgumentNullException)
        {
            Response.Write("Owner username is null! ");
        }
        catch (CouldNotFindException)
        {
            Response.Write("Could not find the username in database! ");
        }
        catch (SqlException err)
        {
            Response.Write("An unknown error has happened");
            Logger.LogMessage("AddGroup.aspx " + err.Message, LoggerLevel.SEVERE);
        }

        if (owner != null)
        {
            group = new GroupDAO(owner);
            group.Name = Request["groupNameBox"];
            group.GroupTag = Request["groupTagBox"];
            group.Description = Request["groupDescriptionBox"];
        }
        bool added = false;
        try
        {
            added = controller.CreateGroup(group);
        }
        catch (ArgumentNullException)
        {
            Response.Write("The group being inserted is NULL ");
        }
        catch (EntryAlreadyExistsException)
        {
            Response.Write("This group already exists! ");
        }
        catch(SqlException error){
            Response.Write("An unknown error has happened");
            Logger.LogMessage("AddGroup.aspx "+  error.Message, LoggerLevel.SEVERE);
        }

        if (added)
        {
            Response.Write("Your group was created successfully! ");
        }
        else
        {
            Response.Write("Your group was not created successfully. Please try again!");
        }


    }

}