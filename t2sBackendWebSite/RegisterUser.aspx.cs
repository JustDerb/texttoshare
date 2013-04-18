using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected System.Web.UI.WebControls.Label MyLabel;
        protected System.Web.UI.WebControls.Button MyButton;
        protected System.Web.UI.WebControls.TextBox MyTextBox;

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
                Response.Write("The passwords you entered do not match. Please try again.");
                return;
            }

            SqlController controller = new SqlController();
            UserDAO user = new UserDAO()
            {
                FirstName = Request["firstNameBox"],
                LastName = Request["lastNameBox"],
                UserName = Request["userNameBox"],
                PhoneNumber = Request["PhoneNumberBox"],
                Carrier = (PhoneCarrier)Request["CarrierBox"],
                PhoneEmail = Request["PhoneNumberBox"] + (PhoneCarrier)Request["CarrierBox"],
                IsBanned = false,
                IsSuppressed = false
            };

            //check to see is needs to be hashed before
            try
            {
                controller.CreateUser(user, password);

            }
            catch (EntryAlreadyExistsException)
            {
                Response.Write("A user with that name already exists.");
                return;
            }
            catch (ArgumentNullException)
            {
                Response.Write("A field was left blank");
                return;
            }
            catch (SqlException)
            {
                //logger.logMessage("SQLException when Registering User", 4);
            }

            //Response.Write("You successfully registered!");
            //set the session the same as user login
            Session["username"] = user.UserName;
            Session["lastName"] = user.LastName;
            Session["firstName"] = user.FirstName;
            Session["carrier"] = user.Carrier;
            Session["phoneNumber"] = user.PhoneNumber;
            Session["userid"] = user.UserID;
            Session["phoneEmail"] = user.PhoneEmail;

            Response.Redirect("Index.apsx");

            return;
        }

        /// <summary>
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
            String phoneCarrier = Request["rCarrierBox"];
            String password = Request["rPasswordBox"];
            String userName = Request["rUserNameBox"];
        }
    


}