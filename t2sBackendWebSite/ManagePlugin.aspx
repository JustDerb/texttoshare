<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManagePlugin.aspx.cs" Inherits="ManagePlugin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title><asp:Literal ID="PageTitle" runat="server"></asp:Literal></title>
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <link href="Content/bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            body {
                padding-top: 60px;
                padding-bottom: 40px;
            }

            @media (max-width: 980px) {
                .navbar-text .pull-right {
                    float: none;
                    padding-left: 5px;
                    padding-right: 5px;
                }
            }
        </style>
        <link href="Content/bootstrap/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    </head>
    <body>

        <div class="navbar navbar-fixed-top">
            <div class="navbar-inner">
                <div class="container-fluid">
                    <button type="button" class="btn btn-navbar collapsed" data-toggle="collapse" data-target=".nav-collapse">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="brand" href="Index.aspx">Text2Share</a>
                    <div class="nav-collapse collapse">
                        <ul class="nav pull-right" role="navigation">
                            <li>
                                <a href="ManageUser.aspx">Settings</a>
                            </li>
                            <li class="divider-vertical"></li>
                            <li>
                                <asp:HyperLink ID="logoutButton" runat="server" href="Logout.aspx">Logout</asp:HyperLink>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div class="container">
            <% if (showErrorMessage)
                { %>
            <div class="row-fluid">
                <div class="span12">
                    <div class="alert alert-error">
                        <button type="button" class="close" data-dismiss="alert">&times;</button>
                        <strong>Error!</strong> <asp:Literal ID="errorMessage" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
            <% } %>
            <div class="row-fluid">
                <div class="span6">
                    <form id="managePlugin" class="tts-form" runat="server">
                        <h2 class="tts-form-heading">Plugin <a href="<%= PluginEditURL %>" id="editPluginButton" class="btn btn-info pull-right">Edit Code</a></h2>
                        <div class="control-group">
                            <label class="control-label" for="pluginNameBox">Plugin Name</label>
                            <div class="controls">
                                <asp:TextBox ID="pluginNameBox" class="input-block-level uneditable-input" name="pluginNameBox" placeholder="Plugin Name" runat="server" disabled="disabled" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="helpTextBox">Help Text</label>
                            <div class="controls">
                                <asp:TextBox ID="helpTextBox" class="input-block-level" name="helpTextBox" placeholder="Help Text" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="pluginDescriptionBox">Plugin Description</label>
                            <div class="controls">
                                <asp:TextBox ID="pluginDescriptionBox" class="input-block-level" name="pluginDescriptionBox" placeholder="Description of Plugin" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="versionBox">Plugin Version</label>
                            <div class="controls">
                                <asp:TextBox ID="versionBox" class="input-block-level" name="versionBox" placeholder="(1.0.0, etc)" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group">
                            <div class="controls">
                                <asp:Button type="submit" ID="updatePluginButton" class="btn btn-primary" name="updatePluginButton" Text="Update Plugin" OnClick="updatePlugin_Click" runat="server"></asp:Button>
                                <asp:Button ID="deletePluginButton" class="btn btn-danger pull-right" name="deletePluginButton" Text="Delete" OnClick="deletePlugin_Click" runat="server"></asp:Button>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="span6">
                    <div class="well">
                        <h3>Plugin Details:</h3>
                        <ul>
                            <li>Plugin errors: <asp:Literal ID="PluginErrorCount" runat="server"></asp:Literal></li>
                            <li>Is this plugin disabled? <strong><asp:Literal ID="PluginDisabled" runat="server"></asp:Literal></strong></li>
                        </ul>
                    </div>
                    <div class="well">
                        <p>If your plugin has been disabled, you will need to edit your LUA plugin and fix anything that is wrong with it.</p>
                    </div>
                </div>
            </div>
        </div>
    </body>
</html>
