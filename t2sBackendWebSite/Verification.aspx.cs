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

        PageTitle.Text = "Text2Share - Register";
        GetNumberToSendVerificationTo();
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
            verificationCode.Text = controller.GetCurrentVerificationValueForUser(_currentUser);
            t2sAccountEmail.Text = controller.GetPairEntryValue("t2sEmailAccount");
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (CouldNotFindException ex)
        {
            Logger.LogMessage("Verification.aspx: " + ex.Message, LoggerLevel.SEVERE);
            errorMessage.Text = "An unknown error occured. Please try again later.";
            return;
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("Verification.aspx: " + ex.Message, LoggerLevel.SEVERE);
            errorMessage.Text = "An unknown error occured. Please try again later.";
            return;
        }
    }

    protected void ReloadOrRedirect_Click(object sender, EventArgs e)
    {
        try
        {

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