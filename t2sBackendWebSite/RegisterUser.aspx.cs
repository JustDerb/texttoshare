using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;
using System.Data.SqlClient;

public partial class _Default : BasePage
{
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
    public void Register_Click(Object sender, EventArgs e)
    {
        String password = Request["passwordBox"];
        String verifyPassword = Request["verifyPasswordBox"];

        if (!password.Equals(verifyPassword))
        {
            invalidCredentials.Text = "The passwords you entered do not match. Please try again.";
            return;
        }

        SqlController controller = new SqlController();

        String phoneNumber = Request["phoneNumberBox"].Replace("-", String.Empty);
        UserDAO user = new UserDAO()
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

   /* /// <summary>
    /// returns the query user
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void getUser_Click(Object sender, EventArgs e)
    {
        SqlController controller = new SqlController();
        UserDAO user = new UserDAO();

        //grab input from textboxs
        String firstName = Request["rfirstNameBox"];
        String lastName = Request["rlastNameBox"];
        String phoneNumber = Request["rPhoneNumberBox"];
        //String phoneCarrier = Request["rCarrierBox"];
        String phoneCarrier = Request["carrierDropdown"];
        Response.Write(phoneCarrier);
        String password = Request["rPasswordBox"];
        String userName = Request["rUserNameBox"];
    }*/

    public void getPhoneCarrierDropDown(){
        Dictionary<string, PhoneCarrier> dic = t2sDbLibrary.PhoneCarrier.getNameInstanceDictionary();
        String[] names = new String[dic.Count];
        ListItem[] list = new ListItem[dic.Count];
        for (int i = 0; i < dic.Count; i++)
        {
            ListItem item = new ListItem(dic.ElementAt(i).Value.GetName());
            list[i] = item;
        }
        Control con = FindControl("carrierDropdown");
        DropDownList Plugins = (DropDownList)con;
        Plugins.Items.AddRange(list);
    }


}