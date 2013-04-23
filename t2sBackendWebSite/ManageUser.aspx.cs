using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using t2sDbLibrary;

/// <summary>
/// contains the code behind for ManageUser.aspx
/// </summary>
public partial class manageUser : BasePage
{
    public bool showErrorMessage = false;
    public bool showSuccessMessage = false;

    /// <summary>
    /// function which is ran on page load
    /// checks that the user is logged in and set the page title
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        base.ClearCache();
        base.CheckLoginSession();
        PageTitle.Text = "Text2Share - Manage User";
        getAndSetUserInfo();
    }

    /// <summary>
    /// gets and sets the user information for the manage user form boxes
    /// </summary>
    private void getAndSetUserInfo()
    {
        UserDAO user = (UserDAO)HttpContext.Current.Session["userDAO"];
        firstNameBox.Text = user.FirstName;
        lastNameBox.Text = user.LastName;
        userNameBox.Text = user.UserName;
        phoneNumberBox.Text = user.PhoneNumber;
        getPhoneCarrierDropDown(user.Carrier);
    }


    /// <summary>
    /// updates the user's information
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="ArgumentNullException">If the given string is null.</exception>
    /// <exception cref="CouldNotFindException">If the user for the given username could not be found.</exception>
    /// <exception cref="SQL Exception">An unknown SQL happened.</exception>
    public void update_Click(Object sender, EventArgs e)
    {
        SqlController controller = new SqlController();
        UserDAO user;
        String firstName = Request["firstNameBox"];
        String lastName = Request["lastNameBox"];
        String userName = Request["userNameBox"];
        String phoneNumber = Request["phoneNumberBox"];
        String carrier = Request["carrierBox"];

        user = Session["userDAO"] as UserDAO;
        //user.UserName = userName;
        //user.PhoneNumber = phoneNumber;
        user.FirstName = firstName;
        user.LastName = lastName;
        try
        {
            //check if user name or phone email is already being used
            //if (controller.UserExists(user.UserName, user.PhoneEmail))
            //{
            //    ShowError("User Name or Phone Number is already taken", false);
            //}
            //else
            {
                controller.UpdateUser(user);
                ShowError("User information successfully updated.", true);
            }
        }
        catch (ArgumentNullException)
        {
            ShowError("An unknown error occured. Please try again later.", true);
        }
        catch (CouldNotFindException)
        {
            ShowError("An unknown error occured. Please try again later.", true);
        }
        catch (SqlException err)
        {
            Logger.LogMessage("ManageUser.aspx: " + err.Message, LoggerLevel.SEVERE);
            ShowError("An unknown error occured. Please try again later.", true);
        }
    }

    private void ShowError(string message, bool redirect)
    {
        if (!redirect)
        {
            showErrorMessage = true;
            errorMessage.Text = message;
        }
        else
        {
            Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(message)));
        }
    }

    private void ShowSuccess(string message, bool redirect)
    {
        if (!redirect)
        {
            showSuccessMessage = true;
            successMessage.Text = message;
        }
        else
        {
            Response.Redirect(string.Format(@"Index.aspx?success={0}", HttpUtility.UrlEncode(message)));
        }
    }

    /// <summary>
    /// gets a readonly dictionary of all the PhoneCarriers and populates the PhoneCarrierDropdown with their names.
    /// </summary>
    public void getPhoneCarrierDropDown(PhoneCarrier selected)
    {
        Dictionary<string, PhoneCarrier> dic = t2sDbLibrary.PhoneCarrier.getNameInstanceDictionary();
        ListItem selectedItem = null;
        for (int i = 0; i < dic.Count; i++)
        {
            PhoneCarrier carrier = dic.ElementAt(i).Value;
            ListItem item = new ListItem(carrier.GetName());
            carrierDropdown.Items.Add(item);
            

            if (selected != null && selected.Equals(carrier))
            {
                selectedItem = item;
            }
        }

        if (selectedItem != null)
            carrierDropdown.SelectedIndex = carrierDropdown.Items.IndexOf(selectedItem);
    }
}