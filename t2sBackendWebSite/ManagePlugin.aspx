<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManagePlugin.aspx.cs" Inherits="ManagePlugin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title><asp:Literal ID="PageTitle" runat="server"></asp:Literal></title>
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
                                <asp:HyperLink id="logoutButton" runat="server" href="Logout.aspx">Logout</asp:HyperLink>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>

        <form id="managePlugin" class="tts-form" runat="server">
            <h2 class="tts-form-heading">Login</h2>
            <div class="control-group">
                <label class="control-label" for="pluginNameBox">Plugin Name</label>
                <div class="controls">
                    <asp:TextBox ID="pluginNameBox" class="input-block-level" name="pluginNameBox" placeholder="Plugin Name" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="helpTextBox">Help Text</label>
                    <div class="controls">                
                    <asp:TextBox ID="helpTextBox" class="input-block-level" name="helpTextBox" placeholder="Help Text" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="pluginDescriptionBox">Plugin Description</label>
                    <div class="controls">                
                    <asp:TextBox ID="pluginDescriptionBox" class="input-block-level" name="pluginDescriptionBox" placeholder="Description of Plugin" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="versionBox">Plugin Version</label>
                    <div class="controls">                
                    <asp:TextBox ID="versionBox" class="input-block-level" name="versionBox" placeholder="(1.0.0, etc)" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:Button ID="updatePluginButton" class="btn btn-primary" name="updatePluginButton" Text="Update Plugin" OnClick="updatePlugin_Click" runat="server"></asp:Button>
                    <asp:Literal ID="editPluginButton" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="control-group">
                <asp:Label id="invalidPlugin" class="control-label text-error" runat="server"></asp:Label>
            </div>
        </form>
    </body>
</html>
