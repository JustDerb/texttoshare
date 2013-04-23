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
    public bool showErrorMessage = false;

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
        
        UserDAO owner = Session["userDAO"] as UserDAO;

        GroupDAO group = new GroupDAO(owner);
        group.Name = Request["groupNameBox"];
        group.GroupTag = Request["groupTagBox"];
        group.Description = Request["groupDescriptionBox"];

        if (string.IsNullOrWhiteSpace(group.Name) || group.Name.Length >= GroupDAO.NameMaxLength)
        {
            ShowError(string.Format("Invalid group name. Please enter a name under {0} characters.", GroupDAO.NameMaxLength));
            groupNameBox.Focus();
        }
        else if (string.IsNullOrWhiteSpace(group.GroupTag) || group.GroupTag.Length > GroupDAO.GroupTagMaxLength || group.GroupTag.Length < 4)
        {
            ShowError(string.Format("Invalid group tag. Please enter a tag between {0} and {1} characters.", 4, GroupDAO.GroupTagMaxLength));
            groupTagBox.Focus();
        }
        else if (string.IsNullOrWhiteSpace(group.Description) || group.Description.Length >= GroupDAO.DescriptionMaxLength)
        {
            ShowError(string.Format("Invalid group description. Please enter a name under {0} characters.", GroupDAO.DescriptionMaxLength));
            groupDescriptionBox.Focus();
        }
        else
        {
            try
            {
                if (controller.CreateGroup(group))
                {
                    // Redirect to the manage page
                    Response.Redirect(string.Format("ManageGroup.aspx?grouptag={0}", HttpUtility.UrlEncode(group.GroupTag)));
                }
                else
                {
                    ShowError("Your group was not created successfully. Please try again!");
                }
            }
            catch (ArgumentNullException)
            {
                ShowError("An unknown error has happened. Please try again later.");
            }
            catch (EntryAlreadyExistsException)
            {
                ShowError("This group already exists!");
            }
            catch (SqlException error)
            {
                ShowError("An unknown error has happened. Please try again later.");
                Logger.LogMessage("AddGroup.aspx: " + error.Message, LoggerLevel.SEVERE);
            }
        }
    }

    public void ShowError(string message)
    {
        showErrorMessage = true;
        errorMessage.Text = message;
    }
}