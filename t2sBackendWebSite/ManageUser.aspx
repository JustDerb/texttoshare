<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageUser.aspx.cs" Inherits="manageUser" %>


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
                    <strong>Error!</strong> <asp:Literal ID="errorMessage" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
        <% } %>
        <% if (showSuccessMessage)
            { %>
        <div class="row-fluid">
            <div class="span12">
                <div class="alert alert-success">
                    <button type="button" class="close" data-dismiss="alert">&times;</button>
                    <strong>Success!</strong> <asp:Literal ID="successMessage" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
        <% } %>
        <div class="row-fluid">
            <div class="span12">
                <form id="loginForm" class="tts-form" runat="server">
                    <h2 class="tts-form-heading">Manage User</h2>
                    <div class="control-group">
                        <div class="controls">
                            <label for="firstNameBox">First Name</label>
                            <asp:TextBox ID="firstNameBox" name="firstNameBox" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="controls">
                            <label for="lastNameBox">Last Name</label>
                            <asp:TextBox ID="lastNameBox" name="lastNameBox" placeholder="Last Name" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="controls">
                            <label for="userNameBox">Username</label>
                            <asp:TextBox ID="userNameBox" name="userNameBox" placeholder="Username" runat="server" CssClass="uneditable-input"></asp:TextBox>
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="controls">
                            <label for="phoneNumberBox">Phone Number</label>
                            <asp:TextBox ID="phoneNumberBox" name="phoneNumberBox" placeholder="Phone Number" runat="server" CssClass="uneditable-input"></asp:TextBox>
                        </div>
                    </div>
                    <!--
                    <div class="control-group">
                        <div class="controls">
                            <label for="carrierBox">Carrier</label>
                            <asp:DropDownList ID="carrierDropdown" class="input-block-level" name="carrierDropdown" runat="server" CssClass="uneditable-input"></asp:DropDownList>
                        </div>
                    </div>
                    -->
                    <div class="control-group">
                        <div class="controls">
                            <asp:Button ID="register" placeholder="Update User" OnClick="update_Click" runat="server" Text="Update" CssClass="btn btn-primary"></asp:Button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</body>
</html>
