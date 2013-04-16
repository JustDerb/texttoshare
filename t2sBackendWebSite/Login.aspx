﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="login.Login" Src="login.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TextToShare - Login</title>
    <link href="Content/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" class="tts-form" runat="server">
        <h1>Login</h1>
        <p>
            <label for="userNameBox">Username</label>
            <asp:TextBox id="userNameBox" name="userNameBox" placeholder="Username" runat="server" required></asp:TextBox>
        </p>
        <p>
            <label for="passwordBox">Password</label>
            <asp:TextBox id="passwordBox" name="passwordBox" TextMode="Password" placeholder="Password" runat="server" required></asp:TextBox>
        </p>
        
        <p>
            <asp:Button id="login" OnClick="Login_Click" runat="server" Text="Login"></asp:Button>
        </p>
        
        <p>
            <a class="tts-button" href="RegisterUser.aspx">Register Here</a>
        </p>  
    </form>
</body>
</html>
