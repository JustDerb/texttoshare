﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterUser.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title><asp:Literal ID="PageTitle" runat="server"></asp:Literal></title>
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
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
                    <asp:TextBox id="firstNameBox" class="input-block-level" name="firstNameBox" placeholder="First Name" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="lastNameBox" class="input-block-level" name="lastNameBox" placeholder="Last Name" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="userNameBox" class="input-block-level" name="userNameBox" placeholder="Username" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox type="tel" pattern="\d{3}\d{3}\d{4}" id="phoneNumberBox" class="input-block-level" name="phoneNumberBox" placeholder="Phone Number (5555555555)" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" title="Enter your phone number here. Do not include spaces or dashes." required></asp:TextBox>
                </div>
            </div>

            <!--<div class="control-group">
                <div class="controls">
                    <asp:TextBox id="carrierBox" class="input-block-level" name="carrierBox" placeholder="Phone Carrier (Verizon, AT&T, ...)" runat="server" required></asp:TextBox>
                </div>
            </div>-->

            <div class="control-group">
                <div class="controls">
                    <asp:DropDownList id="carrierDropdown" class="input-block-level" name="carrierDropdown"  runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:DropDownList>
               </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="passwordBox" class="input-block-level" name="passwordBox" TextMode="Password" placeholder="Password" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="verifyPasswordBox" class="input-block-level" name="verifyPasswordBox" TextMode="Password" placeholder="Verify Password" runat="server" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:Button id="register" class="btn btn-large btn-primary" placeholder="Register User" OnClick="Register_Click" runat="server" Text="Register"></asp:Button>
                    <a class="btn btn-large pull-right" href="Login.aspx">Login</a>
                </div>
            </div>
            <div class="control-group">
                <asp:Label id="invalidCredentials" class="control-label text-error" runat="server"></asp:Label>
            </div>
        </form>
    </body>
</html>
