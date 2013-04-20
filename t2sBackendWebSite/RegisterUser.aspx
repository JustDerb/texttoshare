<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterUser.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title><asp:Literal ID="PageTitle" runat="server"></asp:Literal></title>
        <link href="Content/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
			body {
				padding-top: 40px;
				padding-bottom: 40px;
				background-color: #f5f5f5;
			}
        </style>
        <link href="Content/bootstrap/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <form id="RegisterForm" class="tts-form" runat="server">
            <h2 class="tts-form-heading">Register</h2>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="firstNameBox" class="input-block-level" name="firstNameBox" placeholder="First Name" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="lastNameBox" class="input-block-level" name="lastNameBox" placeholder="Last Name" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="userNameBox" class="input-block-level" name="userNameBox" placeholder="Username" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox type="tel" pattern="\d{3}[\-]\d{3}[\-]\d{4}" id="phoneNumberBox" class="input-block-level" name="phoneNumberBox" placeholder="Phone Number (555-555-5555)" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="carrierBox" class="input-block-level" name="carrierBox" placeholder="Phone Carrier (Verizon, AT&T, ...)" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="passwordBox" class="input-block-level" name="passwordBox" TextMode="Password" placeholder="Password" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="verifyPasswordBox" class="input-block-level" name="verifyPasswordBox" TextMode="Password" placeholder="Verify Password" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:Button id="register" class="btn btn-large btn-primary" placeholder="Register User" OnClick="Register_Click" runat="server" Text="Register" required></asp:Button>
                    <a class="btn btn-large pull-right" href="Login.aspx">Login</a>
                </div>
            </div>
            <div class="control-group">
                <asp:Label id="invalidCredentials" class="control-label text-error" runat="server"></asp:Label>
            </div>
        </form>
    </body>
</html>
