using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;


namespace users
{
    /// <summary>
    /// Summary description for GetUser
    /// </summary>
    public class GetUser : Page
    {
        public GetUser()
        {
         
        }

        protected System.Web.UI.WebControls.Label MyLabel;
        protected System.Web.UI.WebControls.Button MyButton;
        protected System.Web.UI.WebControls.TextBox MyTextBox;


        public void getUser_Click(Object sender, EventArgs e)
        {
            //TODO
            SqlController controller = new SqlController();
            UserDAO user = new UserDAO();
            //grab input from textboxs
            String firstName = Request["rFirstNameBox"];
            String lastName = Request["rLastNameBox"];
            String phoneNumber = Request["rPhoneNumberBox"];
            String phoneCarrier = Request["rCarrierBox"];
            String password = Request["rPasswordBox"];
            String userName = Request["rUserNameBox"];
            //controller.

            
            if (!firstName.Equals(""))
            {



            }else if(!lastName.Equals("")){

            }else if(!phoneNumber.Equals("")){


            }else if(!phoneCarrier.Equals("")){


            }else if(!password.Equals("")){


            }else if(!userName.Equals("")){


            }
        }


    }

}