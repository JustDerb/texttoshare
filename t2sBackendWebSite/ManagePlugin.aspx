<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManagePlugin.aspx.cs" Inherits="ManagePlugin" %>
<% 
    pluginOwner.Text = Session["username"].ToString();
%>

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

        <form id="managePlugin" runat="server">
            <p>
                <asp:DropDownList ID="Plugins" runat="server">
                    <asp:ListItem Text="<Select Plugin>" Value="0" />
                </asp:DropDownList>
            </p>
            <p>
                <label for="pluginNameBox">Plugin Name</label>
                <asp:TextBox ID="pluginNameBox" name="pluginNameBox" placeholder="Plugin Name" runat="server"></asp:TextBox>
            </p>
            <p>
                <label for="pluginOwner">Plugin Owner</label>
                <asp:TextBox ID="pluginOwner" name="pluginOwner" runat="server"></asp:TextBox>
            </p>
            <p>
                <label for="helpTextBox">Help Text</label>
                <asp:TextBox ID="helpTextBox" name="helpTextBox" placeholder="Help Text" runat="server"></asp:TextBox>
            </p>
            <p>
                <label for="pluginDescriptionBox">Plugin Description</label>
                <asp:TextBox ID="pluginDescripationBox" name="pluginDescripationBox" placeholder="Descripation of Plugin" runat="server"></asp:TextBox>
            </p>
            <p>
                <label for="versionBox">Plugin Version</label>
                <asp:TextBox ID="versionBox" name="versionBox" placeholder="Version Number" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="updatePluginButton" name="updatePluginButton" Text="Update Plugin" OnClick="updatePlugin_Click" runat="server"></asp:Button>
            </p>

            <div>
            </div>
        </form>
    </body>
</html>
