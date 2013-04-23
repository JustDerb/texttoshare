using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;

public partial class ManagePlugin : BasePage
{
    private UserDAO _currentUser = null;
    private PluginDAO _currentPlugin = null;

    public string PluginEditURL = "";

    public bool showErrorMessage = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
        _currentUser = Session["userDAO"] as UserDAO;
        GetPagePlugin();

        if (!Page.IsPostBack)
        {
            PopulatePage();
        }
    }

    private void PopulatePage()
    {
        if (_currentPlugin != null)
        {
            // Are they the owner?
            if (_currentPlugin.OwnerID != _currentUser.UserID)
            {
                Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"You cannot edit plugins you do not own.")));
                return;
            }

            PluginEditURL = string.Format(@"EditPlugin.aspx?pluginname={0}", HttpUtility.HtmlEncode(HttpUtility.UrlEncode(_currentPlugin.Name)));
            pluginNameBox.Text = _currentPlugin.Name;
            pluginDescriptionBox.Text = _currentPlugin.Description;
            versionBox.Text = _currentPlugin.VersionNum;
            helpTextBox.Text = _currentPlugin.HelpText;

            PluginDisabled.Text = (_currentPlugin.IsDisabled) ? "YES" : "NO";
            PluginErrorCount.Text = new SqlController().GetPluginFailedAttemptCount(_currentPlugin.PluginID).ToString();
        }
        else
        {
            Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"An error occurred retrieving the plugin information")));
            return;
        }

        PageTitle.Text = "Text2Share - Manage Plugin " + _currentPlugin.Name;
    }

    private void GetPagePlugin()
    {
        if (null == Request["pluginname"])
        {
            Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"An error occurred retrieving the plugin information")));
            return;
        }

        try
        {
            IDBController controller = new SqlController();
            _currentPlugin = controller.RetrievePlugin(Request["pluginname"]);
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (CouldNotFindException)
        {
            Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"An unknown error occurred loading plugin data. Please try again soon.")));
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("ManagePlugin: " + ex.Message, LoggerLevel.SEVERE);
            Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"An unknown error occurred loading plugin data. Please try again soon.")));
        }
    }

    private void ShowError(string error)
    {
        showErrorMessage = true;
        errorMessage.Text = error;
    }

    public void deletePlugin_Click(Object sender, EventArgs e)
    {
        if (_currentPlugin != null)
        {
            // Are they the owner?
            if (_currentPlugin.OwnerID != _currentUser.UserID)
            {
                Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"You cannot edit plugins you do not own.")));
            }

            try
            {
                IDBController database = new SqlController();
                if (database.DeletePlugin(_currentPlugin))
                {
                    Response.Redirect(string.Format(@"Index.aspx?success={0}", HttpUtility.UrlEncode(@"The plugin has been deleted.")));
                }
                else
                {
                    ShowError("Failed to delete plugin.");
                }
            }
            catch (CouldNotFindException)
            {
                // Shouldn't happen
            }
            catch (ArgumentNullException)
            {
                // Shouldn't happen
            }
            catch (SqlException ex)
            {
                Logger.LogMessage("ManagePlugin: " + ex.Message, LoggerLevel.SEVERE);
                ShowError("An unknown error occurred loading plugin data. Please try again soon.");
            }
        }

        PopulatePage();
    }

    public void updatePlugin_Click(Object sender, EventArgs e)
    {
        //String pluginName = Request["pluginNameBox"];
        String pluginDescription = Request["pluginDescriptionBox"];
        String pluginHelpText = Request["helpTextBox"];
        String pluginVersion = Request["versionBox"];

        try
        {
            // Are they the owner?
            if (_currentPlugin.OwnerID != _currentUser.UserID)
            {
                Response.Redirect(string.Format(@"Index.aspx?error={0}", HttpUtility.UrlEncode(@"You cannot edit plugins you do not own.")));
            }

            //if (string.IsNullOrWhiteSpace(pluginName) || pluginName.Length >= PluginDAO.NameMaxLength)
            //{
            //    ShowError("Plugin name cannot be empty or all spaces, and must be less than 64 characters.");
            //    return;
            //}
            if (string.IsNullOrWhiteSpace(pluginDescription) || pluginDescription.Length >= PluginDAO.DescriptionMaxLength)
            {
                ShowError("Plugin description cannot be empty or all spaces.");
            }
            else if (string.IsNullOrWhiteSpace(pluginHelpText) || pluginHelpText.Length >= PluginDAO.HelpTextMaxLength)
            {
                ShowError("Plugin help text cannot be empty or all spaces, and must be less than 160 characters.");
            }
            else if (string.IsNullOrWhiteSpace(pluginVersion) || pluginVersion.Length >= PluginDAO.VersionNumberMaxLength)
            {
                ShowError("Plugin version number cannot be empty or all spaces, and must be less than 32 characters.");
            }
            else
            {
                // Everything checks out--set the current plugin information
                //_currentPlugin.Name = pluginName;
                _currentPlugin.Description = pluginDescription;
                _currentPlugin.HelpText = pluginHelpText;
                _currentPlugin.VersionNum = pluginVersion;

                IDBController controller = new SqlController();
                //controller.UpdatePluginOwner(_currentPlugin, _currentUser);
                controller.UpdatePlugin(_currentPlugin);
            }
        }
        catch (CouldNotFindException)
        {
            // Shouldn't happen
        }
        catch (ArgumentNullException)
        {
            // Shouldn't happen
        }
        catch (SqlException ex)
        {
            Logger.LogMessage("ManagePlugin: " + ex.Message, LoggerLevel.SEVERE);
            ShowError("An unknown error occurred loading plugin data. Please try again soon.");
        }

        PopulatePage();
    }
}