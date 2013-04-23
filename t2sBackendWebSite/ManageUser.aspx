<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageUser.aspx.cs" Inherits="manageUser" %>


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

        <form id="manageUserForm" runat="server">
   
            <h1>Manage User</h1>
            <p>
                <label for="firstNameBox">First Name</label>
                <asp:TextBox ID="firstNameBox" name="firstNameBox" runat="server"></asp:TextBox>
            </p>
            <p>
                <label for="lastNameBox">Last Name</label>
                <asp:TextBox ID="lastNameBox" name="lastNameBox" placeholder="Last Name" runat="server" ></asp:TextBox>
            </p>
            <p>
                <label for="userNameBox">Username</label>
                <asp:TextBox ID="userNameBox" name="userNameBox" placeholder="Username" runat="server" ></asp:TextBox>
            </p>
            <p>
                <label for="phoneNumberBox">Phone Number</label>
                <asp:TextBox ID="phoneNumberBox" name="phoneNumberBox" placeholder="Phone Number" runat="server" ></asp:TextBox>
            </p>
            <p>
                <label for="carrierBox">Carrier</label>
                <asp:TextBox ID="carrierBox" name="carrierBox" placeholder="Phone Carrier" runat="server" ></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="register" placeholder="Update User" OnClick="update_Click" runat="server" Text="Update" ></asp:Button>
            </p>
        </form>
    </body>
</html>
