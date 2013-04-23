using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;
using System.Data.SqlClient;

/// <summary>
/// class which contains the code  beihind for RegisterUser.aspx
/// </summary>
public partial class _Default : BasePage
{
    /// <summary>
    /// function which is ran on page load
    /// sets the page title and poplulate the phone carrier dropdown menu
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        PageTitle.Text = "Text2Share - Register";
        getPhoneCarrierDropDown();
    }

    /// <summary>
    /// registers a user 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="ArgumentNullException">If the given string is null.</exception>
    /// <exception cref="EntryAlreadyExitsException">If the user for the given username already exits.</exception>
    /// <exception cref="SQLException">If an unknown databasae exception happends.</exception>
    /// <exception cref="InvalidCastException">If the phonecarrier string value can not be casted to a existing phoneCarrier.</exception>
 
    public void Register_Click(Object sender, EventArgs e)
    {
        String password = Request["passwordBox"];
        String verifyPassword = Request["verifyPasswordBox"];
        //verify password fields match
        if (!password.Equals(verifyPassword))
        {
            invalidCredentials.Text = "The passwords you entered do not match. Please try again.";
            return;
        }

        SqlController controller = new SqlController();

        String phoneNumber = Request["phoneNumberBox"].Replace("-", String.Empty);
        //create a new userDAO and set it fields
        UserDAO user = null;
        try
        {
             user = new UserDAO()
            {
                FirstName = Request["firstNameBox"],
                LastName = Request["lastNameBox"],
                UserName = Request["userNameBox"],
                PhoneNumber = phoneNumber,
                Carrier = (PhoneCarrier)(Request["carrierDropdown"]),
                PhoneEmail = phoneNumber + ((PhoneCarrier)(Request["carrierDropdown"])).GetEmail(),
                IsBanned = false,
                IsSuppressed = false
            };
        }
        catch (InvalidCastException)
        {
            Response.Write("Could not find phone carrier! Please try again!");
        }

        //check to see is needs to be hashed before
        try
        {
            if (!controller.CreateUser(user, password))
            {
                Response.Write("The user was not created");
            }
        }
        catch (EntryAlreadyExistsException)
        {
            invalidCredentials.Text = "A user with that name or phone number already exists. Please try again.";
            return;
        }
        catch (ArgumentNullException)
        {
            invalidCredentials.Text = "A field was left blank. Please make sure the form is fully completed.";
            return;
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("Register.aspx: " + ex.Message, LoggerLevel.SEVERE);
            invalidCredentials.Text = "An unknown error occured.  Please try again.";
            return;
        }

        //set the session the same as user login
        HttpContext.Current.Session["userDAO"] = user;

        Response.Redirect("Index.aspx");
    }

   
    /// <summary>
    /// gets a readonly dictionary of all the PhoneCarriers and populates the PhoneCarrierDropdown with their names.
    /// </summary>
    public void getPhoneCarrierDropDown(){
        Dictionary<string, PhoneCarrier> dic = t2sDbLibrary.PhoneCarrier.getNameInstanceDictionary();
        String[] names = new String[dic.Count];
        ListItem[] list = new ListItem[dic.Count];
        for (int i = 0; i < dic.Count; i++)
        {
            ListItem item = new ListItem(dic.ElementAt(i).Value.GetName());
            list[i] = item;
        }
        carrierDropdown.Items.AddRange(list);
    }


}