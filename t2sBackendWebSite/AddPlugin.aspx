<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddPlugin.aspx.cs" Inherits="AddPlugin" %>

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

        <form id="formAddPlugin" class="tts-form" method="post" runat="server" enctype="multipart/form-data">
            <h2 class="tts-form-heading">Add Plugin</h2>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox ID="pluginNameBox" class="input-block-level" name="pluginNameBox" placeholder="Plugin Name" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox ID="pluginDescriptionBox" class="input-block-level" name="pluginDescription" placeholder="Description of Plugin" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox ID="helpTextBox" class="input-block-level" name="helpTextBox" placeholder="Help Text" runat="server" title="Tells the user what this plugin does when they text 'HELP plugin'." required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox ID="versionBox" class="input-block-level" name="versionBox" placeholder="Version Number (1.0.0, etc)" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:Button ID="addPluginButton" class="btn btn-primary" Text="Add Plugin" OnClick="AddPlugin_Click" runat="server"></asp:Button>
                    <a href="Index.aspx" id="cancelButton" class="btn">Cancel</a>
                </div>
            </div>
            <div class="control-group">
                <asp:Label id="errorMessage" class="control-label text-error" runat="server"></asp:Label>
            </div>
        </form>
    </body>
</html>
