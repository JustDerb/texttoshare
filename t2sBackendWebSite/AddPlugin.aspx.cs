using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using t2sDbLibrary;


public partial class AddPlugin : BasePage
{
    private UserDAO _currentUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();
        _currentUser = Session["userDAO"] as UserDAO;

        PageTitle.Text = "Text2Share - Add Plugin";
    }

    public void AddPlugin_Click(Object sender, EventArgs e)
    {
        PluginDAO plugin = new PluginDAO()
        {
            Name = Request["pluginNameBox"],
            Description = Request["pluginDescriptionBox"],
            HelpText = Request["helpTextBox"],
            IsDisabled = false,
            VersionNum = Request["versionBox"],
            OwnerID = _currentUser.UserID
        };

        // Plugin name length - 64 char
        // Plugin description - MAX
        // Plugin help text - 160
        // Version - 10 char
        if (plugin.Name == null || plugin.Name.Length >= PluginDAO.NameMaxLength)
        {
            ShowError("Plugin name is invalid.");
            pluginNameBox.Focus();
            return;
        }
        else if (plugin.Description == null || plugin.Description.Length >= PluginDAO.DescriptionMaxLength)
        {
            ShowError("Plugin description is invalid.");
            pluginDescriptionBox.Focus();
            return;
        }
        else if (plugin.HelpText == null || plugin.HelpText.Length >= PluginDAO.HelpTextMaxLength)
        {
            ShowError("Plugin help text is invalid.");
            helpTextBox.Focus();
            return;
        }
        else if (plugin.VersionNum == null || plugin.VersionNum.Length >= PluginDAO.VersionNumberMaxLength)
        {
            ShowError("Plugin version is invalid.");
            versionBox.Focus();
            return;
        }
        else
        {
            IDBController controller = new SqlController();
            try
            {
                if (controller.CreatePlugin(plugin))
                {
                    // Create a blank file
                    string path = LUADefinitions.getLuaScriptLocation(Request["pluginNameBox"]);
                    try
                    {
                        using (File.Create(path)) { }
                    }
                    catch (Exception)
                    {
                        // Clean up
                        controller.DeletePlugin(plugin);
                        ShowError("Error creating plugin.  Please try again later.");
                        return;
                    }

                    // Shoot them to the editor
                    Response.Redirect(string.Format("EditPlugin.aspx?p={0}", HttpUtility.UrlEncode(plugin.Name)));
                }
            }
            catch (EntryAlreadyExistsException)
            {
                // Error
                ShowError("That plugin name already exists.");
                return;
            }
            catch (SqlException ex)
            {
                // Error
                Logger.LogMessage("AddPlugin.aspx: " + ex.Message, LoggerLevel.SEVERE);
                return;
            }
        }
    }

    public void ShowError(string error)
    {
        errorMessage.Text = error;
    }
}