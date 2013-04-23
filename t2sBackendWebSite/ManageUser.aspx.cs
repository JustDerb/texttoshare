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
        UserDAO user = (UserDAO) HttpContext.Current.Session["userDAO"];
        firstNameBox.Text = user.FirstName;
        lastNameBox.Text = user.LastName;
        userNameBox.Text = user.UserName;
        phoneNumberBox.Text = user.PhoneNumber;
        carrierBox.Text = user.Carrier.GetName();
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
       
            user = (UserDAO) HttpContext.Current.Session["userDAO"];
            user.UserName = userName;
            user.PhoneNumber = phoneNumber;
            user.FirstName = firstName;
            try
            {
            //check if user name or phone email is already being used
             if (controller.UserExists(user.UserName, user.PhoneEmail))
             {
                 Response.Write("User Name or Phone Number is already taken");
             }
             else
             {
                controller.UpdateUser(user);
                Response.Write("User information successfull updated");
             }
        }
        catch (ArgumentNullException)
        {
            Response.Write("Argument is Null");
        }
        catch (CouldNotFindException)
        {
            Response.Write("Could not find user");
        }
        catch (SqlException err)
        {
            Logger.Equals("ManageUser.aspx " + err.Message, LoggerLevel.SEVERE);
            Response.Write("Unknown error happend!");
        }


    }



}