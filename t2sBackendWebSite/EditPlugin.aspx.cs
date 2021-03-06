﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using t2sDbLibrary;

public partial class EditPlugin : BasePage
{
    public string extraJavascript = "";

    public string formPluginName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckLoginSession();

        String pluginName = HttpUtility.HtmlEncode(Request.QueryString["pluginname"]);
        if (pluginName != null)
            formPluginName = pluginName;

        StringBuilder title = new StringBuilder();
        title.Append("Edit Plugin");
        if (Request.HttpMethod.Equals("post", StringComparison.OrdinalIgnoreCase))
        {
            if (!doPOST())
                return;
        }
        else if (Request.HttpMethod.Equals("get", StringComparison.OrdinalIgnoreCase))
        {
            if (!doGET())
                return;
        }
        
        if (pluginName != null)
        {
            title.Append(" (");
            title.Append(pluginName);
            title.Append(")");

            PluginNameEditor.InnerText = pluginName;
        }

        PageTitle.Text = title.ToString();

        String successMsg = Request.QueryString["success"];
        if (!String.IsNullOrEmpty(successMsg))
        {
            pluginFeedback.InnerText = successMsg;
        }
    }

    private bool doPOST()
    {
        UserDAO user = (UserDAO)Session["userDAO"];
        String pluginName = Request.Form["pluginName"];

        String successMessage = "";

        if (pluginName == null)
        {
            // Redirect them back
            SendErrorMessage("Please specify a plugin");
            return false;
        }

        IDBController controller = new SqlController();
        PluginDAO plugin = null;
        try
        {
            plugin = controller.RetrievePlugin(pluginName);

            if (!plugin.OwnerID.Equals(user.UserID))
            {
                SendErrorMessage("That is not a plugin you have written.");
                return false;
            }
            else
            {
                // Go ahead and save it
                String luacodeFileLoc = LUADefinitions.getLuaScriptLocation(plugin.Name);

                // See if it's there
                if (File.Exists(luacodeFileLoc))
                {
                    String luacode = Request.Form["editorText"];
                    try
                    {
                        File.WriteAllText(luacodeFileLoc, luacode);
                        controller.ResetPluginFailedAttemptCount(plugin.PluginID);
                        if (controller.GetPluginFailedAttemptCount(plugin.PluginID) == 0)
                        {
                            // Reenable the plugin
                            controller.EnableGlobalPlugin(plugin.PluginID);
                        }
                        successMessage = "Plugin has been updated.";
                    }
                    catch (Exception)
                    {
                        SendErrorMessage("Could not save plugin.");
                        return false;
                    }
                }
                else
                {
                    SendErrorMessage("Could not save plugin.");
                    return false;
                }
            }
        }
        catch (CouldNotFindException)
        {
            SendErrorMessage("That is not a valid plugin");
            return false;
        }

        
        // Always redirect on POST
        Response.Redirect(string.Format("EditPlugin.aspx?pluginname={0}&success={1}", HttpUtility.UrlEncode(pluginName), HttpUtility.UrlEncode(successMessage)));

        return false;
    }

    private bool doGET()
    {
        UserDAO user = (UserDAO)Session["userDAO"];
        String pluginName = Request.QueryString["pluginname"];
        if (pluginName == null)
        {
            // Redirect them back
            SendErrorMessage("Please specify a plugin");
            return false;
        }
        IDBController controller = new SqlController();
        PluginDAO plugin = null;
        try
        {
            plugin = controller.RetrievePlugin(pluginName);
            PluginDescriptionEditor.InnerText = plugin.Description;

            if (!plugin.OwnerID.Equals(user.UserID))
            {
                //SendErrorMessage("That is not a plugin you have written");
                //return false;
                extraJavascript = @"editor.setReadOnly(true);";
            }

            String luacodeFileLoc = LUADefinitions.getLuaScriptLocation(plugin.Name);

            // See if it's there
            if (File.Exists(luacodeFileLoc))
            {
                String luacode = "";
                try
                {
                    luacode = File.ReadAllText(luacodeFileLoc);
                }
                catch (Exception)
                {
                    SendErrorMessage("Could not find plugin " + pluginName);
                    return false;
                }

                editorText.InnerText = luacode;
            }
            else
            {
                SendErrorMessage("Could not find plugin " + pluginName);
                return false;
            }
        }
        catch (CouldNotFindException)
        {
            SendErrorMessage("That is not a valid plugin");
            return false;
        }

        return true;
    }

    private void SendErrorMessage(string message)
    {
        Response.Redirect("Index.aspx?error=" + HttpUtility.UrlEncode(message));
        //errorMessage.Text = message;
    }
}