using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;

namespace codebehind
{
    /// <summary>
    /// Summary description for Default
    /// </summary>
    public class Default : Page
    {
        public Default()
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
           // MyLabel.Text = FirstTextBox.Text.ToString();
            SqlController controller = new SqlController();
            UserDAO user = new UserDAO();
           //grab input from textboxs
            String firstName = Request["firstNameBox"];
            String lastName = Request["lastNameBox"];
            String phoneNumber = Request["PhoneNumberBox"];
            String phoneCarrier = Request["CarrierBox"];
            String password = Request["passwordBox"];
            String userName = Request["userNameBox"];
           
           //set fields of the userDAO
            user.UserName = userName;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.PhoneNumber = phoneNumber;
            //to be be able to retrieve from database correct carrier ending
            user.PhoneEmail = phoneNumber + "@txt.att.com";
            user.IsBanned = false;
            user.IsSuppressed = false;
            //check to see is needs to be hashed before
            controller.CreateUser(user,password);
        }

        /// <summary>
        /// returns the query user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void getRegister_Click(Object sender, EventArgs e)
        {


        }





    }





}