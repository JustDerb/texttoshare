<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<% IDBController controller = new SqlController(); %>

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

        <div class="container-fluid">
            <div class="row-fluid">
                <div class="span6">
                    <h2>Groups</h2>
                    <div class="well" style="padding: 8px 0;">
                        <ul class="nav nav-list">
                            <li class="nav-header">Groups I Own</li>
                            <asp:Literal id="groupsUserOwns" runat="server"></asp:Literal>
                        </ul>
                        <ul class="nav nav-list">
                            <li class="nav-header">Groups I Moderate</li>
                            <asp:Literal id="groupsUserModerates" runat="server"></asp:Literal>
                        </ul>
                        <ul class="nav nav-list">
                            <li class="nav-header">Groups I'm In</li>
                            <asp:Literal id="groupsUserIsIn" runat="server"></asp:Literal>
                        </ul>
                    </div>
                </div>
                <div class="span6">
                    <h2>Plugins</h2>
                    <div class="well" style="padding: 8px 0;">
                        <ul class="nav nav-list">
                            <li class="nav-header">Plugins I Own</li>
                            <asp:Literal id="pluginsUserOwns" runat="server"></asp:Literal>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </body>

    <script src="Scripts/jquery-1.7.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</html>