﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>TextToShare - Login</title>
        <link href="Content/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
        <link href="Content/style.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
			body {
				padding-top: 40px;
				padding-bottom: 40px;
				background-color: #f5f5f5;
			}
        </style>
    </head>
    <body>
        <form id="loginForm" class="tts-form" runat="server">
            <h2 class="tts-form-heading">Login</h2>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="userNameBox" class="input-block-level" name="userNameBox" placeholder="Username" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:TextBox id="passwordBox" class="input-block-level" name="passwordBox" TextMode="Password" placeholder="Password" runat="server" required></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <div class="controls">
                    <asp:Button id="login" class="btn btn-large btn-primary" OnClick="Login_Click" runat="server" Text="Login"></asp:Button>
                    <a href="RegisterUser.aspx" class="btn btn-large pull-right">Register</a>
                </div>
            </div>
        </form>
    </body>
</html>
