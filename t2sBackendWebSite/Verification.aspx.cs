using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;

public partial class Verification : BasePage
{
    private UserDAO _currentUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
        _currentUser = Session["userDAO"] as UserDAO;

        base.ClearCache();

        PageTitle.Text = "Text2Share - Verification";
        if (null != Request.QueryString["generateNew"])
        {
            GetNumberToSendVerificationTo();
        }
        else
        {
            GetCurrentVerificationCodeForUser();
        }
    }

    /// <summary>
    /// Grabs the value associated with the key "t2sAccountEmail" and sets
    /// the literal in the .aspx page for users to send their codes to.
    /// </summary>
    protected void GetNumberToSendVerificationTo()
    {
        try
        {
            IDBController controller = new SqlController();
            //verificationCode.Text = controller.GetCurrentVerificationValueForUser(_currentUser);
            string code = VerificationGenerator.GenerateString(6);

            verificationCode.Text = code;
            verificationCodeText.Text = "Register " + code;
            t2sAccountEmail.Text = controller.GetPairEntryValue("t2sEmailAccount");
            controller.SetVerificationCodeForUser(code, _currentUser);
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (CouldNotFindException ex)
        {
            Logger.LogMessage("Verification.aspx: " + ex.Message, LoggerLevel.SEVERE);
            errorMessage.Text = "An unknown error occured. Please try again later.1";
            return;
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("Verification.aspx: " + ex.Message, LoggerLevel.SEVERE);
            errorMessage.Text = "An unknown error occured. Please try again later.2";
            return;
        }
    }

    protected void GetNewVerificationCode_Click(object sender, EventArgs e)
    {
        GetNumberToSendVerificationTo();
    }

    protected void GetCurrentVerificationCodeForUser()
    {
        try
        {
            IDBController controller = new SqlController();
            string code = controller.GetCurrentVerificationValueForUser(_currentUser);
            if ("-1".Equals(code))
            {
                GetNumberToSendVerificationTo();
            }
            else
            {
                t2sAccountEmail.Text = controller.GetPairEntryValue("t2sEmailAccount");
                verificationCode.Text = code;
                verificationCodeText.Text = "Register " + code;
            }
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (CouldNotFindException ex)
        {
            Logger.LogMessage("Verification.aspx: " + ex.Message, LoggerLevel.SEVERE);
            errorMessage.Text = "An unknown error occured. Please try again later.3";
            return;
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("Verification.aspx: " + ex.Message, LoggerLevel.SEVERE);
            errorMessage.Text = "An unknown error occured. Please try again later.4";
            return;
        }
    }

    protected void ReloadOrRedirect_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.isVerified(_currentUser))
            {
                Response.Redirect("Verification.aspx");
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (CouldNotFindException)
        {

        }
    }
}