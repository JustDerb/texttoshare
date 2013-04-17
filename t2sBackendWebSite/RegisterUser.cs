using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;
using System.Data.SqlClient;


namespace codebehind
{
    /// <summary>
    /// Summary description for Default
    /// </summary>
    public class RegisterUser : Page
    {
        public RegisterUser()
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

            // MyLabel.Text = FirstTextBox.Text.ToString();
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

            //grab input from textboxs
            String firstName = Request["firstNameBox"];
            String lastName = Request["lastNameBox"];
            String phoneNumber = Request["PhoneNumberBox"];
            String phoneCarrier = Request["CarrierBox"];
            String userName = Request["userNameBox"];

            //set fields of the userDAO
            user.UserName = userName;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.PhoneNumber = phoneNumber;

            // TODO: need to be able to retrieve from database correct carrier ending
            //user.PhoneEmail = phoneNumber + "@txt.att.com";
            user.PhoneEmail = phoneNumber + (PhoneCarrier)phoneCarrier;
            user.IsBanned = false;
            user.IsSuppressed = false;

            //check to see is needs to be hashed before
            try
            {
                controller.CreateUser(user, password);

            }
            catch (EntryAlreadyExistsException)
            {
                Response.Write("User Already Exists");
                return;

            }
            catch (ArgumentNullException)
            {
                Response.Write("A Field was left blank");
                return;
            }
            catch (SqlException error)
            {
                //logger.logMessage("SQLException when Registering User", 4);
            }

            Response.Write("You successfully registered!");
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





}