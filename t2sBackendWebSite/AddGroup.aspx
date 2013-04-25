<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddGroup.aspx.cs" Inherits="GetUser" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <asp:Literal ID="PageTitle" runat="server"></asp:Literal></title>
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
                    <strong>Error!</strong>
                    <asp:Literal ID="errorMessage" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
        <% } %>
        <div class="row-fluid">
            <div class="span12">
                <form id="AddGroup" class="tts-form" method="post" runat="server">
                    <h2 class="tts-form-heading">Add Group</h2>
                    <div class="control-group">
                        <div class="controls">
                            <asp:TextBox ID="groupNameBox" class="input-block-level" name="groupNameBox" placeholder="Group Name" runat="server" required></asp:TextBox>
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="controls">
                            <asp:TextBox ID="groupTagBox" class="input-block-level" name="groupTagBox" placeholder="Group Tag" runat="server" required></asp:TextBox>
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="controls">
                            <asp:TextBox ID="groupDescriptionBox" class="input-block-level" name="groupDescriptionBox" placeholder="Description of Group" runat="server" required></asp:TextBox>
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="controls">
                            <asp:Button ID="addGroupButton" class="btn btn-large btn-primary" Text="Add Group" OnClick="addGroup_Click" runat="server"></asp:Button>
                            <a class="btn btn-large pull-right" href="Index.aspx">Cancel</a>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</body>
</html>
